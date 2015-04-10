
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
	const int ExpressionType = 8;
	const int Equal = 9;
	const int Not = 10;
	const int And = 11;
	const int Or = 12;
	const int Xor = 13;
	const int Equivalence = 14;
	const int Implication = 15;
	const int Sheffer = 16;
	const int Pirse = 17;
	const int LeftRoundBracket = 18;
	const int RightRoundBracket = 19;
	const int LeftListBracket = 20;
	const int RightListBracket = 21;
	const int EndOfCommand = 22;
	const int Comma = 23;

	enum ENonTerminal
	{
		TableCloth,
		ManyOrOneCommand,
		Command,
		ExpressionOrCreateNewVariableCommand,
		DeleteVariableCommand,
		GetExpressionTypeCommand,
		ExpressionCode,
		CreateNewVariableCommand,
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
		ListOfArguments,
		ListOfExpression,
		ExpressionEnumeration
	}

	public const int maxT = 24;

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
	
    string CurrentToken { get { return t.val; } }
	
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
			ExpressionOrCreateNewVariableCommand();
		} else if (la.kind == Clear) {
			DeleteVariableCommand();
		} else if (la.kind == ExpressionType) {
			GetExpressionTypeCommand();
		} else SynErr(25);
		ProductionEnd( ENonTerminal.Command );
	}

	void ExpressionOrCreateNewVariableCommand() {
		ProductionBegin( ENonTerminal.ExpressionOrCreateNewVariableCommand );
		if (GetNextTokenKind() != Equal) {
			ExpressionCode();
		} else if (la.kind == IdentifierString || la.kind == New) {
			CreateNewVariableCommand();
		} else SynErr(26);
		ProductionEnd( ENonTerminal.ExpressionOrCreateNewVariableCommand );
	}

	void DeleteVariableCommand() {
		ProductionBegin( ENonTerminal.DeleteVariableCommand );
		Expect(Clear);
		Expect(LeftRoundBracket);
		Identifier();
		Expect(RightRoundBracket);
		ProductionEnd( ENonTerminal.DeleteVariableCommand );
	}

	void GetExpressionTypeCommand() {
		ProductionBegin( ENonTerminal.GetExpressionTypeCommand );
		Expect(ExpressionType);
		Expect(LeftRoundBracket);
		ExpressionCode();
		Expect(RightRoundBracket);
		ProductionEnd( ENonTerminal.GetExpressionTypeCommand );
	}

	void ExpressionCode() {
		ProductionBegin( ENonTerminal.ExpressionCode );
		Expression();
		ProductionEnd( ENonTerminal.ExpressionCode );
	}

	void CreateNewVariableCommand() {
		ProductionBegin( ENonTerminal.CreateNewVariableCommand );
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
		} else SynErr(27);
		ProductionEnd( ENonTerminal.CreateNewVariableCommand );
	}

	void Identifier() {
		ProductionBegin( ENonTerminal.Identifier );
		Expect(IdentifierString);
		PushString( t.val ); 
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
				PushEqu(); 
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
		if (StartOf(3)) {
			SimplyExpression();
		} else if (la.kind == Not) {
			Get();
			NotExpression();
			PushNot(); 
		} else SynErr(28);
		ProductionEnd( ENonTerminal.NotExpression );
	}

	void SimplyExpression() {
		ProductionBegin( ENonTerminal.SimplyExpression );
		if (la.kind == IdentifierString) {
			IdentifierOrFunction();
		} else if (StartOf(4)) {
			Constant();
		} else if (la.kind == LeftRoundBracket) {
			Get();
			Expression();
			Expect(RightRoundBracket);
		} else SynErr(29);
		ProductionEnd( ENonTerminal.SimplyExpression );
	}

	void IdentifierOrFunction() {
		ProductionBegin( ENonTerminal.IdentifierOrFunction );
		Identifier();
		PushVariable(t.val); 
		if (la.kind == LeftRoundBracket) {
			FunctionBracketsAndArguments();
			TcDebug.Log( "func" ); 
		}
		ProductionEnd( ENonTerminal.IdentifierOrFunction );
	}

	void Constant() {
		ProductionBegin( ENonTerminal.Constant );
		if (la.kind == TrueConstant || la.kind == True) {
			ConstantT();
		} else if (la.kind == FalseConstant || la.kind == False) {
			ConstantF();
		} else SynErr(30);
		ProductionEnd( ENonTerminal.Constant );
	}

	void FunctionBracketsAndArguments() {
		ProductionBegin( ENonTerminal.FunctionBracketsAndArguments );
		Expect(LeftRoundBracket);
		if (StartOf(5)) {
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
		} else SynErr(31);
		ProductionEnd( ENonTerminal.ConstantT );
	}

	void ConstantF() {
		ProductionBegin( ENonTerminal.ConstantF );
		if (la.kind == False) {
			Get();
		} else if (la.kind == FalseConstant) {
			Get();
		} else SynErr(32);
		ProductionEnd( ENonTerminal.ConstantF );
	}

	void ListOfArguments() {
		ProductionBegin( ENonTerminal.ListOfArguments );
		if (StartOf(6)) {
			ExpressionCode();
		} else if (la.kind == LeftListBracket) {
			ListOfExpression();
		} else SynErr(33);
		if (la.kind == Comma) {
			Get();
			ListOfArguments();
		}
		ProductionEnd( ENonTerminal.ListOfArguments );
	}

	void ListOfExpression() {
		ProductionBegin( ENonTerminal.ListOfExpression );
		Expect(LeftListBracket);
		ExpressionEnumeration();
		Expect(RightListBracket);
		ProductionEnd( ENonTerminal.ListOfExpression );
	}

	void ExpressionEnumeration() {
		ProductionBegin( ENonTerminal.ExpressionEnumeration );
		ExpressionCode();
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
		{T,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F},
		{F,T,T,T, T,T,T,T, T,F,T,F, F,F,F,F, F,F,T,F, F,F,F,F, F,F},
		{F,T,T,T, T,T,T,F, F,F,T,F, F,F,F,F, F,F,T,F, F,F,F,F, F,F},
		{F,T,T,T, T,T,F,F, F,F,F,F, F,F,F,F, F,F,T,F, F,F,F,F, F,F},
		{F,F,T,T, T,T,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F,F,F, F,F},
		{F,T,T,T, T,T,F,F, F,F,T,F, F,F,F,F, F,F,T,F, T,F,F,F, F,F},
		{F,T,T,T, T,T,F,F, F,F,T,F, F,F,F,F, F,F,T,F, F,F,F,F, F,F}

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
			case 8: s = "ExpressionType expected"; break;
			case 9: s = "Equal expected"; break;
			case 10: s = "Not expected"; break;
			case 11: s = "And expected"; break;
			case 12: s = "Or expected"; break;
			case 13: s = "Xor expected"; break;
			case 14: s = "Equivalence expected"; break;
			case 15: s = "Implication expected"; break;
			case 16: s = "Sheffer expected"; break;
			case 17: s = "Pirse expected"; break;
			case 18: s = "LeftRoundBracket expected"; break;
			case 19: s = "RightRoundBracket expected"; break;
			case 20: s = "LeftListBracket expected"; break;
			case 21: s = "RightListBracket expected"; break;
			case 22: s = "EndOfCommand expected"; break;
			case 23: s = "Comma expected"; break;
			case 24: s = "??? expected"; break;
			case 25: s = "invalid Command"; break;
			case 26: s = "invalid ExpressionOrCreateNewVariableCommand"; break;
			case 27: s = "invalid CreateNewVariableCommand"; break;
			case 28: s = "invalid NotExpression"; break;
			case 29: s = "invalid SimplyExpression"; break;
			case 30: s = "invalid Constant"; break;
			case 31: s = "invalid ConstantT"; break;
			case 32: s = "invalid ConstantF"; break;
			case 33: s = "invalid ListOfArguments"; break;

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