#include "page.h"

table_node* table_head = NULL;
char* pathname = NULL;
int current_table_id = 0;

//functions about translate page<->internal,leaf,header

void leaf_to_page(leaf_page* src,page_t* dest)
{
	leaf_page* leaf = malloc(Page_size);
	page_t* page;
	void* ptr;

	leaf->parent_page_num = src->parent_page_num;
	leaf->Is_Leaf = src->Is_Leaf;
	leaf->Number_of_Keys = src->Number_of_Keys;
	for(int i =0;i<13;i++) leaf->dummy[i] = src->dummy[i];
	leaf->right_sibling_number = src->right_sibling_number;
	for(int i =0;i<leaf->Number_of_Keys;i++)
	{
		leaf->records[i].key = src->records[i].key;
		strcpy(leaf->records[i].value,src->records[i].value);
	}

	ptr = (void*)leaf;
	page = (page_t*)ptr;

	dest->next_free_page_number = page->next_free_page_number;
	for(int i =0;i<511;i++) dest->dummy[i] = page->dummy[i];

	free(leaf);
}

void page_to_leaf(page_t* src,leaf_page* dest)
{
	leaf_page* leaf;
	page_t* page = malloc(Page_size);
	void* ptr;

	page->next_free_page_number = src->next_free_page_number;
	for(int i =0;i<511;i++) page->dummy[i] = src->dummy[i];

	ptr = (void*)page;
	leaf = (leaf_page*)ptr;

	dest->parent_page_num = leaf->parent_page_num;
	dest->Is_Leaf = leaf->Is_Leaf;
	dest->Number_of_Keys=leaf->Number_of_Keys;
	for(int i =0;i<13;i++) dest->dummy[i] = leaf->dummy[i];
	dest->right_sibling_number = leaf->right_sibling_number;
	for(int i =0;i<dest->Number_of_Keys;i++)
	{
		dest->records[i].key = leaf->records[i].key;
		strcpy(dest->records[i].value,leaf->records[i].value);
	}
	free(page);

}

void internal_to_page(internal_page* src,page_t* dest)
{
	internal_page* internal = malloc(Page_size);
	page_t* page;
	void* ptr;

	internal->parent_page_num=src->parent_page_num;
	internal->Is_Leaf = src->Is_Leaf;
	internal->Number_of_Keys = src->Number_of_Keys;
	for(int i =0;i<13;i++) internal->dummy[i] = src->dummy[i];
	internal->one_more_page_number = src->one_more_page_number;
	for(int i =0;i<internal->Number_of_Keys;i++)
	{
		internal->pointers[i].key = src->pointers[i].key;
		internal->pointers[i].page_num = src->pointers[i].page_num;
	}

	ptr = (void*)internal;
	page = (page_t*)ptr;

	dest->next_free_page_number = page->next_free_page_number;
	for(int i =0;i<511;i++)
	{
		dest->dummy[i] = page->dummy[i];
	}

	free(internal);
}

void page_to_internal(page_t* src, internal_page* dest)
{
	page_t* page = malloc(Page_size);
	internal_page* internal;
	void* ptr;

	page->next_free_page_number = src->next_free_page_number;
	for(int i =0;i<511;i++)
	{
		page->dummy[i] = src->dummy[i];
	}

	ptr = (void*)page;
	internal = (internal_page*)ptr;

	dest->parent_page_num = internal->parent_page_num;
	dest->Is_Leaf = internal->Is_Leaf;
	dest->Number_of_Keys = internal->Number_of_Keys;
	for(int i =0;i<13;i++) dest->dummy[i] = internal->dummy[i];
	dest->one_more_page_number = internal->one_more_page_number;
	for(int i =0;i<dest->Number_of_Keys;i++)
	{
		dest->pointers[i].key = internal->pointers[i].key;
		dest->pointers[i].page_num = internal->pointers[i].page_num;
	}

	free(page);
}

