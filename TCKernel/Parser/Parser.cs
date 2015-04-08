
using System;
using System.Collections.Generic;

namespace TableClothKernel
{


public partial class Parser
{
	public const int EOF = 0;
	public const int IdentifierString = 1;
	public const int TrueConstant = 2;
	public const int FalseConstant = 3;
	public const int True = 4;
	public const int False = 5;
	public const int New = 6;
	public const int Clear = 7;
	public const int ExpressionType = 8;
	public const int Equal = 9;
	public const int Not = 10;
	public const int And = 11;
	public const int Or = 12;
	public const int Xor = 13;
	public const int Equivalence = 14;
	public const int Implication = 15;
	public const int Sheffer = 16;
	public const int Pirse = 17;
	public const int LeftRoundBracket = 18;
	public const int RightRoundBracket = 19;
	public const int LeftListBracket = 20;
	public const int RightListBracket = 21;
	public const int EndOfCommand = 22;
	public const int Comma = 23;
	public const int maxT = 24;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	Scanner _scanner;
	public ParserErrors Errors { get; set; }

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

int GetNextTokenKind() { return _scanner.Peek().kind; }



    public Parser()
    {
        Errors = new ParserErrors();
    }

	void SynErr (int n)
	{
		if (errDist >= minErrDist) Errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg)
	{
		if (errDist >= minErrDist) Errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get ()
	{
		for (;;)
		{
			t = la;
			la = _scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n)
	{
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s)
	{
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow)
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
		ManyOrOneCommand();
	}

	void ManyOrOneCommand() {
		Command();
		if (la.kind == EndOfCommand) {
			Get();
			if (StartOf(1)) {
				ManyOrOneCommand();
			}
		}
	}

	void Command() {
		if (StartOf(2)) {
			ExpressionOrCreateNewVariableCommand();
		} else if (la.kind == Clear) {
			DeleteVariableCommand();
		} else if (la.kind == ExpressionType) {
			GetExpressionTypeCommand();
		} else SynErr(25);
	}

	void ExpressionOrCreateNewVariableCommand() {
		if (GetNextTokenKind() != Equal) {
			ExpressionCode();
		} else if (la.kind == IdentifierString || la.kind == New) {
			CreateNewVariableCommand();
		} else SynErr(26);
	}

	void DeleteVariableCommand() {
		Expect(Clear);
		Expect(LeftRoundBracket);
		Identifier();
		Expect(RightRoundBracket);
	}

	void GetExpressionTypeCommand() {
		Expect(ExpressionType);
		Expect(LeftRoundBracket);
		ExpressionCode();
		Expect(RightRoundBracket);
	}

	void ExpressionCode() {
		Expression();
	}

	void CreateNewVariableCommand() {
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
	}

	void Identifier() {
		Expect(IdentifierString);
		PushString( t.val ); 
	}

	void Expression() {
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
	}

	void EquImplExpression() {
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
	}

	void XorExpression() {
		OrExpression();
		if (la.kind == Xor) {
			Get();
			XorExpression();
			PushXor(); 
		}
	}

	void OrExpression() {
		AndExpression();
		if (la.kind == Or) {
			Get();
			OrExpression();
			PushOr(); 
		}
	}

	void AndExpression() {
		NotExpression();
		if (la.kind == And) {
			Get();
			AndExpression();
			PushAnd(); 
		}
	}

	void NotExpression() {
		if (StartOf(3)) {
			SimplyExpression();
		} else if (la.kind == Not) {
			Get();
			NotExpression();
			PushNot(); 
		} else SynErr(28);
	}

	void SimplyExpression() {
		if (la.kind == IdentifierString) {
			IdentifierOrFunction();
		} else if (StartOf(4)) {
			Constant();
		} else if (la.kind == LeftRoundBracket) {
			Get();
			Expression();
			Expect(RightRoundBracket);
		} else SynErr(29);
	}

	void IdentifierOrFunction() {
		Identifier();
		PushVariable(t.val); 
		if (la.kind == LeftRoundBracket) {
			FunctionBracketsAndArguments();
			TcDebug.Log( "func" ); 
		}
	}

	void Constant() {
		if (la.kind == True) {
			Get();
			PushTrueConstant(); 
		} else if (la.kind == TrueConstant) {
			Get();
			PushTrueConstant(); 
		} else if (la.kind == False) {
			Get();
			PushFalseConstant(); 
		} else if (la.kind == FalseConstant) {
			Get();
			PushFalseConstant(); 
		} else SynErr(30);
	}

	void FunctionBracketsAndArguments() {
		Expect(LeftRoundBracket);
		if (StartOf(5)) {
			ListOfArguments();
		}
		Expect(RightRoundBracket);
	}

	void ListOfArguments() {
		if (StartOf(6)) {
			ExpressionCode();
		} else if (la.kind == LeftListBracket) {
			ListOfExpression();
		} else SynErr(31);
		if (la.kind == Comma) {
			Get();
			ListOfArguments();
		}
	}

	void ListOfExpression() {
		Expect(LeftListBracket);
		ExpressionEnumeration();
		Expect(RightListBracket);
	}

	void ExpressionEnumeration() {
		ExpressionCode();
		if (la.kind == Comma) {
			Get();
			ExpressionEnumeration();
		}
	}



    public void Parse( Scanner s )
    {
        _scanner = s;
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
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x},
		{_x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_T,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x},
		{_x,_T,_T,_T, _T,_T,_T,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x},
		{_x,_x,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x},
		{_x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x}

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
			case 31: s = "invalid ListOfArguments"; break;

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