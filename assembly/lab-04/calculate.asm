BITS 64
section .text
global calculate

calculate:
    ; -- vector, size,   d    --
    ; -- rdi,    rsi,    rdx  -- 

    ; -- dereference first layer of vector's pointer --
    ; WHY TF THERE ARE TWO LAYERS OF POINTERS ??? DONNOW :)
    mov     rdi, [rdi]

    xor     r8, r8            ; <r8> - index
    xor     r9, r9            ; <r9> - counter

loop:
    mov rax, [rdi + r8 * 4]   ; dereference a[i] element to <rax>

    cmp rax, 0                ; compare <rax> and 0
    jge skip                  ; skip incrementing <r9> if <rax> >= 0

    cmp rax, rdx              ; compare <rax> and <rdx>
    jg skip                   ; skip incrementing <r9> if <rax> > d

    inc r9                    ; increment counter in <r9>, if all checks passed
    
skip:
    inc r8                    ; increment index in <r8>
    cmp r8, rsi               ; check if index == vec.size()
    jl loop                   ; if index == vec.size() jump to loop's end

    mov rax, r9               ; put couner to <rax>
    ret                       ; return <rax> value