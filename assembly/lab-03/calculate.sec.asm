BITS 64
section .text
global calculate

calculate:
    ; Arguments:
    ;   rdi - pointer to the beginning of the vector
    ;   rsi - vector size

    mov rdi, [rdi]

    xor     rax, rax           ; Initialize counter to zero
    xor     rcx, rcx           ; Initialize loop counter to zero

loop_start:
    cmp     rcx, rsi           ; Compare loop counter to vector size
    je      loop_end           ; If equal, end loop

    mov     rdx, QWORD [rdi]   ; Load current element of vector into rdx
    cmp     rdx, 0             ; Check if element is negative
    jl      increment_counter  ; If negative, increment counter

    ; Continue loop
    add     rdi, 4             ; Move pointer to next element (64-bit integer)
    inc     rcx                ; Increment loop counter
    jmp     loop_start         ; Jump back to loop start

increment_counter:
    inc     rax                ; Increment counter
    ; Continue loop
    add     rdi, 4             ; Move pointer to next element (64-bit integer)
    inc     rcx                ; Increment loop counter
    jmp     loop_start         ; Jump back to loop start

loop_end:
    ret