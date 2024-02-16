global calculate

section .text
calculate:
    ; -- arg0, arg1, arg2, arg3 --
    ; -- rdi,  rsi,  rdx,  r10  -- 
    ; -- (-2 * a + 6 - 3 * b)/(-2 * c * c - 55 / d) -- 
    mov rax, rdi
    add rax, rsi
    add rax, rdx
    add rax, rcx
    ret