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

start_loop:
    mov rcx, [rdi + r8 * 4]   ; dereference a[i] element to <rcx>

    cmp rcx, 0                ; compare <rcx> and 0
    jge skip                  ; skip incrementing <r9> if <rcx> >= 0

    cmp rcx, rdx              ; compare <rcx> and <rdx>
    jg skip                   ; skip incrementing <r9> if <rcx> > d

    inc r9                    ; increment counter in <r9>, if all checks passed

skip:
    inc r8                    ; increment index in <r8>
    cmp r8, rsi               ; check if index == vec.size()
    jl start_loop             ; if index == vec.size() jump to loop's end

    mov rax, r9               ; put counter to <rcx>
    ret                       ; return <rcx> value
