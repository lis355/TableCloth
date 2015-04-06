using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BinaryCalc.Functions;
using System.Windows;

namespace BinaryCalc
{
    class CalcResult
    {
        public bool Success;
        public int ErrorCode, ErrorPos;
        public FormulaTree Tree;
        public List<string> Output;
        public TimeSpan TimeCalc;

        public CalcResult()
        {
            Success = false;
            ErrorCode = ErrorPos = 0;
            Tree = new FormulaTree();
            Output = new List<string>(1);
        }

        public void SetError(int n, int pos)
        {
            Success = false;
            ErrorCode = n;
            ErrorPos = pos;
        }
    }

    static class CalcCondition
    {
        public const int
            Begin = 0,
            ReadVariable = 1,
            ReadInversion = 2,
            ReadOperation = 3,
            ReadOpenBracket = 4,
            ReadCloseBracket = 5, 
            // ReadSemicolon == Begin
            ReadEqual = 6,
            ReadConstant = 7,
            ReadFunctionWithOneParameter = 8,
            ReadFunctionWithOneParameterAndCanUseInExpression = 9,
            ReadFunctionWithManyParameters = 10,
            ReadFunctionWithManyParametersAndCanUseInExpression = 11;
    }

    /// <summary>
    /// Основной класс для рассчета строки
    /// </summary>
    static class Calc
    {
        public static CalcResult CalcExpression(string S, ref Variables Var)
        {
            DateTime TimeCalcStart = DateTime.Now;// время рассчетов
            CalcResult Result = new CalcResult();// результат
            Stack<Vertex> St = new Stack<Vertex>(30);// стек вершин
            Vertex t1, t2;
            string Identificator = "";
            int CurrentPosition = 0, R = Parse.GetNextToken(ref S, ref CurrentPosition, ref Identificator);
            int Condition = CalcCondition.Begin; // состояние КА 
            bool EqualInThisExpression = false;// если true то текущее выражение в виде <переменная> = <выражение>
            int BracketCounter = 0;// счетчик вложенности скобок
            Formula Ans = new Formula();// временная формула для рассчетов в каждом выражении
            Parse.DeleteSpace(ref S);//удаляем пробелы, табуляции и комментарии

            while (R != SyntaxResult.EndOfLine)
            {
                #region AnotherSymbol
                if (R == SyntaxResult.AnotherSymbol)
                {
                    Result.SetError(1,CurrentPosition);
                    return Result;
                }
                #endregion
                #region Variable
                else if (R == SyntaxResult.Variable)
                {
                    if (Condition == CalcCondition.Begin ||
                        Condition == CalcCondition.ReadOpenBracket ||
                        Condition == CalcCondition.ReadEqual ||
                        Condition == CalcCondition.ReadOperation)
                    {
                        // Проверим идентификатор на формулу из списка переменных
                        // сначала проверим, не является ли обнаруженный идентификатор
                        // глобальной переменной
                        if (Var.IsExist(Identificator))
                        {
                            // нужно вставить формулу из переменных
                            St.Push(Var[Identificator].Tree);
                            // тут нужно ченить придумать так как потом может идти = а я уже перед ним всю формулу вставил
                            // ... хотя я вставил только одну вершину, мб просто хранить идентификатор предыдущих переменных а если 
                            // встретили = то удалять из стека?
                        }
                        else
                        {
                            // Добавляем переменную в список временной формулы
                            Ans.AddVariableName(Identificator);
                            St.Push(new Vertex(null, null, null, 0, false, Identificator, Ans.GetIndexOfVar(Identificator)));
                        }
                        Condition = CalcCondition.ReadVariable;
                    }
                    else
                    {
                        Result.SetError(2, CurrentPosition);
                        return Result;
                    }
                }
                #endregion
                #region Constant
                else if (R == SyntaxResult.Constant)
                {

                }
                #endregion
                #region FunctionWithOneParameter
                else if (R == SyntaxResult.FunctionWithOneParameter)
                {

                }
                #endregion
                #region FunctionWithOneParameterAndCanUseInExpression
                else if (R == SyntaxResult.FunctionWithOneParameterAndCanUseInExpression)
                {

                }
                #endregion
                #region FunctionWithManyParameters
                else if (R == SyntaxResult.FunctionWithManyParameters)
                {

                }
                #endregion
                #region FunctionWithManyParametersAndCanUseInExpression
                else if (R == SyntaxResult.FunctionWithManyParametersAndCanUseInExpression)
                {

                }
                #endregion
                #region OpenBracket
                else if (R == SyntaxResult.OpenBracket)
                {
                    if (Condition == CalcCondition.Begin ||
                        Condition == CalcCondition.ReadOpenBracket ||
                        Condition == CalcCondition.ReadEqual ||
                        Condition == CalcCondition.ReadFunctionWithOneParameter ||
                        Condition == CalcCondition.ReadFunctionWithOneParameterAndCanUseInExpression ||
                        Condition == CalcCondition.ReadFunctionWithManyParameters ||
                        Condition == CalcCondition.ReadFunctionWithManyParametersAndCanUseInExpression ||                        
                        Condition == CalcCondition.ReadOperation)
                    {
                        BracketCounter++;
                        Condition = CalcCondition.ReadOpenBracket;
                    }
                    else
                    {
                        Result.SetError(7, CurrentPosition);
                        return Result;
                    }
                }
                #endregion
                #region CloseBracket
                else if (R == SyntaxResult.CloseBracket)
                {
                    if (Condition == CalcCondition.ReadVariable ||
                        Condition == CalcCondition.ReadCloseBracket ||
                        Condition == CalcCondition.ReadConstant)
                    {
                        BracketCounter--;
                        if (BracketCounter < 0)
                        {
                            Result.SetError(6, CurrentPosition);
                            return Result;
                        }
                        Condition = CalcCondition.ReadCloseBracket;
                    }
                    else
                    {
                        Result.SetError(8, CurrentPosition);
                        return Result;
                    }
                }
                #endregion
                #region Operation
                else if (R == SyntaxResult.Operation)
                {
                    if (Condition == CalcCondition.ReadVariable ||
                        Condition == CalcCondition.ReadCloseBracket ||
                        Condition == CalcCondition.ReadConstant)
                    {
                        t1 = St.Pop();
                        t2 = St.Pop();

                        St.Push(new Vertex(t2, t1, null, Math.Max(t1.Level, t2.Level) + 1, true, "+", 0));

                        t1.Parent = t2.Parent = St.Peek();

                        Condition = CalcCondition.ReadOperation;
                    }
                    else
                    {
                        Result.SetError(8, CurrentPosition);
                        return Result;
                    }
                }
                #endregion
                #region Inversion
                else if (R == SyntaxResult.Inversion)
                {

                }
                #endregion
                #region EqualSymbol
                else if (R == SyntaxResult.EqualSymbol)
                {
                    if (Condition == CalcCondition.ReadVariable)
                    {
                        //work
                        // так как встретили оператор = то предыдущий идентификатор,
                        // опознанный как переменная мы вынимаем из стека и храним его имя, 
                        // чтобы потом присвоить ему функцию
                        // IdentificatorToEqual = 
                        EqualInThisExpression = true;
                    }
                    else
                    {
                        Result.SetError(3, CurrentPosition);
                        return Result;
                    }

                    Condition = CalcCondition.ReadEqual;
                }
                #endregion
                #region SemicolumnSymbol
                else if (R == SyntaxResult.SemicolumnSymbol)
                {
                    if (Condition == CalcCondition.Begin)
                    {
                        Result.SetError(4, CurrentPosition);
                        return Result;
                    }

                    if (Condition == CalcCondition.ReadCloseBracket ||
                        Condition == CalcCondition.ReadVariable)
                    {
                        //work
                        // Здесь нужно собрать выражение. Если было присвоение, то собрать по дереву формулу
                        // и присвоить переменной. Если была введена формула или присвоение, то напечатать ее 
                        // строковое представление.
                        
                        // Formula tmp = create formula from vertex (st.top)

                        if (EqualInThisExpression)
                        {
                            // IdentificatorToEqual = tmp
                            //Result.Output.Add(tmp.tostring)
                        }
                        else
                        {
                            // if здесь нужно напечатать результат
                        }
                    }
                    else
                    {
                        Result.SetError(5, CurrentPosition);
                        return Result;
                    }

                    Condition = CalcCondition.Begin;
                    Ans = new Formula();// обнуляем временную формулу
                }
                #endregion
                R = Parse.GetNextToken(ref S, ref CurrentPosition, ref Identificator);
            }
            #region old_code
            /*
            if (S.IndexOf('=') != -1)
            {
                #region <строка> ::= <переменная> = <выражение>
                string VariableName = S.Substring(0, S.IndexOf('=')),
                        NewS = S.Substring(S.IndexOf('=') + 1);
                if (Syntax.IsVariable(VariableName) == false)
                {
                    //error
                    OutString = "Неправильный синтаксис идентификатора";
                    return false;
                };
                if (NewS == "")
                {
                    //error
                    OutString = "Пустое выражение";
                    return false;
                };

                //теперь считаем строку после '=' и рассчитываем ее
                if (CalcExpression(NewS, ref OutString, ref V, ref Ans, true) == false) return false;

                //присваиваем переменной результат
                V.Set(VariableName, Ans);

                return true;
                #endregion
            }
            else
            {
                #region <строка> ::= <выражение>
                if (S.IndexOf(',') != -1)
                {
                    #region <выражение> ::= <функция(... , ... , ...)>

                    //сначала до '(' должно идти имя функции
                    if (S.IndexOf('(') == -1)
                    {
                        OutString = "Нет открывающей скобки у функции";
                        return false;
                    };

                    string MainFunctionName = S.Substring(0, S.IndexOf('('));
                    if (Syntax.IsFunction(MainFunctionName) == false)
                    {
                        OutString = "Неправильный синтаксис идентификатора функции";
                        return false;
                    };

                    if (Syntax.IsFuncWithManyParameters(MainFunctionName) == false)
                    {
                        OutString = "Данная функция не поддерживает больше одного аргумента";
                        return false;
                    };


                    // теперь идентификатор правильный, нужно получить массив аргументов

                    //сначала получим строку с аргементами через запятую, нужно удалить часть строки в начале
                    // скобку в конце строки

                    S = S.Substring(S.IndexOf('(') + 1);
                    if (S.LastIndexOf(')') == -1)
                    {
                        OutString = "Нет закрывающей скобки у функции";
                        return false;
                    }

                    S = S.Substring(0, S.LastIndexOf(')'));
                    char[] Params = { ',' };
                    string[] Arguments = S.Split(Params, StringSplitOptions.None);

                    foreach (string x in Arguments)
                    {
                        if (x == "")
                        {
                            OutString = "Параметр является пустой строкой";
                            return false;
                        }
                    }

                    #region 4 convert to basis
                    /*
                    if (MainFunctionName == Syntax.FunctionNames[4])
                    {
                        // всего 2 аргумента
                        if (Arguments.Length != 2)
                        {
                            OutString = "Данная функция поддерживает 2 аргумента";
                            return false;
                        };

                        // проверяем первый аргумент на корректный идентификатор
                        if (Syntax.IsVariable(Arguments[0]) == false)
                        {
                            OutString = "Неправильный синтаксис идентификатора";
                            return false;
                        };

                        // проверяем первый аргумент на существующий базис 
//                         if (Syntax.IsVariable(Arguments[0]) == false)
//                         {
//                             OutString = "Неправильный синтаксис идентификатора";
//                             return false;
//                         };

                        //второй аргумент - формула
                        Formula CalculatedArgument = new Formula();
                        if (CalcSimplyString(Arguments[1], ref OutString, ref CalculatedArgument, EquateToVar) == false) return false;
                    
                        // работаем
                        Ans = FConvert.ToBasis(Arguments[0], ref CalculatedArgument);
                        return true;
                    }
                    
                    #endregion
            
                    #region 16 calc on vector
                    if (MainFunctionName == Syntax.FunctionNames[16])
                    {
                        //первый аргумент - формула
                        Formula CalculatedArgument = new Formula();
                        if (CalcSimplyString(Arguments[0], ref OutString, ref CalculatedArgument, EquateToVar) == false) return false;

                        // остальные аргументы должны быть либо 0 либо 1 и их долэно быть столько, сколько переменных у формулы.
                        if ((Arguments.Length - 1) != CalculatedArgument.VariableCount)
                        {
                            OutString = "Длина вектора не соответствует количеству переменных в формуле";
                            return false;
                        };

                        bool[] b = new bool[CalculatedArgument.VariableCount];

                        for (int k = 1; k < Arguments.Length; k++)
                            if (!(Arguments[k] == "0" || Arguments[k] == "1"))
                            {
                                OutString = "Допустимы только константы 0 и 1";
                                return false;
                            }
                            else
                            {
                                b[k - 1] = (Arguments[k] == "0") ? false : true;
                            }

                        //присваиваем переменной результат
                        OutString = "{ " + ((CalculatedArgument.CaclOnBoolVector(b)) ? "1" : "0") + " }";
                        return true;
                    }
                    #endregion

                    #region 19 create from vector
                    if (MainFunctionName == Syntax.FunctionNames[19])
                    {
                        // проверяем первый аргумент на корректный идентификатор
                        if (Syntax.IsVariable(Arguments[0]) == false)
                        {
                            OutString = "Неправильный синтаксис идентификатора";
                            return false;
                        };

                        // остальные аргументы должны быть либо 0 либо 1 и их долэно быть 2^n.
                        if (Syntax.GetNumberOfVariableFromFunctionVector(Arguments.Length - 1) == -1)
                        {
                            OutString = "Длина вектора функции должна быть степень двойки";
                            return false;
                        };

                        bool[] b = new bool[Arguments.Length - 1];

                        for (int k = 1; k < Arguments.Length; k++)
                            if (!(Arguments[k] == "0" || Arguments[k] == "1"))
                            {
                                OutString = "Допустимы только константы 0 и 1";
                                return false;
                            }
                            else
                            {
                                b[k - 1] = (Arguments[k] == "0") ? false : true;
                            }

                        //присваиваем переменной результат
                        Ans = FCreate.CreateFromVector(b);
                        V.Set(Arguments[0], Ans); 
                        return true;
                    }
                    #endregion

                    //теперь нужно получить формулу в командном представлении для каждого аргумента
                    Formula[] CalculatedArguments = new Formula[Arguments.Length];
                    for (int i = 0; i < Arguments.Length; i++)
                    {
                        CalculatedArguments[i] = new Formula();
                        if (CalcSimplyString(Arguments[i], ref OutString, ref CalculatedArguments[i], EquateToVar) == false) return false;
                    }

                    #region 10 check full system
                    if (MainFunctionName == Syntax.FunctionNames[10])
                    {
                        OutString = FCheck.FullSystem(ref CalculatedArguments);
                    }
                    #endregion

                    #region 11 check basis
                    if (MainFunctionName == Syntax.FunctionNames[11])
                    {
                        OutString = FCheck.Basis(ref CalculatedArguments);
                    }
                    #endregion

                    return true;
                    #endregion
                }
                else
                {
                    #region <выражение> ::= <простое выражение[со скобками и функциями с 1 параметром]>
                    return CalcSimplyString(S, ref OutString, ref Ans, EquateToVar);
                    #endregion
                }
                #endregion
            }
            */
            #endregion
            Result.Success = true;
            Result.TimeCalc = DateTime.Now - TimeCalcStart;
            return Result;
        }

