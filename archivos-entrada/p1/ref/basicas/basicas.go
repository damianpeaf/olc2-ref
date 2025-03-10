package main

import "fmt"

func main() {
	puntos := 0

	fmt.Println("=== Archivo de prueba básico ===")

	// 1. Declaración de variables (2 puntos)
	fmt.Println("==== Declaración de variables ====")
	puntosDeclaracion := 0

	fmt.Println("Declaración explícita con tipo y valor")
	var entero int = 42
	var decimal float64 = 3.14159
	var texto string = "Hola, mundo!"
	var booleano bool = true

	fmt.Println("entero:", entero)
	fmt.Println("decimal:", decimal)
	fmt.Println("texto:", texto)
	fmt.Println("booleano:", booleano)

	if entero == 42 && decimal > 3.0 && texto == "Hola, mundo!" && booleano == true {
		puntosDeclaracion = puntosDeclaracion + 1
		fmt.Println("OK Declaración explícita con tipo y valor: correcto")
	} else {
		fmt.Println("X Declaración explícita con tipo y valor: incorrecto")
	}

	fmt.Println("\nDeclaración explícita con tipo y sin valor")
	var enteroSinValor int
	var decimalSinValor float64
	var textoSinValor string
	var booleanoSinValor bool

	fmt.Println("enteroSinValor:", enteroSinValor)
	fmt.Println("decimalSinValor:", decimalSinValor)
	fmt.Println("textoSinValor:", textoSinValor)
	fmt.Println("booleanoSinValor:", booleanoSinValor)

	// Verificamos que se inicialicen con valores por defecto
	if enteroSinValor == 0 && decimalSinValor == 0.0 && textoSinValor == "" && booleanoSinValor == false {
		puntosDeclaracion = puntosDeclaracion + 1
		fmt.Println("OK Declaración explícita con tipo y sin valor: correcto")
	} else {
		fmt.Println("X Declaración explícita con tipo y sin valor: incorrecto")
	}

	fmt.Println("\nErrores de redeclaración")
	// var entero int = 100 // ! ERROR: Comentar esta línea para que el programa compile
	if entero == 100 {
		puntosDeclaracion = puntosDeclaracion - 1
		fmt.Println("X Errores de redeclaración: incorrecto")
	} else {
		fmt.Println("OK Errores de redeclaración: correcto")
	}

	// 2. Asignación de variables (2 puntos)
	fmt.Println("\n==== Asignación de variables ====")
	puntosAsignacion := 0

	fmt.Println("Asignación con tipo correcto")
	entero = 99
	decimal = 9.9
	texto = "Texto modificado"
	booleano = !booleano

	fmt.Println("entero:", entero)
	fmt.Println("decimal:", decimal)
	fmt.Println("texto:", texto)
	fmt.Println("booleano:", booleano)

	if entero == 99 && decimal == 9.9 && texto == "Texto modificado" && booleano == false {
		puntosAsignacion = puntosAsignacion + 1
		fmt.Println("OK Asignación con tipo correcto: correcto")
	} else {
		fmt.Println("X Asignación con tipo correcto: incorrecto")
	}

	fmt.Println("\nAsignación con tipo incorrecto")
	// entero = "esto debería fallar" // ! ERROR: Comentar esta línea para que el programa compile
	fmt.Println("OK Asignación con tipo incorrecto: Se detectaron errores de tipo correctamente")
	puntosAsignacion = puntosAsignacion + 1

	// 3. Operaciones Aritméticas (2 puntos)
	fmt.Println("\n==== Operaciones Aritméticas ====")
	puntosOperacionesAritmeticas := 0

	fmt.Println("Suma")
	resultadoSuma1 := 10 + 5
	resultadoSuma2 := 10.5 + 5.5
	resultadoSuma3 := 10 + 5.5
	resultadoSuma4 := 10.5 + 5

	fmt.Println("10 + 5 =", resultadoSuma1)
	fmt.Println("10.5 + 5.5 =", resultadoSuma2)
	fmt.Println("10 + 5.5 =", resultadoSuma3)
	fmt.Println("10.5 + 5 =", resultadoSuma4)

	if resultadoSuma1 == 15 && resultadoSuma2 == 16.0 && resultadoSuma3 == 15.5 && resultadoSuma4 == 15.5 {
		puntosOperacionesAritmeticas = puntosOperacionesAritmeticas + 1
		fmt.Println("OK Suma: correcto")
	} else {
		fmt.Println("X Suma: incorrecto")
	}

	fmt.Println("\nResta")
	resultadoResta1 := 10 - 5
	resultadoResta2 := 10.5 - 5.5
	resultadoResta3 := 10 - 5.5
	resultadoResta4 := 10.5 - 5

	fmt.Println("10 - 5 =", resultadoResta1)
	fmt.Println("10.5 - 5.5 =", resultadoResta2)
	fmt.Println("10 - 5.5 =", resultadoResta3)
	fmt.Println("10.5 - 5 =", resultadoResta4)

	if resultadoResta1 == 5 && resultadoResta2 == 5.0 && resultadoResta3 == 4.5 && resultadoResta4 == 5.5 {
		puntosOperacionesAritmeticas = puntosOperacionesAritmeticas + 1
		fmt.Println("OK Resta: correcto")
	} else {
		fmt.Println("X Resta: incorrecto")
	}

	// 4. Operaciones Relacionales (2 puntos)
	fmt.Println("\n==== Operaciones Relacionales ====")
	puntosOperacionesRelacionales := 0

	fmt.Println("Igualdad")
	resultadoIgualdad1 := 10 == 10
	resultadoIgualdad2 := 10 == 5
	resultadoIgualdad3 := 10.5 == 10.5
	resultadoIgualdad4 := 10.5 == 5.5

	fmt.Println("10 == 10:", resultadoIgualdad1)
	fmt.Println("10 == 5:", resultadoIgualdad2)
	fmt.Println("10.5 == 10.5:", resultadoIgualdad3)
	fmt.Println("10.5 == 5.5:", resultadoIgualdad4)

	if resultadoIgualdad1 == true && resultadoIgualdad2 == false &&
		resultadoIgualdad3 == true && resultadoIgualdad4 == false {
		puntosOperacionesRelacionales = puntosOperacionesRelacionales + 1
		fmt.Println("OK Igualdad: correcto")
	} else {
		fmt.Println("X Igualdad: incorrecto")
	}

	fmt.Println("\nDesigualdad")
	resultadoDesigualdad1 := 10 != 10
	resultadoDesigualdad2 := 10 != 5
	resultadoDesigualdad3 := 10.5 != 10.5
	resultadoDesigualdad4 := 10.5 != 5.5

	fmt.Println("10 != 10:", resultadoDesigualdad1)
	fmt.Println("10 != 5:", resultadoDesigualdad2)
	fmt.Println("10.5 != 10.5:", resultadoDesigualdad3)
	fmt.Println("10.5 != 5.5:", resultadoDesigualdad4)

	if resultadoDesigualdad1 == false && resultadoDesigualdad2 == true &&
		resultadoDesigualdad3 == false && resultadoDesigualdad4 == true {
		puntosOperacionesRelacionales = puntosOperacionesRelacionales + 1
		fmt.Println("OK Desigualdad: correcto")
	} else {
		fmt.Println("X Desigualdad: incorrecto")
	}

	// 5. Operaciones Lógicas (2 puntos)
	fmt.Println("\n==== Operaciones Lógicas ====")
	puntosOperacionesLogicas := 0

	fmt.Println("AND")
	resultadoAnd1 := true && true
	resultadoAnd2 := true && false
	resultadoAnd3 := false && true
	resultadoAnd4 := false && false
	resultadoAnd5 := (10 == 10) && (5 == 5)
	resultadoAnd6 := (10 == 10) && (5 == 6)

	fmt.Println("true && true:", resultadoAnd1)
	fmt.Println("true && false:", resultadoAnd2)
	fmt.Println("false && true:", resultadoAnd3)
	fmt.Println("false && false:", resultadoAnd4)
	fmt.Println("(10 == 10) && (5 == 5):", resultadoAnd5)
	fmt.Println("(10 == 10) && (5 == 6):", resultadoAnd6)

	if resultadoAnd1 == true && resultadoAnd2 == false &&
		resultadoAnd3 == false && resultadoAnd4 == false &&
		resultadoAnd5 == true && resultadoAnd6 == false {
		puntosOperacionesLogicas = puntosOperacionesLogicas + 1
		fmt.Println("OK AND: correcto")
	} else {
		fmt.Println("X AND: incorrecto")
	}

	fmt.Println("\nOR")
	resultadoOr1 := true || true
	resultadoOr2 := true || false
	resultadoOr3 := false || true
	resultadoOr4 := false || false
	resultadoOr5 := (10 == 10) || (5 == 5)
	resultadoOr6 := (10 == 10) || (5 == 6)
	resultadoOr7 := (10 == 11) || (5 == 6)

	fmt.Println("true || true:", resultadoOr1)
	fmt.Println("true || false:", resultadoOr2)
	fmt.Println("false || true:", resultadoOr3)
	fmt.Println("false || false:", resultadoOr4)
	fmt.Println("(10 == 10) || (5 == 5):", resultadoOr5)
	fmt.Println("(10 == 10) || (5 == 6):", resultadoOr6)
	fmt.Println("(10 == 11) || (5 == 6):", resultadoOr7)

	if resultadoOr1 == true && resultadoOr2 == true &&
		resultadoOr3 == true && resultadoOr4 == false &&
		resultadoOr5 == true && resultadoOr6 == true &&
		resultadoOr7 == false {
		puntosOperacionesLogicas = puntosOperacionesLogicas + 1
		fmt.Println("OK OR: correcto")
	} else {
		fmt.Println("X OR: incorrecto")
	}

	// 6. fmt.Println (2 puntos)
	fmt.Println("\n==== fmt.Println ====")
	puntosPrintln := 0

	fmt.Println("###Validacion Manual")
	fmt.Println("Validando impresión de un solo argumento")
	fmt.Println(42)
	fmt.Println(3.14159)
	fmt.Println("Cadena de texto")
	fmt.Println(true)

	fmt.Println("\nValidando impresión de múltiples argumentos")
	fmt.Println("Número:", 42, "Pi:", 3.14159, "Booleano:", true)

	fmt.Println("\nValidando impresión de expresiones")
	fmt.Println(10 + 5)
	fmt.Println(10 > 5)
	fmt.Println(10 * 5 / 2)

	fmt.Println("\nValidando impresión de variables")
	fmt.Println(entero)
	fmt.Println(decimal)
	fmt.Println(texto)
	fmt.Println(booleano)

	fmt.Println("\nValidando impresión de slices")
	numeros := []int{1, 2, 3, 4, 5}
	fmt.Println(numeros)

	// Asumimos que la implementación de fmt.Println es correcta
	puntosPrintln = puntosPrintln + 2
	fmt.Println("OK fmt.Println: correcto")

	// 7. Manejo de valor nulo (2 puntos)
	fmt.Println("\n==== Manejo de valor nulo ====")
	puntosValorNulo := 0

	fmt.Println("\nOperaciones con nil")
	// fmt.Println("nilVar + 5:", nil + 5) 	// ! ERROR: Comentar esta línea para que el programa compile

	puntosValorNulo = puntosValorNulo + 2
	fmt.Println("OK Manejo de valor nulo: correcto")

	// 8. Opcionalidad del ; (2 puntos)
	fmt.Println("\n==== Opcionalidad del ; ====")
	puntosPuntoYComa := 0

	fmt.Println("###Validacion Manual")

	// Instrucciones sin punto y coma
	a := 10
	b := 20
	c := a + b
	fmt.Println("a =", a)
	fmt.Println("b =", b)
	fmt.Println("c = a + b =", c)

	puntosPuntoYComa = puntosPuntoYComa + 2
	fmt.Println("OK Opcionalidad del punto y coma: correcto")

	// Resumen de puntos
	puntos = puntosDeclaracion + puntosAsignacion + puntosOperacionesAritmeticas +
		puntosOperacionesRelacionales + puntosOperacionesLogicas +
		puntosPrintln + puntosValorNulo + puntosPuntoYComa

	fmt.Println("\n=== Errores ===")
	fmt.Println("###Validacion Manual")
	fmt.Println("Errores esperados ?/3")

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
}
