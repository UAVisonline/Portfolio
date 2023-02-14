#include "file.h"
#include "buf.h"

extern table_node* table_head; // table list
extern char* pathname; // char*_value_using_openfile
extern int current_table_id; // table_id_currently_in_use

pagenum_t file_alloc_page()
{
	int ret_number, fd;
	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	Get_Buffer(current_table_id,0,1,head_page); //Read Header_page(table_id)
	page_to_header(head_page,header);

	ret_number = header->next_free_page;
	if(ret_number!=0)
	{
		page_t* page = (page_t*)malloc(Page_size);
		Get_Buffer(current_table_id,ret_number,0,page); //Read page(table_id) in Buffer
		header->next_free_page = page->next_free_page_number;
		free(page);
	}
	else
	{
		page_t* page = (page_t*)malloc(Page_size);
		page->next_free_page_number = 0;
		for(int i =0;i<511;i++) page->dummy[i] = 0;
		//new page created_empty(not has trash_value)
		
		fd = open(pathname, O_RDWR| O_SYNC );
		if(fd == -1)
		{
			perror("Error");
			exit(-1);
		}		
		lseek(fd,0,SEEK_END);
		write(fd,page,Page_size); // Extend file_insert_new_page_information
		fsync(fd);
		close(fd);
		free(page);

		ret_number = header->Num_of_pages;
		header->Num_of_pages++;
	}
	header_to_page(header,head_page);
	Set_Buffer(current_table_id,0,1,head_page); // Write page(table_id) in Buffer
	free(head_page);
	free(header);
	//Unpin_Buffer(); // Unpin_in_Buffer(pinned page)
	return ret_number;
}

void file_free_page(pagenum_t pagenum)
{
	int previous_page_num;
	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	Get_Buffer(current_table_id,0,1,head_page); //Read header(table_id) in Buffer
	page_to_header(head_page,header);

	if(pagenum>=header->Num_of_pages)
	{
		printf("Error file_free_page\nArgument pagenum is bigger than page number in file\n");
		free(header);
		//Unpin_Buffer();
		return;
	}

	page_t* free_page = (page_t*)malloc(Page_size);
	free_page->next_free_page_number = 0;
	for(int i =0;i<511;i++)
	{
		free_page->dummy[i] = 0;
	}
	//create_new empty page
	Set_Buffer(current_table_id,pagenum,1,free_page);//Write page_deleted(table_id) in Buffer(empty page insert)
	free(free_page); 
	
	if(header->next_free_page==0)
	{
		header->next_free_page = pagenum;
		header_to_page(header,head_page);
		Set_Buffer(current_table_id,0,1,head_page);//Write header_page(table_id) in Buffer
	}	
	else
	{
		previous_page_num = header->next_free_page;
		page_t* dummy = (page_t*)malloc(Page_size);
		Get_Buffer(current_table_id,previous_page_num,0,dummy); // Read free_page(table_id) in Buffer
		while(dummy->next_free_page_number!=0)
		{
			previous_page_num = dummy->next_free_page_number;
			Get_Buffer(current_table_id,previous_page_num,0,dummy); // Read free_page(table_id) in Buffer
		}
		dummy->next_free_page_number = pagenum; // change free_page->next_free_page_value to new_empty_page
		Set_Buffer(current_table_id,previous_page_num,1,dummy); // Write free_page(table_id) that changed information in Buffer
		free(dummy);
	}
	free(head_page);
	free(header);
	//Unpin_Buffer(); // Unpin in Buffer pages
}

void file_read_page(pagenum_t pagenum,page_t* dest)
{
	long int size = 0;
	int fd;

	fd = open(pathname, O_RDWR | O_SYNC );
	if(fd==-1)
	{
		printf("Error file open\nfunction : file_read_page\n");
		exit(-1);
	}
	page_t* tmp = (page_t*)malloc(Page_size);

	lseek(fd,pagenum*Page_size,SEEK_SET);
	read(fd,tmp,Page_size);

	dest->next_free_page_number = tmp->next_free_page_number;

	for(int i=0;i<511;i++)
	{
		dest->dummy[i] = tmp->dummy[i];
	}

	free(tmp);
	close(fd);
}

