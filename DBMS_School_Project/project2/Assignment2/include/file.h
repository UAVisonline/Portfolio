#ifndef _FILE_H_
#define _FILE_H_

#define _GNU_SOURCE

//#ifndef O_DIRECT
//#define O_DIRECT 00040000
//#endif

#include <sys/time.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <errno.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <unistd.h>
#include <fcntl.h>

#define Page_size 4096

typedef uint64_t pagenum_t;

typedef struct header_page
{
	uint64_t next_free_page;
	uint64_t root_page;
	uint64_t Num_of_pages;
	uint64_t dummy[509];
}header_page;

typedef struct page_t
{
	pagenum_t next_free_page_number;
	uint64_t dummy[511];
}page_t;

typedef struct table_node
{
	char* table_name;
	int table_id;
	struct table_node* next;
}table_node;

extern table_node* head;

extern char* pathname;

// essential func
pagenum_t file_alloc_page(); //allocate on a disk_page from the free page list

void file_free_page(pagenum_t pagenum); //free on-disk page to the free page list 

void file_read_page(pagenum_t pagenum,page_t* dest); //Read a disk-page into in-memory page(dest)

void file_write_page(pagenum_t pagenum, const page_t* src); //Write an in-memory page(src) into disk-page

// function using in library
int open_table(char* pathname);

int take_table_id(table_node* curr,char* pathname);

void exit_table_information();

int table_open_check();

// util func

void read_header(header_page* tmp);

void write_header(header_page* header);
#endif //_FILE_H_