        /// <summary>
        /// Рассчет строки, состоящей из идентификаторов переменных, скобок и констант 
        /// Для корректности на вход подается пустая формула
        /// </summary>
        public static bool CalcSimplyString(string S, ref string OutString, ref Formula Ans, bool EquateToVar)
        {
            /*
            #region Переменные
            List<SpecialSymbol> Symbols = new List<SpecialSymbol>(); //список из символов для упрощенного анализа
            string Identificator;
            string SyntaxScript = "";
            bool MeetNot = false;//встретили операцию !
            #endregion

            #region Формирование строк для алгоритма

            // унарный оператор может применяться несколько раз
                S = S.Replace("!!", "");

            for (int i = 0; i < S.Length; i++)
            {
                #region Встретили унарную операцию !
                if (S[i] == '!')
                {
                    MeetNot = true;
                    S = S.Remove(i, 1);
                    //теперь нужно заного попасть на этот символ
                    i--;
                    continue;
                }
                #endregion

                #region Встретили букву a-z
                if (Char.IsLetter(S[i]))
                {
                    // Получить идентификатор
                    Identificator = Parse.GetIdentificator(S.Substring(i));

                    if (Syntax.IsVariable(Identificator) == false)
                    {
                        OutString = "Некорректный идентификатор";
                        return false;
                    };

                    #region Проверим идентификатор на функцию
                    if (Syntax.IsFunction(Identificator) == true)
                    {
                        bool SingleFunction = false;

                        // Если встретили функцию, то есть 4 вида функций, которые могут встретиться в
                        // простом выражении и которые возвращают формулу
                        if (Syntax.IsFuncCanUseInExpression(Identificator) == false)
                        {
                            // если это функция которая возвращает НЕ формулу, то ее идентификатор
                            // должен идти с начала строки. иначе - ошибка

                            // если эту функцию вызвали, а перед этим было присвоение переменной
                            // то синтаксис не верен
                            if (EquateToVar)
                            {
                                OutString = "Данная функция не может учавствовать в выражении";
                                return false;
                            }

                            // так как функция на первом месте то последнее вхождение это 0й символ
                            if (S.LastIndexOf(Identificator) == 0)
                            {
                                // ставим флаг обрабатываем функцию
                                SingleFunction = true;
                            }
                            else
                            {
                                OutString = "Данная функция не может учавствовать в выражении";
                                return false;
                            }
                        };

                        //локализуем функцию
                        int startf = i + Identificator.Length;
                        // сначала у функции должна быть '('
                        if (startf >= S.Length)
                        {
                            OutString = "Нет открывающей скобки у функции";
                            return false;
                        }

                        // получаем закрывающую скобку
                        int endf = Parse.GetNextBracket(S, startf);
                        if (endf == -1)
                        {
                            OutString = "Нет закрывающей скобки у функции";
                            return false;
                        }

                        if (endf != S.Length - 1)
                        {
                            OutString = "После закрывающей скобки не должно быть операторов";
                            return false;
                        }

                        if (startf == endf - 1)
                        {
                            OutString = "Аргумент у функции является пустой строкой";
                            return false;
                        }

                        // теперь все правильно, получим строку-аргумент
                        string InherentlyString = S.Substring(startf + 1, endf - startf - 1);
                        startf -= Identificator.Length;

                        //Рассчитываем функцию
                        Formula InherentlyFormula = new Formula();
                        if (CalcSimplyString(InherentlyString, ref OutString, ref InherentlyFormula, EquateToVar) == false) return false;
                        S = S.Remove(startf, endf - startf + 1);

                        //
                        // применяем функцию для полученного внутреннего представления аргумента
                        //

                        Formula T = new Formula();
                        if (Identificator == Syntax.FunctionNames[0]) T = FConvert.ToDual(ref InherentlyFormula);
                        else if (Identificator == Syntax.FunctionNames[1]) T = FConvert.ToSDNF(ref InherentlyFormula);
                        else if (Identificator == Syntax.FunctionNames[2]) T = FConvert.ToSCNF(ref InherentlyFormula);
                        else if (Identificator == Syntax.FunctionNames[3]) T = FConvert.ToJegalkinPoly(ref InherentlyFormula);
                        else if (Identificator == Syntax.FunctionNames[18]) T = FMinimize.MinimizeQuine(ref InherentlyFormula);

                        else if (SingleFunction == true)
                        {
                            if (Identificator == Syntax.FunctionNames[14]) { FGet.GetTruthTable(ref InherentlyFormula); OutString = "#"; }
                            else if (Identificator == Syntax.FunctionNames[15]) { FGet.GetScheme(ref InherentlyFormula); OutString = "#"; }
                            else if (Identificator == Syntax.FunctionNames[5]) OutString = FCheck.BelongTZero(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[6]) OutString = FCheck.BelongTOne(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[7]) OutString = FCheck.BelongS(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[8]) OutString = FCheck.BelongM(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[9]) OutString = FCheck.BelongL(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[12]) OutString = FGet.GetFictitiousVars(ref InherentlyFormula); 
                            else if (Identificator == Syntax.FunctionNames[13]) OutString = FGet.GetFormulaStringVector(ref InherentlyFormula);
                            else if (Identificator == Syntax.FunctionNames[10]) { Formula[] arg = { InherentlyFormula }; OutString = FCheck.FullSystem(ref arg); }
                            else if (Identificator == Syntax.FunctionNames[11]) { Formula[] arg = { InherentlyFormula }; OutString = FCheck.Basis(ref arg); } 
                            return true;
                        }

                        for (int k = 0; k < T.VariableCount; k++)
                            Ans.AddVariableName(T.VariableName[k]);

                        S = S.Insert(startf, "(" + T.ConvertToString() + ")");

                        //теперь нужно заного попасть на этот символ
                        i--;
                        continue;
                    }
                    #endregion

                    #region Проверим идентификатор на формулу из списка переменных
                    // сначала проверим, не является ли обнаруженный идентификатор
                    // глобальной переменной - формулой
                    if (V.IsExist(Identificator) == true)
                    {
                        S = S.Remove(i, Identificator.Length);
                        S = S.Insert(i, "(" + V[Identificator].ConvertToString() + ")");
                        //теперь нужно заного попасть на этот символ
                        i--;
                        continue;
                    }
                    #endregion

                    // Добавляем переменную в список
                    Ans.AddVariableName(Identificator);
                    Symbols.Add(new SpecialSymbol('$', Ans.GetIndexOfVar(Identificator), MeetNot));
                    MeetNot = false;
                    SyntaxScript += '$';

                    // Инкрементируем счетчик на длину идентификатора
                    i += Identificator.Length - 1;
                    MeetNot = false;
                    continue;
                }
                #endregion

                #region Встретили константу 0 или 1
                if (S[i] == '0' || S[i] == '1')
                {
                    // Добавляем константу в список
                    Symbols.Add(new SpecialSymbol(S[i], -1, MeetNot));
                    SyntaxScript += '$';
                    MeetNot = false;
                    continue;
                }
                #endregion

                #region Встретили скобку (
                if (S[i] == '(')
                {
                    // Добавляем скобку в список
                    Symbols.Add(new SpecialSymbol(S[i], -1, MeetNot));
                    SyntaxScript += S[i];
                    MeetNot = false;
                    continue;
                }
                #endregion

                #region Проверка на отсутствие !
                // теперь если установлен флаг ! то ошибка, т.к. перменные, константы и скобки 
                // уже прошли и перед закрывающей скобкой или операцией ! не может идти
                if (MeetNot == true)
                {
                    OutString = "Нет операнда у оператора !";
                    return false;
                }
                #endregion

                #region Встретили скобку )
                if (S[i] == ')')
                {
                    // Добавляем скобку в список
                    Symbols.Add(new SpecialSymbol(S[i], -1, MeetNot));
                    SyntaxScript += S[i];
                    continue;
                }
                #endregion

                #region Встретили операцию
                if (Syntax.IsOperation(S.Substring(i, 1)))
                {
                    // Добавляем операцию в список
                    Symbols.Add(new SpecialSymbol(S[i], -1, MeetNot));
                    SyntaxScript += "?";
                    continue;
                }
                #endregion

                #region Посторонний символ - ошибка
                // если дошли до сюда, значит в строке есть некорректные символы
                OutString = "Некорректный символ в строке";
                return false;
                #endregion
            }
            #endregion

            #region Проверяем на стандартный базис
            List<int> flv = new List<int>(Ans.VariableCount);
            bool fj = true;// флаг того что  переменые не представляют стандартный базис
                           // в ПРАВИЛЬНОМ порядке
            for (int i = 0; i < Ans.VariableCount; i++)
                if (Syntax.IsStandartVariable(Ans.VariableName[i]))
                {
                    flv.Add(Convert.ToInt16(Ans.VariableName[i].Substring(1)));
                    if (flv[i] != i + 1) fj = false;
                }
            if (!fj)
            {
                flv.Sort(delegate(int a,int b){return a.CompareTo(b);});
                if (flv[Ans.VariableCount-1]==Ans.VariableCount)//стандартный набор
                {
                    Ans.VariableName.Clear();
                    for (int i = 0; i < flv.Count; i++)
                        Ans.AddVariableName("x" + Convert.ToString(i + 1));
                    return CalcSimplyString(S, ref OutString, ref Ans, false);
                }
            }
            #endregion

            #region Синтаксическая проверка скриптовой строки
            //после составления скриптовой строки нужно проверить ее на корректность,
            //так как например ** могло быть и не отслежено
            if (MeetNot == true)
            {
                OutString = "Нет операнда у операции !";
                return false;
            };
            if (Syntax.BracketsCheck(SyntaxScript) == false)
            {
                OutString = "Неправильная расстановка скобок";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\?\?"))
            {
                OutString = "Две подряд ищущих операции";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\$\$"))
            {
                OutString = "Два подряд ищущих операнда";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\(\)"))
            {
                OutString = "В группе скобок пустое выражение";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\)\("))
            {
                OutString = "Между закрывающейся и открывающейся скобками нет операции";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\?\)"))
            {
                OutString = "Закрывающеяся скобка сразу после операции";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\(\?"))
            {
                OutString = "Операция не может следовать за открывающей скобкой";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\$\("))
            {
                OutString = "Скобка не может следовать сразу за операндом";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\)\$"))
            {
                OutString = "Операнд не может следовать сразу за скобкой";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"\?$"))
            {
                OutString = "Операция не может быть в конеце строки";
                return false;
            };
            if (Regex.IsMatch(SyntaxScript, @"^\?"))
            {
                OutString = "Операция не может быть в начале строки";
                return false;
            };
            #endregion

            #region Обработка случаев если строка пустая либо содержит один операнд
            //если в строке только один операнд то нужно добавить его в стек
            if (Symbols.Count == 1)
            {
                AddCommands(Symbols[0], ref Ans);
                return true;
            }

            // если строка пустая, функция тождественна 0
            else if (Symbols.Count == 0)
            {
                AddCommands(new SpecialSymbol('0', 0, false), ref Ans);
                return true;
            }
            #endregion
            
            #region Упрощения выражения основанные на аксиомах
            
            #region  Аксиомы
//              * x * x = x
//              * x * 1 = x
//              * x * 0 = 0
//              * 
//              * x + x = x
//              * x + 1 = 1
//              * x + 0 = x
//              * 
//              * x ^ x = 0
//              * x ^ 0 = x
//              * 
//              * 0 | x = 1
//              * 
//              * 1 ~ x = 0
//              * 
//              * 0 > x = 1
//              * 1 > x = x
//              * x > 1 = 1
//              * x > x = 1
            #endregion
            for (int i = 0; i < SyntaxScript.Length; i++)
            {
                // встретили операцию
                if (SyntaxScript[i] == '?')
                {
                    // получаем левый и правый аргумент соответственно
                    SpecialSymbol L = Symbols[i - 1];
                    SpecialSymbol R = Symbols[i + 1];
                    #region Один аргумент переменная, другой - константа
                    /*
                    if ((L.Symbol == '$' && R.Symbol == '0') ||
                        (L.Symbol == '$' && R.Symbol == '1') ||
                        (L.Symbol == '0' && R.Symbol == '$') ||
                        (L.Symbol == '1' && R.Symbol == '$'))
                    {
//                          * x * 1 = x
//                          * x * 0 = 0
//                          * 
//                          * x + 1 = 1
//                          * x + 0 = x
//                          * 
//                          * x ^ 0 = x
//                          * 
//                          * 0 | x = 1
//                          * 
//                          * 1 ~ x = 0
//                          * 
//                          * 0 > x = 1
//                          * 1 > x = x
//                          * x > 1 = 1
//                          * x > 0 = 1
                         

                        SpecialSymbol Const = (L.Symbol != '$') ? L : R;

                        Symbols[i - 1] = (L.Symbol == '$') ? L : R; // назначили переменной, но впоследствии может затереться константой

                        if (Symbols[i].Symbol == '*' && Const.Symbol == '0') Symbols[i - 1] = Const;
                        else if (Symbols[i].Symbol == '+' && Const.Symbol == '1') Symbols[i - 1] = Const;
                        else if (Symbols[i].Symbol == '|' && Const.Symbol == '0')
                        {
                            Symbols[i - 1] = Const; 
                            Symbols[i - 1].Symbol = '1'; 
                        }
                        else if (Symbols[i].Symbol == '~' && Const.Symbol == '1')
                        {
                            Symbols[i - 1] = Const;
                            Symbols[i - 1].Symbol = '0';
                        }
                        else if (Symbols[i].Symbol == '>')
                        {
                             if (L.Symbol == '0' && R.Number != -1 ||// 0 > x
                                 R.Number != -1)// x > 0/1
                             {
                                 Symbols[i - 1] = Const;
                                 Symbols[i - 1].Symbol = '1'; 
                             }
                        }

                        Symbols.RemoveRange(i, 2);
                        SyntaxScript = SyntaxScript.Remove(i, 2);
                        i--;
                    }
                    
                    #endregion

                    #region оба аргумента - константы
                    //else 
            if (L.Symbol != '$' && R.Symbol != '$')
                    {
                        #region 
//                         * 0 * any = 0
//                          * 1 * 1 = 1
//                          * 
//                          * 1 + any = 1
//                          * 0 + 0 = 0
//                          * 
//                          * 1 ^ 1 = 0
//                          * 0 ^ 0 = 0
//                          * 1 ^ 0 = 1
//                          * 
//                          * 0 | any = 1
//                          * 1 | 1 = 0
//                          * 
//                          * 1 ~ any = 0
//                          * 0 ~ 0 = 1
//                          * 
//                          * 0 > any = 1
//                          * 1 > 1 = 1
//                          * 1 > 0 = 0

                        #endregion
                        bool t1 = (L.Symbol == '0') ? false : true; if (L.IsInverse) t1 = !t1;
                        bool t2 = (R.Symbol == '0') ? false : true; if (R.IsInverse) t2 = !t2;
                        if (Symbols[i].Symbol == '*') t1 = t1 && t2;
                        else if (Symbols[i].Symbol == '+') t1 = t1 || t2;
                        else if (Symbols[i].Symbol == '^') t1 = !t1 && t2 || t1 && !t2;
                        else if (Symbols[i].Symbol == '|') t1 = !(t1 && t2);
                        else if (Symbols[i].Symbol == '~') t1 = !(t1 || t2);
                        else if (Symbols[i].Symbol == '>') t1 = t1 || !t2;

                        Symbols[i - 1] = new SpecialSymbol((t1) ? '1' : '0', -1, false);
                        Symbols.RemoveRange(i, 2);
                        SyntaxScript = SyntaxScript.Remove(i, 2);
                        i--;
                    }
                    #endregion

                    #region оба аргумента - ОДИНАКОВЫЕ переменные
                    else if (L.Symbol == '$' && R.Symbol == '$' && L.Number == R.Number)
                    {
                        #region
//                         * x * x = x
//                          * 
//                          * x + x = x
//                          * 
//                          * x ^ x = 0
//                          * 
//                          * x > x = 1
                         
                        #endregion
                        if (Symbols[i].Symbol == '^') Symbols[i - 1] = new SpecialSymbol('0', -1, false);
                        else if (Symbols[i].Symbol == '>') Symbols[i - 1] = new SpecialSymbol('1', -1, false);

                        Symbols.RemoveRange(i, 2);
                        SyntaxScript = SyntaxScript.Remove(i, 2);
                        i--;
                    }
                    #endregion
                }
            }
            
            #endregion

            #region Алгоритм перевода в постфиксную запись

            Stack<SpecialSymbol> PStack = new Stack<SpecialSymbol>();

            for (int i = 0; i < Symbols.Count; i++)
            {
                if (Symbols[i].Symbol == '$' || Symbols[i].Symbol == '0' || Symbols[i].Symbol == '1')
                {
                    AddCommands(Symbols[i], ref Ans);
                }
                else
                {
                    // а) если стек пуст, то опеpация из входной стpоки пеpеписывается в стек
                    // в) если очеpедной символ из исходной стpоки есть откpывающая скобка, 
                    //    то он пpоталкивается в стек
                    if (PStack.Count == 0 ||
                        Syntax.GetPriority(Symbols[i].Symbol) == 0) { PStack.Push(Symbols[i]); continue; }

                    // г) закpывающая кpуглая скобка выталкивает все опеpации из стека 
                    //    до ближайшей откpывающей скобки, сами скобки в выходную стpоку не пеpеписываются,
                    //    а уничтожают дpуг дpуга. 
                    if (Symbols[i].Symbol == ')')
                    {
                        // выталкиваем операции ДО скобки
                        while (PStack.Peek().Symbol != '(') AddCommands(PStack.Pop(), ref Ans);
                        // выталкиваем саму скобку
                        if (PStack.Peek().Symbol == '(') AddCommands(PStack.Pop(), ref Ans);
                        continue;
                    }

                    // б) опеpация выталкивает из стека все опеpации с большим или pавным пpиоpитетом 
                    //    в выходную стpоку
                    while (PStack.Count > 0)
                    {
                        if (Syntax.GetPriority(PStack.Peek().Symbol) != 0
                            && Syntax.GetPriority(PStack.Peek().Symbol) <= Syntax.GetPriority(Symbols[i].Symbol))
                        {
                            AddCommands(PStack.Pop(), ref Ans);
                        }
                        else break;
                    }
                    PStack.Push(Symbols[i]);
                }
            }
            // если в стеке чтото осталось
            while (PStack.Count != 0)
            {
                AddCommands(PStack.Pop(), ref Ans);
            }
            #endregion
            */
            return true;
        }

