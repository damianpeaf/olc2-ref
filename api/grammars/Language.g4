grammar Language;

expr:
	expr ('*' | '/') expr	# MulDiv
	| expr ('+' | '-') expr	# AddSub
	| INT					# Number
	| '(' expr ')'			# Parens;

INT: [0-9]+;
WS: [ \t\r\n]+ -> skip;