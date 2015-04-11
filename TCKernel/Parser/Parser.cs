
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
	const int New = 6;
	const int Clear = 7;
	const int Equal = 8;
	const int Not = 9;
	const int And = 10;
	const int Or = 11;
	const int Xor = 12;
	const int Equivalence = 13;
	const int Implication = 14;
	const int Sheffer = 15;
	const int Pirse = 16;
	const int LeftRoundBracket = 17;
	const int RightRoundBracket = 18;
	const int EndOfCommand = 19;
	const int Comma = 20;

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
		Expression,
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
		ListOfArguments
	}

	public const int maxT = 21;

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
		} else SynErr(22);
		ProductionEnd( ENonTerminal.Command );
	}

	void ExpressionOrDefineVariableCommand() {
		ProductionBegin( ENonTerminal.ExpressionOrDefineVariableCommand );
		if (StartOf(3)) {
			ExpressionCode();
		} else if (la.kind == IdentifierString || la.kind == New) {
			DefineVariableCommand();
		} else SynErr(23);
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
		Expression();
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
		} else SynErr(24);
		ProductionEnd( ENonTerminal.DefineVariableCommand );
	}

	void Identifier() {
		ProductionBegin( ENonTerminal.Identifier );
		Expect(IdentifierString);
		ProductionEnd( ENonTerminal.Identifier );
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
		if (StartOf(4)) {
			SimplyExpression();
		} else if (la.kind == Not) {
			Get();
			NotExpression();
			PushNot(); 
		} else SynErr(25);
		ProductionEnd( ENonTerminal.NotExpression );
	}

	void SimplyExpression() {
		ProductionBegin( ENonTerminal.SimplyExpression );
		if (la.kind == IdentifierString) {
			IdentifierOrFunction();
		} else if (StartOf(5)) {
			Constant();
		} else if (la.kind == LeftRoundBracket) {
			Get();
			Expression();
			Expect(RightRoundBracket);
		} else SynErr(26);
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
		if (la.kind == TrueConstant || la.kind == True) {
			ConstantT();
		} else if (la.kind == FalseConstant || la.kind == False) {
			ConstantF();
		} else SynErr(27);
		ProductionEnd( ENonTerminal.Constant );
	}

	void FunctionBracketsAndArguments() {
		ProductionBegin( ENonTerminal.FunctionBracketsAndArguments );
		Expect(LeftRoundBracket);
		if (StartOf(3)) {
			ListOfArguments();
		}
		Expect(RightRoundBracket);
		ProductionEnd( ENonTerminal.FunctionBracketsAndArguments );
	}

	void ConstantT() {
		ProductionBegin( ENonTerminal.ConstantT );
		if (la.kind == True) {
			Get();
		} else if (la.kind == TrueConstant) {
			Get();
		} else SynErr(28);
		ProductionEnd( ENonTerminal.ConstantT );
	}

	void ConstantF() {
		ProductionBegin( ENonTerminal.ConstantF );
		if (la.kind == False) {
			Get();
		} else if (la.kind == FalseConstant) {
			Get();
		} else SynErr(29);
		ProductionEnd( ENonTerminal.ConstantF );
	}

	void ListOfArguments() {
		ProductionBegin( ENonTerminal.ListOfArguments );
		Expression();
		PushArgumentToFunction(); 
		if (la.kind == Comma) {
			Get();
			ListOfArguments();
		}
		ProductionEnd( ENonTerminal.ListOfArguments );
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
		{T,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F},
		{F,T,T,T, T,T,T,T, F,T,F,F, F,F,F,F, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,T,F, F,T,F,F, F,F,F,F, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,F,F, F,T,F,F, F,F,F,F, F,T,F,F, F,F,F},
		{F,T,T,T, T,T,F,F, F,F,F,F, F,F,F,F, F,T,F,F, F,F,F},
		{F,F,T,T, T,T,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F}

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
			case 6: s = "New expected"; break;
			case 7: s = "Clear expected"; break;
			case 8: s = "Equal expected"; break;
			case 9: s = "Not expected"; break;
			case 10: s = "And expected"; break;
			case 11: s = "Or expected"; break;
			case 12: s = "Xor expected"; break;
			case 13: s = "Equivalence expected"; break;
			case 14: s = "Implication expected"; break;
			case 15: s = "Sheffer expected"; break;
			case 16: s = "Pirse expected"; break;
			case 17: s = "LeftRoundBracket expected"; break;
			case 18: s = "RightRoundBracket expected"; break;
			case 19: s = "EndOfCommand expected"; break;
			case 20: s = "Comma expected"; break;
			case 21: s = "??? expected"; break;
			case 22: s = "invalid Command"; break;
			case 23: s = "invalid ExpressionOrDefineVariableCommand"; break;
			case 24: s = "invalid DefineVariableCommand"; break;
			case 25: s = "invalid NotExpression"; break;
			case 26: s = "invalid SimplyExpression"; break;
			case 27: s = "invalid Constant"; break;
			case 28: s = "invalid ConstantT"; break;
			case 29: s = "invalid ConstantF"; break;

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