        /// <summary>
        /// Добавляет команды в формулу, по данному ключу
        /// </summary>
        /*
        public static void AddCommands(SpecialSymbol S, ref Formula Ans)
        {
            if (S.Symbol == '0')
            {
                if (S.IsInverse) Ans.Commands.Add(CommandsCode.PushTrue);
                else Ans.Commands.Add(CommandsCode.PushFalse);
            }
            else if (S.Symbol == '1')
            {
                if (S.IsInverse) Ans.Commands.Add(CommandsCode.PushFalse);
                else Ans.Commands.Add(CommandsCode.PushTrue);
            }
            else if (S.Symbol == '$')
            {
                Ans.Commands.Add(CommandsCode.PushVariable);
                Ans.Commands.Add(S.Number);
                if (S.IsInverse == true)
                {
                    Ans.Commands.Add(CommandsCode.OperationNot);
                }
            }
            else if (S.Symbol == '*')
            {
                Ans.Commands.Add(CommandsCode.OperationAnd);
            }
            else if (S.Symbol == '+')
            {
                Ans.Commands.Add(CommandsCode.OperationOr);
            }
            else if (S.Symbol == '^')
            {
                Ans.Commands.Add(CommandsCode.OperationXor);
            }
            else if (S.Symbol == '|')
            {
                Ans.Commands.Add(CommandsCode.OperationSheffer);
            }
            else if (S.Symbol == '~')
            {
                Ans.Commands.Add(CommandsCode.OperationPirse);
            }
            else if (S.Symbol == '>')
            {
                Ans.Commands.Add(CommandsCode.OperationImplication);
            }
            else if (S.Symbol == '(' && S.IsInverse)
            {
                Ans.Commands.Add(CommandsCode.OperationNot);
            }
        }
        */
    }
}
