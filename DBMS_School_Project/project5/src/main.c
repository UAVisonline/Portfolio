#include "bpt.h"
#include "buf.h"
#include "file.h"
#include "lock.h"

int main(int argc, char ** argv)
{
	//int val; test_value
	char *file_pathname;
	char cmd;
	uint64_t input_key;
	char* values;
	char* find_values;
	int table_id, temp_id, test;

	file_pathname = (char*)malloc(120);
	if(argc>1)
	{
		strcpy(file_pathname,argv[1]);
		//pathname = (char*)malloc(sizeof(char)*strlen(argv[1])+1);
		//pathname = argv[1];
		//open path name data file
		//if not open, make it, but if not open to do, program exit
		//close the file
	}
	else
	{
		strcpy(file_pathname,"default_DB_File.db");
		//pathname = "default_DB_File";
	}
	table_id = open_table(file_pathname);
	init_db(1024);
	//init_lock_table();
	test = trx_begin();
	printf("%d\n",test);
	//init_db(16);

	special_thanks();
	usage_prototype();
	values = (char*)malloc(120);
	find_values = (char*)malloc(120);
	printf("> ");
	while(scanf("%c",&cmd) != EOF)
	{
		switch(cmd)
		{
			case 'd':
				scanf("%ld",&input_key);
				db_delete(table_id,input_key);
				break;
			/*case 'D':
				scanf("%d",&temp_id);
				db_delete(temp_id,input_key);
				break;*/
			case 'i':
				scanf("%ld    %s",&input_key,values);
				db_insert(table_id,input_key,values);
				//insert record into Tree
				break;
			/*case 'I':
				scanf("%d",&temp_id);
				scanf("%ld %s",&input_key,values);
				db_insert(temp_id,input_key,values);
				break;*/
			case 'f':
				scanf("%ld",&input_key);
				if(db_find(table_id,input_key,find_values,1)==0)
				{
					printf("%ld key : %s\n",input_key,find_values);
				}
				//find the record and return value into find_values pointer
				break;
			case 'm':
				scanf("%ld    %s",&input_key,values);
				db_update(table_id,input_key,values,1);
				break;
			case 't':
				trx_commit(1);
				break;
			case 'T':
				trx_abort(1);
				break;
			/*case 'F':
				scanf("%d",&temp_id);
				scanf("%ld",&input_key);
				if(db_find(temp_id,input_key,find_values)==0)
				{
					printf("%ld key : %s\n",input_key,find_values);
				}
				//find the record and return value into find_values pointer
				break;*/
			case 'q':
				while(getchar()!= (int)'\n');
				free(find_values);
				free(values);
				free(file_pathname);
				shutdown_db();
				exit_table_information();
				//allocated memory free
				return EXIT_SUCCESS;
				break;
			case 'l':
				print_leaves(table_id);
				break;
			case 'c':
				scanf("%s",file_pathname);
				table_id = open_table(file_pathname);
				printf("%d\n",table_id);
				break;
			case 'C':
				scanf("%d",&temp_id);
				close_table(temp_id);
				break;
			case 'p':
				print_header_page(table_id);
				break;
			case 'P':
				Print_Buffer_Status();
				break;
			case 'a': // command for test
				scanf("%ld",&input_key);
				for(int i =0;i<input_key;i+=1)
				{
					strcpy(values,"i");
					db_insert(table_id,i,values);
				}
				break;
			case 'e': // command for test
				scanf("%ld",&input_key);
				for(int i = input_key-1;i>=0;i-=1)
				{
					db_delete(table_id,i);
				}
				break;
			default:
				break;
		}
		while(getchar()!=(int)'\n');
		usage_prototype();
		printf(">");
	}
	printf("\n");

	return EXIT_SUCCESS;
}




