#include "lock_table.h"
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>

#define hash_size 100

typedef struct lock_entry // hash_table component struct
{
	int table_id;
	int record_id;
	struct lock_entry* next_entry;
	struct lock_t* next;
}lock_entry;

struct lock_t // lock_object struct
{
	struct lock_t* prev;
	struct lock_t* next;
	struct lock_entry* entry;
	int table_id;
	int64_t record_id;
	int hash;
};

typedef struct lock_t lock_t;

lock_entry** lock_hash_table = NULL; // hash_table
pthread_mutex_t hash_mutex = PTHREAD_MUTEX_INITIALIZER; // mutex
pthread_cond_t hash_cond = PTHREAD_COND_INITIALIZER; // condition variable
//static pthread_mutex_t hash_mutex[hash_size];
//static pthread_cond_t hash_cond[hash_size];

int global_lock_acquire = 0;
int global_lock_release = 0;

int init_lock_table() //make hash_table
{
	lock_hash_table = (lock_entry**)malloc(sizeof(lock_entry*)*hash_size);

	for(int i =0;i<hash_size;i++) 
	{
		lock_hash_table[i] = NULL;
		//pthread_mutex_init(&hash_mutex[i],NULL);
		//pthread_cond_init(&hash_cond[i],NULL);
	}

	return 0;
}

lock_t* lock_acquire(int _table_id, int64_t _key)
{
	//printf("Acquire %d   %d\n",_table_id,_key);
	int hash_value = _key % hash_size; // get hash_key

	pthread_mutex_lock(&hash_mutex); // lock mutex

	lock_entry* entry = lock_hash_table[hash_value]; // go component of hash_table
	lock_entry* entry_reference = NULL;
	lock_entry* temp = NULL;

	if(entry==NULL) // if no hash_table component here, we make hash_table_component
	{
		temp = (lock_entry*)malloc(sizeof(lock_entry));
		temp->table_id = _table_id;
		temp->record_id = _key;
		temp->next_entry = NULL;
		temp->next =NULL;
		lock_hash_table[hash_value] = temp;
		entry = lock_hash_table[hash_value];
	}
	else
	{
		while(1)
		{
			if(entry->table_id == _table_id && entry->record_id == _key) // find right hash_table_component
			{
				break;
			}
			else if(entry->next_entry==NULL) // can't find right component in hash_table, we make it
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
			else // go next component in hash_table
			{
				entry = entry->next_entry;
			}
		}
	}
	entry_reference = entry; // reference hash_table_component definition

	lock_t* lock_obj = (lock_t*)malloc(sizeof(lock_t)); // allocate lock_object
	lock_obj->table_id = _table_id;
	lock_obj->record_id = _key;
	lock_obj->hash = hash_value;
	lock_obj->prev = NULL;
	lock_obj->next = NULL;
	lock_obj->entry = entry_reference;
	

	lock_t* obj_reference = entry_reference->next; // get lock_object in hash_table_component
	if(obj_reference==NULL) // no lock_object in hash_table_component
	{
		obj_reference = lock_obj; 
		entry_reference->next = obj_reference; // allocated lock_object is first object in hash_table_component
	}
	else
	{
		while(obj_reference->next!=NULL) 
		{
			obj_reference = obj_reference->next; // get last lock_object in hash_table_component
		}
		obj_reference->next = lock_obj;
		lock_obj->prev = obj_reference; // allocated lock_object is last object in hash_table_component
	}

	while(lock_obj->prev!=NULL) // allocated lock_object isn't first lock object in hash_table_component
	{
		pthread_cond_wait(&hash_cond,&hash_mutex); // Wait
	}

	pthread_mutex_unlock(&hash_mutex);//unlock mutex
	return (void*)lock_obj;
}

int lock_release(lock_t* lock_obj)
{
	//printf("Release %d   %d\n",lock_obj->table_id,lock_obj->record_id);
	
	int hash_value = lock_obj->hash;

	pthread_mutex_lock(&hash_mutex); //lock mutex
	
	lock_entry* temp = lock_obj->entry; // get hash_table_component that object has
	if(temp->next==lock_obj)
	{
		lock_t* obj_temp = lock_obj->next;
		temp->next = obj_temp; // first lock object of hash_table_component change that next lock object
		if(obj_temp!=NULL)
		{
			//printf("NOT NULL\n");
			obj_temp->prev = NULL; // set next lock object is first lock object
		}
	}
	else
	{
		return -1;
	}

	free(lock_obj); // free parameter lock_object
	
	pthread_cond_broadcast(&hash_cond); // Awake mutex that have conditional variable(hash_cond)
	pthread_mutex_unlock(&hash_mutex); // unlock mutex
	
	return 0;
}
