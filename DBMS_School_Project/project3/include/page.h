#ifndef _PAGE_H_
#define _PAGE_H_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <stdbool.h>
#include <unistd.h>
#include <fcntl.h>

#define Page_size 4096
#define FALSE 0
#define TRUE 1
#define Leaf_factor 32
#define Internal_factor 249
//32 249

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

// record that have key and value
// key is uint64_t, value is char[120], so size of record is 128bytes
typedef struct record{
	int64_t key;
	char value[120];
} record;

// pointer used in internal_page
typedef struct key_pointer
{
	int64_t key;
	pagenum_t page_num;
} key_pointer;

// Leaf_page structure in memory
typedef struct _Leaf_Page
{
	pagenum_t parent_page_num; //Root = 0
	int Is_Leaf;
	int Number_of_Keys;
	int64_t dummy[13];
	pagenum_t right_sibling_number; // right_most_page = 0
	record records[31];
} leaf_page;

// Internal_page structure in memory
typedef struct _Internal_Page
{
	pagenum_t parent_page_num; //Root = 0
	int Is_Leaf;
	int Number_of_Keys;
	int64_t dummy[13];
	pagenum_t one_more_page_number;
	key_pointer pointers[248];
} internal_page;

typedef struct table_node
{
	char* table_name; // store table_name
	int table_id; //store table_id
	int open; // store status_of_table_open
	struct table_node* next; //next table_node
}table_node;

//table_node* table_head; // table list
//char* pathname; // char*_value_using_openfile
//int current_table_id; // table_id_currently_in_use

// Transfer code
void leaf_to_page(leaf_page* src,page_t* dest);
void page_to_leaf(page_t* src,leaf_page* dest);
void internal_to_page(internal_page* src, page_t* dest);
void page_to_internal(page_t* src,internal_page* dest);
void header_to_page(header_page* src,page_t* dest);
void page_to_header(page_t* src, header_page* dest);

int table_open_check(int id); // check that table(has id) is open

void exit_table_information(); // function that free_memory_table_node

// util func

void pathname_change_temporary(int table_id); // use in close_table to change pathname

void pathname_change_current(); // use in close_table to restore original_pathname

int close_table_in_node(int table_id); // use in close table to change table_node_open_value

#endif



