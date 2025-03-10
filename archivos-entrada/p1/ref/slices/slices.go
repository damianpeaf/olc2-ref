package main

import (
	"fmt"
	"slices"
	"strings"
)

func main() {
	puntos := 0

	fmt.Println("=== Archivo de prueba de slices ===")

	// 1. Creación de slices (3 puntos)
	fmt.Println("==== Creación de slices ====")
	puntosCreacion := 0

	fmt.Println("Creación con literales:")
	numeros := []int{1, 2, 3, 4, 5}
	fmt.Println("###Validacion Manual")
	fmt.Println("numeros:", numeros)
	fmt.Println("OK Creación con literales: correcto")
	puntosCreacion = puntosCreacion + 3

	// 2. Acceso de Elementos (3 puntos)
	fmt.Println("\n==== Acceso de Elementos ====")
	puntosAcceso := 0

	fmt.Println("Acceso por índice:")
	primerElemento := numeros[0]
	fmt.Println("Primer elemento:", primerElemento)

	if primerElemento == 1 {
		puntosAcceso = puntosAcceso + 1
		fmt.Println("OK Acceso por índice: correcto")
	} else {
		fmt.Println("X Acceso por índice: incorrecto")
	}

	fmt.Println("Acceso fuera de rango:")
	// fmt.Println(numeros[5]) // ! ERROR: Comentar esta línea para que el programa compile
	fmt.Println("OK Acceso fuera de rango: correcto")

	fmt.Println("\nModificación de elementos:")
	numeros[0] = 10
	fmt.Println("numeros después de modificar:", numeros)

	if numeros[0] == 10 {
		puntosAcceso = puntosAcceso + 2
		fmt.Println("OK Modificación de elementos: correcto")
	} else {
		fmt.Println("X Modificación de elementos: incorrecto")
	}

	// 3. Array Multidimensional (4 puntos)
	fmt.Println("\n==== Array Multidimensional ====")
	puntosMultidimensional := 0

	fmt.Println("Creación de matriz 2D con literales:")
	matriz := [][]int{
		{1, 2, 3},
		{4, 5, 6},
		{7, 8, 9},
	}
	fmt.Println("matriz:")
	fmt.Println("###Validacion Manual")
	fmt.Println(matriz)
	puntosMultidimensional = puntosMultidimensional + 2

	fmt.Println("\nMatrices irregulares:")
	matrizIrregular := [][]int{
		{1, 2, 3, 4},
		{5, 6},
		{7, 8, 9},
	}
	fmt.Println("matrizIrregular:")
	fmt.Println("###Validacion Manual")
	fmt.Println(matrizIrregular)
	puntosMultidimensional = puntosMultidimensional + 2

	// 4. Acceso Array Multidimensional (4 puntos)
	fmt.Println("\n==== Acceso Array Multidimensional ====")
	puntosAccesoMultidimensional := 0

	fmt.Println("Acceso a elementos de matriz 2D:")
	elemento11 := matriz[0][0]
	elemento23 := matriz[1][2]
	elemento33 := matriz[2][2]
	fmt.Println("Elemento [0][0]:", elemento11)
	fmt.Println("Elemento [1][2]:", elemento23)
	fmt.Println("Elemento [2][2]:", elemento33)

	if elemento11 == 1 && elemento23 == 6 && elemento33 == 9 {
		puntosAccesoMultidimensional = puntosAccesoMultidimensional + 1
		fmt.Println("OK Acceso a elementos de matriz 2D: correcto")
	} else {
		fmt.Println("X Acceso a elementos de matriz 2D: incorrecto")
	}

	fmt.Println("\nModificación de elementos en matriz 2D:")
	matriz[0][0] = 100
	matriz[1][1] = 500
	matriz[2][2] = 900

	if matriz[0][0] == 100 && matriz[1][1] == 500 && matriz[2][2] == 900 {
		puntosAccesoMultidimensional = puntosAccesoMultidimensional + 1
		fmt.Println("OK Modificación de elementos en matriz 2D: correcto")
	} else {
		fmt.Println("X Modificación de elementos en matriz 2D: incorrecto")
	}

	fmt.Println("\nAcceso a filas completas:")
	primeraFila := matriz[0]
	fmt.Println("Primera fila de matriz:", primeraFila)

	if primeraFila[0] == 100 && primeraFila[1] == 2 && primeraFila[2] == 3 {
		puntosAccesoMultidimensional = puntosAccesoMultidimensional + 2
		fmt.Println("OK Acceso a filas completas: correcto")
	} else {
		fmt.Println("X Acceso a filas completas: incorrecto")
	}

	// 5. Función slices.Index (1 punto)
	fmt.Println("\n==== Función slices.Index ====")
	puntosIndex := 0

	fmt.Println("Búsqueda de elementos con slices.Index:")
	numeros = []int{10, 20, 30, 40, 50}
	indice1 := slices.Index(numeros, 30)
	indice2 := slices.Index(numeros, 60) // No existe, debería retornar -1
	fmt.Println("Índice de 30:", indice1)
	fmt.Println("Índice de 60:", indice2)

	if indice1 == 2 && indice2 == -1 {
		puntosIndex = puntosIndex + 1
		fmt.Println("OK slices.Index: correcto")
	} else {
		fmt.Println("X slices.Index: incorrecto")
	}

	// 6. Función Strings.Join (1 punto)
	fmt.Println("\n==== Función Strings.Join ====")
	puntosJoin := 0

	fmt.Println("Unión de strings con strings.Join:")
	palabras := []string{"Hola", "mundo", "desde", "Go"}
	frase := strings.Join(palabras, " ")
	fraseConComas := strings.Join(palabras, ", ")
	fmt.Println("Frase con espacios:", frase)
	fmt.Println("Frase con comas:", fraseConComas)

	if frase == "Hola mundo desde Go" && fraseConComas == "Hola, mundo, desde, Go" {
		puntosJoin = puntosJoin + 1
		fmt.Println("OK strings.Join: correcto")
	} else {
		fmt.Println("X strings.Join: incorrecto")
	}

	// 7. Función len (1 punto)
	fmt.Println("\n==== Función len ====")
	puntosLen := 0

	fmt.Println("Longitud de slices con len:")
	longitud1 := len(numeros)
	longitud2 := len(matrizIrregular)
	longitud3 := len(matrizIrregular[1])

	if longitud1 == 5 && longitud2 == 3 && longitud3 == 2 {
		puntosLen = puntosLen + 1
		fmt.Println("OK len: correcto")
	} else {
		fmt.Println("X len: incorrecto")
	}

	// 8. Función append (3 puntos)
	fmt.Println("\n==== Función append ====")
	puntosAppend := 0

	fmt.Println("Agregar elementos con append:")
	numeros = []int{1, 2, 3}
	numeros = append(numeros, 4)
	fmt.Println("numeros después de append(numeros, 4):", numeros)

	if numeros[3] == 4 {
		puntosAppend = puntosAppend + 1
		fmt.Println("OK Agregar un elemento: correcto")
	} else {
		fmt.Println("X Agregar un elemento: incorrecto")
	}

	fmt.Println("\nAgregar un slice a otro con append:")
	mtx1 := [][]int{
		{1, 2, 3},
		{4, 5, 6},
		{7, 8, 9},
	}

	mtx1 = append(mtx1, numeros)
	fmt.Println("mtx1 después de append(mtx1, numeros):")
	fmt.Println("###Validacion Manual")
	fmt.Println(mtx1)
	puntosAppend = puntosAppend + 2

	// Resumen de puntos
	puntos = puntosCreacion + puntosAcceso + puntosMultidimensional +
		puntosAccesoMultidimensional + puntosIndex + puntosJoin +
		puntosLen + puntosAppend

	fmt.Println("\n=== Errores ===")
	fmt.Println("###Validacion Manual")
	fmt.Println("Errores esperados ?/1")

	fmt.Println("\n=== Tabla de Resultados ===")
	fmt.Println("+----------------------------------+--------+-------+")
	fmt.Println("| Característica                   | Puntos | Total |")
	fmt.Println("+----------------------------------+--------+-------+")
	fmt.Println("| Creación de slices               | ", puntosCreacion, "    | 3     |")
	fmt.Println("| Acceso de Elementos              | ", puntosAcceso, "    | 3     |")
	fmt.Println("| Array Multidimensional           | ", puntosMultidimensional, "    | 4     |")
	fmt.Println("| Acceso Array Multidimensional    | ", puntosAccesoMultidimensional, "    | 4     |")
	fmt.Println("| Función slices.Index             | ", puntosIndex, "    | 1     |")
	fmt.Println("| Función Strings.Join             | ", puntosJoin, "    | 1     |")
	fmt.Println("| Función len                      | ", puntosLen, "    | 1     |")
	fmt.Println("| Función append                   | ", puntosAppend, "    | 3     |")
	fmt.Println("+----------------------------------+--------+-------+")
	fmt.Println("| TOTAL                            | ", puntos, "   | 20    |")
	fmt.Println("+----------------------------------+--------+-------+")
}
