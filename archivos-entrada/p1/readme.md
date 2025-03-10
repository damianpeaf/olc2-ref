# Instrucciones

1. Cargar los archivos directamente desde la interfaz de usuario.
2. Mencionar las modificaciones que se harán en el código. (si es necesario)
3. Mostrar la salida de la ejecución del código.

# Consideraciones:

- Los archivos _en principio_ no tienen errores.
- Algunos archivos cuentan con errores comentados. Descomentarlos uno a uno para ver el resultado.

```go
// El error siempre es acompañado del siguiente tipo de comentario:
// ! ERROR: Comentar esta línea para que el programa compile
```

Ejemplo:

```go
var entero int = 100 // ! ERROR: Comentar esta línea para que el programa compile
```

El número de errores en cada archivos es indicado por el archivo de entrada:

```go
	fmt.Println("\n=== Errores ===")
	fmt.Println("###Validacion Manual")
	fmt.Println("Errores esperados ?/3")
```

- Algunos puntos de los archivos de entrada se evaluaran manualmente, comparando la salida con la referencia en GoLang. Para ello copiar la salida en un editor como Vscode y buscar el término "###"

```go
   	fmt.Println("###Validacion Manual")
```

- Cada archivo cuenta con un resumen de la puntuación obtenida en la evaluación de la salida del programa.

```go
    fmt.Println("\n=== Tabla de Resultados ===")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| Característica           | Puntos | Total |")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| Declaración de variables | ", puntosDeclaracion, "    | 2     |")
	fmt.Println("| Asignación de variables  | ", puntosAsignacion, "    | 2     |")
	fmt.Println("| Operaciones Aritméticas  | ", puntosOperacionesAritmeticas, "    | 2     |")
	fmt.Println("| Operaciones Relacionales | ", puntosOperacionesRelacionales, "    | 2     |")
	fmt.Println("| Operaciones Lógicas      | ", puntosOperacionesLogicas, "    | 2     |")
	fmt.Println("| fmt.Println              | ", puntosPrintln, "    | 2     |")
	fmt.Println("| Manejo de valor nulo     | ", puntosValorNulo, "    | 2     |")
	fmt.Println("| Opcionalidad del ;       | ", puntosPuntoYComa, "    | 2     |")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| TOTAL                    | ", puntos, "   | 16    |")
	fmt.Println("+--------------------------+--------+-------+")
```

- Cada archivo tiene su equivalente en GoLang (Go orginal), se puede visualizar en la carpeta de [referencia](./ref/).
