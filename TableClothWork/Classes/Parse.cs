using System;
using System.Text.RegularExpressions;

namespace TableClothKernel
{
    public static class Parse
    {
        /// <summary>
        /// Удаляет пробельные символы в строке и комментарии
        /// </summary>
        public static void DeleteSpace(ref string S)
        {
            if (S.Length == 0) return;

            // удаляем комментарии
            int d = S.IndexOf("//"), e = 0;
            S += '\n'; // добавляем в конец строки данный символ чтобы не проверять потом комментарий в конце строки
            while (d != -1)
            {
                e = d + 1;
                do e++; while (S[e] != '\n');
                S = S.Remove(d, e - d + 1);
                d = S.IndexOf("//");
            }

            // удаляем все переновы строки
            S = S.Replace("\n","");
            S = S.Replace("\r", "");

            // теперь нужно удалить пробелы и табуляции так, чтобы если они разделяют 2 идентификатора, 
            // то эти идентификаторы не слились воедино
            for (int i = 1; i < S.Length - 1; i++)
            {
                if (S[i] == ' ' || S[i] == '\t')
                    if (!(Char.IsLetterOrDigit(Convert.ToChar(S[i - 1])) && Char.IsLetterOrDigit(Convert.ToChar(S[i + 1]))))
                        S = S.Remove(i--, 1);
            }
            
            // еще пробелы могут остаться в конце и в начале строки
            if (S.Length == 0) return;
            while (S[0] == ' ' || S[0] == '\t') S = S.Remove(0, 1);
            while (S[S.Length - 1] == ' ' || S[S.Length - 1] == '\t') S = S.Remove(S.Length - 1, 1);
        }

        /// <summary>
        /// Получает первый идентификатор из строки
        /// </summary>
        public static string GetIdentificator(string S)
        {
            Match P = Regex.Match(S, Variable);
            return S.Substring(P.Index, P.Length);
        }

        /// <summary>
        /// Для текущей скобки '(' на ходит парную
        /// </summary>
        public static int GetNextBracket(string S, int h)
        {
            if (S[h] != '(') return -1;
            int Deep = 0;
            for (int i = h + 1; i < S.Length; i++)
            {
                if (S[i] == ')')
                {
                    if (Deep == 0) return i;
                    else Deep--;
                }
                if (S[i] == '(') Deep++;
            }
            return -1;
        }

        /// <summary>
        /// Получает следующий токен в выражении
        /// </summary>
        /// <param name="S">Входная строка</param>
        /// <param name="n">Позиция, с кторой начинается поиск</param>
        /// <returns></returns>
        public static int GetNextToken(ref string S, ref int n, ref string Identificator)
        {
            if (n >= S.Length) return SyntaxResult.EndOfLine;
            else if (S[n] == '0' || S[n] == '1') { n++; return SyntaxResult.Constant; }
            else if (S[n] == '(') { n++; return SyntaxResult.OpenBracket; }
            else if (S[n] == ')') { n++; return SyntaxResult.CloseBracket; }
            else if (S[n] == '=') { n++; return SyntaxResult.EqualSymbol; }
            else if (S[n] == ';') { n++; return SyntaxResult.SemicolumnSymbol; }
            else if (S[n] == '!') { n++; return SyntaxResult.Inversion; }
            else if (S[n] == '+' || S[n] == '*' || S[n] == '^' || S[n] == '>' || S[n] == '|'
                || S[n] == '~') { n++; return SyntaxResult.Operation; }
            else if (Char.IsLetter(S[n]))
            {
                Identificator = Parse.GetIdentificator(S.Substring(n));
                n += Identificator.Length;
                if (Syntax.IsFunctionWithOneParameter(Identificator))
                {
                    if (Syntax.IsFunctionWithOneParameterAndCanUseInExpression(Identificator))
                        return SyntaxResult.FunctionWithOneParameterAndCanUseInExpression;
                    return SyntaxResult.FunctionWithOneParameter;
                }
                else if (Syntax.IsFunctionWithManyParameters(Identificator))
                {
                    if (Syntax.IsFunctionWithManyParametersAndCanUseInExpression(Identificator))
                        return SyntaxResult.FunctionWithManyParametersAndCanUseInExpression;
                    return SyntaxResult.FunctionWithManyParameters;
                }
                return SyntaxResult.Variable;
            }
            else return SyntaxResult.AnotherSymbol;
        }

        /// <summary>
        /// Регулярные выражения для переменной и операции
        /// </summary>
        public static string Variable = @"^([a-zA-Z]+\d*)+";
        public static string Operation = @"^[~!\+\*^>\|]$";
        public static string StandartVariable = @"^[xX]\d+";

