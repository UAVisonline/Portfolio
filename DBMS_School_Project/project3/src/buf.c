#include "buf.h"
#include "file.h"

buffer_node* buf_head = NULL;
buffer_node* buf_tail = NULL;
//LRU double-linked-list
int buffer_size = 0;

int init_db(int size) // LibraryAPI_create buffer list
{
	//int test_id = 0;
	if(buf_head!=NULL) // if buffer list is exist,
	{
		return -1; // can't create buffer list
	}
	buffer_size = size;
	for(int i =0;i<size;i++)
	{
		buffer_node* temp = (buffer_node*)malloc(sizeof(buffer_node));
		temp->frame = NULL;
		temp->table_id = 0;
		//temp->table_id = test_id++;
		temp->page_number = 0;
		temp->is_dirty = 0;
		temp->pinned = 0;
		temp->prev = NULL;
		temp->next = NULL;
		// make new buffer
		if(buf_head==NULL) // empty buffer list
		{
			buf_head = buf_tail = temp;
		}
		else // not empty buffer list->new buffer is tail
		{
			buffer_node* tmp = buf_tail;
			tmp->next = temp;
			temp->prev = tmp;
			buf_tail = temp;
		}
	}
	return 0;
}

void free_db() // free memory in buffer_list
{
	if(buf_head==NULL)
	{
		return;
	}
	buffer_node* tmp = buf_head;
	while(tmp->next!=NULL)
	{
		buffer_node* temp = tmp;
		tmp = tmp->next;
		if(temp->frame!=NULL)
		{
			free(temp->frame);
		}
		free(temp);
	}
	
	if(tmp->frame!=NULL)
	{
		free(tmp->frame);
	}
	free(tmp);
	buffer_size = 0;
	buf_head = NULL;
	buf_tail = NULL;
}

