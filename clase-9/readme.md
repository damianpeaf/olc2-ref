```sh
aarch64-linux-gnu-as -mcpu=cortex-a57 hello-world.s -o hello-world.o
aarch64-linux-gnu-ld hello-world.o -o hello-world

qemu-aarch64 -g 1234 ./hello-world

gdb-multiarch -q --nh \
  -ex 'set architecture aarch64' \
  -ex 'file hello-world' \
  -ex 'target remote localhost:1234' \
  -ex 'layout split' \
  -ex 'layout regs'

```

- `step` = This is for C/C++ programs, or for Breakpoints. This will run the program and stop it at the next line in the C source or at the next Breakpoint (if one was set).
- `stepi` = step execution by 1 instruction (for Assembly view only)
- `nexti` = step the very next instruction below; all branches are treated as nops
- `break` [function name] = set a breakpoint at a function
- `next` = allow program to run til next breakpoint
- `continue` = run the program since it is halted, will halt again if a breakpoint gets hit
- `quit` = quit GDB
- `delete` = delete all breakpoints
- `info` vector = list FPRs as Vector data first, then as Float data
- `layout` next = swap to different view (C/C++ vs Assembly)
- `layout` prev = swap to your previous view
