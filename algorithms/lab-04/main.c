#include <stdio.h>
#include <stdbool.h>

bool check_parens(char *s) {
    char stack[32];
    int top = -1;

    char bracket_map[256] = {0};
    bracket_map['('] = ')';
    bracket_map['['] = ']';
    bracket_map['{'] = '}';

    for (int i = 0; s[i] != '\0'; i++) {
        char ch = s[i];
        if (ch == '(' || ch == '[' || ch == '{') {
            stack[++top] = ch;
        } else {
            if (top == -1 || bracket_map[(unsigned char)stack[top--]] != ch) {
                return false;
            }
        }
    }

    return top == -1;
}

int find_duplicate(int* nums) {
    int tortoise = nums[0];
    int hare = nums[0];
    do {
        tortoise = nums[tortoise];
        hare = nums[nums[hare]];
    } while (tortoise != hare);

    tortoise = nums[0];
    while (tortoise != hare) {
        tortoise = nums[tortoise];
        hare = nums[hare];
    }

    return hare;
}

int main(void) {
    char parens[32] = "";

    printf("Enter string with parens -> ");
    scanf("%s", parens);

    bool is_balanced = check_parens(parens);
    if (is_balanced) {
        printf("String '%s' is a valid sequence of parens\n", parens);
    }
    else {
        printf("String '%s' is incorrect sequence of parens\n", parens);
    }

    return 0;
}
