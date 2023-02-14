#include "lock.h"
#include "buf.h"

#define hash_size 100

/*typedef struct xact_node
{
	int trx_id;
	struct xact_node* prev;
	struct xact_node* next;
	struct lock_t* head_lock_object;
	struct lock_t* tail_lock_object;
}xact_node;

typedef struct lock_entry
{
	int table_id;
	int record_id;
	struct lock_entry* next_entry;
	struct lock_t* next;
}lock_entry;

typedef struct lock_t
{
	struct lock_t* prev;
	struct lock_t* next;
	struct lock_entry* entry;
	//struct lock_t* same_thread_prev;
	struct lock_t* same_thread_next;

	int lock_mode;// 0 == shared, 1 == exclusive
	int owner_trx_id;
	int hash;
	int wait;
	char* recovery_value; // only lock_mode==1->use this value
}lock_t;*/

lock_entry** lock_hash_table = NULL;
pthread_mutex_t hash_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t deadlock_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t trx_mutex = PTHREAD_MUTEX_INITIALIZER;
static pthread_cond_t hash_cond[hash_size];

xact_node* trx_head = NULL;
xact_node* trx_tail = NULL;

int global_trx_id = 1;

int trx_begin() //Library_for_transaction
{
	pthread_mutex_lock(&trx_mutex);
		
	xact_node* temp = (xact_node*)malloc(sizeof(xact_node));
	temp->trx_id = global_trx_id++;
	temp->head_lock_object = NULL;
	temp->tail_lock_object = NULL;
	if(trx_head == NULL)
	{
		temp->prev = NULL;
		temp->next = NULL;
		trx_head = trx_tail = temp;
		
	}
	else
	{
		temp->prev = trx_tail;
		temp->next = NULL;
		trx_tail->next = temp;
		trx_tail = temp;
	}

	pthread_mutex_unlock(&trx_mutex);
	return temp->trx_id;
}

int trx_commit(int trx_id) //Library_for_transaction
{
	xact_node* temp = trx_head;
	while(temp!=NULL)
	{
		if(temp->trx_id==trx_id)
		{
			lock_t* lock_temp = temp->head_lock_object;
			while(lock_temp!=NULL)
			{
				lock_t* delete_lock = lock_temp;
				lock_temp = lock_temp->same_thread_next;
				if(lock_release(delete_lock)!=0)
				{
					return 0;
				}
			}
			temp->head_lock_object = NULL;
			temp->tail_lock_object = NULL;
			return temp->trx_id;
		}
		else
		{
			temp = temp->next;
		}
	}
	return 0;

}

int trx_abort(int trx_id)
{
	pthread_mutex_lock(&deadlock_mutex);

	xact_node* temp = trx_head;
	while(temp!=NULL)
	{
		if(temp->trx_id==trx_id)
		{
			lock_t* lock_temp = temp->head_lock_object;
			while(lock_temp!=NULL)
			{
				lock_t* delete_lock = lock_temp;
				lock_temp = lock_temp->same_thread_next;
				int hash_value = delete_lock->hash;
				if(delete_lock->recovery_value != NULL)
				{
					lock_entry* tmp_entry = delete_lock->entry;
					int table_id = tmp_entry->table_id;
					int record_id = tmp_entry->record_id;
					uint64_t root_number = 0;
					internal_page* internal = (internal_page*)malloc(Page_size);
					leaf_page* update_leaf = (leaf_page*)malloc(Page_size);
					header_page* header = (header_page*)malloc(Page_size);
					page_t* page = (page_t*)malloc(Page_size);

					Get_Buffer(table_id,0,0,page);
					page_to_header(page,header);
					root_number = header->root_page;
					free(header);

					Get_Buffer(table_id,root_number,0,page);
					page_to_internal(page,internal);
					while(TRUE)
					{
						if(internal->Is_Leaf==0)
						{
							root_number = internal->one_more_page_number;
							for(int i =0;i<internal->Number_of_Keys;i++)
							{
								if(record_id>=internal->pointers[i].key)
								{
									root_number = internal->pointers[i].page_num;
								}
								else
								{
									break;
								}
							}
							Get_Buffer(table_id,root_number,0,page);
							page_to_internal(page,internal);
						}
						else if(internal->Is_Leaf==1)
						{
							break;
						}
					}
					free(internal);

					page_to_leaf(page,update_leaf);
					for(int i =0;i<update_leaf->Number_of_Keys;i++)
					{
						if(record_id==update_leaf->records[i].key)
						{
							strcpy(update_leaf->records[i].value,delete_lock->recovery_value);
							leaf_to_page(update_leaf,page);
							Set_Buffer(table_id,root_number,1,page);
							break;
						}
					}
					free(update_leaf);
					free(page);
					free(delete_lock->recovery_value);
				}
				lock_t* prev_obj = delete_lock->prev;
				lock_t* next_obj = delete_lock->next;
				if(prev_obj==NULL)
				{
					lock_entry* tmp_entry = delete_lock->entry;
					tmp_entry->next = next_obj;
					if(next_obj!=NULL)
					{
						next_obj->prev = prev_obj;
					}
				}
				else
				{
					prev_obj->next = next_obj;
					if(next_obj!=NULL)
					{
						next_obj->prev = prev_obj;
					}
				}
				free(delete_lock);
				pthread_cond_broadcast(&hash_cond[hash_value]);
			}
			
			xact_node* xact_prev = temp->prev;
			xact_node* xact_next = temp->next;
			if(temp==trx_head)
			{
				trx_head = xact_next;
				if(trx_head == NULL)
				{
					trx_tail==NULL;
				}
				else
				{
					xact_next->prev = NULL;
				}
			}
			else if(temp==trx_tail)
			{
				trx_tail = xact_prev;
				trx_tail->prev = NULL;
			}
			else
			{
				xact_prev->next = xact_next;
				xact_next->prev = xact_prev;
			}
			free(temp);
			break;
		}
		else
		{
			temp = temp->next;
		}
	}
	pthread_mutex_unlock(&deadlock_mutex);
	return 0;
}

