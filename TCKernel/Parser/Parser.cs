
using System;
using System.Collections.Generic;

namespace TableClothKernel
{


public partial class Parser
{
	const int EOF = 0;
	const int IdentifierString = 1;
	const int TrueConstant = 2;
	const int FalseConstant = 3;
	const int True = 4;
	const int False = 5;
	const int TrueCaps = 6;
	const int FalseCaps = 7;
	const int New = 8;
	const int Clear = 9;
	const int Equal = 10;
	const int Not = 11;
	const int And = 12;
	const int Or = 13;
	const int Xor = 14;
	const int Equivalence = 15;
	const int Implication = 16;
	const int Sheffer = 17;
	const int Pirse = 18;
	const int LeftRoundBracket = 19;
	const int RightRoundBracket = 20;
	const int LeftListBracket = 21;
	const int RightListBracket = 22;
	const int EndOfCommand = 23;
	const int Comma = 24;

	public enum ENonTerminal
	{
		TableCloth,
		ManyOrOneCommand,
		Command,
		ExpressionOrDefineVariableCommand,
		DeleteVariableCommand,
		ExpressionCode,
		DefineVariableCommand,
		Identifier,
		ExpressionOrListOfExpression,
		Expression,
		ListOfExpression,
		EquImplExpression,
		XorExpression,
		OrExpression,
		AndExpression,
		NotExpression,
		SimplyExpression,
		IdentifierOrFunction,
		Constant,
		FunctionBracketsAndArguments,
		ConstantT,
		ConstantF,
		ListOfArguments,
		ExpressionEnumeration
	}

	public const int maxT = 25;

	const bool T = true;
	const bool F = false;
	const int minErrDist = 2;
	
	Scanner _scanner;

	Token t;    // last recognized token
	Token la;   // lookahead token
	int errDist;
	
    partial void ProductionBegin( ENonTerminal production );
    partial void ProductionEnd( ENonTerminal production );
	
	public ParserErrors Errors { get; set; }
	
    public string CurrentToken { get { return t.val; } }
	
	int GetNextTokenKind() { return _scanner.Peek().kind; }
	


    public Parser()
    {
        Errors = new ParserErrors();
    }

