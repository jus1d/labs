section .data
    zero_div_message db "Can't divide by zero. Use other values.", 10

section .text
global calculate

calculate:
    ; -- a,    b --
    ; -- arg0, arg1 --
    ; -- rdi,  rsi -- 

    ; -- Expression --
    ; (b + 1)/a + 1,    a > b
    ; -b,               a = b
    ; (a - 5)/(a + b),  a < b
    
    ; -- Compare a and b --
    cmp     rdi, rsi
    jl      less
    jg      greater
    je      equal


equal:
    neg     rsi
    mov     rax, rsi
    ret

greater:
    ; -- Check if a == 0 --
    test    rdi, rdi
    jz      error_zero_div

    inc     rsi
    mov     rax, rsi
    cqo
    idiv    rdi
    inc     rax
    ret

less:
    mov rax, rdi
    add rax, rsi
    cmp rax, 0
    je error_zero_div
    mov rax, 4
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