int init_lock_table()
{
	lock_hash_table = (lock_entry**)malloc(sizeof(lock_entry*)*hash_size);
	for(int i = 0;i<hash_size;i++)
	{
		lock_hash_table[i] = NULL;
		pthread_cond_init(&hash_cond[i],NULL);
	}

	return 0;
}

lock_t* lock_acquire(int _table_id,int64_t _key,int _trx_id, int _lock_mode)
{
	xact_node* xact_temp = trx_head;
	while(xact_temp!=NULL)
	{
		if(xact_temp->trx_id==_trx_id)
		{
			break;
		}
		xact_temp = xact_temp->next;
	}
	if(xact_temp==NULL)
	{
		return NULL;
	}
	int hash_value = _key%hash_size;

	pthread_mutex_lock(&hash_mutex);
	
	if(lock_hash_table==NULL)
	{
		init_lock_table();
	}
	lock_entry* entry = lock_hash_table[hash_value];
	lock_entry* entry_reference = NULL;
	lock_entry* temp = NULL;

	if(entry == NULL)
	{
		temp = (lock_entry*)malloc(sizeof(lock_entry));
		temp->table_id = _table_id;
		temp->record_id = _key;
		temp->next_entry = NULL;
		temp->next = NULL;
		lock_hash_table[hash_value] = temp;
		entry = lock_hash_table[hash_value];
	}
	else
	{
		while(1)
		{
			if(entry->table_id == _table_id && entry->record_id == _key)
			{
				break;
			}
			else if(entry->next_entry==NULL)
			{
				temp = (lock_entry*)malloc(sizeof(lock_entry));
				temp->table_id = _table_id;
				temp->record_id = _key;
				temp->next_entry = NULL;
				temp->next = NULL;
				entry->next_entry = temp;
				entry = entry->next_entry;
				break;
			}
			else
			{
				entry = entry->next_entry;
			}
		}
	}
	entry_reference = entry;

	lock_t* lock_obj = (lock_t*)malloc(sizeof(lock_t));
	lock_t* tmp_lock_obj = xact_temp->tail_lock_object;
	lock_t* obj_reference = entry_reference->next;

	lock_obj->prev = NULL;
	lock_obj->next = NULL;
	lock_obj->entry = entry_reference;
	lock_obj->same_thread_next = NULL;
	lock_obj->hash = hash_value;
	lock_obj->owner_trx_id = _trx_id;
	lock_obj->lock_mode = _lock_mode;
	lock_obj->can_have_recovery = 1;
	lock_obj->wait = 0;
	lock_obj->deadlock_check = 1;
	lock_obj->recovery_value = NULL;
	if(obj_reference==NULL)
	{
		obj_reference = lock_obj;
		entry_reference->next = obj_reference;
	}
	else
	{
		while(obj_reference->next!=NULL)
		{
			obj_reference = obj_reference->next;
		}
		obj_reference->next = lock_obj;
		lock_obj->prev = obj_reference;
	}
	
	if(tmp_lock_obj==NULL)
	{
		xact_temp->head_lock_object = xact_temp->tail_lock_object = lock_obj;
	}
	else
	{
		tmp_lock_obj->same_thread_next = lock_obj;
		xact_temp->tail_lock_object = lock_obj;
	}
	
	while(1)
	{
		int wait_status = 0;
		if(lock_obj->lock_mode==0)//shared_lock
		{
			lock_t* tmp = lock_obj->prev;
			while(tmp!=NULL)
			{
				if(tmp->owner_trx_id!=lock_obj->owner_trx_id)
				{
					if(tmp->lock_mode==0)
					{
						tmp = tmp->prev;
					}
					else
					{
						wait_status = 1;
						lock_obj->wait = 1;
						break;
					}
				}
				else if(tmp->owner_trx_id==lock_obj->owner_trx_id)
				{
					tmp = tmp->prev;
				}
			}
		}
		else if(lock_obj->lock_mode==1)//exclusive_lock
		{
			lock_t* tmp = lock_obj->prev;
			while(tmp!=NULL)
			{
				if(tmp->owner_trx_id!=lock_obj->owner_trx_id)
				{
					wait_status=1;
					lock_obj->wait = 1;
					break;
				}
				else if(tmp->owner_trx_id==lock_obj->owner_trx_id)
				{
					if(tmp->lock_mode==1)
					{
						lock_obj->can_have_recovery = 0;
					}
					tmp = tmp->prev;
				}
			}
		}

		if(wait_status==0)
		{
			break;
		}
		else
		{
			if(lock_obj->deadlock_check==1)
			{
				lock_t* prev_obj = lock_obj->prev;
				while(prev_obj!=NULL)
				{
					if(prev_obj->owner_trx_id!=lock_obj->owner_trx_id)
					{
						if(deadlock_detection(prev_obj->owner_trx_id,lock_obj->owner_trx_id)!=0)
						{
							trx_abort(lock_obj->owner_trx_id);
							pthread_mutex_unlock(&hash_mutex);
							return NULL;
						}
					}
					prev_obj = prev_obj->prev;
				}
			}
			lock_obj->deadlock_check = 0;
			pthread_cond_wait(&hash_cond[hash_value],&hash_mutex);
		}
	}
	
	lock_obj->wait = 0;
	pthread_mutex_unlock(&hash_mutex);
	return (void*)lock_obj;
}

