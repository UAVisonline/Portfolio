import sys
import math

global train_data
global attribute_list # attributes that except class label 
global attribute_label # class label
global dc_tree # dc tree Root

class attribute:
    def __init__(self,name):
        self.name = name
        self.values = []
        self.size = 0
        
    def add_value(self,value):
        if value not in self.values:
            self.values.append(value)
        self.size += 1
    
    def test_print(self):
        print(self.name,end = ' : ')
        for i in self.values:
            print(i,end = ', ')
        print()

class element:
    def __init__(self,attribute_value,result_attribute):
        self.attributes = attribute_value
        self.class_label = result_attribute
    
    def test_print(self):
        for i in self.attributes:
            print(i,end = ', ')
        print(self.class_label)

class node:
    def __init__(self,size): # size is attribute list size
        self.branch = True
        self.label = None #if not branch, label value exist ()
        self.attribute_index = None # what attribute used (represent index)
        self.elements = [] # element list in this node
        self.information = None # information gain of this node elements
        self.upward_attribute = [] # attribute that upward node used
        tmp = 0 # attribute numbers
        while tmp < size:
            self.upward_attribute.append(False)
            tmp += 1
        self.childs = [] # child nodes
        
    def set_upward(self,upward):
        self.upward_attribute = list_deep_copy(upward)
        
    def test_print(self):
        print(self.branch, end = '(')
        print(self.label,end = ',')
        print(self.elements,end=')')
        print(self.upward_attribute)
        for i in self.childs:
            i.test_print()

def list_deep_copy(value):
    ret_value = []
    for i in value:
        ret_value.append(i)
    return ret_value

def find_value_in_list(arr, value): # find value in arr and return index of value
    index = 0
    for i in arr:
        if i == value:
            return index
        index += 1
    return -1

def information_gain(arr): # get information gain 
    demo = 0
    for i in arr:
        demo += i
    
    gain = 0
    for i in arr:
        if i==0:
            continue
        gain -= (i/demo)*math.log2(i/demo)
    return gain
    
def arr_sum(arr): # get sum of list(arr)
    ret = 0
    for i in arr:
        ret += i
    return ret

def list_return_string(arr): # get string about output format (attribute)
    ret = ""
    size = 0
    for x in arr:
        if size == 0:
            ret += x
        elif size > 0:
            ret += ('\t' + x)
        size += 1
    ret += '\n'
    return ret
    
def make_tree(node_value): # make tree node
    global train_data
    global attribute_list
    global attribute_label
    
    branch_condition_1 = True
    label = train_data[node_value.elements[0]].class_label
    for i in node_value.elements:
        if label != train_data[i].class_label:
            branch_condition_1 = False
            break
    
    branch_condition_2 = True
    for i in node_value.upward_attribute:
        if  i == False:
            branch_condition_2 = False
            break

    if branch_condition_1==True or branch_condition_2==True:
        node_value.branch = False
        if branch_condition_1 == True:
            node_value.label = label
            return
        
        label_list = []
        size = 0
        while size < len(attribute_label.values):
            label_list.append(0)
            size += 1
        
        for i in node_value.elements:
            tmp_label = train_data[i].class_label
            index = find_value_in_list(attribute_label.values,tmp_label)
            label_list[index] += 1
        
        temp_max = -1
        temp_index = None
        index = 0
        while index < len(label_list):
            if label_list[index] > temp_max:
                temp_max = label_list[index]
                temp_index = index
            index += 1
        node_value.label = attribute_label.values[temp_index]
        return
    # make label value about tree_node (can't make child tree_node)

    dominator = len(node_value.elements)
    numerator = []
    size = 0
    while size < len(attribute_label.values):
        numerator.append(0)
        size += 1
    
    for tmp_element in node_value.elements:
        tmp_label = train_data[tmp_element].class_label
        index = find_value_in_list(attribute_label.values, tmp_label)
        numerator[index] += 1
    
    node_value.information_gain = information_gain(numerator) # get information gain about class label attribute (using element in node)
    
    index = 0
    save_index = None
    save_gain = None
    for i in node_value.upward_attribute:
        if i == False: # this attribute class not using that upward node 
            size = 0
            coefficient = []
            while size < len(attribute_list[index].values):
                coefficient.append([])
                size += 1
                
            size = 0
            while size < len(coefficient): 
                tmp_size = 0
                while tmp_size < len(attribute_label.values):
                    coefficient[size].append(0)
                    tmp_size += 1
                size += 1
            
            for tmp_element in node_value.elements: # make 2nd array
                tmp_attr = train_data[tmp_element].attributes[index]
                tmp_label = train_data[tmp_element].class_label
                index_first = find_value_in_list(attribute_list[index].values,tmp_attr)
                index_second = find_value_in_list(attribute_label.values,tmp_label)
                coefficient[index_first][index_second] += 1
            
            tmp_information = 0
            for co in coefficient:
                tmp_information += (arr_sum(co)/dominator)*information_gain(co)
            
            tmp_gain = node_value.information_gain - tmp_information # get information gain
            if save_gain == None or save_gain < tmp_gain:
                save_gain = tmp_gain
                save_index = index
                
        index += 1
        
    # make information gain about attribute class & check it maximum information gain
    node_value.attribute_index = save_index
    upward = list_deep_copy(node_value.upward_attribute)
    upward[save_index] = True
    
    size = 0
    while size < len(attribute_list[save_index].values):
        branch_node = node(len(node_value.upward_attribute))
        branch_node.set_upward(upward)
        node_value.childs.append(branch_node)
        size += 1
    
    for tmp_element in node_value.elements:
        tmp_attr = train_data[tmp_element].attributes[save_index]
        child_index = find_value_in_list(attribute_list[save_index].values,tmp_attr)
        node_value.childs[child_index].elements.append(tmp_element)
    # make child node
    
    for chi in node_value.childs:
        if len(chi.elements) > 0:
            make_tree(chi)
        elif len(chi.elements) == 0: # child node's element is empty
            label_list = []
            size = 0
            while size < len(attribute_label.values):
                label_list.append(0)
                size += 1
        
            for pos in node_value.elements:
                tmp_label = train_data[pos].class_label
                index = find_value_in_list(attribute_label.values,tmp_label)
                label_list[index] += 1
                
            temp_max = -1
            temp_index = None
            index = 0
            while index < len(label_list):
                if label_list[index] > temp_max:
                    temp_max = label_list[index]
                    temp_index = index
                index += 1
            chi.label = attribute_label.values[temp_index]
            chi.branch = False            
            # get label that parent node and child label init it
            
    
