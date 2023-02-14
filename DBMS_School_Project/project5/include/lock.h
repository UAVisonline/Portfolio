#ifndef __LOCK_H__
#define __LOCK_H__

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pthread.h>
#include <stdint.h>
#include <string.h>
#include "page.h"

//typedef struct xact_node;
//typedef struct lock_entry lock_entry;
//typedef struct lock_t lock_t;

typedef struct xact_node
{
	int trx_id;
	struct xact_node* prev;
	struct xact_node* next;
	struct lock_t* head_lock_object;
	struct lock_t* tail_lock_object;
}xact_node;

typedef struct lock_entry
{
	int table_id;
	int record_id;
	struct lock_entry* next_entry;
	struct lock_t* next;
}lock_entry;

typedef struct lock_t
{
	struct lock_t* prev;
	struct lock_t* next;
	struct lock_entry* entry;
	//struct lock_t* same_thread_prev;
	struct lock_t* same_thread_next;

	int lock_mode;// 0 == shared, 1 == exclusive
	int owner_trx_id;
	int hash;
	int can_have_recovery;
	int wait;
	int deadlock_check;
	char* recovery_value; // only lock_mode==1->use this value
}lock_t;

// API fot xact manager
int trx_begin();
int trx_commit(int trx_id);

int trx_abort(int trx_id);

// APIs for lock table
int init_lock_table();
lock_t* lock_acquire(int table_id, int64_t key, int trx_id, int lock_mode);
int lock_release(lock_t* lock_obj);
void recovery_value_save(lock_t* lock_obj,char *value);

int deadlock_detection(int trx_id,int my_trx_id);

#endif /* __LOCK_H__ */
