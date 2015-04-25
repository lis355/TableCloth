$namespace=TableClothKernel

COMPILER TableCloth

CHARACTERS

	Letter = 'A' .. 'Z' + 'a' .. 'z' .
	Digit = "0123456789" .
	CaretSymbol  = '\r' .
	EOL = '\n' .
	EOF = '\0' . 
	Tab = '\t' .

TOKENS

	IdentifierString = Letter { Letter | Digit } .
	TrueConstant = '1' . 
	FalseConstant = '0' .

// Константы
	True = "true" .
	False = "false" .
	TrueCaps = "True" .
	FalseCaps = "False" .
// Работа с переменными
	New = "new" .
	Clear = "clear" . 
//	ExpressionType = "type" .
// Операторы
	Equal = '=' .
	Not  = '!'  | "[not]" .
	And  = "&&" | "[and]" .
	Or   = "||" | "[or]" .
	Xor  = "^"  | "[xor]" .
	Equivalence = "==" | "[equ]" .
	Implication = "=>" | "[impl]" .
	Sheffer = "[shef]" .
	Pirse = "[pirse]" .
// Символы
	LeftRoundBracket = '(' .
	RightRoundBracket = ')' .
	LeftListBracket = '{' .
	RightListBracket = '}' .
	EndOfCommand = ';' .
	Comma = ',' .

COMMENTS FROM "/*" TO "*/" NESTED
COMMENTS FROM "//" TO EOL

IGNORE CaretSymbol + EOL + EOF + Tab

PRODUCTIONS

TableCloth =
	ManyOrOneCommand .

ManyOrOneCommand = 
	Command [ EndOfCommand [ ManyOrOneCommand ] ] .

Command = 
	ExpressionOrDefineVariableCommand
	| DeleteVariableCommand .
	//| GetExpressionTypeCommand .

ExpressionOrDefineVariableCommand =
	IF ( GetNextTokenKind() != Equal && la.kind != New ) ExpressionCode
	| DefineVariableCommand .
	
DefineVariableCommand =
	Identifier Equal ExpressionCode 
	| New LeftRoundBracket Identifier Comma ExpressionCode RightRoundBracket .

DeleteVariableCommand = Clear LeftRoundBracket Identifier RightRoundBracket	.

//GetExpressionTypeCommand = ExpressionType LeftRoundBracket ExpressionCode RightRoundBracket .

// Приоритет булевых операций
// Скобки, отрицание, конъюнкция, дизъюнкция, сумма по модулю 2, 
// импликация, эквиваленция, штрих Шеффера, стрелка Пирса

ExpressionCode =
	ExpressionOrListOfExpression .

ExpressionOrListOfExpression =
	Expression 
	| ListOfExpression.

Expression =
	EquImplExpression [ Pirse Expression (. PushPirse(); .)
	| Sheffer Expression (. PushSheffer(); .) ] .

EquImplExpression =
	XorExpression[Equivalence EquImplExpression (.PushEquivalence(); .)
	| Implication EquImplExpression (. PushImplication(); .) ] .

XorExpression =
	OrExpression [ Xor XorExpression (. PushXor(); .)] .

OrExpression =
	AndExpression [ Or OrExpression (. PushOr(); .)] .

AndExpression =
	NotExpression [ And AndExpression (. PushAnd(); .)] .

NotExpression =
	SimplyExpression
	| Not NotExpression (. PushNot(); .) .

SimplyExpression =
	IdentifierOrFunction
	| Constant 
	| LeftRoundBracket Expression RightRoundBracket .

IdentifierOrFunction =
	Identifier
	[ FunctionBracketsAndArguments ] .
	
Constant = 
	ConstantT 
	| ConstantF .
	
ConstantT =
	True
	| TrueCaps
	| TrueConstant .
	
ConstantF =
	False
	| FalseCaps
	| FalseConstant .
	
FunctionBracketsAndArguments =
	LeftRoundBracket [ ListOfArguments ] RightRoundBracket .

Identifier =
	IdentifierString .

ListOfArguments =
	ExpressionOrListOfExpression (. PushArgumentToFunction(); .)
	[ Comma ListOfArguments ] .
	
ListOfExpression =
	LeftListBracket [ ExpressionEnumeration ] RightListBracket .

ExpressionEnumeration =
	ExpressionOrListOfExpression (. PushExpressionToList(); .)
	[ Comma ExpressionEnumeration ] . 
	
END TableCloth .
