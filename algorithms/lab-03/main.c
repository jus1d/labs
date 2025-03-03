#include <stdio.h>
#include <stdlib.h>

typedef struct Node Node;

struct Node {
    int value;
    Node *next;
};

Node *ll_from_array(int *xs, size_t xs_sz) {
    if (xs_sz == 0) return NULL;

    Node *head = malloc(sizeof(Node));
    Node *cur = head;
    for (size_t i = 0; i < xs_sz; ++i) {
        cur->value = xs[i];
        if (i != xs_sz-1) {
            cur->next = malloc(sizeof(Node));
        }
        cur = cur->next;
    }
    return head;
}

void ll_print(Node *head) {
    Node *cur = head;

    if (cur == NULL) return;

    printf("%d", cur->value);
    cur = cur->next;

    while (cur != NULL) {
        printf(" -> %d", cur->value);
        cur = cur->next;
    }
    printf("\n");
}

Node *ll_copy(Node *head) {
    if (head == NULL) return NULL;

    Node *copied = malloc(sizeof(Node));
    Node *c1 = head;
    Node *c2 = copied;

    while (c1 != NULL) {
        c2->value = c1->value;
        if (c1->next != NULL) {
            c2->next = malloc(sizeof(Node));
        }

        c1 = c1->next;
        c2 = c2->next;
    }

    return copied;
}

void copy_demo(void) {
    int xs[] = {1, 2, 3, 4, 5};
    Node *head = ll_from_array(xs, sizeof(xs) / sizeof(xs[0]));

    printf("Initial list: ");
    ll_print(head);

    Node *copied = ll_copy(head);

    printf("Copied list:  ");
    ll_print(copied);
}

int main(void) {
    copy_demo();
    return 0;
}
