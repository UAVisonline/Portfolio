#include "file.h"

table_node* head = NULL;
char* pathname = NULL;

pagenum_t file_alloc_page()
{
	int ret_number, fd;
	header_page* header = (header_page*)malloc(Page_size); 
	read_header(header);
	ret_number = header->next_free_page;
	if(ret_number!=0)
	{
		page_t* page = (page_t*)malloc(Page_size);
		file_read_page(ret_number,page);
		header->next_free_page = page->next_free_page_number;
		write_header(header);
		free(page);
	}
	else
	{
		page_t* page = (page_t*)malloc(Page_size);
		page->next_free_page_number = 0;
		fd = open(pathname, O_RDWR| O_SYNC );
		if(fd == -1)
		{
			perror("Error");
			exit(-1);
		}		
		lseek(fd,0,SEEK_END);
		write(fd,page,Page_size);
		fsync(fd);
		close(fd);
		free(page);

		ret_number = header->Num_of_pages;
		header->Num_of_pages++;
		write_header(header);
	}
	free(header);
	return ret_number;
}

void file_free_page(pagenum_t pagenum)
{
	int previous_page_num;
	header_page* header = (header_page*)malloc(Page_size);
	read_header(header);
	if(pagenum>=header->Num_of_pages)
	{
		printf("Error file_free_page\nArgument pagenum is bigger than page number in file\n");
		free(header);
		return;
	}

	page_t* free_page = (page_t*)malloc(Page_size);
	free_page->next_free_page_number = 0;
	for(int i =0;i<511;i++)
	{
		free_page->dummy[i] = 0;
	}
	file_write_page(pagenum,free_page);
	free(free_page); 
	
	if(header->next_free_page==0)
	{
		header->next_free_page = pagenum;
		write_header(header);
	}	
	else
	{
		previous_page_num = header->next_free_page;
		page_t* dummy = (page_t*)malloc(Page_size);
		file_read_page(previous_page_num,dummy);
		while(dummy->next_free_page_number!=0)
		{
			previous_page_num = dummy->next_free_page_number;
			file_read_page(previous_page_num,dummy);
		}
		dummy->next_free_page_number = pagenum;
		file_write_page(previous_page_num,dummy);
		free(dummy);
	}
	free(header);
}

void file_read_page(pagenum_t pagenum,page_t* dest)
{
	long int size = 0;
	int fd;
	header_page* header = (header_page*)malloc(Page_size);
	read_header(header);
	if(header->Num_of_pages<=pagenum)
	{
		printf("Error read_page\nargument pagenumber is bigger than file in page\n");
		printf("Error Page_num %ld\n",pagenum);
		free(header);
		exit(-1);
	}
	free(header);

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
	header_page* header = (header_page*)malloc(Page_size);
	read_header(header);
	if(header->Num_of_pages<=pagenum)
	{
		printf("Error write_page\nargument pagenumber is bigger than file in page\n");
		printf("Error Page_num %ld\n",pagenum);
		free(header);
		exit(-1);
	}
	free(header);

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

int open_table(char* file_pathname)
{
	int id = 0;
	int fd;
	void* ptr;
	table_node* curr;
	if(head==NULL)
	{
		head = (table_node*)malloc(sizeof(table_node));
		head->table_name = NULL;
		head->next = NULL;
	}
	curr = head;

	if(pathname==NULL)
	{
		pathname = (char*)malloc(120);
	}
	strcpy(pathname,file_pathname);

	while(1)
	{
		if(curr->table_name==NULL)
		{
			curr->table_name = (char*)malloc(120);
			strcpy(curr->table_name,file_pathname);
			curr->table_id = id;
			break;
		}
		else if(curr->table_name!=NULL)
		{
			if(strcmp(curr->table_name,file_pathname)==0)
			{
				id = curr->table_id;
				break;
			}
			else
			{
				if(curr->next!=NULL)
				{
					curr = curr->next;
				}
				else if(curr->next==NULL)
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
	if(fd == -1)
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

int table_open_check()
{
	int fd;
	fd = open(pathname,O_RDWR|O_SYNC);
	if(fd==-1)
	{	
		close(fd);
		return -1;
	}
	close(fd);
	return 0;
}

void exit_table_information()
{
	while(head!=NULL)
	{
		table_node* prev = head;
		head = head->next;
		free(prev->table_name);
		free(prev);
	}
	free(pathname);
}

void read_header(header_page* tmp)
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
}


