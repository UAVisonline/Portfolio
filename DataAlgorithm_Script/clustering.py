import sys
import math

global input_name
global pts
global number_of_cluster
global radius
global min_pts
global matrix
global cluster_pt_area

global min_number_x
global max_number_x
global min_number_y
global max_number_y
global max_mat_x
global max_mat_y

class point:
    def __init__(self,number,x,y):
        self.number = number
        self.x = x
        self.y = y
        self.matrix_x = -1
        self.matrix_y = -1
        self.cluster = -1
        self.outlier = True
    
    def get_number(self):
        return self.number
    
    def get_x(self):
        return self.x
        
    def get_y(self):
        return self.y
    
    def get_mat_x(self):
        return self.matrix_x

    def get_mat_y(self):
        return self.matrix_y
    
    def get_cluster(self):
        return self.cluster
    
    def get_outlier(self):
        return self.outlier
    
    def set_matrix_position(self,mat_x,mat_y):
        self.matrix_x = mat_x
        self.matrix_y = mat_y
        
    def set_cluster(self,cluster):
        self.cluster = cluster
     
    def false_outlier(self):
        self.outlier = False
    
    def test_print(self):
        print(self.number, end = '(')
        print(self.x, end = '/')
        print(self.y, end = '){')
        print(self.matrix_x, end = ',')
        print(self.matrix_y, end = '}\n')
    
class matrix_element:
    def __init__(self,x,y,min_x,max_x,min_y,max_y):
        self.points = []
        self.count = 0
        self.x = x
        self.y = y
        self.min_x = min_x
        self.min_y = min_y
        self.max_x = max_x
        self.max_y = max_y
        
    def pt_insert(self,pt_element):
        self.points.append(pt_element)
        self.count += 1
    
    def return_pts(self):
        return self.points
        
    def test_print(self):
        print(str(self.x) + "   " + str(self.y) + "///" + str(self.count) + '[' + str(self.min_x) + ',' + str(self.max_x) + '],[' + str(self.min_y) + ',' + str(self.max_y)+']')
        for pt in self.points:
            pt.test_print()
        #print()

def insert_pts_matrix():
    global radius
    global matrix
    global pts
    
    global min_number_x
    global min_number_y

    num = 0
    for pt in pts:
        correction_x = pt.get_x() - min_number_x
        correction_y = pt.get_y() - min_number_y
        
        correction_x = int(correction_x/radius)
        correction_y = int(correction_y/radius)
        
        pt.set_matrix_position(correction_x,correction_y)
        matrix[correction_y][correction_x].pt_insert(pt)
        num += 1
            
def db_scan_core_point():
    global input_name
    global pts
    global number_of_cluster
    global radius
    global min_pts
    global matrix
    global cluster_pt_area
    
    global max_mat_x
    global max_mat_y
    
    cluster_number = 0
    for pt in pts:
        if cluster_number >= number_of_cluster:
            break
        
        if pt.get_cluster()==-1 and pt.get_outlier()==True:
            search_mat_x = []
            search_mat_y = []
            search_mat_x.append(pt.matrix_x)
            search_mat_y.append(pt.matrix_y)
            
            if pt.get_mat_x()>0:
                search_mat_x.append(pt.matrix_x-1)
            if pt.get_mat_y()>0:
                search_mat_y.append(pt.matrix_y-1)
            if pt.get_mat_x()<max_mat_x:
                search_mat_x.append(pt.matrix_x+1)
            if pt.get_mat_y()<max_mat_y:
                search_mat_y.append(pt.matrix_y+1)
                
                
            number_of_pts = 0
            current_x = pt.x
            current_y = pt.y
            cluster_pt_area = []
            
            for y in search_mat_y:
                for x in search_mat_x:
                    for element in matrix[y][x].return_pts():
                        x_dis = element.x - current_x
                        y_dis = element.y - current_y
                        distance = math.sqrt((x_dis*x_dis) + (y_dis*y_dis))
                        
                        if distance <= radius:
                            cluster_pt_area.append(element)
                            number_of_pts += 1
            
            if number_of_pts >= min_pts:
                for tmp in cluster_pt_area:
                    tmp.set_cluster(cluster_number)
                    tmp.false_outlier()
                
                size = 0
                while size < len(cluster_pt_area):
                    db_scan_check_point(cluster_pt_area[size],cluster_number)
                    size += 1
                    
                output_file = open(input_name+"_cluster_"+str(cluster_number) + ".txt",'w')
                for tmp in cluster_pt_area:
                    output_file.write(str(tmp.get_number()) + '\n')
                    #output_file.write(str(tmp.get_number()) + '/' + str(tmp.x) + ',' + str(tmp.y) +'\n')
                output_file.close()
                
                cluster_number += 1
                
                

