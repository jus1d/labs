global calculate

section .data
    zero_div_message db "Can't divide by zero. Use other values.", 10

section .text
calculate:
    ; -- a,    b,    c,    d --
    ; -- arg0, arg1, arg2, arg3 --
    ; -- rdi,  rsi,  rdx,  rcx  -- 

    mov     rax, rdi ; <rax> = a
    mov     rbx, rsi ; <rbx> = b
    mov     rdi, rcx ; <rdi> = d - temp
    mov     rcx, rdx ; <rcx> = c
    mov     rdx, rdi ; <rdx> = d
    jmp sec
    ; -- (-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d) -- 

    ; -- (-2 * a + 6 - 3 * b) --
    imul    rax, -2
    imul    rbx, -3
    add     rax, 6
    add     rax, rbx
    push    rax ; 1

    ; -- (-2 * c * c - 55 / d) --
    imul    rcx, rcx
    imul    rcx, -2
sec:
    mov rax, -55
    cqo
    idiv rdx            ; rax = -55 / d

    mov rax, rdx
    sub rcx, rax

    pop rbx
    cqo                 ; Sign-extend rax into rdx:rax
    idiv rcx            ; rax = rbx / rcx, rdx = remainder
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