void header_to_page(header_page* src, page_t* dest)
{
	header_page* header = malloc(Page_size);
	page_t* page;
	void* ptr;

	header->next_free_page = src->next_free_page;
	header->root_page = src->root_page;
	header->Num_of_pages = src->Num_of_pages;
	for(int i =0;i<509;i++)
	{
		header->dummy[i] = src->dummy[i];
	}

	ptr = (void*)header;
	page = (page_t*)ptr;

	dest->next_free_page_number = page->next_free_page_number;
	for(int i =0;i<511;i++) dest->dummy[i] = page->dummy[i];

	free(header);
}

void page_to_header(page_t* src,header_page* dest)
{
	header_page* header;
	page_t* page = malloc(Page_size);
	void* ptr;

	page->next_free_page_number = src->next_free_page_number;
	for(int i =0;i<511;i++) page->dummy[i] = src->dummy[i];

	ptr = (void*)page;
	header = (header_page*)ptr;

	dest->next_free_page = header->next_free_page;
	dest->root_page = header->root_page;
	dest->Num_of_pages = header->Num_of_pages;
	for(int i =0;i<509;i++)
	{
		dest->dummy[i] = header->dummy[i];
	}

	free(page);
}

int table_open_check(int id) // return status that you can use table(parameter_id)
{
	table_node* curr = table_head;
	if(curr==NULL)
	{
		return -1;	
	}
	else
	{
		while(curr->next!=NULL)
		{
			if(id==curr->table_id)
			{
				if(curr->open==0)
				{
					return -1;
				}
				strcpy(pathname,curr->table_name); // pathname value_change to table(id)_name and we can open file using pathname value
				current_table_id = id;
				//printf("%s\n",pathname);
				break;
			}
			curr = curr->next;
		}
		if(curr->next==NULL)
		{
			if(id==curr->table_id)
			{
				if(curr->open==0)
				{
					return -1;
				}
				strcpy(pathname,curr->table_name); // pathname value_change to table(id)_name and we can open file using pathname value
				current_table_id = id;
				//printf("%s\n",pathname);
			}
			else
			{
				return -1;
			}
		}		
	}	

	int fd;
	fd = open(pathname,O_RDWR|O_SYNC);
	if(fd==-1) // No table file
	{	
		close(fd);
		return -1;
	}
	close(fd);
	return 0;
}

void exit_table_information()
{
	while(table_head!=NULL)
	{
		table_node* prev = table_head;
		table_head = table_head->next;
		free(prev->table_name);
		free(prev);
	}
	free(pathname);
}                   

void pathname_change_temporary(int t_id) // When write buffer to file that not current_table_id, we need to change pathname to open file(parameter t_id)
{
	table_node* curr = table_head;
        while(curr->next!=NULL)
        {
        	if(t_id==curr->table_id)
        	{
                	strcpy(pathname,curr->table_name);
                	break;
                }
                curr = curr->next;
        }
        if(curr->next==NULL)
        {
                if(t_id==curr->table_id)
                {
                        strcpy(pathname,curr->table_name);
                }
       }
}

void pathname_change_current() // End writing buffer to file that not current_table_id, change pathname_current_table_id
{
	table_node* curr = table_head;
        while(curr->next!=NULL)
        {
                if(current_table_id==curr->table_id)
                {
                        strcpy(pathname,curr->table_name);
                        break;
                }
                curr = curr->next;
        }
        if(curr->next==NULL)
        {
                if(current_table_id==curr->table_id)
                {
                        strcpy(pathname,curr->table_name);
                }
       }
}

int close_table_in_node(int t_id) // for close table API, close table->we need to change table_status->this table is close
{
	table_node* curr = table_head;
       	while(curr->next!=NULL)
        {
                if(t_id==curr->table_id)
                {
			if(curr->open==0)
			{
				return -1;
			}
                        strcpy(pathname,curr->table_name);
			curr->open = 0;
                        break;
                }
                curr = curr->next;
        }
        if(curr->next==NULL)
        {
                if(t_id==curr->table_id)
                {
			if(curr->open==0)
			{
				return -1;
			}
                        strcpy(pathname,curr->table_name);
			curr->open = 0;
                }
        }

	return 0;
}//we don't change pathname because we use insert/delete/find API we check current_table_id and change pathname in check_function

