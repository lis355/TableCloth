using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinaryCalc
{
    public class CommandsCode
    {
        public const int
        PushFalse = 0,
        PushTrue = 1,
        PushVariable = 2, // После данной команды идет порядковый номер переменной
        OperationNot = 3,
        OperationAnd = 4,
        OperationOr = 5,
        OperationXor = 6,
        OperationImplication = 7,
        OperationSheffer = 8,
        OperationPirse = 9;
    }

    /// <summary>
    /// Класс формулы хранит в себе стековое представление формулы, 
    /// количество и идентификаторы содержащихся в ней переменных
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Формула в текстовой записи, обновляется при вызове функции СonvertToString
        /// </summary>
        string FormulaString;

        /// <summary>
        /// Стек команд
        /// </summary>
        public List<int> Commands;

        /// <summary>
        /// Количество переменных в формуле
        /// </summary>
        public int VariableCount
        {
            get
            {
                return VariableName.Count;
            }
        }

        /// <summary>
        /// Список имен переменных
        /// </summary>
        public List<string> VariableName;

        /// <summary>
        /// Стек для рассчета формулы
        /// </summary>
        Stack<bool> CalcStack;

        /// <summary>
        /// Конструкторы
        /// </summary>
        public Formula(string S = "")
        {
            Commands = new List<int>(30);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
            FormulaString = S;
        }

        public Formula(ref Formula F)
        {
            Commands = new List<int>(30);
            for (int i = 0; i < F.Commands.Count; i++) Commands.Add(F.Commands[i]);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
            FormulaString = F.FormulaString;
            for (int i = 0; i < F.VariableCount; i++) VariableName.Add(F.VariableName[i]);
        }

        /// <summary>
        /// Рассчет функции на заданном наборе значений переменных
        /// </summary>
        public bool CaclOnBoolVector(bool[] Values)
        {
            if (Commands.Count == 0) CalcStack.Push(false);
            else CalcStack.Clear();

            bool t1, t2;
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i] == CommandsCode.PushFalse)
                {
                    CalcStack.Push(false);
                }
                else if (Commands[i] == CommandsCode.PushTrue)
                {
                    CalcStack.Push(true);
                }
                else if (Commands[i] == CommandsCode.PushVariable)
                {
                    // После данной команды идет порядковый номер переменной
                    CalcStack.Push(Values[Commands[i + 1]]);
                    i++;
                }
                else if (Commands[i] == CommandsCode.OperationNot)
                {
                    t1 = !CalcStack.Pop();
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationAnd)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 && t2);
                }
                else if (Commands[i] == CommandsCode.OperationOr)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 || t2);
                }
                else if (Commands[i] == CommandsCode.OperationXor)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!t1 && t2 || t1 && !t2);
                }
                else if (Commands[i] == CommandsCode.OperationSheffer)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!(t1 && t2));
                }
                else if (Commands[i] == CommandsCode.OperationPirse)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!(t1 || t2));
                }
                else if (Commands[i] == CommandsCode.OperationImplication)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 || !t2);
                }
            }
            return CalcStack.Pop();
        }

        /// <summary>
        /// Парсинг строки в формулу
        /// </summary>
        public bool CreateFromString(string S)
        {
            string O = "";
            Formula F = this;
            return Calc.CalcExpression(S, ref O, ref F);
        }

        /// <summary>
        /// Стек для интерпретерования формулы в строковую запись
        /// </summary>
        class StackForToStringConvert
        {
            public string Data;
            public int Priority;
            public StackForToStringConvert(string D, int P)
            {
                Data = D;
                Priority = P;
            }
        }

        /// <summary>
        /// Конвертирование представления формулы в строку
        /// </summary>
        public string ConvertToString()
        {
            Stack<StackForToStringConvert> CalcStack = new Stack<StackForToStringConvert>();
            if (Commands.Count == 0) return "0";

            StackForToStringConvert t1, t2;

            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i] == CommandsCode.PushFalse)
                {
                    CalcStack.Push(new StackForToStringConvert("0", -1));
                }
                else if (Commands[i] == CommandsCode.PushTrue)
                {
                    CalcStack.Push(new StackForToStringConvert("1", -1));
                }
                else if (Commands[i] == CommandsCode.PushVariable)
                {
                    // После данной команды идет порядковый номер переменной
                    CalcStack.Push(new StackForToStringConvert(VariableName[Commands[i + 1]], -1));
                    i++;
                }
                else if (Commands[i] == CommandsCode.OperationNot)
                {
                    t1 = CalcStack.Pop();
                    if (t1.Priority > 0) t1.Data = "!(" + t1.Data + ")";
                    else t1.Data = "!" + t1.Data + "";
                    t1.Priority = 0;
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationAnd)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority > Syntax.GetPriority('*')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority > Syntax.GetPriority('*')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "*" + t1.Data; t1.Priority = Syntax.GetPriority('*');
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationOr)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority > Syntax.GetPriority('+')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority > Syntax.GetPriority('+')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "+" + t1.Data; t1.Priority = Syntax.GetPriority('+');
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationXor)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority > Syntax.GetPriority('^')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority > Syntax.GetPriority('^')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "^" + t1.Data; t1.Priority = Syntax.GetPriority('^');
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationImplication)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority >= Syntax.GetPriority('>')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority >= Syntax.GetPriority('>')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + ">" + t1.Data; t1.Priority = Syntax.GetPriority('>');
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationSheffer)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority >= Syntax.GetPriority('|')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority >= Syntax.GetPriority('|')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "|" + t1.Data; t1.Priority = Syntax.GetPriority('|');
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationPirse)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority >= Syntax.GetPriority('~')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority >= Syntax.GetPriority('~')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "~" + t1.Data; t1.Priority = Syntax.GetPriority('~');
                    CalcStack.Push(t1);
                }
            }
            return FormulaString = CalcStack.Pop().Data;
        }

        #region Функции списка переменных
        /// <summary>
        /// Добавляет в список переменных новую переменную,
        /// даже если такая уже есть. 
        /// </summary>
        public void AddVariableName(string S)
        {
            if (VariableName.IndexOf(S) == -1)
            {
                VariableName.Add(S);
            }
        }

        /// <summary>
        /// Возвращает индекс имени переменной в списке
        /// Если не найдено, то -1
        /// </summary>
        public int GetIndexOfVar(string S)
        {
            return VariableName.IndexOf(S);
        }
        #endregion

        public string GetString
        {
            get
            {
                return FormulaString;
            }
        }
    }
}
