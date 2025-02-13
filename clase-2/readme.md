1. Descargar e instalar .NET
   https://dotnet.microsoft.com/en-us/download

2. Agregar extensiones

- https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit
- https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
- https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-pack
- https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-runtime
- https://marketplace.visualstudio.com/items?itemName=patcx.vscode-nuget-gallery
- https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions
- https://marketplace.visualstudio.com/items?itemName=mike-lischke.vscode-antlr4

2. Crear proyecto asp.net

```bash
dotnet new webapi -o api

```

3. Descargar ANTLR

- https://www.antlr.org/download.html

```bash
curl -O https://www.antlr.org/download/antlr-4.13.2-complete.jar
```

4. Agregarlo al path

```bash
nano ~/.zshrc

# Agregar estas lineas
export CLASSPATH=".:/Users/damianpeaf/antlr/antlr4.jar:$CLASSPATH"
alias antlr4='java -jar /Users/damianpeaf/antlr/antlr4.jar'
```

5. Instalar Java si no lo tienen

- https://www.oracle.com/java/technologies/downloads/

5. Generar gramática

```bash
java -jar antlr-4.13.2-complete.jar -Dlanguage=CSharp -o analyzer -package analyzer -visitor -no-listener grammars/*.g4


antlr4 -Dlanguage=CSharp -o analyzer -package analyzer -visitor -no-listener ./grammars/*.g4

antlr4 -Dlanguage=CSharp -o analyzer -package analyzer -visitor *.g4

```

6. Instalar paquetes

```bash
dotnet add package Antlr4.Runtime.Standard
```

7. Correr el proyecto

```bash
dotnet watch run
```
