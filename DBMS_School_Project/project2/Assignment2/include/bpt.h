#ifndef __BPT_H_
#define __BPT_H_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <stdint.h>
#include "file.h"

#define FALSE 0
#define TRUE 1
#define Leaf_factor 32
#define Internal_factor 249
//32 249
// Size of Page : 4KB = 4096

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

// print_and_find code
void special_thanks();
void usage_prototype();
void print_header_page(); //test function
void print_leaves();

int db_find(int64_t key,char* ret_value);

// Insertion code
record* make_record(int64_t key, char* values);
int get_left_index(pagenum_t number,int64_t new_key);
int Start_new_tree(record* rec);
int insert_record_into_leaf(pagenum_t number,record* rec);
int insert_record_into_leaf_split(pagenum_t number,record* rec);
int insert_into_parent(pagenum_t number,pagenum_t old_num,int64_t new_key,pagenum_t new_num,int leaf_check);
int insert_key_into_internal(pagenum_t number,int64_t new_key,pagenum_t new_number,int left_index);
int insert_key_into_internal_split(pagenum_t number,int64_t new_key,pagenum_t new_number,int left_index);
int db_insert(int64_t key, char* values);

// Deletion code
int delete_in_leaf(int64_t key,pagenum_t leaf_number);
int delayed_merge_leaf(pagenum_t delete_number, int64_t delete_key);
int delayed_merge_internal(pagenum_t delete_number, int64_t delete_key,pagenum_t prime_pagenum);
int delayed_redistribute_internal(pagenum_t parent_number,int64_t prime_key,pagenum_t uncle_number);
int db_delete(int64_t key);

// Transfer code
void leaf_to_page(leaf_page* src,page_t* dest);
void page_to_leaf(page_t* src,leaf_page* dest);
void internal_to_page(internal_page* src, page_t* dest);
void page_to_internal(page_t* src,internal_page* dest);

#endif //__BPT_H_




