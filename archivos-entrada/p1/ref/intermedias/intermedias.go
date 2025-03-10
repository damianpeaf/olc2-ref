package main

import "fmt"

func main() {
	puntos := 0

	fmt.Println("=== Archivo de prueba de funcionalidades intermedias ===")

	// 1. Manejo de entornos (3 puntos)
	fmt.Println("==== Manejo de entornos ====")
	puntosEntornos := 0

	fmt.Println("Variable redeclarada en el mismo entorno")
	a := 10
	// var a int = 20 // ! ERROR: Comentar esta línea para que el programa compile
	fmt.Println("a =", a)

	if a == 10 {
		puntosEntornos = puntosEntornos + 1
		fmt.Println("OK Detección de redeclaración en mismo entorno: correcto")
	} else {
		fmt.Println("X Detección de redeclaración en mismo entorno: incorrecto")
	}

	fmt.Println("\nVariable redeclarada en un entorno diferente")
	b := 10
	if true {
		// Esto es válido en Go porque crea una nueva variable b en un ámbito diferente
		b := 20
		fmt.Println("b dentro del if =", b)

		if b == 20 {
			puntosEntornos = puntosEntornos + 1
			fmt.Println("OK Redeclaración en entorno diferente: correcto")
		} else {
			fmt.Println("X Redeclaración en entorno diferente: incorrecto")
		}
	}
	fmt.Println("b fuera del if =", b)

	fmt.Println("\nUso de variable en un entorno superior")
	c := 10
	d := 10
	if true {
		// d se redefine en este ámbito
		d := 20
		// c es accesible desde el ámbito superior
		fmt.Println("c dentro del if =", c)
		fmt.Println("d dentro del if =", d)

		// Modificamos c del ámbito superior
		c = 30
		// Modificamos d del ámbito local
		d = 30
	}
	fmt.Println("c fuera del if =", c)
	fmt.Println("d fuera del if =", d)

	if c == 30 && d == 10 {
		puntosEntornos = puntosEntornos + 1
		fmt.Println("OK Uso de variable en entorno superior: correcto")
	} else {
		fmt.Println("X Uso de variable en entorno superior: incorrecto")
	}

	// 2. If / Else (3 puntos)
	fmt.Println("\n==== If / Else ====")
	puntosIfElse := 0

	fmt.Println("If simple")
	if true {
		fmt.Println("Condición verdadera")
		puntosIfElse = puntosIfElse + 1
	}

	fmt.Println("\nIf-Else")
	if true {
		fmt.Println("Condición verdadera en if-else")
	} else {
		fmt.Println("Condición falsa en if-else")
	}

	if false {
		fmt.Println("Esto no debería imprimirse")
	} else {
		fmt.Println("Condición falsa, ejecutando else")
		puntosIfElse = puntosIfElse + 1
	}

	fmt.Println("\nIf-ElseIf-Else")
	if true {
		fmt.Println("Primera condición verdadera")
	} else if true {
		fmt.Println("Segunda condición verdadera, pero no se ejecuta")
	} else {
		fmt.Println("Ninguna condición verdadera")
	}

	if false {
		fmt.Println("Primera condición falsa")
	} else if true {
		fmt.Println("Segunda condición verdadera")
		puntosIfElse = puntosIfElse + 1
	} else {
		fmt.Println("Ninguna condición verdadera")
	}

	// 3. For Tipo While (2 puntos)
	fmt.Println("\n==== For Tipo While ====")
	puntosForWhile := 0

	fmt.Println("For como while simple")
	i := 0
	suma := 0
	for i < 5 {
		fmt.Println("i =", i)
		suma = suma + i
		i = i + 1
	}

	if suma == 10 {
		puntosForWhile = puntosForWhile + 1
		fmt.Println("OK For como while simple: correcto")
	} else {
		fmt.Println("X For como while simple: incorrecto")
	}

	fmt.Println("\nFor como while anidado (patrón X)")
	fmt.Println("###Validacion Manual")

	n := 5
	x := 0
	for x < n {
		j := 0
		fila := ""

		for j < n {
			if x == j || x+j == n-1 {
				fila = fila + "*"
			} else {
				fila = fila + " "
			}
			j = j + 1
		}

		fmt.Println(fila)
		x = x + 1
	}

	if x == 5 {
		puntosForWhile = puntosForWhile + 1
		fmt.Println("OK For como while anidado: correcto")
	} else {
		fmt.Println("X For como while anidado: incorrecto")
	}

	// 4. For Clásico (3 puntos)
	fmt.Println("\n==== For Clásico ====")
	puntosForClasico := 0

	fmt.Println("For clásico simple")
	suma = 0
	for i := 0; i < 5; i++ {
		fmt.Println("i =", i)
		suma = suma + i
	}

	if suma == 10 {
		puntosForClasico = puntosForClasico + 1
		fmt.Println("OK For clásico simple: correcto")
	} else {
		fmt.Println("X For clásico simple: incorrecto")
	}

	fmt.Println("\nFor clásico anidado (tabla de multiplicar)")
	fmt.Println("###Validacion Manual")
	for i := 1; i <= 3; i++ {
		for j := 1; j <= 3; j++ {
			fmt.Println(i, "x", j, "=", i*j)
		}
		fmt.Println()
	}
	puntosForClasico = puntosForClasico + 2

	// 5. For Range (3 puntos)
	fmt.Println("\n==== For Range ====")
	puntosForRange := 0

	fmt.Println("For range con slice")
	numeros := []int{10, 20, 30, 40, 50}
	suma = 0
	sumaIndices := 0

	for i, valor := range numeros {
		fmt.Println("Índice", i, "=", valor)
		suma = suma + valor
		sumaIndices = sumaIndices + i
	}

	if suma == 150 {
		puntosForRange = puntosForRange + 2
		fmt.Println("OK For range con slice: correcto")
	} else {
		fmt.Println("X For range con slice: incorrecto")
	}

	if sumaIndices == 10 {
		puntosForRange = puntosForRange + 1
		fmt.Println("OK For range con índices: correcto")
	} else {
		fmt.Println("X For range con índices: incorrecto")
	}

	// 6. Switch/Case (3 puntos)
	fmt.Println("\n==== Switch/Case ====")
	puntosSwitch := 0

	fmt.Println("Switch simple")
	dia := 1

	switch dia {
	case 1:
		fmt.Println("Lunes")
		puntosSwitch = puntosSwitch + 1
	case 2:
		fmt.Println("Martes")
	case 3:
		fmt.Println("Miércoles")
	case 4:
		fmt.Println("Jueves")
	case 5:
		fmt.Println("Viernes")
	case 6:
		fmt.Println("Sábado")
	case 7:
		fmt.Println("Domingo")
	default:
		fmt.Println("Día inválido")
	}

	fmt.Println("\nSwitch con default")
	numero := 100

	switch numero {
	case 1:
		fmt.Println("No se debería imprimir")
	case 2:
		fmt.Println("No se debería imprimir")
	default:
		fmt.Println("Número no reconocido, se ejecuta default")
		puntosSwitch = puntosSwitch + 1 // Se suma solo si default se ejecuta correctamente
	}

	fmt.Println("\n==== Switch con break explícito ====")

	numeroBreak := 2

	switch numeroBreak {
	case 1:
		fmt.Println("No se debería imprimir")
	case 2:
		fmt.Println("Caso 2 - Se ejecuta este y debe detenerse")
		puntosSwitch = puntosSwitch + 1
		break
		fmt.Println("No debería ejecutarse si el break funciona")
		puntosSwitch = puntosSwitch - 1
	case 3:
		fmt.Println("No se debería imprimir")
	}

	// 7. Break (3 puntos)
	fmt.Println("\n==== Break ====")
	puntosBreak := 0

	fmt.Println("Break en for infinito")
	contador := 0
	suma = 0

	for true {
		fmt.Println("contador =", contador)
		suma = suma + contador
		contador = contador + 1

		if contador >= 5 {
			break
		}
	}

	if suma == 10 {
		puntosBreak = puntosBreak + 1
		fmt.Println("OK Break en for infinito: correcto")
	} else {
		fmt.Println("X Break en for infinito: incorrecto")
	}

	fmt.Println("\nBreak en for clásico")
	suma = 0

	for i := 0; i < 10; i++ {
		fmt.Println("i =", i)
		suma = suma + i

		if i >= 4 {
			break
		}
	}

	if suma == 10 {
		puntosBreak = puntosBreak + 2
		fmt.Println("OK Break en for clásico: correcto")
	} else {
		fmt.Println("X Break en for clásico: incorrecto")
	}

	// 8. Continue (3 puntos)
	fmt.Println("\n==== Continue ====")
	puntosContinue := 0

	fmt.Println("Continue en for tipo while")
	contador = 0
	impares := 0

	for contador < 10 {
		contador = contador + 1

		if contador%2 == 0 {
			continue
		}

		impares = impares + contador
	}

	fmt.Println("Números impares:", impares)

	if impares == 25 {
		puntosContinue = puntosContinue + 1
		fmt.Println("OK Continue en for tipo while: correcto")
	} else {
		fmt.Println("X Continue en for tipo while: incorrecto")
	}

	fmt.Println("\nContinue en for clásico")
	pares := 0

	for i := 0; i < 10; i++ {
		if i%2 != 0 {
			continue
		}

		pares = pares + i
	}

	fmt.Println("Números pares:", pares)

	if pares == 20 {
		puntosContinue = puntosContinue + 2
		fmt.Println("OK Continue en for clásico: correcto")
	} else {
		fmt.Println("X Continue en for clásico: incorrecto")
	}

	// Resumen de puntos
	puntos = puntosEntornos + puntosIfElse + puntosForWhile + puntosForClasico +
		puntosForRange + puntosSwitch + puntosBreak + puntosContinue

	fmt.Println("\n=== Errores ===")
	fmt.Println("###Validacion Manual")
	fmt.Println("Errores esperados ?/1")

	fmt.Println("\n=== Tabla de Resultados ===")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| Característica           | Puntos | Total |")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| Manejo de entornos       | ", puntosEntornos, "    | 3     |")
	fmt.Println("| If / Else                | ", puntosIfElse, "    | 3     |")
	fmt.Println("| For Tipo While           | ", puntosForWhile, "    | 2     |")
	fmt.Println("| For Clásico              | ", puntosForClasico, "    | 3     |")
	fmt.Println("| For Range                | ", puntosForRange, "    | 3     |")
	fmt.Println("| Switch/Case              | ", puntosSwitch, "    | 3     |")
	fmt.Println("| Break                    | ", puntosBreak, "    | 3     |")
	fmt.Println("| Continue                 | ", puntosContinue, "    | 3     |")
	fmt.Println("+--------------------------+--------+-------+")
	fmt.Println("| TOTAL                    | ", puntos, "   | 23    |")
	fmt.Println("+--------------------------+--------+-------+")
}
