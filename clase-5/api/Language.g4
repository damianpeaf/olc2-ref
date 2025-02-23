grammar Language;

program: dcl*;

dcl: varDcl | stmt;

varDcl: 'var' ID '=' expr ';';

stmt:
	expr ';'								# ExprStmt
	| 'print(' expr ')' ';'					# PrintStmt
	| '{' dcl* '}'							# BlockStmt
	| 'if' '(' expr ')' stmt ('else' stmt)?	# IfStmt
	| 'while' '(' expr ')' stmt				# WhileStmt;

expr:
	'-' expr									# Negate
	| expr op = ('*' | '/') expr				# MulDiv
	| expr op = ('+' | '-') expr				# AddSub
	| expr op = ('>' | '<' | '>=' | '<=') expr	# Relational
	| expr op = ('==' | '!=') expr				# Equality
	| ID '=' expr								# Assign
	| BOOL										# Boolean
	| FLOAT										# Float
	| STRING									# String
	| INT										# Int
	| ID										# Identifier
	| '(' expr ')'								# Parens;

INT: [0-9]+;
BOOL: 'true' | 'false';
FLOAT: [0-9]+ '.' [0-9]+;
STRING: '"' ~'"'* '"';
WS: [ \t\r\n]+ -> skip;
ID: [a-zA-Z]+;