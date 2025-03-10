#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define shift(xs, xs_sz) ((xs_sz)--, *(xs)++)

typedef struct Node Node;

struct Node {
    int value;
    Node *next;
};

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

void ll_remove_duplicates(Node *head) {
    if (head == NULL) return;

    Node *i = head;
    while (i != NULL) {
        Node *prev = i;
        Node *j = i->next;

        while (j != NULL) {
            if (i->value == j->value) {
                prev->next = j->next;
                free(j);
                j = prev->next;
            }
            else {
                prev = j;
                j = j->next;
            }
        }
        i = i->next;
    }
}

Node* ll_find_cycle_head(Node *head) {
    if (head == NULL) return NULL;

    Node *slow = head;
    Node *fast = head;

    while (fast != NULL && fast->next != NULL) {
        slow = slow->next;
        fast = fast->next->next;
        if (slow == fast) {
            break;
        }
    }

    if (fast == NULL || fast->next == NULL) {
        return NULL;
    }

    slow = head;
    while (slow != fast) {
        slow = slow->next;
        fast = fast->next;
    }

    return slow;
}

Node* node_create(int val) {
    Node *new_node = malloc(sizeof(Node));
    if (new_node == NULL) {
        fprintf(stderr, "Buy more RAM :c\n");
        exit(EXIT_FAILURE);
    }
    new_node->value = val;
    new_node->next = NULL;
    return new_node;
}

void ll_print(Node *head) {
    if (head == NULL) {
        printf("List is empty.\n");
        return;
    }

    Node *cycle_head = ll_find_cycle_head(head);

    if (cycle_head == NULL) {
        Node *curr = head;
        while (curr != NULL) {
            printf("%d -> ", curr->value);
            curr = curr->next;
        }
        printf("NULL\n");
        return;
    }

    Node *curr = head;
    while (curr != cycle_head) {
        printf("%d -> ", curr->value);
        curr = curr->next;
    }

    printf("[");
    Node *cycle_curr = cycle_head;
    do {
        printf("%d", cycle_curr->value);
        cycle_curr = cycle_curr->next;
        if (cycle_curr != cycle_head) {
            printf(" -> ");
        }
    } while (cycle_curr != cycle_head);
    printf("]\n");
}

Node* ll_read_cycled(void) {
    int n;

    printf("Enter the number of nodes -> ");
    if (scanf("%d", &n) != 1 || n <= 5) {
        fprintf(stderr, "Invalid number of nodes\n");
        exit(1);
    }

    Node *head = NULL;
    Node *tail = NULL;

    Node **nodes = malloc(n * sizeof(Node*));
    if (!nodes) {
        fprintf(stderr, "Buy more RAM :c\n");
        exit(1);
    }

    for (int i = 0; i < n; i++) {
        int value;
        if (scanf("%d", &value) != 1) {
            fprintf(stderr, "Invalid input\n");
            free(nodes);
            exit(1);
        }
        Node *node = node_create(value);
        nodes[i] = node;

        if (head == NULL) {
            head = tail = node;
        } else {
            tail->next = node;
            tail = node;
        }
    }

    int mid = n / 2;
    tail->next = nodes[mid];
    free(nodes);
    return head;
}

Node* ll_read(void) {
    int n;
    printf("Enter the number of nodes -> ");
    if (scanf("%d", &n) != 1 || n < 1) {
        fprintf(stderr, "Invalid number of nodes.\n");
        exit(EXIT_FAILURE);
    }

    Node *head = NULL, *tail = NULL;

    for (int i = 0; i < n; i++) {
        int value;
        if (scanf("%d", &value) != 1) {
            fprintf(stderr, "Invalid value input.\n");
            exit(1);
        }
        Node *node = node_create(value);
        if (head == NULL) {
            head = tail = node;
        } else {
            tail->next = node;
            tail = node;
        }
    }

    return head;
}

void cycled_ll_demo(void) {
    Node *head = ll_read_cycled();

    ll_print(head);
}

void copy_demo(void) {
    Node *head = ll_read();

    printf("Initial list: ");
    ll_print(head);

    Node *copied = ll_copy(head);

    printf("Copied list:  ");
    ll_print(copied);
}

void remove_dup_demo(void) {
    Node *head = ll_read();

    printf("Initial list: ");
    ll_print(head);

    ll_remove_duplicates(head);

    printf("List of distinct elements: ");
    ll_print(head);
}

int main(int argc, char **argv) {
    char *program = shift(argv, argc);

    if (argc < 1) {
        fprintf(stdout, "Usage: %s [ cycle | copy | dup ]\n", program);
        return 1;
    }

    char *subcommand = shift(argv, argc);
    if (strcmp(subcommand, "cycle") == 0) {
        cycled_ll_demo();
    }
    else if (strcmp(subcommand, "copy") == 0) {
        copy_demo();
    }
    else if (strcmp(subcommand, "dup") == 0)
    {
        remove_dup_demo();
    }

    return 0;
}
