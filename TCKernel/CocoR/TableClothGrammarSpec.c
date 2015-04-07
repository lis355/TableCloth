$namespace=TableClothKernel

COMPILER TableCloth

// ������� ���������������� LL ����������
int GetNextTokenKind() { return _scanner.Peek().kind; }

CHARACTERS
	Letter = 'A' .. 'Z' + 'a' .. 'z' .
	Digit = "0123456789" .
	CaretSymbol  = '\r' .
	EOL = '\n' .
	EOF = '\0' . 
	Tab = '\t' .

TOKENS
	Identifier = Letter { Letter | Digit } .
	TrueConstant = '1' . 
	FalseConstant = '0' .

// ���������
	True = "true" .
	False = "false" .
// ������ � �����������
	New = "new" .
	Clear = "clear" . 
	ExpressionType = "type" .
// ���������
	Equal = '=' .
	Not  = '!'  | "[not]" .
	And  = "&&" | "[and]" .
	Or   = "||" | "[or]" .
	Xor  = "^"  | "[xor]" .
	Equivalence = "==" | "[equ]" .
	Implication = "=>" | "[impl]" .
	Sheffer = "[shef]" .
	Pirse = "[pirse]" .
// �������
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
	(. Cur = Tree; .)
	Command [ EndOfCommand [ ManyOrOneCommand ] ] .

Command = 
	ExpressionOrCreateNewVariableCommand
	| DeleteVariableCommand
	| GetExpressionTypeCommand .

ExpressionOrCreateNewVariableCommand =
	IF ( GetNextTokenKind() != _Equal)
	ExpressionCode	(. AddCh( new ExpressionCommandNode() ); .)
	| CreateNewVariableCommand .
	
CreateNewVariableCommand =
	Identifier (. tmpIdentifier = t.val; .)
	Equal ExpressionCode (. GlobalVariableList.New(tmpIdentifier, tmpExpression); .)
	| New LeftRoundBracket Identifier Comma ExpressionCode RightRoundBracket .

DeleteVariableCommand = Clear LeftRoundBracket Identifier RightRoundBracket	.
GetExpressionTypeCommand = ExpressionType LeftRoundBracket ExpressionCode RightRoundBracket .

// ��������� ������� ��������
// ������, ���������, ����������, ����������, ����� �� ������ 2, 
// ����������, ������������, ����� �������, ������� �����

ExpressionCode = (. tmpExpression = new Expression(); .)
				Expression
				(. tmpExpression.Root = EV.Pop(); .)
 				(. AddCh(new ExpressionNode(tmpExpression.Root.ToString())); .) .

Expression =
	EquImplExpression [ Pirse Expression (. PushPirse(); .)
	| Sheffer Expression (. PushSheffer(); .) ] .

EquImplExpression =
	XorExpression [ Equivalence EquImplExpression (. PushEqu(); .)
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
	Identifier (. PushVariable(t.val); .)
	[ FunctionBracketsAndArguments (. TcDebug.Log( "func" ); .) ] .
	
Constant = 
	True (. PushTrueConstant(); .)
	| TrueConstant (. PushTrueConstant(); .)
	| False (. PushFalseConstant(); .)
	| FalseConstant (. PushFalseConstant(); .) .
	
FunctionBracketsAndArguments =
	LeftRoundBracket [ ListOfArguments ] RightRoundBracket .

ListOfArguments =
	( ExpressionCode | ListOfExpression) [ Comma ListOfArguments] .

ListOfExpression =
	LeftListBracket ExpressionEnumeration RightListBracket .

ExpressionEnumeration =
	ExpressionCode [ Comma ExpressionEnumeration ] . 

END TableCloth .
