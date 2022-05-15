#include <stdio.h>
#include <stdlib.h>
#include <time.h>


int main() {

    srand (time(NULL));

    char firstName [32];
    unsigned int userInput = 0;
    unsigned int key =  random() %65536;

    char lastName[16];

    printf("Please enter your first name!\n");
    fgets(firstName, sizeof(firstName), stdin);

    printf("Please enter your last name!\n");
    fgets(lastName, sizeof(lastName), stdin);

    printf("Your name is:");
    printf(lastName);
    printf("\n");


    printf("Try to guess the secret number %s\n", &firstName);

    scanf("%d", &userInput);
        printf("%d\n", userInput);

    if(userInput==key)
        printf("Format String exploitation is really cool %s\n", &firstName);
    else
        printf("Try again!\n");

    return 0;

}