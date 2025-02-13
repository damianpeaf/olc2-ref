grammar Language;

expr:
	expr op = ('*' | '/') expr		# MulDiv
	| expr op = ('+' | '-') expr	# AddSub
	| INT							# Number
	| '(' expr ')'					# Parens;

INT: [0-9]+;
WS: [ \t\r\n]+ -> skip;