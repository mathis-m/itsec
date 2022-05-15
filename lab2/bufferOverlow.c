#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

int counter = 0;
char username[16];

void win()
{
    printf("You win this round %s\n", username);
    counter++;
}

void loose()
{
    printf("You lose, better luck next time %s!\n\n", username);
    counter = 0;
}

int calculate(char *text, int input1, int input2, int input3, int number1, int number2, int number3)
{
    char name[16];

    if (strlen(text) >= sizeof name)
    {
        return 2;
    } 
    else {
        strcpy(name, text);
        if (number1 == input1 && number2 == input2 && number3 == input3)
            return 0;
        else
            return 1;
    }
}

int main(int argc, char **argv)
{
    int number1, number2, number3;
    int input1 = 0, input2 = 0, input3 = 0;

    printf("Please enter your name!\n");
    fgets(username, sizeof(username), stdin);

    while (counter < 5)
    {
        printf("Can you beat this minigame?\n\nEnter three numbers between 0-10 if you guess all correct you win, otherwise you lose!\n");

        printf("Enter your first guess!\n");
        scanf("%d", &input1, sizeof(number1));
        printf("Enter your second guess!\n");
        scanf("%d", &input2, sizeof(number2));
        printf("Enter your third guess!\n");
        scanf("%d", &input3, sizeof(number3));

        srand((unsigned int)time);

        number1 = rand() % 10;
        number2 = rand() % 10;
        number3 = rand() % 10;
        int res = calculate(argv[1], input1, input2, input3, number1, number2, number3);
        if (res == 0)
            win();
        else if(res == 2)
        {
            printf("Do not try to attempt a Buffer Overlow!\n");
            exit(0);
            return 0;
        }
        else
        {
            loose();
        }
    }

    printf("Against all odds you beat the game!\nCongratulation %s", username);

    exit(0);
    return 0;
}