void Get_Buffer(int t_id, pagenum_t p_number, int pin, page_t* dest)//Get page in buffer_list(buffer version of file_read_page function)
{
	int can_t_find = 0;
	buffer_node* tmp = buf_head;
	buffer_node* replace;
	while(tmp->next!=NULL) // find buffer that t_id,p_number(head_to_tail)
	{
		if(tmp->frame==NULL) // this buffer frame is NULL->all of next buffer frame is also NULL
		{
			can_t_find = 1; // no t_id,p_number page in buffer_list
			break;
		}
		if(tmp->table_id==t_id && tmp->page_number==p_number) // find t_id,p_number page in buffer list
		{
			dest->next_free_page_number = tmp->frame->next_free_page_number;
			for(int i =0;i<511;i++)
			{
				dest->dummy[i] = tmp->frame->dummy[i];
			}

			if(pin==1)
			{
				tmp->pinned = 1;
			}
			break;
		}
		tmp = tmp->next;

		if(tmp->next==NULL) // end of buffer list
		{
			if(tmp->frame==NULL)
			{
				can_t_find = 1; // no t_id,p_number page in buffer_list
				break;
			}

			if(tmp->table_id==t_id && tmp->page_number==p_number) // tail is t_id,p_number page
			{
				dest->next_free_page_number = tmp->frame->next_free_page_number;
				for(int i =0;i<511;i++)
				{
					dest->dummy[i] = tmp->frame->dummy[i];
				}

				if(pin==1)
				{
					tmp->pinned = 1;
				}
				/*
				buf_tail = tmp->prev;
				buf_tail->next = NULL;
				tmp->next = buf_head;
				buf_head->prev = tmp;
				buf_head = tmp;
				buf_head->prev = NULL;
				*/
				break;
			}
			else
			{
				can_t_find = 1;
				break;
			}
		}
	}

	if(can_t_find == 0)//using buffer go to Head
	{
		if(tmp!=buf_head && tmp!=buf_tail)
		{
                	tmp->prev->next = tmp->next;
                	tmp->next->prev = tmp->prev;
                	tmp->next = buf_head;
                	buf_head->prev = tmp;
                	buf_head = tmp;
                	buf_head->prev = NULL;
		}
		else if(tmp == buf_tail)
		{
			buf_tail = tmp->prev;
			buf_tail->next = NULL;
			tmp->next = buf_head;
			buf_head->prev = tmp;
			buf_head = tmp;
			buf_head->prev = NULL;
		}

	}


	if(can_t_find == 1) // can't find buffer that t_id,p_number
	{
		replace = buf_tail; // we go to buf_tail to find buffer(empty or unpinned)

		while(replace->prev!=NULL)
		{
			if(replace->frame==NULL) // we find empty buffer
			{
				replace->frame = (page_t*)malloc(Page_size);
				file_read_page(p_number,replace->frame); // read page in file->buffer has information of p_number(call t_id use pathname)
				can_t_find = 2; // we change buffer
				break;
			}
			else if(replace->frame!=NULL)
			{
				if(replace->pinned==0) // find unpinned buffer
				{
					if(replace->is_dirty==1) // buf buffer information changed
					{
						pathname_change_temporary(replace->table_id); // change pathname temporary to write page of buffer
						file_write_page(replace->page_number,replace->frame);
						pathname_change_current(); // change pathname originally
					}
					file_read_page(p_number,replace->frame); //read page in file->buffer has information of p_number(call t_id use pathname)
					can_t_find = 2; // we change buffer
					break;
				}
			}
			replace = replace->prev;

			if(replace->prev==NULL) // we reach head
			{
				if(replace->frame==NULL)
				{
					replace->frame = (page_t*)malloc(Page_size);
					file_read_page(p_number,replace->frame);
					can_t_find = 2;
					break;
				}
				else if(replace->frame!=NULL)
				{
					if(replace->pinned==0)
					{
						if(replace->is_dirty==1)
						{
							pathname_change_temporary(replace->table_id);
							file_write_page(replace->page_number,replace->frame);
							pathname_change_current();
						}
						file_read_page(p_number,replace->frame);
						can_t_find = 2;
						break;
					}
				}
			}
		}
	}

	if(can_t_find==2) // find old buffer to change and go to Head
	{
		replace->table_id = t_id;
		replace->page_number = p_number;
		replace->is_dirty = 0;
		if(pin==1) replace->pinned = 1;

		dest->next_free_page_number = replace->frame->next_free_page_number;
		for(int i =0;i<511;i++)
		{
			dest->dummy[i] = replace->frame->dummy[i];
		}
		
		if(replace==buf_tail)
		{
			buf_tail = replace->prev;
			buf_tail->next = NULL;
			replace->next = buf_head;
			buf_head->prev = replace;
			buf_head = replace;
			buf_head->prev = NULL;
		}
		else if(replace!=buf_tail && replace!=buf_head)
		{
			replace->prev->next = replace->next;
			replace->next->prev = replace->prev;
			replace->next = buf_head;
			buf_head->prev = replace;
			buf_head = replace;
			buf_head->prev = NULL;
		}
	}
	else if(can_t_find==1) // All buffer is using, we create temporary page and use it to make replica of page
	{
		page_t* temp_page = (page_t*)malloc(Page_size);
		file_read_page(p_number,temp_page);
		
		dest->next_free_page_number = temp_page->next_free_page_number;
		for(int i =0;i<511;i++)
		{
			dest->dummy[i] = temp_page->dummy[i];
		}
		free(temp_page);
	}
}

