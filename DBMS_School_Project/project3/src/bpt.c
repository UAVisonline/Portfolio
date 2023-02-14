#include "bpt.h"
#include "buf.h"
#include "file.h"

//All file_read and file_write function change buffer_read and buffer_write
//This design use in-memory buffer when read/write page
//but make replica page in the insert/delete function, so memory use is much bigger than situation referencing buffer page

void special_thanks()
{
	printf("This code is based on B+ Tree Implementation Copyright Amittai Aviram\n");
	printf("Thank you Amittai Aviram\n");
}

void usage_prototype()
{
	printf("\nCommand list : \n");
	printf("i : <int> <string> -- Insert Record that key <int> and value <string>\n");
	printf("f : <int> -- Find the value under the key <int>\n");
	printf("d : <int> -- Delete the record under the key <int>\n");
	printf("l : Print leafs of Tree\n");
	printf("c : <string> -- <string> file name open or create\n");
	printf("q : Exit the program\n");
}

void print_header_page(int t_id)
{
	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	Get_Buffer(t_id,0,0,head_page);
	page_to_header(head_page,header);

	printf("%ld  %ld  %ld\n",header->next_free_page,header->root_page,header->Num_of_pages);//
	free(head_page);
	free(header);
} //test function : print header page information

void print_leaves(int table_id)
{
	uint64_t root_number = 0;
	internal_page* internal = (internal_page*)malloc(Page_size);
	leaf_page* leaf = (leaf_page*)malloc(Page_size);
	page_t* page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	page_t* head_page = (page_t*)malloc(Page_size);

	if(table_open_check(table_id)==-1)
	{
		free(header);
		free(internal);
		free(leaf);
		free(page);
		free(head_page);
		printf("Can't not found table\n");		
		return;
	}

	Get_Buffer(table_id,0,0,head_page);
	page_to_header(head_page,header);
	
	root_number = header->root_page; //root page information acquire
	free(head_page);
	free(header);
	if(root_number == 0)
	{
		printf("Empty Tree\n");
	}
	else
	{
		Get_Buffer(table_id,root_number,0,page);
		page_to_internal(page,internal);

		while(TRUE) //go to leftmost leaf page
		{
			if(internal->Is_Leaf==0)
			{
				root_number = internal->one_more_page_number;
				Get_Buffer(table_id,root_number,0,page);
				page_to_internal(page,internal);
			}
			else if(internal->Is_Leaf==1)
			{
				break;
			}
		}

		page_to_leaf(page,leaf);

		while(TRUE) //print records in leaf page and go to right_sibling leaf
		{
			for(int i =0;i<leaf->Number_of_Keys;i++)
			{
				printf("(%ld,%s)",leaf->records[i].key,leaf->records[i].value);
			}
			if(leaf->right_sibling_number==0)
			{
				break;
			}
			else if(leaf->right_sibling_number!=0)
			{
				root_number = leaf->right_sibling_number;
				Get_Buffer(table_id,root_number,0,page);
				page_to_leaf(page,leaf);
				printf("||");
			}

		}

		printf("\n");
	}
	free(internal);
	free(page);
	free(leaf);
}

