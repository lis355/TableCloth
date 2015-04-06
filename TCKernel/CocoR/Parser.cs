
using System;
using System.Collections.Generic;

namespace TableClothKernel {



partial class Parser {
	public const int _EOF = 0;
	public const int _Identifier = 1;
	public const int _TrueConstant = 2;
	public const int _FalseConstant = 3;
	public const int _True = 4;
	public const int _False = 5;
	public const int _New = 6;
	public const int _Clear = 7;
	public const int _ExpressionType = 8;
	public const int _Equal = 9;
	public const int _Not = 10;
	public const int _And = 11;
	public const int _Or = 12;
	public const int _Xor = 13;
	public const int _Equivalence = 14;
	public const int _Implication = 15;
	public const int _Sheffer = 16;
	public const int _Pirse = 17;
	public const int _LeftRoundBracket = 18;
	public const int _RightRoundBracket = 19;
	public const int _LeftListBracket = 20;
	public const int _RightListBracket = 21;
	public const int _EndOfCommand = 22;
	public const int _Comma = 23;
	public const int maxT = 24;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

int GetNextTokenTypeEqual() { return scanner.Peek().kind; }



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
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
		Cur = Tree; 
		Command();
		if (la.kind == 22) {
			Get();
			if (StartOf(1)) {
				ManyOrOneCommand();
			}
		}
	}

	void Command() {
		if (GetNextTokenTypeEqual() != _Equal) {
			ExpressionCode();
			AddCh(new ExpressionCommandNode()); /*resultString += ExpressionString.ConstantToString(tmpExpression.Root.CalcExpressionOnThisVertex()) + "; ";*/ 
		} else if (la.kind == 1 || la.kind == 6) {
			CreateNewVariableCommand();
		} else if (la.kind == 7) {
			DeleteVariableCommand();
		} else if (la.kind == 8) {
			GetExpressionTypeCommand();
		} else SynErr(25);
	}

	void ExpressionCode() {
		tmpExpression = new Expression(); 
		Expression();
		tmpExpression.Root = EV.Pop(); 
		AddCh(new ExpressionNode(tmpExpression.Root.ToString())); 
	}

	void CreateNewVariableCommand() {
		if (la.kind == 1) {
			Get();
			tmpIdentifier = t.val; 
			Expect(9);
			ExpressionCode();
			GlobalVariableList.New(tmpIdentifier, tmpExpression); 
		} else if (la.kind == 6) {
			Get();
			Expect(18);
			Expect(1);
			Expect(23);
			ExpressionCode();
			Expect(19);
		} else SynErr(26);
	}

	void DeleteVariableCommand() {
		Expect(7);
		Expect(18);
		Expect(1);
		Expect(19);
	}

	void GetExpressionTypeCommand() {
		Expect(8);
		Expect(18);
		ExpressionCode();
		Expect(19);
	}

	void Expression() {
		EquImplExpression();
		if (la.kind == 16 || la.kind == 17) {
			if (la.kind == 17) {
				Get();
				Expression();
				PushPirse(); 
			} else {
				Get();
				Expression();
				PushShef(); 
			}
		}
	}

	void EquImplExpression() {
		XorExpression();
		if (la.kind == 14 || la.kind == 15) {
			if (la.kind == 14) {
				Get();
				EquImplExpression();
				PushEqu(); 
			} else {
				Get();
				EquImplExpression();
				PushImpl(); 
			}
		}
	}

	void XorExpression() {
		OrExpression();
		if (la.kind == 13) {
			Get();
			XorExpression();
			PushXor(); 
		}
	}

	void OrExpression() {
		AndExpression();
		if (la.kind == 12) {
			Get();
			OrExpression();
			PushOr(); 
		}
	}

	void AndExpression() {
		NotExpression();
		if (la.kind == 11) {
			Get();
			AndExpression();
			PushAnd(); 
		}
	}

	void NotExpression() {
		if (StartOf(2)) {
			SimplyExpression();
		} else if (la.kind == 10) {
			Get();
			NotExpression();
			PushNot(); 
		} else SynErr(27);
	}

	void SimplyExpression() {
		if (GetNextTokenTypeEqual() == _LeftRoundBracket) {
			Expect(1);
			FunctionBracketsAndArguments();
		} else if (la.kind == 1) {
			Get();
			PushVariable(t.val); 
		} else if (StartOf(3)) {
			Constant();
		} else if (la.kind == 18) {
			Get();
			Expression();
			Expect(19);
		} else SynErr(28);
	}

	void FunctionBracketsAndArguments() {
		Expect(18);
		if (StartOf(4)) {
			ListOfArguments();
		}
		Expect(19);
	}

	void Constant() {
		if (la.kind == 4) {
			Get();
			PushTrueConstant(); 
		} else if (la.kind == 2) {
			Get();
			PushTrueConstant(); 
		} else if (la.kind == 5) {
			Get();
			PushFalseConstant(); 
		} else if (la.kind == 3) {
			Get();
			PushFalseConstant(); 
		} else SynErr(29);
	}

	void ListOfArguments() {
		if (StartOf(5)) {
			ExpressionCode();
		} else if (la.kind == 20) {
			ListOfExpression();
		} else SynErr(30);
		if (la.kind == 23) {
			Get();
			ListOfArguments();
		}
	}

	void ListOfExpression() {
		Expect(20);
		ExpressionEnumeration();
		Expect(21);
	}

	void ExpressionEnumeration() {
		ExpressionCode();
		if (la.kind == 23) {
			Get();
			ExpressionEnumeration();
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		TableCloth();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,T,T,T, T,T,T,T, T,x,T,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x},
		{x,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x},
		{x,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,T,T,T, T,T,x,x, x,x,T,x, x,x,x,x, x,x,T,x, T,x,x,x, x,x},
		{x,T,T,T, T,T,x,x, x,x,T,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x}

	};
} // end Parser


class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "Identifier expected"; break;
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
			case 26: s = "invalid CreateNewVariableCommand"; break;
			case 27: s = "invalid NotExpression"; break;
			case 28: s = "invalid SimplyExpression"; break;
			case 29: s = "invalid Constant"; break;
			case 30: s = "invalid ListOfArguments"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}