// Set_Buffer_function structure similar Get_Buffer_function
void Set_Buffer(int t_id, pagenum_t p_number, int pin, page_t* src) //Set page in buffer_list(buffer version of file_write_page function)
{
	int can_t_find = 0;
	buffer_node* tmp = buf_head;
	buffer_node* replace;

	while(tmp->next!=NULL) // find buffer that t_id,p_number
	{
		if(tmp->frame==NULL)
		{
			can_t_find = 1;
			break;
		}
		if(tmp->table_id==t_id && tmp->page_number==p_number)
		{
			tmp->frame->next_free_page_number = src->next_free_page_number;
			for(int i =0;i<511;i++)
			{
				tmp->frame->dummy[i] = src->dummy[i];
			}
			tmp->is_dirty = 1;
			if(pin==1)
			{
				tmp->pinned = 1;
			}
			break;
		}
		tmp = tmp->next;

		if(tmp->next==NULL)
		{
			if(tmp->frame==NULL)
			{
				can_t_find = 1;
				break;
			}

			if(tmp->table_id==t_id && tmp->page_number==p_number)
			{
				tmp->frame->next_free_page_number = src->next_free_page_number;
				for(int i =0;i<511;i++)
				{
					tmp->frame->dummy[i] = src->dummy[i];
				}
				tmp->is_dirty = 1;
				if(pin==1)
				{
					tmp->pinned = 1;
				}

				buf_tail = tmp->prev;
				buf_tail->next = NULL;
				tmp->next = buf_head;
				buf_head->prev = tmp;
				buf_head = tmp;
				buf_head->prev = NULL;
				break;
			}
			else
			{
				can_t_find = 1;
				break;
			}
		}
	}

	if(can_t_find == 0)//using buffer go to Head
	{
		if(tmp!=buf_head && tmp!=buf_tail)
		{
                	tmp->prev->next = tmp->next;
                	tmp->next->prev = tmp->prev;
                	tmp->next = buf_head;
                	buf_head->prev = tmp;
                	buf_head = tmp;
                	buf_head->prev = NULL;
		}
		else if(tmp == buf_tail)
		{
			buf_tail = tmp->prev;
			buf_tail->next = NULL;
			tmp->next = buf_head;
			buf_head->prev = tmp;
			buf_head = tmp;
			buf_head->prev = NULL;
		}

	}


	if(can_t_find == 1) // can't find buffer that t_id,p_number
	{
		replace = buf_tail;

		while(replace->prev!=NULL)
		{
			if(replace->frame==NULL)
			{
				replace->frame = (page_t*)malloc(Page_size);
				can_t_find = 2;
				break;
			}
			else if(replace->frame!=NULL)
			{
				if(replace->pinned==0)
				{
					if(replace->is_dirty==1)
					{
						pathname_change_temporary(replace->table_id);
						file_write_page(replace->page_number,replace->frame);
						pathname_change_current();
					}
					can_t_find = 2;
					break;
				}
			}
			replace = replace->prev;

			if(replace->prev==NULL)
			{
				if(replace->frame==NULL)
				{
					replace->frame = (page_t*)malloc(Page_size);
					can_t_find = 2;
					break;
				}
				else if(replace->frame!=NULL)
				{
					if(replace->pinned==0)
					{
						if(replace->is_dirty==1)
						{
							pathname_change_temporary(replace->table_id);
							file_write_page(replace->page_number,replace->frame);
							pathname_change_current();
						}
						can_t_find = 2;
						break;
					}
				}
			}
		}
	}

	if(can_t_find==2) // find old buffer to change and go to Head
	{
		replace->table_id = t_id;
		replace->page_number = p_number;
		replace->is_dirty = 1;
		if(pin==1) replace->pinned = 1;

		replace->frame->next_free_page_number = src->next_free_page_number;
		for(int i =0;i<511;i++)
		{
			replace->frame->dummy[i] = src->dummy[i];
		}

		if(replace==buf_tail)
		{
			buf_tail = replace->prev;
			buf_tail->next = NULL;
			replace->next = buf_head;
			buf_head->prev = replace;
			buf_head = replace;
			buf_head->prev = NULL;
		}
		else if(replace!=buf_tail && replace!=buf_head)
		{
			replace->prev->next = replace->next;
			replace->next->prev = replace->prev;
			replace->next = buf_head;
			buf_head->prev = replace;
			buf_head = replace;
			buf_head->prev = NULL;
		}
	}
	else if(can_t_find==1) // All buffer is using
	{
		page_t* temp_page = (page_t*)malloc(Page_size);
		file_read_page(p_number,temp_page);
		
		temp_page->next_free_page_number = src->next_free_page_number;
		for(int i =0;i<511;i++)
		{
			temp_page->dummy[i] = src->dummy[i];
		}
		file_write_page(p_number,temp_page);
		free(temp_page);
	}
}

