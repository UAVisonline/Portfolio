#ifndef __BPT_H_
#define __BPT_H_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <stdint.h>
#include "page.h"

// Size of Page : 4KB = 4096

// print_and_find code
void special_thanks();
void usage_prototype();
void print_header_page(int table_id); //test function
void print_leaves(int table_id);

int db_find(int table_id,int64_t key,char* ret_value,int trx_id);
int db_update(int table_id,int64_t key,char* values,int trx_id);

// Insertion code
record* make_record(int64_t key, char* values);
int get_left_index(int table_id,pagenum_t number,int64_t new_key);
int Start_new_tree(int table_id,record* rec);
int insert_record_into_leaf(int table_id,pagenum_t number,record* rec);
int insert_record_into_leaf_split(int table_id, pagenum_t number,record* rec);
int insert_into_parent(int table_id, pagenum_t number,pagenum_t old_num,int64_t new_key,pagenum_t new_num,int leaf_check);
int insert_key_into_internal(int table_id, pagenum_t number,int64_t new_key,pagenum_t new_number,int left_index);
int insert_key_into_internal_split(int table_id,pagenum_t number,int64_t new_key,pagenum_t new_number,int left_index);
int db_insert(int table_id,int64_t key, char* values);

// Deletion code
int delete_in_leaf(int table_id,int64_t key,pagenum_t leaf_number);
int delayed_merge_leaf(int table_id,pagenum_t delete_number, int64_t delete_key);
int delayed_merge_internal(int table_id,pagenum_t delete_number, int64_t delete_key,pagenum_t prime_pagenum);
int delayed_redistribute_internal(int table_id,pagenum_t parent_number,int64_t prime_key,pagenum_t uncle_number);
int db_delete(int table_id, int64_t key);


#endif //__BPT_H_




