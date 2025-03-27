#!/bin/bash
if [ -z "$1" ]; then
    echo "Uso: $0 <archivo_ensamblador.s>"
    exit 1
fi

input_file="$1"
output_file="${input_file%.s}"  # Remueve la extensión .s

aarch64-linux-gnu-as -mcpu=cortex-a57 "$input_file" -o "$output_file.o"
aarch64-linux-gnu-ld "$output_file.o" -o "$output_file"

echo "Compilado exitosamente: $output_file"
