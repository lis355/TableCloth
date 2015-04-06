using System.Collections.Generic;

namespace BinaryCalc
{
    /// <summary>
    /// Класс формулы хранит в себе стековое представление формулы, 
    /// количество и идентификаторы содержащихся в ней переменных
    /// </summary>
    public class Formula
    {
        public struct Command { byte Operation, Index; }

        /// <summary>
        /// Стек команд
        /// </summary>
        public List<Command> Commands;

        /// <summary>
        /// Список имен переменных
        /// </summary>
        public List<string> VariableName;

        /// <summary>
        /// Стек для рассчета формулы
        /// </summary>
        Stack<bool> CalcStack;

        /// <summary>
        /// Дерево формулы
        /// </summary>
        public Vertex Tree;

        /// <summary>
        /// Количество переменных в формуле
        /// </summary>
        public int VariableCount
        {
            get { return VariableName.Count; }
        }

        public Formula()
        {
            Commands = new List<Command>(30);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
        }

        public Formula(ref Formula F)
        {
            Commands = new List<Command>(30);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
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
                if (Commands[i] == CommandsCode.PushFalse) CalcStack.Push(false);
                else if (Commands[i] == CommandsCode.PushTrue) CalcStack.Push(true);
                else if (Commands[i] == CommandsCode.PushVariable) CalcStack.Push(Values[Commands[i + 1]]);
                else if (Commands[i] == CommandsCode.OperationNot)
                {
                    t1 = !CalcStack.Pop();
                    CalcStack.Push(t1);
                }
                else
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (Commands[i] == CommandsCode.OperationAnd) CalcStack.Push(t1 && t2);
                    else if (Commands[i] == CommandsCode.OperationOr) CalcStack.Push(t1 || t2);
                    else if (Commands[i] == CommandsCode.OperationXor) CalcStack.Push(t1 ^ t2);
                    else if (Commands[i] == CommandsCode.OperationSheffer) CalcStack.Push(!(t1 && t2));
                    else if (Commands[i] == CommandsCode.OperationEquivalence) CalcStack.Push(!(t1 ^ t2));
                    else if (Commands[i] == CommandsCode.OperationImplication) CalcStack.Push(t1 || !t2);
                }
            }
            return CalcStack.Pop();
        }

        #region Конвертирование представления формулы в строку
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
                else if (Commands[i] == CommandsCode.OperationEquivalence)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority >= Syntax.GetPriority('~')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority >= Syntax.GetPriority('~')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "~" + t1.Data; t1.Priority = Syntax.GetPriority('~');
                    CalcStack.Push(t1);
                }
            }
            return CalcStack.Pop().Data;
        }
        #endregion

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
    }
}