def make_label_to_string(tree_node,element):
    global attribute_list 
    
    if tree_node.label != None:
        element.class_label = tree_node.label
        ret = ""
        size = 0
        for x in element.attributes:
            if size == 0:
                ret += x
            elif size>0:
                ret += ('\t' + x)
            size += 1
        ret += ('\t' + element.class_label)
        ret += '\n'
        return ret
    
    node_index = tree_node.attribute_index
    element_index = find_value_in_list(attribute_list[node_index].values,element.attributes[node_index])
    return make_label_to_string(tree_node.childs[element_index],element)   

def main():
    train_data_name = sys.argv[1]
    input_data_name = sys.argv[2]
    output_data_name = sys.argv[3]
    
    global train_data
    global attribute_list
    global attribute_label
    global dc_tree
    
    train_file = open(train_data_name,'r')
    train_list = train_file.readlines()
    train_size = len(train_list)-1
    
    train_data = []
    attribute_list = []
    attribute_label = None
    attribute_line = train_list[0]
    name = ""
    
    for x in attribute_line: # make attribute list and class label
        if x != '\t' and x != '\n':
            name += x
        elif x == '\t':
            attr = attribute(name)
            attribute_list.append(attr)
            name = ""
        elif x=='\n':
            attr =attribute(name)
            attribute_label = attr
            name = ""
    
    size = 1
    train_data_number = [] # number of elements
    
    while size < len(train_list): # make element that train_data 
        index = 0
        attribute_value = []
        label_value = None
        for x in train_list[size]:
            if x != '\t' and x != '\n':
                name += x
            elif x == '\t':
                attribute_value.append(name)
                attribute_list[index].add_value(name) # add attribute value
                name = ""
                index += 1
            elif x=='\n':
                label_value = name
                attribute_label.add_value(name) # add class_label value
                elem = element(attribute_value,label_value)
                train_data.append(elem) # in-memory data insert 
                train_data_number.append(size-1) # number of elements insert
                name = ""
        size += 1
    train_file.close()
    # make in-memory data about train_data using (input file sys.argv[1])


    dc_tree = node(len(attribute_list))
    dc_tree.elements = list_deep_copy(train_data_number) # node element = number of elements (all element in train_data inserted root node)
    make_tree(dc_tree)
    
    input_file = open(input_data_name,'r')
    output_file = open(output_data_name,'w')
    input_list = input_file.readlines()
    size = 0
    print_attribute = []
    name = ""
    while size < len(input_list):
        if size==0: # write output file that attribute catergory and class_label category
            for x in input_list[size]:
                if x != '\t' and x != '\n':
                    name += x
                elif x == '\t':
                    print_attribute.append(name)
                    name = ""                 
                elif x=='\n':
                    print_attribute.append(name)
                    print_attribute.append(attribute_label.name)
                    output_file.write(list_return_string(print_attribute))
                    name = ""
        elif size > 0: # write output file that element attribute and class_label
            attribute_value = []
            label_value = None
            for x in input_list[size]:
                if x != '\t' and x != '\n':
                    name += x
                elif x == '\t':
                    attribute_value.append(name)
                    name = ""
                elif x == '\n':
                    attribute_value.append(name)
                    elem = element(attribute_value,label_value)
                    name = ""
                    output_file.write(make_label_to_string(dc_tree,elem))
        size += 1
    input_file.close()
    output_file.close()
    #dc_tree.test_print()

if __name__ == '__main__':
    main()