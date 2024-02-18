global calculate

section .data
    zero_div_message db "Can't divide by zero. Use other values.", 10

section .text
calculate:
    ; -- a,    b,    c,    d --
    ; -- arg0, arg1, arg2, arg3 --
    ; -- rdi,  rsi,  rdx,  rcx  -- 

    mov rax, -55
    mov rbx, rcx
    cdq
    idiv rbx


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