        #region Правильные имена функций
        public static string[] FunctionNames =
        {
/*0         Formula*/"ConvertToDual",/*(Formula)*/           
/*1         Formula*/"ConvertToSDNF",/*(Formula)*/
/*2         Formula*/"ConvertToSCNF",/*(Formula)*/
/*3         Formula*/"ConvertToJegalkinPoly",/*(Formula)*/
/*4         Formula*/"ConvertToBasis",/*(BasisName,Formula)*/ 
/*5      True/False*/"CheckBelongTZero",/*(Formula)*/
/*6      True/False*/"CheckBelongTOne",/*(Formula)*/
/*7      True/False*/"CheckBelongS",/*(Formula)*/
/*8      True/False*/"CheckBelongM",/*(Formula)*/
/*9      True/False*/"CheckBelongL",/*(Formula)*/
/*10     True/False*/"CheckFullSystem",/*(Formula1,Formula2,...)*/
/*11     True/False*/"CheckBasis",/*(Formula1,Formula2,...)*/
/*12    {x1,x2,...}*/"GetFictitiousVars",/*(Formula)*/
/*13  {0/1,0/1,...}*/"GetFormulaVector",/*(Formula)*/
/*14               */"GetTruthTable",/*(Formula)*/
/*15               */"GetScheme",/*(Formula)*/
/*16            0/1*/"CalcOnVector",/*(Formula,0/1,0/1,...)*/
/*17     True/False*/"CreateBasis",/*(BasisName,Formula1,Formula2,...)*/
/*18        Formula*/"MinimizeQuine",/*(Formula)*/
/*19        Formula*/"CreateFromVector"/*({0/1,0/1,...})*/
        };
        #endregion

        /// <summary>
        /// Проверка строки на корректный идентификатор
        /// </summary>
        public static bool IsVariable(string S)
        {
            return Regex.IsMatch(S, Variable, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Проверка строки на стандартный идентификатор x1 x2 ...
        /// </summary>
        public static bool IsStandartVariable(string S)
        {
            return Regex.IsMatch(S, StandartVariable, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Проверка строки на корректную операцию
        /// </summary>
        public static bool IsOperation(string S)
        {
            return Regex.IsMatch(S, Operation);
        }

        /// <summary>
        /// Проверка символа на операнд из скриптовой строки
        /// </summary>
        public static bool IsOperand(char C)
        {
            return (C == '$' || C == '%' || C == '0' || C == '1');
        }

        /// <summary>
        /// Проверка строки на корректный идентификатор функции
        /// </summary>
        public static bool IsFunction(string S)
        {
            bool result = false;
            for (int i = 0; i < FunctionNames.Length; i++)
            {
                if (S == FunctionNames[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static bool IsFunctionWithOneParameter(string S)
        {
            return (S == FunctionNames[0] ||
                    S == FunctionNames[1] ||
                    S == FunctionNames[2] ||
                    S == FunctionNames[3] ||
                    S == FunctionNames[5] ||
                    S == FunctionNames[6] ||
                    S == FunctionNames[7] ||
                    S == FunctionNames[8] ||
                    S == FunctionNames[9] ||
                    S == FunctionNames[12] ||
                    S == FunctionNames[13] ||
                    S == FunctionNames[14] ||
                    S == FunctionNames[15] ||
                    S == FunctionNames[18]) ? true : false;
        }

        public static bool IsFunctionWithOneParameterAndCanUseInExpression(string S)
        {
            return (S == FunctionNames[0] ||
                    S == FunctionNames[1] ||
                    S == FunctionNames[2] ||
                    S == FunctionNames[3] ||
                    S == FunctionNames[18]) ? true : false;
        }

        public static bool IsFunctionWithManyParameters(string S)
        {
            return (S == FunctionNames[4] ||
                    S == FunctionNames[10] ||
                    S == FunctionNames[11] ||
                    S == FunctionNames[16] ||
                    S == FunctionNames[17] ||
                    S == FunctionNames[19]) ? true : false;
        }

        public static bool IsFunctionWithManyParametersAndCanUseInExpression(string S)
        {
            return (S == FunctionNames[4] ||
                    S == FunctionNames[16] ||
                    S == FunctionNames[19]) ? true : false;
        }

        /// <summary>
        /// Проверка корректности скобок
        /// </summary>
        public static bool BracketsCheck(string S)
        {
            Stack<char> D = new Stack<char>();
            for (int i = 0; i < S.Length; i++)
            {
                if (S[i] == '(')
                {
                    D.Push('(');
                }
                else if (S[i] == ')')
                {
                    if (D.Count == 0) return false;
                    if (D.Pop() != '(') return false;
                }
                else continue;
            }
            if (D.Count != 0) return false;
            return true;
        }

        public static int GetPriority(char C)
        {
            if (C == '(') return 0;
            else if (C == '*') return 1;
            else if (C == '+') return 2;
            else if (C == '^') return 3;
            else if (C == '>') return 4;
            else if (C == '~') return 5;
            else if (C == '|') return 6;
            else return -1;
        }

        public static int GetNumberOfVariableFromFunctionVector(int n)
        {
            int j = 1, k = 0;
            while (j < n) { j = j << 1; k++; }
            return (j == n) ? k : -1;
        }
    }
}
