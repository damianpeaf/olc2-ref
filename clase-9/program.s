.section .text
    
.globl _start

    mov x0, #1
    adr x1, msg
    mov x2, len
    mov w8, #64
    svc #0

    mov x0, #0
    mov w8, #93
    svc #0


msg: .ascii "Hello, World!\n"
len = . - msg