void Unpin_Buffer() //Unpin all of buffer in buffer_list
{
	buffer_node* tmp = buf_head;
	if(tmp!=NULL)
	{
		while(tmp->next!=NULL)
		{
			tmp->pinned = 0;
			tmp = tmp->next;
		}
		tmp->pinned = 0;
	}
}

int close_table(int t_id) //LibraryAPI_close table
{
	int head_check = 0; // value that check close buffer is buf_head?

	if(close_table_in_node(t_id)==-1) // this table is already close, and change pathname t_id table
	{
		return -1;
	}
	else
	{
		buffer_node* temp = buf_head;
		if(temp==NULL)
		{
			return -1;
		}
		while(1)
		{
			if(temp->frame==NULL) //buffer page is NULL->all of next buffer page is NULL
			{
				break;
			}

			if(temp->table_id==t_id) // find buffer to close
			{
				if(temp->is_dirty==1) // value changed?
				{
					file_write_page(temp->page_number,temp->frame);//write file
				}
				temp->table_id = 0;
				temp->page_number = 0;
				temp->is_dirty = 0;
				temp->pinned = 0;
				free(temp->frame);
				temp->frame=NULL;
				// init to default status

				if(temp==buf_head) // closed buffer is head
				{
					head_check = 1; // check it

					temp->prev = buf_tail;
					temp->next->prev = NULL;
					buf_head = temp->next;
					temp->next = NULL;
					buf_tail->next = temp;
					buf_tail = temp;
					// head go to tail
					temp = buf_head;
					// start new buf_head
				}
				else if(temp!=buf_head && temp!=buf_tail) // closed buffer is not tail
				{
					buffer_node* temp_back = temp->prev;

					temp->prev->next = temp->next;
					temp->next->prev = temp->prev;
					buf_tail->next = temp;
					temp->prev = buf_tail;
					temp->next = NULL;
					buf_tail = temp;
					// this buffer go to tail
					temp = temp_back;
					// start back of closed_buffer
				}
				else if(temp==buf_tail) // closed buffer is tail->next buffer isn't here
				{
					buffer_node* temp_back = temp->prev;
					temp = temp_back;
					break; // function close
				}
			}
			temp = temp->next; // go to next_buffer
			if(head_check==1) // but closed_buffer is head, we can't check new buf_head
			{
				head_check = 0;
				temp = buf_head; // so we go buf_head
			}

		}
	}
	return 0;
}

int shutdown_db()//LibraryAPI_shutdown table
{
	if(buf_head==NULL)
	{
		return -1;
	}
	else
	{
		for(int i =1;i<=10;i++)
		{
			close_table(i);
		}
	}
	free_db();
	return 0;
}

void Print_Buffer_Status() // test function->print information of all buffer in Buffer list
{
	int buf_id = 1;
	printf("buffer_size : %d\n\n",buffer_size);
	buffer_node* tmp = buf_head;
	if(tmp!=NULL)
	{
		while(tmp->next!=NULL)
		{
			printf("%d buffer\n",buf_id);
			printf("table_id, page_number: %d  %ld\n",tmp->table_id,tmp->page_number);
			printf("is dirty:%d\npinned:%d\n\n",tmp->is_dirty,tmp->pinned);
			tmp = tmp->next;
			buf_id++;
		}
		printf("%d buffer\n",buf_id);
		printf("table_id, page_number: %d  %ld\n",tmp->table_id,tmp->page_number);
		printf("is dirty:%d\npinned:%d\n",tmp->is_dirty,tmp->pinned);
	}
}