int db_find(int table_id,int64_t key,char* ret_values) // LibraryAPI_find
{
	uint64_t root_number = 0;
	internal_page* internal = (internal_page*)malloc(Page_size);
	page_t* find_page = (page_t*)malloc(Page_size);
	leaf_page* find_leaf = (leaf_page*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	page_t* head_page = (page_t*)malloc(Page_size);

	if(table_open_check(table_id)==-1)
	{
		free(header);
		free(head_page);
		free(internal);
		free(find_page);
		free(find_leaf);
		//printf("Can't not found table\n");		
		return -1;
	}
	Get_Buffer(table_id,0,0,head_page);
	page_to_header(head_page,header);

	root_number = header->root_page;
	free(head_page);
	free(header);
	if(root_number == 0) // Empty Tree case
	{
		free(internal);
		free(find_page);
		free(find_leaf);
		return -1;
	}
	else // Not Empty Tree
	{
		Get_Buffer(table_id,root_number,0,find_page);
		page_to_internal(find_page,internal);
		while(TRUE) // go to leaf page that can have a key in page
		{
			if(internal->Is_Leaf==0)
			{
				root_number = internal->one_more_page_number;
				for(int i =0;i<internal->Number_of_Keys;i++)
				{
					if(key>=internal->pointers[i].key)
					{
						root_number = internal->pointers[i].page_num;
					}
					else
					{
						break;
					}
				}
			}
			else if(internal->Is_Leaf==1)
			{
				break;
			}
			Get_Buffer(table_id,root_number,0,find_page);
			page_to_internal(find_page,internal);
		}

		free(internal);

		page_to_leaf(find_page,find_leaf);
		for(int i =0;i<find_leaf->Number_of_Keys;i++)
		{
			if(key==find_leaf->records[i].key) // find key
			{
				strcpy(ret_values,find_leaf->records[i].value);
				break;
			}
			else if(key<find_leaf->records[i].key) //no key in leaf page case 1
			{
				free(find_page);
				free(find_leaf);
				return -1;
				break;
			}
			else if(key!=find_leaf->records[i].key && i==find_leaf->Number_of_Keys-1) // no key in leaf page case 2
			{
				free(find_page);
				free(find_leaf);
				return -1;
				break;			
			}
		}
	}
	free(find_page);
	free(find_leaf);
	return 0;
}

record* make_record(int64_t input_key, char* input_values)
{
	record* tmp = (record*)malloc(sizeof(record));
	if(tmp == NULL)
	{
		perror("Record Creation.\n");
		exit(EXIT_FAILURE);
	}
	else
	{
		tmp->key = input_key;
		strcpy(tmp->value,input_values);
	}
	return tmp;
}

int Start_new_tree(int table_id,record* rec)
{
	pagenum_t root_number;
	leaf_page* leaf;
	page_t* tmp;
	
	root_number = file_alloc_page(); // new page we need in free page list
	tmp = (page_t*)malloc(Page_size);
	leaf = (leaf_page*)malloc(Page_size);
	Get_Buffer(table_id,root_number,1,tmp);
	page_to_leaf(tmp,leaf);

	leaf->parent_page_num = 0;
	leaf->Is_Leaf = 1;
	leaf->Number_of_Keys = 1;
	leaf->right_sibling_number = 0;
	leaf->records[0].key = rec->key;
       	strcpy(leaf->records[0].value,rec->value);
	//new root leaf page information set

	leaf_to_page(leaf,tmp);
	Set_Buffer(table_id,root_number,1,tmp);
	free(leaf);
	free(tmp);

	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	Get_Buffer(table_id,0,1,head_page);
	page_to_header(head_page,header);

	header->root_page = root_number;

	header_to_page(header,head_page);
	Set_Buffer(table_id,0,1,head_page);
	free(head_page);
	free(header);

	//header infromation change (new root page)

	Unpin_Buffer();
	return 0;
}

int insert_record_into_leaf(int table_id,pagenum_t number,record* rec)
{
	//printf("insert_leaf\n");
	int i, insertion_point;
	int64_t record_key;
	page_t* page = (page_t*)malloc(Page_size);
	leaf_page* leaf = (leaf_page*)malloc(Page_size);
	Get_Buffer(table_id,number,1,page);
	page_to_leaf(page,leaf);
	record_key = rec->key;
	
	insertion_point = 0;
	while(insertion_point<leaf->Number_of_Keys && leaf->records[insertion_point].key<record_key)//find appropriate position about new record
	{
		insertion_point++;
	}

	for(i = leaf->Number_of_Keys;i>insertion_point;i--) //go right bigger key
	{
		leaf->records[i].key = leaf->records[i-1].key;
		strcpy(leaf->records[i].value,leaf->records[i-1].value);
	}
	leaf->records[i].key = rec->key;
	strcpy(leaf->records[i].value,rec->value);
	leaf->Number_of_Keys++;
	//insert new record

	leaf_to_page(leaf,page);
	Set_Buffer(table_id,number,1,page);
	free(leaf);
	free(page);

	Unpin_Buffer();
	return 0;
}

int insert_record_into_leaf_split(int table_id,pagenum_t old_number,record* rec)
{
	//printf("insert_leaf_split\n");
	pagenum_t new_number,parent_number;
	record temp[Leaf_factor];
	leaf_page* old_leaf;
        leaf_page* new_leaf;
	page_t* old_page;
	page_t*	new_page;
	int insertion_index,split,i,j;
	int64_t new_key;
	
	old_leaf = (leaf_page*)malloc(Page_size);
	new_leaf = (leaf_page*)malloc(Page_size);
	old_page = malloc(Page_size);
	new_page = malloc(Page_size);

	new_number = file_alloc_page(); // we need new leaf page in free page list 
	
	Get_Buffer(table_id,new_number,1,new_page);
	
	Get_Buffer(table_id,old_number,1,old_page);
	
	page_to_leaf(old_page,old_leaf);
	page_to_leaf(new_page,new_leaf);
	
	new_leaf->parent_page_num = old_leaf->parent_page_num;
	new_leaf->Is_Leaf = 1;
	new_leaf->Number_of_Keys = 0;
	//new leaf information set

	insertion_index = 0;
	while(insertion_index < Leaf_factor-1 && old_leaf->records[insertion_index].key < rec->key)
	{
		insertion_index++;
	}
	
	for(i = 0,j=0;i<old_leaf->Number_of_Keys;i++,j++)
	{
		if(j == insertion_index) j++;
		temp[j].key = old_leaf->records[i].key;
		strcpy(temp[j].value,old_leaf->records[i].value);
	}
	temp[insertion_index].key = rec->key;
	strcpy(temp[insertion_index].value,rec->value);
	//sort all record in old_leaf and new record

	old_leaf->Number_of_Keys = 0;
	split = (Leaf_factor)/2;

	for(i = 0;i<split;i++) //old_leaf record set
	{
		old_leaf->records[i].key = temp[i].key;
		strcpy(old_leaf->records[i].value,temp[i].value);
		old_leaf->Number_of_Keys++;
	}
	for(i = split,j=0;i<Leaf_factor;i++,j++) //new_leaf record set
	{
		new_leaf->records[j].key = temp[i].key;
		strcpy(new_leaf->records[j].value,temp[i].value);
		new_leaf->Number_of_Keys++;
	}

	new_leaf->right_sibling_number = old_leaf->right_sibling_number;
	old_leaf->right_sibling_number = new_number;
	//set leaf_pages right_sibling number
	
	new_key = new_leaf->records[0].key;// save first key in new_leaf for insert parent_internal_page 
	parent_number = new_leaf->parent_page_num;

	leaf_to_page(old_leaf,old_page);
	leaf_to_page(new_leaf,new_page);
	Set_Buffer(table_id,old_number,1,old_page);
	Set_Buffer(table_id,new_number,1,new_page);
	//file_write_page(old_number,old_page);
	//file_write_page(new_number,new_page);

	free(old_page);
	free(new_page);
	free(old_leaf);
	free(new_leaf);

	Unpin_Buffer();
	return insert_into_parent(table_id,parent_number,old_number,new_key,new_number,1);

	
}

int insert_into_parent(int table_id,pagenum_t parent_number,pagenum_t old_number, int64_t new_key,pagenum_t new_number,int leaf_check)
{
	//printf("insert_parent\n");
	// new_number page created right of old_number page, so we do not think about one_more_page_number case
	int left_index;
	int return_number = 0;
	if(parent_number == 0) //page of child has no parent case, we make new root_internal_page
	{
		page_t* page = (page_t*)malloc(Page_size);
		internal_page* internal = (internal_page*)malloc(Page_size);
		leaf_page* leaf;
		parent_number = file_alloc_page(); // new internal page in free page list

		Get_Buffer(table_id,parent_number,1,page);
		//file_read_page(parent_number,page);
		page_to_internal(page,internal);

		internal->parent_page_num = 0;
		internal->Is_Leaf = 0;
		internal->Number_of_Keys = 1;
		internal->pointers[0].key = new_key;
		internal->pointers[0].page_num = new_number;
		internal->one_more_page_number = old_number;
		//set information about new internal_page

		internal_to_page(internal,page);
		Set_Buffer(table_id,parent_number,1,page);
		//file_write_page(parent_number,page);

		free(page);
		free(internal);

		if(leaf_check==1) //child_page is leaf
		{
			page = (page_t*)malloc(Page_size);
			leaf = (leaf_page*)malloc(Page_size);
			Get_Buffer(table_id,old_number,1,page);
			//file_read_page(old_number,page);
			page_to_leaf(page,leaf);
			leaf->parent_page_num = parent_number;
			leaf_to_page(leaf,page);
			Set_Buffer(table_id,old_number,1,page);
			//file_write_page(old_number,page);
			free(page);
			free(leaf);

			page = (page_t*)malloc(Page_size);
			leaf = (leaf_page*)malloc(Page_size);
			Get_Buffer(table_id,new_number,1,page);
			//file_read_page(new_number,page);
			page_to_leaf(page,leaf);
			leaf->parent_page_num = parent_number;
			leaf_to_page(leaf,page);
			Set_Buffer(table_id,new_number,1,page);
			//file_write_page(new_number,page);
			free(page);
			free(leaf);
		}
		else if(leaf_check==0) //child_page is internal
		{
			page = (page_t*)malloc(Page_size);
			internal = (internal_page*)malloc(Page_size);
			Get_Buffer(table_id,old_number,1,page);
			//file_read_page(old_number,page);
			page_to_internal(page,internal);
			internal->parent_page_num = parent_number;
			internal_to_page(internal,page);
			Set_Buffer(table_id,old_number,1,page);
			//file_write_page(old_number,page);
			free(page);
			free(internal);

			page = (page_t*)malloc(Page_size);
			internal = (internal_page*)malloc(Page_size);
			Get_Buffer(table_id,new_number,1,page);
			//file_read_page(new_number,page);
			page_to_internal(page,internal);
			internal->parent_page_num = parent_number;
			internal_to_page(internal,page);
			Set_Buffer(table_id,new_number,1,page);
			//file_write_page(new_number,page);
			free(page);
			free(internal);
		}
		
		page_t* head_page = (page_t*)malloc(Page_size);
		header_page* header = (header_page*)malloc(Page_size);
		Get_Buffer(table_id,0,1,head_page);
		//file_read_page(0,head_page); 
		page_to_header(head_page,header);
		
		header->root_page = parent_number; //change header page : new root number
		
		header_to_page(header,head_page);
		Set_Buffer(table_id,0,1,head_page);
		//file_write_page(0,head_page);
		free(head_page);
		free(header);

		Unpin_Buffer();
		return return_number;
	}
	else // child_page have parent_page
	{
		left_index = 0;
		page_t* page = (page_t*)malloc(Page_size);
		internal_page* internal = (internal_page*)malloc(Page_size);
		Get_Buffer(table_id,parent_number,0,page);
		//file_read_page(parent_number,page);
		page_to_internal(page,internal);
		
		left_index = get_left_index(table_id,parent_number,new_key); // find appropriate position new key

		if(internal->Number_of_Keys<Internal_factor-1)// parent not full
		{
			return_number = insert_key_into_internal(table_id,parent_number,new_key,new_number,left_index);
			
		}
		else if(internal->Number_of_Keys==Internal_factor-1) //parent full
		{
			return_number = insert_key_into_internal_split(table_id,parent_number,new_key,new_number,left_index);
		}

		free(page);
		free(internal);
		return return_number;
	}
}

int get_left_index(int table_id,pagenum_t number,int64_t new_key)
{
	int left_index;
	page_t* page = (page_t*)malloc(Page_size);
	internal_page* internal = (internal_page*)malloc(Page_size);
	

	left_index=0; //key set leftmost position (except one_more_page_number)
	Get_Buffer(table_id,number,0,page);
	//file_read_page(number,page);
	page_to_internal(page,internal);

	while(left_index<internal->Number_of_Keys&&new_key>=internal->pointers[left_index].key)
	{
		left_index++; //key position move 1 right direction
	}

	free(page);
	free(internal);
	return left_index;
}

int insert_key_into_internal(int table_id,pagenum_t number,int64_t new_key,pagenum_t new_number,int left_index)
{
	//printf("insert_internal\n");
	page_t* page = (page_t*)malloc(Page_size);
	internal_page* internal = (internal_page*)malloc(Page_size);
	int i;

	Get_Buffer(table_id,number,1,page);
	//file_read_page(number,page);
	page_to_internal(page,internal);
	for(i=internal->Number_of_Keys;i>left_index;i--)
	{
		internal->pointers[i].key = internal->pointers[i-1].key;
		internal->pointers[i].page_num = internal->pointers[i-1].page_num;
	}
	//right move key and page bigger than new_key
	internal->pointers[left_index].key = new_key;
	internal->pointers[left_index].page_num = new_number;
	internal->Number_of_Keys++;
	//set new_key and new_number	

	internal_to_page(internal,page);
	Set_Buffer(table_id,number,1,page);
	//file_write_page(number,page);
	free(internal);
	free(page);

	Unpin_Buffer();
	return 0;
}

int insert_key_into_internal_split(int table_id, pagenum_t old_number,int64_t new_key,pagenum_t new_number,int left_index)
{
	//printf("insert_internal_split\n");
	pagenum_t new_internal_number, parent_number;
	int64_t temp_key[Internal_factor];
	pagenum_t temp_page[Internal_factor];
	internal_page* old_internal = (internal_page*)malloc(Page_size);
	internal_page* new_internal = (internal_page*)malloc(Page_size);
	page_t* old_page = (page_t*)malloc(Page_size);
	page_t* new_page = (page_t*)malloc(Page_size);
	int i,j,split;
	int64_t k_prime_key;
	
	new_internal_number = file_alloc_page(); //we creadted new internal page, right position
	Get_Buffer(table_id,old_number,1,old_page);
	Get_Buffer(table_id,new_internal_number,1,new_page);
	//file_read_page(old_number,old_page);
	//file_read_page(new_internal_number,new_page);
	page_to_internal(old_page,old_internal);
	page_to_internal(new_page,new_internal);
	
	new_internal->parent_page_num = old_internal->parent_page_num;
	new_internal->Is_Leaf = 0;
	new_internal->Number_of_Keys = 0;
	// set information about new internal page
	for(i=0,j=0;i<old_internal->Number_of_Keys;i++,j++)
	{
		if(j==left_index) j++;
		temp_key[j]=old_internal->pointers[i].key;
		temp_page[j]=old_internal->pointers[i].page_num;
	}
	temp_key[left_index] = new_key;
	temp_page[left_index] = new_number;
	//we store all key and page in old_page(except one more page number) and new_key, new_number page	

	split = (Internal_factor/2);
	old_internal->Number_of_Keys = 0;

	for(i=0;i<split-1;i++)
	{
		old_internal->pointers[i].key = temp_key[i];
		old_internal->pointers[i].page_num = temp_page[i];
		old_internal->Number_of_Keys++;
	}
	//old page key,page information reset

	new_internal->one_more_page_number = temp_page[i];//new internal page->one_more_page_number set
	k_prime_key = temp_key[i];//key for parent page

	for(++i,j=0;i<Internal_factor;i++,j++)
	{
		new_internal->pointers[j].key = temp_key[i];
		new_internal->pointers[j].page_num = temp_page[i];
		new_internal->Number_of_Keys++;
	}
	//new page key,page information reset

	page_t* child_page = (page_t*)malloc(Page_size);
	internal_page* child_internal = (internal_page*)malloc(Page_size);
	leaf_page* child_leaf = (leaf_page*)malloc(Page_size);
	Get_Buffer(table_id,new_internal->one_more_page_number,0,child_page);
	//file_read_page(new_internal->one_more_page_number,child_page);
	page_to_internal(child_page,child_internal);
	if(child_internal->Is_Leaf==0)
	{
		child_internal->parent_page_num = new_internal_number;
		internal_to_page(child_internal,child_page);
		Set_Buffer(table_id,new_internal->one_more_page_number,0,child_page);
		//file_write_page(new_internal->one_more_page_number,child_page);
		for(i=0;i<new_internal->Number_of_Keys;i++)
		{
			Get_Buffer(table_id,new_internal->pointers[i].page_num,0,child_page);
			//file_read_page(new_internal->pointers[i].page_num,child_page);
			page_to_internal(child_page,child_internal);
			child_internal->parent_page_num = new_internal_number;
			internal_to_page(child_internal,child_page);
			Set_Buffer(table_id,new_internal->pointers[i].page_num,0,child_page);
			//file_write_page(new_internal->pointers[i].page_num,child_page);
		}
	}
	else if(child_internal->Is_Leaf==1)
	{
		page_to_leaf(child_page,child_leaf);
		child_leaf->parent_page_num = new_internal_number;
		leaf_to_page(child_leaf,child_page);
		Set_Buffer(table_id,new_internal->one_more_page_number,0,child_page);
		//file_write_page(new_internal->one_more_page_number,child_page);
		for(i=0;i<new_internal->Number_of_Keys;i++)
		{
			Get_Buffer(table_id,new_internal->pointers[i].page_num,0,child_page);
			//file_read_page(new_internal->pointers[i].page_num,child_page);
			page_to_leaf(child_page,child_leaf);
			child_leaf->parent_page_num = new_internal_number;
			leaf_to_page(child_leaf,child_page);
			Set_Buffer(table_id,new_internal->pointers[i].page_num,0,child_page);
			//file_write_page(new_internal->pointers[i].page_num,child_page);
		}
			
	}
	//we set information about child of new internal page(new parent_number set)
	free(child_internal);
	free(child_leaf);
	free(child_page);

	parent_number = new_internal->parent_page_num;

	internal_to_page(old_internal,old_page);
	internal_to_page(new_internal,new_page);
	Set_Buffer(table_id,old_number,1,old_page);
	Set_Buffer(table_id,new_internal_number,1,new_page);
	//file_write_page(old_number,old_page);
	//file_write_page(new_internal_number,new_page);

	free(old_page);
	free(new_page);
	free(old_internal);
	free(new_internal);

	Unpin_Buffer();
	return insert_into_parent(table_id,parent_number,old_number,k_prime_key,new_internal_number,0);
	// insert new_key and new_internal_page in parent page 
}

int db_insert(int table_id,int64_t key,char* values) //LibraryAPI_insert
{
	int return_number,duplicate;
	pagenum_t page_number;
	record* rec = make_record(key,values);
	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = malloc(Page_size);

	if(table_open_check(table_id)==-1)
	{
		free(head_page);
		free(header);
		free(rec);
		//printf("Can't not found table\n");	
		return -1;
	}	
	
	Get_Buffer(table_id,0,0,head_page);
	//file_read_page(0,head_page); 
	page_to_header(head_page,header);
	page_number = header->root_page;

	free(head_page);
	free(header);
	if(page_number == 0) // Empty tree->so we build new tree
	{	
		return_number = Start_new_tree(table_id,rec);
	}
	else
	{
		page_t* page;
		internal_page* internal;
		leaf_page* leaf;
		while(TRUE)
		{
			page = (page_t*)malloc(Page_size);
			internal = (internal_page*)malloc(Page_size);
			Get_Buffer(table_id,page_number,0,page);
			//file_read_page(page_number,page);
			page_to_internal(page,internal);
			if(internal->Is_Leaf==1)
			{
				leaf = (leaf_page*)malloc(Page_size);
				page_to_leaf(page,leaf);
				if(leaf->Is_Leaf!=1)
				{
					//printf("Error Insert data\nNo possible value of Is_Leaf leaf page\n");
					exit(-1);
				}
				free(page);
				free(internal);
				break;
			}
			else if(internal->Is_Leaf==0)
			{
				//page_number
				if(key<internal->pointers[0].key)
				{
					page_number = internal->one_more_page_number;
				}
				else if(key>=internal->pointers[0].key)
				{
					page_number = internal->pointers[0].page_num;
					for(int i = 1;i<internal->Number_of_Keys&&i<Internal_factor-1;i++)
					{
						if(key>=internal->pointers[i].key)
						{
							page_number = internal->pointers[i].page_num;
						}
						else
						{
							break;
						}
					}
				}

				/*
				printf("\nInternal_Page %ld\n",page_number);
				printf("(%ld)",internal->one_more_page_number);
				for(int j = 0;j<internal->Number_of_Keys && j<Internal_factor-1;j++)
				{
					printf("(%ld,%ld)",internal->pointers[j].key,internal->pointers[j].page_num);
				}
				printf("\n");
				*/

				free(page);
				free(internal);
			}
			else
			{
				//printf("Error Insert data\nNo Possible value of Is_Leaf internal page\n");
				exit(-1);
			}
			
		}
		//find appropriate position about key

		duplicate = 0;
		for(int i =0;i<leaf->Number_of_Keys;i++)
		{
			if(key==leaf->records[i].key) // in page, there is same key
			{
				duplicate = 1;
				return_number = -1;
				//printf("Same key Error!!!\n%ld key is in the B+Tree\n",key);
				break;	
			}
			else if(key<leaf->records[i].key)
			{
				break;
			}
		}
		
		if(duplicate==0) // no same key in page
		{
			if(leaf->Number_of_Keys==Leaf_factor-1) //full leaf page
			{
				return_number = insert_record_into_leaf_split(table_id,page_number,rec);
			}
			else if(leaf->Number_of_Keys<Leaf_factor-1) // non-full leaf page
			{
				return_number = insert_record_into_leaf(table_id,page_number,rec);
			}

		}
		free(leaf);
		
		/*
		leaf = (leaf_page*)malloc(Page_size);
		page = (page_t*)malloc(Page_size);
	        file_read_page(page_number,page);
		page_to_leaf(page,leaf);
		printf("\n%ld Page\n",page_number);
		for(int i =0;i<leaf->Number_of_Keys && i<Leaf_factor-1;i++)
		{	
			printf("(%ld,%s)",leaf->records[i].key,leaf->records[i].value);//
		}	
		printf("\n");
		free(page);
		free(leaf);
		*/
	}
	free(rec);
	//printf("Insert key %ld\n",key);
	return return_number;
}

int delete_in_leaf(int table_id,int64_t key,pagenum_t leaf_number)
{
	uint64_t parent_number = 0;
	int64_t key_prime;
	int i;
	page_t* page = (page_t*)malloc(Page_size);
	leaf_page* leaf = (leaf_page*)malloc(Page_size);
	
	Get_Buffer(table_id,leaf_number,1,page);
	//file_read_page(leaf_number,page);
	page_to_leaf(page,leaf);

	i=0;

	while(leaf->records[i].key!=key)
	{
		i++;
	}
	//find position of key we want to delete

	for(++i;i<leaf->Number_of_Keys;i++)
	{
		leaf->records[i-1].key = leaf->records[i].key;
		strcpy(leaf->records[i-1].value,leaf->records[i].value);
	}
	//left 1 direction bigger than key

	leaf->Number_of_Keys--;
	parent_number = leaf->parent_page_num;
	
	leaf_to_page(leaf,page);
	Set_Buffer(table_id,leaf_number,1,page);
	//file_write_page(leaf_number,page);

	key_prime = leaf->records[0].key; // key information for delete parent_key

	if(leaf->Number_of_Keys!=0)
	{
		free(leaf);
		free(page);
		Unpin_Buffer();
		return 0;
	}
	else if(leaf->Number_of_Keys==0) // leaf is empty, so we delete empty leaf page(delayed_merge)
	{
		if(parent_number==0)// leaf page is root page
		{
			page_t* head_page = (page_t*)malloc(Page_size);
			header_page* header = (header_page*)malloc(Page_size);
			
			//file_read_page(0,head_page);
			Get_Buffer(table_id,0,1,head_page);
			page_to_header(head_page,header);

			header->root_page = 0;

			header_to_page(header,head_page);
			Set_Buffer(table_id,0,1,head_page);
			//file_write_page(0,head_page);
			free(head_page);
			free(header);
			free(leaf);
			free(page);

			file_free_page(leaf_number);
			return 0;
		}
		else if(parent_number!=0) // go parent page and delete leaf_page
		{
			free(leaf);
			free(page);
			Unpin_Buffer();
			return delayed_merge_leaf(table_id,parent_number,key_prime);
		}
	}
}

int delayed_merge_leaf(int table_id,pagenum_t parent_number,int64_t prime_key)//leaf(child)->internal(parent) case
{
	// design of merge leaf is saved left_most_page_number, so case of delete left_most page, we delete right page
	pagenum_t deleted_number, prime_page_num;
	int64_t key_prime;
	int i, deleted_index, grand_parent_number;
	internal_page* parent_internal;
	leaf_page* child_leaf;
	page_t* parent_page;
	page_t* child_page;

	parent_page = (page_t*)malloc(Page_size);
	parent_internal = (internal_page*)malloc(Page_size);
	Get_Buffer(table_id,parent_number,1,parent_page);
	//file_read_page(parent_number,parent_page);
	page_to_internal(parent_page,parent_internal);
	
	if(prime_key<parent_internal->pointers[0].key) //deleted page is left most page
	{
		deleted_number = parent_internal->one_more_page_number;
		deleted_index = -1;
	}
	else
	{
		for(i=0;i<parent_internal->Number_of_Keys;i++)
		{
			if(prime_key>=parent_internal->pointers[i].key)
			{
				deleted_number = parent_internal->pointers[i].page_num; // deleted page is not left most page
			}
			else
			{
				break;
			}
		}
		deleted_index = i - 1;
	}
	

	child_page = (page_t*)malloc(Page_size);
	child_leaf = (leaf_page*)malloc(Page_size);
	Get_Buffer(table_id,deleted_number,1,child_page);
	//file_read_page(deleted_number,child_page);
	page_to_leaf(child_page,child_leaf);

	if(deleted_number == parent_internal->one_more_page_number)
	{
		pagenum_t real_delete_number;
		page_t* copy_page = (page_t*)malloc(Page_size);
		leaf_page* copy_leaf = (leaf_page*)malloc(Page_size);

		real_delete_number = parent_internal->pointers[0].page_num; // we call right page of left_most page
		Get_Buffer(table_id,real_delete_number,1,copy_page);
		//file_read_page(real_delete_number,copy_page);
		page_to_leaf(copy_page,copy_leaf);

		for(i  = 0;i<copy_leaf->Number_of_Keys;i++)
		{
			child_leaf->records[i].key = copy_leaf->records[i].key;
			strcpy(child_leaf->records[i].value,copy_leaf->records[i].value);
		}
		child_leaf->Number_of_Keys = copy_leaf->Number_of_Keys;
		child_leaf->right_sibling_number = copy_leaf->right_sibling_number;
		//all data of right page moved to left_most page

		leaf_to_page(child_leaf,child_page);
		Set_Buffer(table_id,deleted_number,1,child_page);
		//file_write_page(deleted_number,child_page);

		free(child_page);
		free(child_leaf);
		free(copy_leaf);
		free(copy_page);

		for(i=1;i<parent_internal->Number_of_Keys;i++)
		{
			parent_internal->pointers[i-1].key = parent_internal->pointers[i].key;
			parent_internal->pointers[i-1].page_num = parent_internal->pointers[i].page_num;
		}
		//parent_page information set
		file_free_page(real_delete_number); // we delete right page of left_most page
	}
	else
	{
		pagenum_t right_sibling_temp,change_leaf_number;
		page_t* changed_page = (page_t*)malloc(Page_size);
		leaf_page* changed_leaf = (leaf_page*)malloc(Page_size);	
		
		right_sibling_temp = child_leaf->right_sibling_number;
		free(child_page);
		free(child_leaf);

		if(deleted_number == parent_internal->pointers[0].page_num) // delete page number is right of left_most page
		{
			change_leaf_number = parent_internal->one_more_page_number;
		}
		else
		{
			change_leaf_number = parent_internal->pointers[deleted_index-1].page_num;
		}
		//file_read_page(change_leaf_number,changed_page);
		Get_Buffer(table_id,change_leaf_number,1,changed_page);
		page_to_leaf(changed_page,changed_leaf);
		changed_leaf->right_sibling_number = right_sibling_temp; //left page of delete page, we set right_sibling number
		leaf_to_page(changed_leaf,changed_page);
		Set_Buffer(table_id,change_leaf_number,1,changed_page);
		//file_write_page(change_leaf_number,changed_page);

		free(changed_page);
		free(changed_leaf);


		for(i = deleted_index+1;i<parent_internal->Number_of_Keys;i++)
		{
			parent_internal->pointers[i-1].key = parent_internal->pointers[i].key;
			parent_internal->pointers[i-1].page_num = parent_internal->pointers[i].page_num;
		}
		//parent_page information set
		file_free_page(deleted_number); // we delete page
	}

	parent_internal->Number_of_Keys--;
	internal_to_page(parent_internal,parent_page);
	Set_Buffer(table_id,parent_number,1,parent_page);
	//file_write_page(parent_number,parent_page);
	Unpin_Buffer();

	key_prime = parent_internal->pointers[0].key; // key information for deleted page for grand_parent
	prime_page_num = parent_internal->one_more_page_number; // we always save left_most page when delete it, so we need this for grand_parent delete
	grand_parent_number = parent_internal->parent_page_num;
	

	if(parent_internal->Number_of_Keys!=0)
	{
		free(parent_internal);
		free(parent_page);
		return 0;
	}
	else if(parent_internal->Number_of_Keys==0) // we delete parent_page
	{
		if(grand_parent_number==0) // parent_page is root
		{
			page_t* page = (page_t*)malloc(Page_size);
			page_t* head_page = (page_t*)malloc(Page_size);
			header_page* header = (header_page*)malloc(Page_size);
			leaf_page* leaf = (leaf_page*)malloc(Page_size);

			Get_Buffer(table_id,0,1,head_page);
			//file_read_page(0,head_page); 
			page_to_header(head_page,header);
			
			Get_Buffer(table_id,parent_internal->one_more_page_number,1,page);	
			//file_read_page(parent_internal->one_more_page_number,page);
			page_to_leaf(page,leaf);
			leaf->parent_page_num = 0;
			leaf_to_page(leaf,page);
			Set_Buffer(table_id,parent_internal->one_more_page_number,1,page);
			//file_write_page(parent_internal->one_more_page_number,page);
			free(leaf);
			//we set left_most_child is root page

			header->root_page = parent_internal->one_more_page_number;
			header_to_page(header,head_page);
			Set_Buffer(table_id,0,1,head_page);
			//file_write_page(0,head_page);		

			free(head_page);
			free(header);
			free(page);
			free(parent_internal);
			free(parent_page);	

			file_free_page(parent_number);
			return 0;
		}
		else
		{
			int distribute_point;
			pagenum_t uncle_number;
			internal_page* grand_internal = (internal_page*)malloc(Page_size);
			internal_page* uncle_internal = (internal_page*)malloc(Page_size);
			page_t* grand_page = (page_t*)malloc(Page_size);
			page_t* uncle_page = (page_t*)malloc(Page_size);
	
			//file_read_page(grand_parent_number,grand_page);
			Get_Buffer(table_id,grand_parent_number,0,grand_page);
			page_to_internal(grand_page,grand_internal);
			if(key_prime<grand_internal->pointers[0].key) //if parent_number is left_most page of grand_parent page
			{
				distribute_point = 0; //set uncle page is pointers[0].page_num
			}
			else
			{
				distribute_point = -1;
				for(i = 0;i<grand_internal->Number_of_Keys;i++)
				{
					if(key_prime>=grand_internal->pointers[i].key)
					{
						distribute_point = i - 1;
					}
					else
					{
						break;
					}
				}
			}
			
			if(distribute_point==-1) // set uncle page is grand_parent->left_most_page
			{
				uncle_number = grand_internal->one_more_page_number;
			}
			else // set uncle page is right of page
			{
				uncle_number = grand_internal->pointers[distribute_point].page_num;		
			}
			free(grand_internal);
			free(grand_page);
			Get_Buffer(table_id,uncle_number,0,uncle_page);
			//file_read_page(uncle_number,uncle_page);
			page_to_internal(uncle_page,uncle_internal);

			if(uncle_internal->Number_of_Keys==Internal_factor-1) // oh, uncle is full
			{
				free(uncle_internal);
				free(uncle_page);	
				free(parent_internal);
				free(parent_page);
				return delayed_redistribute_internal(table_id,grand_parent_number,key_prime,uncle_number);
				//we can't merge because we insert one key and one page to uncle or take away 
				//all uncle key and page, but that case, Number_of_Key is equal Internal_factor
				//it is wrong about design of B+Tree
			}			
			else if(uncle_internal->Number_of_Keys<Internal_factor-1) // oh, uncle is not full
			{
				free(uncle_internal);
				free(uncle_page);	
				free(parent_internal);
				free(parent_page);
				return delayed_merge_internal(table_id,grand_parent_number,key_prime,prime_page_num);
				//we can merge uncle page
			}

		}
	}
}

int delayed_merge_internal(int table_id,pagenum_t parent_number, int64_t prime_key,pagenum_t prime_pagenum)//internal(child)->internal(parent)
{
	pagenum_t deleted_number, prime_page_num;
	int64_t key_prime;
	int i, deleted_index, grand_parent_number;
	internal_page* parent_internal;
	internal_page* child_internal;
	page_t* parent_page;
        page_t*	child_page;

	parent_page = (page_t*)malloc(Page_size);
	parent_internal = (internal_page*)malloc(Page_size);
	Get_Buffer(table_id,parent_number,1,parent_page);
	//file_read_page(parent_number,parent_page);
	page_to_internal(parent_page,parent_internal);
	
	if(prime_key<parent_internal->pointers[0].key) //deleted page is left_most page
	{
		deleted_number = parent_internal->one_more_page_number;
		deleted_index = -1;
	}
	else //deleted page is not left_most page
	{
		for(i=0;i<parent_internal->Number_of_Keys;i++)
		{
			if(prime_key>=parent_internal->pointers[i].key)
			{
				deleted_number = parent_internal->pointers[i].page_num;
			}
			else
			{
				break;
			}
		}
		deleted_index = i - 1;
	}

	child_page = (page_t*)malloc(Page_size);
	child_internal = (internal_page*)malloc(Page_size);
	Get_Buffer(table_id,deleted_number,1,child_page);
	//file_read_page(deleted_number,child_page);
	page_to_internal(child_page,child_internal);

	if(deleted_number == parent_internal->one_more_page_number) //left_most page case
	{
		pagenum_t real_delete_number;
		page_t* copy_page = (page_t*)malloc(Page_size);
		internal_page* copy_internal = (internal_page*)malloc(Page_size);
		page_t* tmp_page = (page_t*)malloc(Page_size);
		internal_page* tmp_internal = (internal_page*)malloc(Page_size);
		leaf_page* tmp_leaf = (leaf_page*)malloc(Page_size);

		real_delete_number = parent_internal->pointers[0].page_num; //we call right of left_most page
		Get_Buffer(table_id,real_delete_number,1,copy_page);
		//file_read_page(real_delete_number,copy_page);
		page_to_internal(copy_page,copy_internal);
		
		child_internal->pointers[0].key = parent_internal->pointers[0].key; // we take one key in parent
		child_internal->pointers[0].page_num = copy_internal->one_more_page_number; //we take one_more_page_number in right page
		child_internal->Number_of_Keys++;
		for(i=1;i<=copy_internal->Number_of_Keys;i++)
		{
			child_internal->pointers[i].key = copy_internal->pointers[i-1].key;
			child_internal->pointers[i].page_num = copy_internal->pointers[i-1].page_num;
			child_internal->Number_of_Keys++;	
		}
		//we take all key and page in right page

		//file_read_page(child_internal->pointers[0].page_num,tmp_page);
		Get_Buffer(table_id,child_internal->pointers[0].page_num,0,tmp_page);
		page_to_internal(tmp_page,tmp_internal);
		if(tmp_internal->Is_Leaf==0)
		{
			for(int i =0;i<child_internal->Number_of_Keys;i++)
			{
				//file_read_page(child_internal->pointers[i].page_num,tmp_page);
				Get_Buffer(table_id,child_internal->pointers[i].page_num,0,tmp_page);
				page_to_internal(tmp_page,tmp_internal);
				tmp_internal->parent_page_num = deleted_number;
				internal_to_page(tmp_internal,tmp_page);
				Set_Buffer(table_id,child_internal->pointers[i].page_num,0,tmp_page);
				//file_write_page(child_internal->pointers[i].page_num,tmp_page);
			}
		}
		else if(tmp_internal->Is_Leaf==1)
		{
			for(int i =0;i<child_internal->Number_of_Keys;i++)
			{
				//file_read_page(child_internal->pointers[i].page_num,tmp_page);
				Get_Buffer(table_id,child_internal->pointers[i].page_num,0,tmp_page);
				page_to_leaf(tmp_page,tmp_leaf);
				tmp_leaf->parent_page_num = deleted_number;
				leaf_to_page(tmp_leaf,tmp_page);
				Set_Buffer(table_id,child_internal->pointers[i].page_num,0,tmp_page);
				//file_write_page(child_internal->pointers[i].page_num,tmp_page);
			}

		}
		else
		{
			printf("Error\nNo possible Is_Leaf data");
			exit(-1);
		}
		//set moved page of parent

		internal_to_page(child_internal,child_page);
		Set_Buffer(table_id,deleted_number,1,child_page);
		//file_write_page(deleted_number,child_page);

		free(child_page);
		free(child_internal);
		free(copy_internal);
		free(copy_page);
		free(tmp_page);
		free(tmp_internal);
		free(tmp_leaf);

		for(i=1;i<parent_internal->Number_of_Keys;i++)
		{
			parent_internal->pointers[i-1].key = parent_internal->pointers[i].key;
			parent_internal->pointers[i-1].page_num = parent_internal->pointers[i].page_num;
		}
		//parent_page 
		file_free_page(real_delete_number); // delete right page
	}
	else
	{
		pagenum_t more_page_number,change_internal_number;
		page_t* changed_page = (page_t*)malloc(Page_size);
		internal_page* changed_internal = (internal_page*)malloc(Page_size);
		
		//more_page_number = child_internal->one_more_page_number;

		if(deleted_number == parent_internal->pointers[0].page_num)
		{
			change_internal_number = parent_internal->one_more_page_number;
		}
		else
		{
			change_internal_number = parent_internal->pointers[deleted_index-1].page_num;
		}
		//left of delete page called

		//file_read_page(change_internal_number,changed_page);
		Get_Buffer(table_id,change_internal_number,1,changed_page);
		page_to_internal(changed_page,changed_internal);
		changed_internal->pointers[changed_internal->Number_of_Keys].key = parent_internal->pointers[deleted_index].key;
		changed_internal->pointers[changed_internal->Number_of_Keys].page_num = prime_pagenum;
		changed_internal->Number_of_Keys++;
		//we set left page information(take one_more_page of delete page, parent_key)
		internal_to_page(changed_internal,changed_page);
		Set_Buffer(table_id,change_internal_number,1,changed_page);
		//file_write_page(change_internal_number,changed_page);
		
		//file_read_page(prime_pagenum,changed_page);
		Get_Buffer(table_id,prime_pagenum,0,changed_page);
		page_to_internal(changed_page,changed_internal);
		if(changed_internal->Is_Leaf==0)
		{
			changed_internal->parent_page_num = change_internal_number;
			internal_to_page(changed_internal,changed_page);
			Set_Buffer(table_id,prime_pagenum,0,changed_page);
			//file_write_page(prime_pagenum,changed_page);
		}
		else
		{
			leaf_page* tmp_leaf = (leaf_page*)malloc(Page_size);
			page_to_leaf(changed_page,tmp_leaf);
			tmp_leaf->parent_page_num = change_internal_number;
			leaf_to_page(tmp_leaf,changed_page);
			Set_Buffer(table_id,prime_pagenum,0,changed_page);
			//file_write_page(prime_pagenum,changed_page);
			free(tmp_leaf);
		}
		//set information taked page of parent page

		free(changed_internal);
		free(changed_page);

		
		for(i=deleted_index+1;i<parent_internal->Number_of_Keys;i++)
		{
			parent_internal->pointers[i-1].key = parent_internal->pointers[i].key;
			parent_internal->pointers[i-1].page_num = parent_internal->pointers[i].page_num;
		}
		//set parent_internal page information
		file_free_page(deleted_number);//delete page
	}
	
	//remainder of code is same that delayed_merge_leaf
	parent_internal->Number_of_Keys--;
	internal_to_page(parent_internal,parent_page);
	Set_Buffer(table_id,parent_number,1,parent_page);
	Unpin_Buffer();
	//file_write_page(parent_number,parent_page);
	key_prime = parent_internal->pointers[0].key;
	prime_page_num = parent_internal->one_more_page_number;
	grand_parent_number = parent_internal->parent_page_num;

	if(parent_internal->Number_of_Keys!=0)
	{
		free(parent_internal);
		free(parent_page);
		return 0;
	}
	else if(parent_internal->Number_of_Keys==0)
	{
		if(grand_parent_number==0)
		{
			page_t* page = (page_t*)malloc(Page_size);
			page_t* head_page = (page_t*)malloc(Page_size);
			header_page* header = (header_page*)malloc(Page_size);
			internal_page* internal = (internal_page*)malloc(Page_size);

			//file_read_page(0,head_page); 
			Get_Buffer(table_id,0,1,head_page);
			page_to_header(head_page,header);

			Get_Buffer(table_id,parent_internal->one_more_page_number,1,page);
			//file_read_page(parent_internal->one_more_page_number,page);
			page_to_internal(page,internal);
			internal->parent_page_num = 0;
			internal_to_page(internal,page);
			Set_Buffer(table_id,parent_internal->one_more_page_number,1,page);
			//file_write_page(parent_internal->one_more_page_number,page);
			free(internal);

			header->root_page = parent_internal->one_more_page_number;
			header_to_page(header,head_page);
			Set_Buffer(table_id,0,1,head_page);
			//file_write_page(0,head_page);

			free(head_page);
			free(header);
			free(page);
			free(parent_internal);
			free(parent_page);

			file_free_page(parent_number);
			return 0;
		}
		else
		{
			int distribute_point;
			pagenum_t uncle_number;
			internal_page* grand_internal = (internal_page*)malloc(Page_size);
			internal_page* uncle_internal = (internal_page*)malloc(Page_size);
			page_t* grand_page = (page_t*)malloc(Page_size);
			page_t* uncle_page = (page_t*)malloc(Page_size);
	
			//file_read_page(grand_parent_number,grand_page);
			Get_Buffer(table_id,grand_parent_number,0,grand_page);
			page_to_internal(grand_page,grand_internal);
			if(key_prime<grand_internal->pointers[0].key)
			{
				distribute_point = 0;
			}
			else
			{
				distribute_point = -1;
				for(i = 0;i<grand_internal->Number_of_Keys;i++)
				{
					if(key_prime>=grand_internal->pointers[i].key)
					{
						distribute_point = i - 1;
					}
					else
					{
						break;
					}
				}
			}
			
			if(distribute_point==-1)
			{
				uncle_number = grand_internal->one_more_page_number;
			}
			else
			{
				uncle_number = grand_internal->pointers[distribute_point].page_num;		
			}
			free(grand_internal);
			free(grand_page);
			//file_read_page(uncle_number,uncle_page);
			Get_Buffer(table_id,uncle_number,0,uncle_page);
			page_to_internal(uncle_page,uncle_internal);

			if(uncle_internal->Number_of_Keys==Internal_factor-1)
			{
				free(uncle_internal);
				free(uncle_page);	
				free(parent_internal);
				free(parent_page);
				return delayed_redistribute_internal(table_id,grand_parent_number,key_prime,uncle_number);
			}			
			else if(uncle_internal->Number_of_Keys<Internal_factor-1)
			{
				free(uncle_internal);
				free(uncle_page);	
				free(parent_internal);
				free(parent_page);
				return delayed_merge_internal(table_id,grand_parent_number,key_prime,prime_page_num);
			}

		}
	}
}

int delayed_redistribute_internal(int table_id,pagenum_t parent_number,int64_t prime_key,pagenum_t uncle_number)
{
	int solo_position;
	pagenum_t solo_number, tmp_number;
	page_t* parent_page = (page_t*)malloc(Page_size);
	internal_page* parent_internal = (internal_page*)malloc(Page_size);
	page_t* uncle_page = (page_t*)malloc(Page_size);
	page_t* solo_page = (page_t*)malloc(Page_size);
	internal_page* uncle_internal = (internal_page*)malloc(Page_size);	
	internal_page* solo_internal = (internal_page*)malloc(Page_size);

	//file_read_page(parent_number,parent_page);
	Get_Buffer(table_id,parent_number,1,parent_page);
	page_to_internal(parent_page,parent_internal);
	//file_read_page(uncle_number,uncle_page);
	Get_Buffer(table_id,uncle_number,1,uncle_page);
	page_to_internal(uncle_page,uncle_internal);

	if(prime_key<parent_internal->pointers[0].key)
	{
		solo_position = -1;
	}
	else
	{
		solo_position = 0;
		for(int i =1;i<parent_internal->Number_of_Keys;i++)
		{
			if(prime_key>=parent_internal->pointers[i].key)	
			{
				solo_position = i;
			}
			else
			{
				break;
			}
		}
	}
	//find empty page that uncle page give one key and one page

	if(solo_position==-1)
	{
		solo_number = parent_internal->one_more_page_number;
	}
	else
	{	
		solo_number =parent_internal->pointers[solo_position].page_num;
	}
	//file_read_page(solo_number,solo_page);
	Get_Buffer(table_id,solo_number,1,solo_page);
	page_to_internal(solo_page,solo_internal);

	if(solo_position==-1) // left most case, page take one_more_page of uncle_page
	{
		solo_internal->pointers[0].key = parent_internal->pointers[0].key;
		solo_internal->pointers[0].page_num = uncle_internal->one_more_page_number;
		solo_internal->Number_of_Keys++;
		tmp_number = solo_internal->pointers[0].page_num;

		parent_internal->pointers[0].key = uncle_internal->pointers[0].key;
		
		uncle_internal->one_more_page_number = uncle_internal->pointers[0].page_num;
		for(int i =1;i<uncle_internal->Number_of_Keys;i++)
		{
			uncle_internal->pointers[i-1].key = uncle_internal->pointers[i].key;
			uncle_internal->pointers[i-1].page_num = uncle_internal->pointers[i].page_num;
		}
		uncle_internal->Number_of_Keys--;
	}
	else // not left most case, page take last_page of uncle_page
	{
		solo_internal->pointers[0].page_num = solo_internal->one_more_page_number;
		solo_internal->pointers[0].key = parent_internal->pointers[solo_position].key;
		solo_internal->one_more_page_number = uncle_internal->pointers[uncle_internal->Number_of_Keys-1].page_num;
		solo_internal->Number_of_Keys++;
		tmp_number = solo_internal->one_more_page_number;

		parent_internal->pointers[solo_position].key = uncle_internal->pointers[uncle_internal->Number_of_Keys-1].key;

		uncle_internal->Number_of_Keys--;
	}
	internal_to_page(parent_internal,parent_page);
	internal_to_page(solo_internal,solo_page);
	internal_to_page(uncle_internal,uncle_page);
	Set_Buffer(table_id,parent_number,1,parent_page);
	Set_Buffer(table_id,solo_number,1,solo_page);
	Set_Buffer(table_id,uncle_number,1,uncle_page);
	//file_write_page(parent_number,parent_page);
	//file_write_page(solo_number,solo_page);
	//file_write_page(uncle_number,uncle_page);
	
	free(parent_internal);
	free(parent_page);
	free(solo_internal);
	free(solo_page);
	free(uncle_internal);
	free(uncle_page);
	
	if(tmp_number!=0)
	{
		page_t* tmp_page = (page_t*)malloc(Page_size);
		internal_page* tmp_internal = (internal_page*)malloc(Page_size);
		leaf_page* tmp_leaf = (leaf_page*)malloc(Page_size);

		//file_read_page(tmp_number,tmp_page);
		Get_Buffer(table_id,tmp_number,0,tmp_page);
		page_to_internal(tmp_page,tmp_internal);
		if(tmp_internal->Is_Leaf==0)
		{
			tmp_internal->parent_page_num = solo_number;
			internal_to_page(tmp_internal,tmp_page);
			Set_Buffer(table_id,tmp_number,0,tmp_page);
			//file_write_page(tmp_number,tmp_page);
		}
		else
		{
			page_to_leaf(tmp_page,tmp_leaf);
			tmp_leaf->parent_page_num = solo_number;
			leaf_to_page(tmp_leaf,tmp_page);
			Set_Buffer(table_id,tmp_number,0,tmp_page);
			//file_write_page(tmp_number,tmp_page);
		}
		free(tmp_leaf);
		free(tmp_internal);
		free(tmp_page);
	}
	//set parent_number 

	Unpin_Buffer();
	return 0;
}

int db_delete(int table_id,int64_t key)//LibraryAPI_delete
{
	int return_number;
	uint64_t page_num;
	page_t* head_page = (page_t*)malloc(Page_size);
	header_page* header = (header_page*)malloc(Page_size);
	internal_page* internal;
	leaf_page* leaf;
	page_t* page;

	if(table_open_check(table_id)==-1)
	{
		free(head_page);
		free(header);
		//printf("Can't not found table\n");	
		return -1;
	}

	return_number = 0;
	//file_read_page(0,head_page); 
	Get_Buffer(table_id,0,0,head_page);
	page_to_header(head_page,header);
	page_num = header->root_page;
	free(header);
	free(head_page);

	if(page_num == 0) // no root page->empty tree
	{
		//printf("Error Delete\nEmpty B+ Tree\n");
		return_number = -1;
	}
	else
	{
		internal = (internal_page*)malloc(Page_size);
		page = (page_t*)malloc(Page_size);
		//file_read_page(page_num,page);
		Get_Buffer(table_id,page_num,0,page);
		page_to_internal(page,internal);
		while(TRUE)
		{
			if(internal->Is_Leaf==1)
			{
				break;
			}
			else if(internal->Is_Leaf==0)
			{
				page_num = internal->one_more_page_number;
				for(int i =0;i<internal->Number_of_Keys;i++)
				{
					if(key>=internal->pointers[i].key)
					{
						page_num = internal->pointers[i].page_num;
					}
					else if(key<internal->pointers[i].key)
					{
						break;
					}
				}
				//file_read_page(page_num,page);
				Get_Buffer(table_id,page_num,0,page);
				page_to_internal(page,internal);
			}
		}
		//find appropriate position about key		
		
		free(internal);
		leaf = (leaf_page*)malloc(Page_size);
		page_to_leaf(page,leaf);

		for(int i =0;i<leaf->Number_of_Keys;i++)
		{
			if(key==leaf->records[i].key) // there is key in the Tree, so we delete it in leaf_page
			{
				delete_in_leaf(table_id,key,page_num);
				break;
			}
			else if(key<leaf->records[i].key || i==leaf->Number_of_Keys-1) //there is no key in the tree
			{
				//printf("No key in Tree %ld\n",key);
				return_number = -1;
				break;
			}
		}

		free(leaf);
		free(page);
	}
	//printf("Deleted %ld\n",key);
	return return_number;
}
