grammar Language;

program: dcl*;

dcl: varDcl | stmt;

varDcl: 'var' ID '=' expr ';';

stmt: expr ';' # ExprStmt | 'print(' expr ')' ';' # PrintStmt;

expr:
	'-' expr						# Negate
	| expr op = ('*' | '/') expr	# MulDiv
	| expr op = ('+' | '-') expr	# AddSub
	| INT							# Number
	| ID							# Identifier
	| '(' expr ')'					# Parens;

INT: [0-9]+;
WS: [ \t\r\n]+ -> skip;
ID: [a-zA-Z]+;