	void SynErr(int n)
	{
		if (errDist >= minErrDist) Errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	void SemErr(string msg)
	{
		if (errDist >= minErrDist) Errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get()
	{
		while (true)
		{
			t = la;
			la = _scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect(int n)
	{
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf(int s)
	{
		return set[s, la.kind];
	}
	
	void ExpectWeak(int n, int follow)
	{
		if (la.kind == n) Get();
		else
		{
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}

	bool WeakSeparator(int n, int syFol, int repFol)
	{
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else 
		{
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) 
			{
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}
	
	void TableCloth() {
		ProductionBegin( ENonTerminal.TableCloth );
		ManyOrOneCommand();
		ProductionEnd( ENonTerminal.TableCloth );
	}

	void ManyOrOneCommand() {
		ProductionBegin( ENonTerminal.ManyOrOneCommand );
		Command();
		if (la.kind == EndOfCommand) {
			Get();
			if (StartOf(1)) {
				ManyOrOneCommand();
			}
		}
		ProductionEnd( ENonTerminal.ManyOrOneCommand );
	}

	void Command() {
		ProductionBegin( ENonTerminal.Command );
		if (StartOf(2)) {
			ExpressionOrDefineVariableCommand();
		} else if (la.kind == Clear) {
			DeleteVariableCommand();
		} else SynErr(26);
		ProductionEnd( ENonTerminal.Command );
	}

	void ExpressionOrDefineVariableCommand() {
		ProductionBegin( ENonTerminal.ExpressionOrDefineVariableCommand );
		if (GetNextTokenKind() != Equal && la.kind != New ) {
			ExpressionCode();
		} else if (la.kind == IdentifierString || la.kind == New) {
			DefineVariableCommand();
		} else SynErr(27);
		ProductionEnd( ENonTerminal.ExpressionOrDefineVariableCommand );
	}

	void DeleteVariableCommand() {
		ProductionBegin( ENonTerminal.DeleteVariableCommand );
		Expect(Clear);
		Expect(LeftRoundBracket);
		Identifier();
		Expect(RightRoundBracket);
		ProductionEnd( ENonTerminal.DeleteVariableCommand );
	}

	void ExpressionCode() {
		ProductionBegin( ENonTerminal.ExpressionCode );
		ExpressionOrListOfExpression();
		ProductionEnd( ENonTerminal.ExpressionCode );
	}

	void DefineVariableCommand() {
		ProductionBegin( ENonTerminal.DefineVariableCommand );
		if (la.kind == IdentifierString) {
			Identifier();
			Expect(Equal);
			ExpressionCode();
		} else if (la.kind == New) {
			Get();
			Expect(LeftRoundBracket);
			Identifier();
			Expect(Comma);
			ExpressionCode();
			Expect(RightRoundBracket);
		} else SynErr(28);
		ProductionEnd( ENonTerminal.DefineVariableCommand );
	}

	void Identifier() {
		ProductionBegin( ENonTerminal.Identifier );
		Expect(IdentifierString);
		ProductionEnd( ENonTerminal.Identifier );
	}

	void ExpressionOrListOfExpression() {
		ProductionBegin( ENonTerminal.ExpressionOrListOfExpression );
		if (StartOf(3)) {
			Expression();
		} else if (la.kind == LeftListBracket) {
			ListOfExpression();
		} else SynErr(29);
		ProductionEnd( ENonTerminal.ExpressionOrListOfExpression );
	}

	void Expression() {
		ProductionBegin( ENonTerminal.Expression );
		EquImplExpression();
		if (la.kind == Sheffer || la.kind == Pirse) {
			if (la.kind == Pirse) {
				Get();
				Expression();
				PushPirse(); 
			} else {
				Get();
				Expression();
				PushSheffer(); 
			}
		}
		ProductionEnd( ENonTerminal.Expression );
	}

	void ListOfExpression() {
		ProductionBegin( ENonTerminal.ListOfExpression );
		Expect(LeftListBracket);
		if (StartOf(4)) {
			ExpressionEnumeration();
		}
		Expect(RightListBracket);
		ProductionEnd( ENonTerminal.ListOfExpression );
	}

	void EquImplExpression() {
		ProductionBegin( ENonTerminal.EquImplExpression );
		XorExpression();
		if (la.kind == Equivalence || la.kind == Implication) {
			if (la.kind == Equivalence) {
				Get();
				EquImplExpression();
				PushEquivalence(); 
			} else {
				Get();
				EquImplExpression();
				PushImplication(); 
			}
		}
		ProductionEnd( ENonTerminal.EquImplExpression );
	}

	void XorExpression() {
		ProductionBegin( ENonTerminal.XorExpression );
		OrExpression();
		if (la.kind == Xor) {
			Get();
			XorExpression();
			PushXor(); 
		}
		ProductionEnd( ENonTerminal.XorExpression );
	}

	void OrExpression() {
		ProductionBegin( ENonTerminal.OrExpression );
		AndExpression();
		if (la.kind == Or) {
			Get();
			OrExpression();
			PushOr(); 
		}
		ProductionEnd( ENonTerminal.OrExpression );
	}

	void AndExpression() {
		ProductionBegin( ENonTerminal.AndExpression );
		NotExpression();
		if (la.kind == And) {
			Get();
			AndExpression();
			PushAnd(); 
		}
		ProductionEnd( ENonTerminal.AndExpression );
	}

	void NotExpression() {
		ProductionBegin( ENonTerminal.NotExpression );
		if (StartOf(5)) {
			SimplyExpression();
		} else if (la.kind == Not) {
			Get();
			NotExpression();
			PushNot(); 
		} else SynErr(30);
		ProductionEnd( ENonTerminal.NotExpression );
	}

	void SimplyExpression() {
		ProductionBegin( ENonTerminal.SimplyExpression );
		if (la.kind == IdentifierString) {
			IdentifierOrFunction();
		} else if (StartOf(6)) {
			Constant();
		} else if (la.kind == LeftRoundBracket) {
			Get();
			Expression();
			Expect(RightRoundBracket);
		} else SynErr(31);
		ProductionEnd( ENonTerminal.SimplyExpression );
	}

	void IdentifierOrFunction() {
		ProductionBegin( ENonTerminal.IdentifierOrFunction );
		Identifier();
		if (la.kind == LeftRoundBracket) {
			FunctionBracketsAndArguments();
		}
		ProductionEnd( ENonTerminal.IdentifierOrFunction );
	}

	void Constant() {
		ProductionBegin( ENonTerminal.Constant );
		if (la.kind == TrueConstant || la.kind == True || la.kind == TrueCaps) {
			ConstantT();
		} else if (la.kind == FalseConstant || la.kind == False || la.kind == FalseCaps) {
			ConstantF();
		} else SynErr(32);
		ProductionEnd( ENonTerminal.Constant );
	}

	void FunctionBracketsAndArguments() {
		ProductionBegin( ENonTerminal.FunctionBracketsAndArguments );
		Expect(LeftRoundBracket);
		if (StartOf(4)) {
			ListOfArguments();
		}
		Expect(RightRoundBracket);
		ProductionEnd( ENonTerminal.FunctionBracketsAndArguments );
	}

	void ConstantT() {
		ProductionBegin( ENonTerminal.ConstantT );
		if (la.kind == True) {
			Get();
		} else if (la.kind == TrueCaps) {
			Get();
		} else if (la.kind == TrueConstant) {
			Get();
		} else SynErr(33);
		ProductionEnd( ENonTerminal.ConstantT );
	}

	void ConstantF() {
		ProductionBegin( ENonTerminal.ConstantF );
		if (la.kind == False) {
			Get();
		} else if (la.kind == FalseCaps) {
			Get();
		} else if (la.kind == FalseConstant) {
			Get();
		} else SynErr(34);
		ProductionEnd( ENonTerminal.ConstantF );
	}

	void ListOfArguments() {
		ProductionBegin( ENonTerminal.ListOfArguments );
		ExpressionOrListOfExpression();
		PushArgumentToFunction(); 
		if (la.kind == Comma) {
			Get();
			ListOfArguments();
		}
		ProductionEnd( ENonTerminal.ListOfArguments );
	}

	void ExpressionEnumeration() {
		ProductionBegin( ENonTerminal.ExpressionEnumeration );
		ExpressionOrListOfExpression();
		PushExpressionToList(); 
		if (la.kind == Comma) {
			Get();
			ExpressionEnumeration();
		}
		ProductionEnd( ENonTerminal.ExpressionEnumeration );
	}



    public void Parse( Scanner s )
    {
        _scanner = s;
		errDist = minErrDist;
        Errors.Clear();

        Parse();
    }
	
	public void Parse() 
	{
		la = new Token();
		la.val = "";		
		Get();
		TableCloth();
		Expect(EOF);

	}
	
	static readonly bool[,] set =
	{
		{T,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, T,T,F,T, F,F,F,F, F,F,F,T, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, T,F,F,T, F,F,F,F, F,F,F,T, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, F,F,F,T, F,F,F,F, F,F,F,T, F,F,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, F,F,F,T, F,F,F,F, F,F,F,T, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, F,F,F,F, F,F,F,F, F,F,F,T, F,F,F,F, F,F,F},
		{F,F,T,T, T,T,T,T, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F}

	};
}

public class ParserErrors
{
    public int TotalErrorsAmount { get; set; }
    public int TotalWarningsAmount { get; set; }

    public enum EType 
    {
        Error,
        Warning
    }

    public struct Data
    {
        public int Line;
        public int Column;

        public EType Type;

        public string Text;
    }

    public delegate void MessageDelegate( Data data );

    public event MessageDelegate Message;

    public ParserErrors()
    {
        Clear();
    }

    public void Clear()
    {
        TotalErrorsAmount = 0;
        TotalWarningsAmount = 0;
    }
	
    public virtual void SynErr( int line, int col, int n )
    {
		string s;

		switch ( n )
		{
			case 0: s = "EOF expected"; break;
			case 1: s = "IdentifierString expected"; break;
			case 2: s = "TrueConstant expected"; break;
			case 3: s = "FalseConstant expected"; break;
			case 4: s = "True expected"; break;
			case 5: s = "False expected"; break;
			case 6: s = "TrueCaps expected"; break;
			case 7: s = "FalseCaps expected"; break;
			case 8: s = "New expected"; break;
			case 9: s = "Clear expected"; break;
			case 10: s = "Equal expected"; break;
			case 11: s = "Not expected"; break;
			case 12: s = "And expected"; break;
			case 13: s = "Or expected"; break;
			case 14: s = "Xor expected"; break;
			case 15: s = "Equivalence expected"; break;
			case 16: s = "Implication expected"; break;
			case 17: s = "Sheffer expected"; break;
			case 18: s = "Pirse expected"; break;
			case 19: s = "LeftRoundBracket expected"; break;
			case 20: s = "RightRoundBracket expected"; break;
			case 21: s = "LeftListBracket expected"; break;
			case 22: s = "RightListBracket expected"; break;
			case 23: s = "EndOfCommand expected"; break;
			case 24: s = "Comma expected"; break;
			case 25: s = "??? expected"; break;
			case 26: s = "invalid Command"; break;
			case 27: s = "invalid ExpressionOrDefineVariableCommand"; break;
			case 28: s = "invalid DefineVariableCommand"; break;
			case 29: s = "invalid ExpressionOrListOfExpression"; break;
			case 30: s = "invalid NotExpression"; break;
			case 31: s = "invalid SimplyExpression"; break;
			case 32: s = "invalid Constant"; break;
			case 33: s = "invalid ConstantT"; break;
			case 34: s = "invalid ConstantF"; break;

			default: s = "error " + n; break;
		}

		TotalErrorsAmount++;
        if ( Message != null )
        {
            Message( new Data { Line = line, Column = col, Type = EType.Error, Text = s } );
        }
	}

    public virtual void SemErr( int line, int col, string s )
    {
        TotalErrorsAmount++;
        if ( Message != null )
        {
            Message( new Data { Line = line, Column = col, Type = EType.Error, Text = s } );
        }
    }

    public virtual void SemErr( string s )
    {
        TotalErrorsAmount++;
        if ( Message != null )
        {
            Message( new Data { Type = EType.Error, Text = s } );
        }
    }

    public virtual void Warning( int line, int col, string s )
    {
        TotalWarningsAmount++;
        if ( Message != null )
        {
            Message( new Data { Line = line, Column = col, Type = EType.Warning, Text = s } );
        }
    }

    public virtual void Warning( string s )
    {
        TotalWarningsAmount++;
        if ( Message != null )
        {
            Message( new Data { Type = EType.Warning, Text = s } );
        }
    }
}

public class FatalError : Exception
{
	public FatalError( string m ):
		base( m )
	{
	}
}
}