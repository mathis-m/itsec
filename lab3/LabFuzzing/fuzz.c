#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define STDIN_FILENO    0
#define MAX_INPUT_LENGTH 64

void stringToHex(char*string) {

	char stringHex[24];
	for (int index = 0, j = 0; index < strlen(string); index++, j += 2) {
		sprintf(stringHex + j, "%02x", string[index] & 0xff);
	}
	printf(" The result in hex is: 0x%s\n", stringHex);
}

void head(char *string) {
	char * head;
	int len = strtol(string +3, &head, 10);
	head += 1;
	head[len] = '\0';
	printf(" Only the first %d characters shown: %s\n", len, head);
}

void stringToUpper(char*string) {
	char stringCapital[24];
	int index = 0; 
	for (; string[index] != '\0'; index++) {

		if (string[index] >= 'a' && string[index] <= 'z') {
			stringCapital[index] = string[index] - 32;
		}
		else
			stringCapital[index] = string[index];
	}
	stringCapital[index] = '\0';
	printf(" The result to upper case is: %s\n", stringCapital);
}

void stringToLower(char*string) {
	
	char *stringLowercase;
	
	//allocate Space for String
	if (strlen(string) - 1 == 0)
		stringLowercase = malloc(1 << 31);
	else 
		stringLowercase = malloc(strlen(string)+1);

	int index = 0;
	for (; string[index] != '\0'; index++) {

		if (string[index] >= 'A' && string[index] <= 'Z') {
			stringLowercase[index] = string[index] + 32;
		}
		else
			stringLowercase[index] = string[index];
	}
	stringLowercase[index-1] = '\0';
	printf(" The result in lower case is: %s\n", stringLowercase);
	free(stringLowercase);
}

void numberOfVowels(char*string) {
	
	int numberVowels = 0;
	for (int index = 0; string[index] != '\0'; index++) {

		if (string[index] == 'a' || string[index] == 'e' || string[index] == 'i' || string[index] == 'o' || string[index] == 'u')
			numberVowels++;
		else if (string[index] == 'A' || string[index] == 'E' || string[index] == 'I' || string[index] == 'O' || string[index] == 'U')
			numberVowels++;
		else if (string[index] == 'ä' || string[index] == 'ü' || string[index] == 'ö')
			numberVowels++;
		else if (string[index] == 'A' || string[index] == 'Ü' || string[index] == 'Ö')
			numberVowels++;

	}
	float percentage = (float) numberVowels/(strlen(string)-1)*100;
	int resutl = numberVowels / (strlen(string) - 1) * 100;

	printf(" The percentage of vowels is: %.2f%%\n", percentage);
}

void percentOfCharater(char *string, char character) {
	int numberCharacters = 0;
	for (int index = 0; string[index] != '\0' ; index++) {
		if (string[index] == character)
			numberCharacters++;
	}
	float percentage = numberCharacters / (strlen(string)-1) *100;
	percentage = (float)numberCharacters / (float)(strlen(string) - 1) * 100;
	printf(" The percentage of the character '%c' is: %.2f%%\n", 'a', percentage);
}

void deleteCharactersAtPosition(char * string, char * stringDeletedCharacters, int position ) {
	strcpy(stringDeletedCharacters, string);
	strcpy(&stringDeletedCharacters[position], &string[position+1]);
	printf(" The result after deleting the characters at position %d is: %s\n", position, stringDeletedCharacters);
}


void stringFormatBlock(char * string, char * stringBlock, int blockSize) {
	
	int position = 0;
	int modulo = 0;
	int counterSpaces = 0;
	for (int index = 0; string[position] != '\0'; index++) {
		if (modulo % blockSize == 0 && modulo == blockSize) {
			stringBlock[index] = ' ';
			modulo = 0;
			counterSpaces++;
		}
		else {
			stringBlock[index] = string[position];
			position++;
			modulo++;
		}
	}
	stringBlock[position + counterSpaces-1] = '\0';
	printf(" The formatted string with block width %d is: %s\n", blockSize, stringBlock);
}

void shiftByOffset(char *string, int offset) {
	char alphabet[] = { 'a', 'b', 'c', 'd', 'e' , 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
	char * alphabetTable = malloc(26);
	char * result = malloc(20);

	memcpy(alphabetTable, alphabet, sizeof(alphabet));

	int i = 0;
	for (; string[i] != '\0'; i++) {
		if (string[i] - 97 + offset > 26 | string[i] - 97 + offset < 0)
			result[i] = string[i];
		else
			result[i] = alphabetTable[string[i] - 97 + offset];
	}
	result[i] = '\0';
	printf(" The result with offset: %d is: %s\n", offset, result);
	free(result);
	free(alphabetTable);
}

void displayOptions() {
	printf(" This is a Text Utitlity Programm - you can enter a command from stdin.\n\n Input\t\t| Output\n ----------------------------------------------------------------------------\n h string\t| Hex Value of the input String.\n u string\t| Convert the input string to Uppercase\n l string\t| Convert the string to lowercase\n v string\t| Shows the percentage of vowels in the string\n p char string\t| Calculate percentage of the specified char in the text\n o n string\t| Shift your strings by n, only works for lowercase\n b size string\t| Format the string into blocks using the mentioned Blocksize\n dp pos string\t| Delete character at specified position\n\ -h n string\t| Show only the first n characters of the string\n\n");
}


int main(int argc, char **argv) {

	char input[MAX_INPUT_LENGTH];
	char stringDeletedCharacters[MAX_INPUT_LENGTH];
	char stringDeletedCharacter[MAX_INPUT_LENGTH];
	char stringBlock[MAX_INPUT_LENGTH];

	displayOptions();
	
	printf(" Enter your command: ");
	fgets(input, MAX_INPUT_LENGTH, stdin);

	if (strncmp(input, "h ", 2 )== 0)
		stringToHex(input+2);
	else if (strncmp(input, "l ", 2) == 0)
		stringToLower(input+2);
	else if (strncmp(input, "u ", 2) == 0)
		stringToUpper(input+2);
	else if (strncmp(input, "v ", 2) == 0)
		numberOfVowels(input+2);
	else if (strncmp(input, "p ", 2) == 0) {
		char character = input[2];
		percentOfCharater(input+4, character);
	}
	else if (strncmp(input, "b ", 2) == 0) {
		int blockSize = strtol(input+2,NULL, 10);
		stringFormatBlock(input+4, &stringBlock, blockSize);
	}
	else if (strncmp(input, "dp ", 2) == 0) {
		int position = strtol(input + 3, NULL, 10);
		deleteCharactersAtPosition(input+5, &stringDeletedCharacters, position);
	}
	else if (strcmp(input, "aaaa\n") == 0)	//Easter egg
		*(char *)1 = 2;
	else if (strncmp(input, "-h ", 3) == 0) {
		head(input);
	}
	else if (strncmp(input, "o ", 2) == 0) {
		int offset = strtol(input + 2, NULL, 10);
		shiftByOffset(input + 4, offset);
	}else{
		printf("Unknown Command: \"%s\"\n", input);
	}

	return 0;
}
