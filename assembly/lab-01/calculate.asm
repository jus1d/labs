section .data
    zero_div_message db "Can't divide by zero. Use other values.", 10

section .text
global calculate

calculate:
    ; -- a,    b,    c,    d --
    ; -- arg0, arg1, arg2, arg3 --
    ; -- rdi,  rsi,  rdx,  rcx  -- 

    ; -- (-2 * a + 6 - 3 * b) / (-2 * c * c - 55 / d) -- 

    ; -- (-2 * a + 6 - 3 * b) --
    shl     rdi, 1      ; <rdi> =  2 * a
    neg     rdi         ; <rdx> = -2 * a
    add     rdi, 6      ; <rdi> = -2 * a + 6
    imul    rsi, 3      ; <rsi> =  b * 3
    sub     rdi, rsi    ; <rdi> = -2 * a + 6 - 3 * b
    push    rdi         ; push first part of expression to stack

    ; -- (-2 * c * c) --
    mov     rax, rdx    ; <rax> = c
    imul    rax, rdx    ; <rax> = c * c
    imul    rax, -2     ; <rax> = -2 * c * c
    push    rax         ; push this part to stack

    ; -- (55 / d) -- 
    mov     rax, 55     ; <rax> = 55
    cqo                 ; <rax> => <rdx:rax> - Extending 64-bit <rax> register to 128-bit <rdx:rax>
    idiv    rcx         ; <rax> = 55 / d
    push    rax         ; push this part to stack

    ; -- (-2 * c * c - 55 / d)
    pop     rbx         ; pop (55 / d) from stack
    pop     rax         ; pop (-2 * c * c) from stack
    sub     rax, rbx    ; <rax> = (-2 * c * c) - (55 / d)
    push    rax         ; push this part to stack

    ; -- (-2 * a + 6 - 3 * b) / (-2 * c * c - 55 / d) -- 
    pop     rbx         ; pop (-2 * c * c) - (55 / d) from stack
    pop     rax         ; pop (-2 * a + 6 - 3 * b) from stack
    cqo                 ; <rax> => <rdx:rax> - Extending 64-bit <rax> register to 128-bit <rdx:rax> 
    idiv    rbx         ; <rax> = (-2 * a + 6 - 3 * b) / (-2 * c * c - 55 / d)
    
    ret