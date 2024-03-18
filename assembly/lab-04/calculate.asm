BITS 64
section .text
global calculate

calculate:
    ; -- a,   b   --
    ; -- rdi, rsi --

    finit                    ; Initialize FPU

    ; Load the values of a and b from memory into FPU stack
    fld qword [rdi]          ; Load a into FPU stack
    fld qword [rsi]          ; Load b into FPU stack

    ; Add the values of a and b
    fadd                     ; Add a and b

    ; Store the result back to memory
    fstp qword [rdi]         ; Store the result back to a

    ; Clean up the FPU stack
    fstp qword [rsi]         ; Pop the second value (b) from the FPU stack

    mov rax, qword [rdi]
    ; Return
    ret