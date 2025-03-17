#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define shift(xs, xs_sz) ((xs_sz)--, *(xs)++)

typedef struct Node Node;

struct Node {
    int value;
    Node *next;
};

typedef struct DoubleNode DoubleNode;

struct DoubleNode {
    int value;
    DoubleNode *next;
    DoubleNode *prev;
};

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
            printf("%d ", curr->value);
            curr = curr->next;
            if (curr != NULL) {
                printf("-> ");
            }
        }
        printf("\n");
        return;
    }

    Node *curr = head;
    while (curr != cycle_head) {
        printf("%d ", curr->value);
        curr = curr->next;
        if (curr != NULL) {
            printf("-> ");
        }
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

void dll_print(DoubleNode *head) {
    while (head != NULL) {
        printf("%d ", head->value);
        head = head->next;
        if (head != NULL) {
            printf("<-> ");
        }
    }
    printf("\n");
}

Node* ll_read_cycled(void) {
    int n;

    printf("Enter the number of nodes (min 5) -> ");
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

    int cycle_start = -1;
    scanf("%d", &cycle_start);

    tail->next = nodes[cycle_start];
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

DoubleNode* double_node_create(int value) {
    DoubleNode *new_node = malloc(sizeof(DoubleNode));
    if (new_node == NULL) {
        fprintf(stderr, "Memory allocation failed.\n");
        exit(EXIT_FAILURE);
    }
    new_node->value = value;
    new_node->prev = NULL;
    new_node->next = NULL;
    return new_node;
}

DoubleNode* dll_read(void) {
    int n;
    printf("Enter the number of nodes -> ");
    if (scanf("%d", &n) != 1 || n < 1) {
        fprintf(stderr, "Invalid number of nodes.\n");
        exit(EXIT_FAILURE);
    }

    DoubleNode *head = NULL, *tail = NULL;

    for (int i = 0; i < n; i++) {
        int value;
        if (scanf("%d", &value) != 1) {
            fprintf(stderr, "Invalid value input.\n");
            exit(EXIT_FAILURE);
        }
        DoubleNode *node = double_node_create(value);
        if (head == NULL) {
            head = tail = node;
        } else {
            tail->next = node;
            node->prev = tail;
            tail = node;
        }
    }

    return head;
}

DoubleNode* dll_copy(DoubleNode *head) {
    if (head == NULL) return NULL;

    DoubleNode *copied_head = double_node_create(head->value);
    DoubleNode *cur = head->next;
    DoubleNode *copied = copied_head;

    while (cur != NULL) {
        DoubleNode *node = double_node_create(cur->value);

        copied->next = node;
        node->prev = copied;

        copied = node;
        cur = cur->next;
    }

    return copied_head;
}

int main(int argc, char **argv) {
    char *program = shift(argv, argc);

    if (argc < 1) {
        fprintf(stdout, "Usage: %s [ cycle | copy | dup ]\n", program);
        return 1;
    }

    char *subcommand = shift(argv, argc);
    if (strcmp(subcommand, "cycle") == 0) {
        Node *head = ll_read_cycled();

        ll_print(head);
    }
    else if (strcmp(subcommand, "copy") == 0) {
        DoubleNode *head = dll_read();

        printf("Initial list: ");
        dll_print(head);

        DoubleNode *copied = dll_copy(head);

        printf("Copied list:  ");
        dll_print(copied);
    }
    else if (strcmp(subcommand, "dup") == 0) {
        Node *head = ll_read();

        printf("Initial list: ");
        ll_print(head);

        ll_remove_duplicates(head);

        printf("List of distinct elements: ");
        ll_print(head);
    }
    else {
        fprintf(stdout, "Usage: %s [ cycle | copy | dup ]\n", program);
        return 1;
    }

    return 0;
}
