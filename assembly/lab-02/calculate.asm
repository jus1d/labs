section .data
    zero_div_message db "Can't divide by zero. Try to use other values", 10

section .text
global calculate

calculate:
    ; -- a,    b --
    ; -- arg0, arg1 --
    ; -- rdi,  rsi -- 

    ; -- Expression --
    ; (b + 1) / a + 1,    a > b
    ; -b,               a = b
    ; (a - 5) / (a + b),  a < b
    ; (a - 5) / (a + b),  a < b

    ; -- Compare a and b --
    cmp     rdi, rsi
    jl      a_lt_b      ; a < b
    jg      a_gt_b      ; a > b
    je      a_eq_b      ; a = b
    ; -- Should be unreachable --

a_eq_b:
    ; -- -b -- 
    neg     rsi         ; <rsi> = -b
    mov     rax, rsi    ; <rax> = -b
    ret                 ; return <rax>

a_gt_b:
    ; -- Check if a == 0 --
    test    rdi, rdi
    jz      err_zero_div

    ; -- (b + 1) / a + 1 --
    inc     rsi         ; <rsi> = b + 1
    mov     rax, rsi    ; <rax> = b + 1
    cqo                 ; <rax> => <rdx:rax> - Extending 64-bit <rax> register to 128-bit <rdx:rax>
    idiv    rdi         ; <rax> = (b + 1) / a
    inc     rax         ; <rax> = (b + 1) / a + 1
    ret                 ; return <rax>

a_lt_b:
    ; -- Check if (a + b) == 0 --
    neg     rsi         ; <rsi> = -b
    cmp     rdi, rsi
    je      err_zero_div

    ; -- (a - 5) / (a + b) --
    mov     rax, rdi    ; <rax> = a
    sub     rax, 5      ; <rax> = a - 5
    mov     rbx, rdi    ; <rbx> = a
    add     rbx, rsi    ; <rbx> = a + b
    cqo                 ; <rax> => <rdx:rax> - Extending 64-bit <rax> register to 128-bit <rdx:rax>
    idiv    rbx         ; <rax> = (a - 5) / (a + b)
    ret                 ; return <rax>

    

err_zero_div:
    ; -- Call write syscall to stderr, with error message --
    mov     rax, 1      ; Write syscall number
    mov     rdi, 2      ; stderr file descriptor
    mov     rsi, zero_div_message
    mov     rdx, 46
    syscall

    ; -- Call exit syscall with non-zero exit code --
    mov     rax, 60     ; Exit syscall number
    mov     rdi, 1      ; Exit code
    syscall