void file_write_page(pagenum_t pagenum,const page_t* src)
{
	long int size = 0;
	int fd;

	fd = open(pathname, O_RDWR | O_SYNC);
	if(fd==-1)
	{
		printf("Error file open\nfunction : file_write_page\n");
		exit(-1);
	}
	page_t* tmp = (page_t*)malloc(Page_size);
	tmp->next_free_page_number = src->next_free_page_number;
	for(int i=0;i<511;i++)
	{
		tmp->dummy[i] = src->dummy[i];
	}
	lseek(fd,pagenum*Page_size,SEEK_SET);
	write(fd,tmp,Page_size);
	fsync(fd);
	free(tmp);
	close(fd);

}

int open_table(char* file_pathname)//LibraryAPI_open_table
{
	int id = 1;
	int fd;
	void* ptr;
	table_node* curr;
	if(table_head==NULL) // No table in table_list
	{
		table_head = (table_node*)malloc(sizeof(table_node));
		table_head->table_name = NULL;
		table_head->next = NULL;
	}
	curr = table_head;

	if(pathname==NULL)
	{
		pathname = (char*)malloc(30);
	}
	strcpy(pathname,file_pathname);

	while(1)
	{
		if(curr->table_name==NULL) //No table that named file_pathname 
		{
			curr->table_name = (char*)malloc(30);
			strcpy(curr->table_name,file_pathname); //set table_name
			curr->table_id = id; // set table_id
			curr->open = 1; // status_open
			current_table_id = curr->table_id;
			break;
		}
		else if(curr->table_name!=NULL)
		{
			if(strcmp(curr->table_name,file_pathname)==0) // Exist table_named_file_pathname
			{
				id = curr->table_id;
				curr->open = 1;
				current_table_id = curr->table_id;
				break;
			}
			else
			{
				if(curr->next!=NULL)
				{
					curr = curr->next; //search next table
				}
				else if(curr->next==NULL) //last table and can't find table_named file_pathname
				{
					curr->next = (table_node*)malloc(sizeof(table_node));
					curr = curr->next;
					curr->table_name = NULL;
					curr->next = NULL;
				}
				id++;
			}
		}
	}

	fd = open(pathname, O_RDWR | O_SYNC );
	if(fd == -1)// new table
	{
		fd = open(pathname, O_RDWR | O_CREAT | O_SYNC );		
		if(fd == -1)
		{
			perror("file open error");
			return -1;
		}
		header_page* header = (header_page*)malloc(Page_size);
		header->next_free_page = 0;
		header->root_page = 0;
		header->Num_of_pages = 1;
		ptr = (void*)header;
		page_t* page = (page_t*)ptr;
		lseek(fd,0,SEEK_SET);
		//header_page insert in new table
		if(write(fd,page,Page_size)==-1)
		{
			perror("error");
			free(header);
			return -1;
		}
		fsync(fd);
		free(header);		
	}
	close(fd);	

	return id;
}


/*void read_header(header_page* tmp)
{
	int fd;
	void* ptr;
	fd = open(pathname, O_RDWR | O_SYNC );
	if(fd == -1)
	{
		printf("Error Open File\nread_header Function\n");
		exit(-1);
	}
	page_t* page = (page_t*)malloc(Page_size);
       	lseek(fd,0,SEEK_SET);
	read(fd,page,Page_size);
	ptr = (void*)page;
	header_page* head = (header_page*)ptr;
	tmp->next_free_page = head->next_free_page;
	tmp->root_page = head->root_page;
	tmp->Num_of_pages = head->Num_of_pages;
	free(page);
	close(fd);
}

void write_header(header_page* header)
{
	int fd;
	void* ptr;
	fd = open(pathname, O_RDWR | O_SYNC );
	if(fd==-1)
	{
		printf("Error Open File\nwrite_header Function\n");
		exit(-1);
	}
	header_page* tmp = (header_page*)malloc(Page_size);
       	tmp->next_free_page = header->next_free_page;
	tmp->root_page = header->root_page;
	tmp->Num_of_pages = header->Num_of_pages;
	ptr = (void*)header;
	page_t* page = (page_t*)ptr;
	lseek(fd,0,SEEK_SET);
	write(fd,page,Page_size);
	fsync(fd);
	free(tmp);
	close(fd);
}*/


