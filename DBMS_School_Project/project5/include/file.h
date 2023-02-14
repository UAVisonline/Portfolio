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
#include "page.h"

// essential func
pagenum_t file_alloc_page(); //allocate on a disk_page from the free page list

void file_free_page(pagenum_t pagenum); //free on-disk page to the free page list 

void file_read_page(pagenum_t pagenum,page_t* dest); //Read a disk-page into in-memory page(dest)

void file_write_page(pagenum_t pagenum, const page_t* src); //Write an in-memory page(src) into disk-page

// function using in library
int open_table(char* pathname); // Open_table_name_pathname

//int all_close_table_feature();

//int ret_current_table_id();

//void read_header(header_page* tmp);

//void write_header(header_page* header);


#endif //_FILE_H_
