```
antlr4 -Dlanguage=CSharp -o analyzer -package analyzer -visitor -no-listener ./*.g4

qemu-aarch64 -g 1234 ./program

gdb-multiarch -q --nh \
  -ex 'set architecture aarch64' \
  -ex 'file program' \
  -ex 'target remote localhost:1234' \
  -ex 'layout split' \
  -ex 'layout regs'

```

<!-- https://viewerjs.org/ -->
