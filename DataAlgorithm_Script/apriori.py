# 2016025069 Hwang Hui Su

import sys
import numpy as np

global support #support probability
global confidence #confidence probability
global input_srting #input file name
global output_string #output file name
global transactions #item in transaction entry
global transactions_size #transaction entry size

class itemset_sup: #itemset related frequent pattern
    def __init__(self, item, sup):
        self.item = item
        self.sup = sup
        
    def test_print(self):
        print(self.item,end=' : ')
        print(self.sup)
        
    def get_item(self):
        return self.item
        
    def get_sup(self):
        return self.sup

class temp_tuple:
    def __init__(self,x,y,index):
        self.x = x
        self.y = y
        self.index = index
        
    def get_x(self):
        return self.x
    
    def get_y(self):
        return self.y

    def get_index(self):
        return self.index

    def set_x(self,x):
        self.x = x
        
    def set_y(self,y):
        self.y = y

    def set_index(self,index):
        self.index = index
        
    def test_print(self):
        print(self.x,end=' : ')
        print(self.y)

class association_itemset: #itemset related association
    def __init__(self,x,y,sup,conf):
        self.x = x
        self.y = y
        self.sup = sup
        self.conf = conf
        
    def set_conf(self,value):
        self.conf = value
    
    def get_x(self):
        return self.x

    def get_y(self):
        return self.y
    
    def get_sup(self):
        return self.sup
    
    def get_conf(self):
        return self.conf
    
    def test_print(self):
        print(self.x,end=' , ')
        print(self.y,end=' : ')
        print(self.sup,end=' /// ')
        print(self.conf)

def list_deep_copy(arr) : # COPY arr in the new list (different memory)
    ans = []
    for x in arr:
        ans.append(x)
    return ans
    
def compare_two_array(arr1,arr2):# arr1 is in the arr2 ?
    for x in arr1:
        check = False
        for y in arr2:
            if x == y:
                check = True
        if check==False:
            return False
    return True
    
def compart_only_one(x,arr2): # x in the arr2 ?
    for y in arr2:
        if x==y: return True
    return False
 
'''def same_array(arr1,arr2): # two array content is same ?
#    x = len(arr1)
#    y = len(arr2)
#    tmp_x = 0
#    tmp_y = 0
#    if x == y:
#        while tmp_x < x:
#            if arr1[tmp_x] != arr2[tmp_y]:
#                return False
#            tmp_x+=1
#            tmp_y+=1
#        return True
#    return False'''
    
def compare_array_of_array(array_array, arr): # if arr in the array_array ?
    for x in array_array:
        #if same_array(x,arr)==True: # arr is in the x (array_array) -> awful part (so much time wasted) 
        if (x==arr)==True:
            return False
    return True
    
def extract_arr(array_array,arr): # extract two dimensional array in array_array that include arr
    ans = []
    for x in array_array:
        if compare_two_array(arr,x)==True:
            ans.append(x)
    return ans

def scan(itemset,iteration):
    global transactions
    global transactions_size
    global support
    global output_string
    
    satisfied_itemset = [] # satisfied itemset making scan
    unsatisfied_itemset = []
    for x in itemset: 
        item = x.get_item() # get items
        tmp_number = 0 
        for transaction in transactions: # visit transaction in transaction list
            if compare_two_array(item,transaction) == True: # if items in transaction
                tmp_number += 1 # +1 support
        if (tmp_number/transactions_size) >= support: # support satisfied support probability
            new_item = itemset_sup(item,round(tmp_number/transactions_size,4)) # make new itemset class (have support) [item content is same, only just updated support]
            satisfied_itemset.append(new_item)  # append it in new itemset list
        else :
            new_item = itemset_sup(item,round(tmp_number/transactions_size,4))
            unsatisfied_itemset.append(new_item) # this itemset can't be satisfied_itemset

    if iteration>=2 and len(satisfied_itemset)>0:
        get_association_support(satisfied_itemset) # get association support

    if len(satisfied_itemset) >= 2 : # get next support itemset, first make itemset that size + 1
        satisfied = []
        unsatisfied = []
        for x in satisfied_itemset:
            satisfied.append(x.get_item())
        for x in unsatisfied_itemset:
            unsatisfied.append(x.get_item())
            
        next_size_itemset = get_next_itemset(satisfied,unsatisfied)
        scan(next_size_itemset,iteration+1) # and go scan using new_itemset and iteration+1
  
def get_next_itemset(satisfied, unsatisfied):
    new_list_itemset = [] # list of new itemset (that using condition)
    new_itemset = [] # list of new itemset
    
    size = len(satisfied)
    first = 0
    second = 0
    while first < size and second < size: # access all elements in satisfied itemset using loop
        if first == second : # don't same first, second
            second+=1
            if second >= size:
                first += 1
                second = 0
        
        if first >= size: # case : first greater than size variable in loop
            break
        
        for x in satisfied[second]: # get satisfied itemset that index value is second
            temp_list = list_deep_copy(satisfied[first]) # make new list that itemset index value is first
            
            if compart_only_one(x,temp_list)==False: # if value of Line 183 itemset not in the Line 184 itemset
                temp_list.append(x) # new item set making
                temp_list.sort()
                tmp_size = len(unsatisfied)
                tmp = 0
                
                if tmp_size==0:
                    if compare_array_of_array(new_list_itemset,temp_list)==True : # condition is not problem : we don't make new itemset that before
                        temp = itemset_sup(temp_list,0.0) 
                        new_list_itemset.append(temp_list)
                        new_itemset.append(temp)
                elif tmp_size>0:
                    for y in unsatisfied: 
                        if compare_two_array(y,temp_list)==True: # condition is not problem : unsatisfied itemset not included in new item set
                            break;
                        else:
                            tmp+=1
                            if tmp >= tmp_size and compare_array_of_array(new_list_itemset,temp_list)==True : # condition is not problem : we don't make new itemset that before
                                temp = itemset_sup(temp_list,0.0) 
                                new_list_itemset.append(temp_list)
                                new_itemset.append(temp)
        
        second+=1
        if second >= size:
            first += 1
            second = 0
        
    return new_itemset
 
