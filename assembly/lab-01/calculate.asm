section .data
    zero_div_message db "Can't divide by zero. Use other values.", 10

section .text
global calculate

calculate:
    ; -- a,    b,    c,    d --
    ; -- arg0, arg1, arg2, arg3 --
    ; -- rdi,  rsi,  rdx,  rcx  -- 

    ; -- (-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d) -- 

    ; -- (-2 * a + 6 - 3 * b) --
    shl     rdi, 1
    neg     rdi
    add     rdi, 6
    imul    rsi, 3
    sub     rdi, rsi       ; (-2 * a + 6 - 3 * b)
    push    rdi            ; load first part of expression to stack
    cmp rax, rbx

    ; -- (-2 * c * c) --
    mov     rax, rdx
    imul    rax, rdx
    imul    rax, -2
    push    rax

    ; -- (55 / d) -- 
    mov     rax, 55
    cqo
    idiv    rcx
    push    rax

    ; -- (-2 * c * c - 55 / d)
    pop     rbx
    pop     rax
    sub     rax, rbx
    push    rax

    ; -- (-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d) -- 
    pop     rbx
    pop     rax
    cqo
    idiv    rbx
    
    ret
    

error_zero_div:
    mov     rax, 1
    mov     rdi, 2
    mov     rsi, zero_div_message
    mov     rdx, 40
    syscall

    mov     rax, 60
    mov     rdi, 1
    syscall