int lock_release(lock_t* lock_obj)
{
	int hash_value = lock_obj->hash;
	pthread_mutex_lock(&hash_mutex);

	lock_t* prev_obj = lock_obj->prev;
	lock_t* next_obj = lock_obj->next;
	if(lock_obj->lock_mode==0)//shared lock
	{
		lock_t* temp = lock_obj->prev;
		while(temp!=NULL)
		{
			if(temp->owner_trx_id!=lock_obj->owner_trx_id)
			{
				if(temp->lock_mode==0)
				{
					temp = temp->prev;
				}
				else if(temp->lock_mode==1)
				{
					pthread_mutex_unlock(&hash_mutex);
					return -1;
				}
			}
			else
			{
				temp = temp->prev;
			}
		}
		if(prev_obj==NULL)
		{
			lock_entry* tmp_entry = lock_obj->entry;
			tmp_entry->next = next_obj;
			if(next_obj!=NULL)
			{
				next_obj->prev = prev_obj;
			}	
		}
		else
		{
			prev_obj->next = next_obj;
			next_obj->prev = prev_obj;
		}

	}
	else if(lock_obj->lock_mode==1)//exclusive lock
	{
		lock_t* temp = lock_obj->prev;
		while(temp!=NULL)
		{
			if(temp->owner_trx_id!=lock_obj->owner_trx_id)
			{
				pthread_mutex_unlock(&hash_mutex);
				return -1;
			}
			else
			{
				temp = temp->prev;
			}
		}
		if(prev_obj==NULL)
		{
			lock_entry* tmp_entry = lock_obj->entry;
			tmp_entry->next = next_obj;
			if(next_obj!=NULL)
			{
				next_obj->prev = prev_obj;
			}
		}
		else
		{
			prev_obj->next = next_obj;
			if(next_obj!=NULL)
			{
				next_obj->prev = prev_obj;
			}
		}
	}
	if(lock_obj->recovery_value !=NULL)
	{
		free(lock_obj->recovery_value);
	}
	free(lock_obj);

	pthread_cond_broadcast(&hash_cond[hash_value]);
	pthread_mutex_unlock(&hash_mutex);
	return 0;
}

void recovery_value_save(lock_t* lock_obj, char* value)
{
	if(lock_obj->can_have_recovery==1)
	{
		if(lock_obj->recovery_value == NULL)
		{
			lock_obj->recovery_value = (char*)malloc(sizeof(char)*120);
			strcpy(lock_obj->recovery_value,value);
		}
	}
	return ;
}

int deadlock_detection(int object_trx_id,int my_trx_id)
{
	xact_node* temp = trx_head;
	while(temp!=NULL)
	{
		if(temp->trx_id == object_trx_id)
		{
			lock_t* detect_lock = temp->head_lock_object;
			while(detect_lock!=NULL)
			{
				if(detect_lock->wait==1)
				{
					lock_t* temp_object = detect_lock->prev;
					while(temp_object!=NULL)
					{
						if(temp_object->owner_trx_id==my_trx_id)
						{
							return -1; // deadlock
						}
						temp_object = temp_object->prev;
					}
				}
				detect_lock = detect_lock->same_thread_next;
			}
			break;
		}
		else
		{
			temp = temp->next;
		}
	}
	return 0;
}
