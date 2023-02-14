.SUFFIXES: .c .o

CC=gcc

SRCDIR=src/
INC=include/
LIBS=lib/

# SRCS:=$(wildcard src/*.c)
# OBJS:=$(SRCS:.c=.o)

# main source file
TARGET_SRC:=$(SRCDIR)main.c
TARGET_OBJ:=$(SRCDIR)main.o

# Include more files if you write another source file.
SRCS_FOR_LIB:=$(SRCDIR)bpt.c $(SRCDIR)file.c $(SRCDIR)buf.c $(SRCDIR)page.c $(SRCDIR)lock.c
OBJS_FOR_LIB:=$(SRCS_FOR_LIB:.c=.o)



CFLAGS+= -g -fPIC -I $(INC)
CFLAGS+= -lpthread

TARGET=main

all: $(TARGET)

$(TARGET): $(TARGET_OBJ)
	$(CC) $(CFLAGS) -o $(SRCDIR)bpt.o -c $(SRCDIR)bpt.c
	$(CC) $(CFLAGS) -o $(SRCDIR)file.o -c $(SRCDIR)file.c
	$(CC) $(CFLAGS) -o $(SRCDIR)buf.o -c $(SRCDIR)buf.c
	$(CC) $(CFLAGS) -o $(SRCDIR)page.o -c $(SRCDIR)page.c
	$(CC) $(CFLAGS) -o $(SRCDIR)lock.o -c $(SRCDIR)lock.c
	make static_library
	$(CC) $(CFLAGS) -o $@ $^ -L $(LIBS) -lbpt

clean:
	rm $(TARGET) $(TARGET_OBJ) $(OBJS_FOR_LIB) $(LIBS)*

library:
	gcc -shared -Wl,-soname,libbpt.so -o $(LIBS)libbpt.so $(OBJS_FOR_LIB)

static_library:
	ar cr $(LIBS)libbpt.a $(OBJS_FOR_LIB)
