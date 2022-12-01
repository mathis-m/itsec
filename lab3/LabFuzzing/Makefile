CFLAGS ?= -g -w -std=c99


all: fuzz 

clean:
	rm -f fuzz 

test: test.c
	${CC} ${CFLAGS} fuzz.c -o fuzz