def db_scan_check_point(pt,cluster_number):
    global radius
    global min_pts
    global matrix
    global cluster_pt_area
    
    global max_mat_x
    global max_mat_y
    
    search_mat_x = []
    search_mat_y = []
    search_mat_x.append(pt.matrix_x)
    search_mat_y.append(pt.matrix_y)
            
    if pt.get_mat_x()>0:
        search_mat_x.append(pt.matrix_x-1)
    if pt.get_mat_y()>0:
        search_mat_y.append(pt.matrix_y-1)
    if pt.get_mat_x()<max_mat_x:
        search_mat_x.append(pt.matrix_x+1)
    if pt.get_mat_y()<max_mat_y:
        search_mat_y.append(pt.matrix_y+1)
        
    number_of_pts = 0
    current_x = pt.x
    current_y = pt.y

    tmp_area = []
    test_number = 0

    for y in search_mat_y:
        for x in search_mat_x:
            for element in matrix[y][x].return_pts():
                x_dis = element.x - current_x
                y_dis = element.y - current_y
                distance = math.sqrt((x_dis*x_dis) + (y_dis*y_dis))              

                if distance <= radius:
                    tmp_area.append(element)
                    number_of_pts += 1
        
        if number_of_pts >= min_pts:
            for tmp in tmp_area:
                if tmp.get_cluster() == -1 and tmp.get_outlier() == True:
                    tmp.set_cluster(cluster_number)
                    tmp.false_outlier()
                    cluster_pt_area.append(tmp)
      

def main():
    global input_name
    global number_of_cluster
    global radius
    global min_pts
    global pts
    global matrix

    input_name = sys.argv[1]
    input_name = input_name[:-4]
    number_of_cluster = int(sys.argv[2])
    radius = float(sys.argv[3])
    min_pts = int(sys.argv[4])
    pts = []
    matrix = []
    
    input_file = open(input_name + ".txt",'r')
    point_list = input_file.readlines()
    
    global min_number_x
    global max_number_x
    global min_number_y
    global max_number_y
    
    min_number_x = None
    max_number_x = None
    min_number_y = None
    max_number_y = None
    
    for x in point_list:
        first_section = -1
        second_section = -1
        section_count = 0
        for y in x:
            if y == '\t':
                if first_section == -1:
                    first_section = section_count
                else:
                    second_section = section_count
            elif y == '\n':
                number = int(x[0:first_section])
                x_number = float(x[first_section+1:second_section])
                y_number = float(x[second_section+1:section_count])
                
                if min_number_x==None:
                    min_number_x = x_number
                elif min_number_x > x_number:
                    min_number_x = x_number

                if max_number_x==None:
                    max_number_x = x_number
                elif max_number_x < x_number:
                    max_number_x = x_number

                if min_number_y==None:
                    min_number_y = y_number
                elif min_number_y > y_number:
                    min_number_y = y_number

                if max_number_y==None:
                    max_number_y = y_number
                elif max_number_y < y_number:
                    max_number_y = y_number
                    
                pt = point(number,x_number,y_number)
                pts.append(pt)
                
            section_count+=1
            
    input_file.close()
    
    #for x in pts:
    #    x.test_print()
    #print(min_number_x)
    #print(max_number_x)
    #print(min_number_y)
    #print(max_number_y)
    
    global max_mat_x
    global max_mat_y
    
    tmp_max_x = max_number_x - min_number_x
    max_mat_x = int(tmp_max_x/radius)
    
    tmp_max_y = max_number_y - min_number_y
    max_mat_y = int(tmp_max_y/radius)
    #print(x_count)
    #print(y_count)    
    
    tmp_y = 0
    tmp_x = 0
    while tmp_y <= max_mat_y:
        row = []
        tmp_x = 0
        while tmp_x <= max_mat_x:
            mat = matrix_element(tmp_x,tmp_y, min_number_x+tmp_x*radius, min_number_x+(1+tmp_x)*radius, min_number_y+tmp_y*radius, min_number_y+(1+tmp_y)*radius)
            row.append(mat)
            tmp_x += 1
        matrix.append(row)
        tmp_y += 1
    
    insert_pts_matrix()
        
    db_scan_core_point()
    
    #for y in matrix:
    #    for x in y:
    #        x.test_print()
        
if __name__ == '__main__':
    main()