def get_association_support(satisfied):
    global transactions
    global support
    global confidence
    global output_string
    
    for x in satisfied: # access element of satisfied itemset
        arr = x.get_item()
        arr_sup = x.get_sup()
        arr_size = len(arr)
        
        tmp_size = 0
        tuple_list = []
        
        while tmp_size < arr_size: # make tuple for association rule using loop
            copy_list = list_deep_copy(arr) # make Y
            tmp = arr[tmp_size] # make X
            copy_list.remove(tmp) # remove X value in Y
            new_tuple = temp_tuple([tmp],copy_list,tmp_size) # make tuple that have index (value X position)
            tuple_list.append(new_tuple)
            tmp_size+=1
        
        tmp_size = 0
        while tmp_size < len(tuple_list):
            tuple_object = tuple_list[tmp_size] # get tuple and X, Y
            left_list = tuple_object.get_x()
            right_list = tuple_object.get_y()
            
            if len(left_list)!=0 and len(right_list)!=0: # X and Y is not empty
                temp_total_arr = extract_arr(transactions,left_list) # find transactions that include X
                tmp_number = 0
                for temp_transaction in temp_total_arr: 
                    if compare_two_array(right_list,temp_transaction) == True: # find transactions that include X and Y
                        tmp_number += 1
                if ( tmp_number/len(temp_total_arr) ) >= confidence: # if confidence satisfy target percent
                    item = association_itemset(left_list,right_list,arr_sup,round(tmp_number/len(temp_total_arr),4)) # make association rule
                    # item.test_print()
                    save_association(item) # and use it to write output file
                        
            index = tuple_object.get_index()
            
            while index < len(right_list): # loop Y using index value
                tmp_left = list_deep_copy(left_list)
                tmp_right = list_deep_copy(right_list)
                tmp_left.append(right_list[index]) # new X
                tmp_right.remove(right_list[index]) # new Y
                new_tuple = temp_tuple(tmp_left,tmp_right,index) # and new Tuple
                tuple_list.append(new_tuple) # tuple list +1
                index += 1
            
            tmp_size += 1
    
def save_association(item): # write output file that information of association itemset
    global output_string
    
    output_file = open(output_string,'a')
    output_file.write("{")
    
    index = 0
    x = item.get_x()
    y = item.get_y()
    sup = round(item.get_sup()*100,2)
    conf = round(item.get_conf()*100,2)
    
    while index < len(x):
        output_file.write(str(x[index]))
        index+=1
        if index < len(x):
            output_file.write(",")
    
    output_file.write("}\t")
    output_file.write("{")
    index = 0
    
    while index < len(y):
        output_file.write(str(y[index]))
        index+=1
        if index < len(y):
            output_file.write(",")
            
    output_file.write("}\t")
    output_file.write(str(sup))
    output_file.write('\t')
    output_file.write(str(conf))
    output_file.write('\n')
    
    output_file.close()
    
 
def main():
    global support
    global confidence
    global input_srting
    global output_string
    global transactions
    global transactions_size
    
    support = float(sys.argv[1])/100 # read second argument(5) and divide it one hundred (5% -> 0.05)
    confidence = support
    input_string = sys.argv[2] # read third argument(input.txt)
    output_string = sys.argv[3] # read fourth argument (output.txt)
    
    input_file = open(input_string,'r') # read file
    input_list = input_file.readlines() # read all list in file
    transactions_size = len(input_list) # get number of transaction size
    transactions = [] # make list that store transaction
    items = [] # make list that itemset size 1
    itemset = [] # make list related upper itemset(items)
    
    for transaction in input_list: # get & make transaction in input_list
        arr = []
        number = 0
        
        for x in transaction:
            if (x != '\n' and x !='\t'): # if member isn't ('\t','\n')
                number = (number*10) + int(x) # read number
            else : #number is done
                arr.append(number) # append number about the transaction
                if number not in items:  # if number not in items(size 1)
                    items.append(number) # append number in items
                number = 0
        
        if(number!=0) : # case : last number in last transaction
            arr.append(number) # append number about the transaction
            if number not in items: # if number not in items(size 1)
                items.append(number) # append number in items
            number = 0
            
        arr.sort() # sort arr(transaction)
        transactions.append(arr) # append arr(transaction) in transaction list
    
    items.sort() # sort itemset(size 1)

    for x in items: # read itemset and make it (class itemset_sup) & append it
        arr = [] 
        arr.append(x) 
        tmp = itemset_sup(arr,0.0)  
        itemset.append(tmp) 
    
    input_file.close()
    
    output_file = open(output_string,'w')
    output_file.close()
    
    scan(itemset,1) # 1st scan start 

if __name__ == '__main__':
    main()
