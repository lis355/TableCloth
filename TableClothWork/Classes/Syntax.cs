using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryCalc
{
    static class SyntaxResult
    {
        public const int
            AnotherSymbol = 0,
            EndOfLine = 1,
            Variable = 2,
            Constant = 3,
            FunctionWithOneParameter = 4,
            FunctionWithOneParameterAndCanUseInExpression = 5,
            FunctionWithManyParameters = 6,
            FunctionWithManyParametersAndCanUseInExpression = 7,
            OpenBracket = 8,
            CloseBracket = 9,
            Operation = 10,
            Inversion = 11,
            EqualSymbol = 12,
            SemicolumnSymbol = 13; // точка с запятой
    }

    static class Syntax
    {
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
            return (S == FunctionNames[0]  ||
                    S == FunctionNames[1]  ||
                    S == FunctionNames[2]  ||
                    S == FunctionNames[3]  ||
                    S == FunctionNames[5]  ||
                    S == FunctionNames[6]  ||
                    S == FunctionNames[7]  ||
                    S == FunctionNames[8]  ||
                    S == FunctionNames[9]  ||
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
            return (S == FunctionNames[4]  ||
                    S == FunctionNames[10] ||
                    S == FunctionNames[11] ||
                    S == FunctionNames[16] ||
                    S == FunctionNames[17] ||
                    S == FunctionNames[19]) ? true : false;
        }

        public static bool IsFunctionWithManyParametersAndCanUseInExpression(string S)
        {
            return (S == FunctionNames[4]  ||
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
