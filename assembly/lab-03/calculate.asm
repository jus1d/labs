BITS 64
section .text
global calculate

calculate:
    ; -- vector, size,   d    -- {1, -2, -8, -9, 4}
    ; -- rdi,    rsi,    rdx  -- 

    ; -- Dereferencing first layer of pointer --
    mov     rdi, [rdi]

    ; -- Check if vector's size is 0, return 69 error code --
    mov     rax, 0
    cmp     rsi, rax
    je      exit_error

    mov     r8, 0 ; <rbx> is index
    mov     r9, 0 ; <rcx> is counter

loop:
    mov     rax, [rdi + r8 * 4] ; <rax> = vec[i]
    inc     r8                  ; incrementing vector's index
    cmp     r8, rsi             ; check    if index == vector.size()
    je      end_loop            ; end loop if index == vector.size()

    ; cmp     rax, 0
    ; jg      skip

    cmp     rax, rdx
    jg      skip

    inc     r9

skip:
    jmp     loop

end_loop:
    mov     rax, r9
    ret

exit_error:
    mov     rax, 0
    ret