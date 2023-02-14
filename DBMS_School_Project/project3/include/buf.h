#ifndef _BUF_H_
#define _BUF_H_

#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include "page.h"

typedef struct buffer_node{
	page_t* frame;
	int table_id;
	pagenum_t page_number;
	int is_dirty;
	int pinned;
	struct buffer_node* prev;
	struct buffer_node* next;
} buffer_node;

extern buffer_node* buf_head;
extern buffer_node* buf_tail;
extern int buffer_size;

// init buffer and free buffer
int init_db(int size);
void free_db();

// Get buffer_function and Set buffer_function
void Get_Buffer(int table_id,pagenum_t page_number,int pinned,page_t* dest);
void Set_Buffer(int table_id,pagenum_t page_number,int pinned,page_t* src);
void Unpin_Buffer();

// buffer read file and empty the buffer node function
int close_table(int table_id);
int shutdown_db();

//Test Function
void Print_Buffer_Status();

#endif //_BUF_H_
