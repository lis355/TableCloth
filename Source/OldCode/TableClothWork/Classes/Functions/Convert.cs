using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinaryCalc.Functions
{/*
    static public class FConvert
    {
        /// <summary>
        /// Заменяет формулу на двойственную
        /// </summary>
        static public Formula ToDual(ref Formula F)
        {
            Formula G = new Formula(ref F);

            //смоотрим было ли отрицание от всей формулы
            bool fnot = false;
            if (G.Commands[G.Commands.Count - 1] == CommandsCode.OperationNot) fnot = true;

            for (int i = 0; i < G.Commands.Count; i++)
            {
                if (G.Commands[i] == CommandsCode.PushVariable)
                {
                    i++;
                    if (i >= G.Commands.Count - 1) break;
                    else
                    {
                        if (G.Commands[i + 1] == CommandsCode.OperationNot) G.Commands.RemoveAt(i + 1);
                        else G.Commands.Insert(i + 1, CommandsCode.OperationNot);
                    }
                }
            }
            if (fnot == true) G.Commands.RemoveAt(F.Commands.Count - 1);
            else G.Commands.Add(CommandsCode.OperationNot);

            return G;
        }

        /// <summary>
        /// Составляет нормальную полную дизъюнктивную форму для формулы
        /// </summary>
        static public Formula ToSDNF(ref Formula F)
        {
            Formula G = new Formula(ref F);

            if (F.VariableCount == 0) { G.Commands.Add(F.Commands[0]); return G; }

            // Отображает текущий набор значений переменных
            bool[] b = new bool[F.VariableCount];
            int k = 0;
            // Цикл по всем значениям функции
            for (int i = 0; i < (1 << F.VariableCount); i++)
            {
                // Если значение функции на наборе истино, то добавляем конъюнкцию
                if (F.CaclOnBoolVector(b) == true)
                {
                    for (int j = 0; j < F.VariableCount; j++)
                    {
                        // Добавляем команду "Положить переменную в стек"
                        G.Commands.Add(CommandsCode.PushVariable);
                        G.Commands.Add(j);
                        // В зависимости от значения переменной в наборе ставим или
                        // не ставим отрицание
                        if (b[j] == false) G.Commands.Add(CommandsCode.OperationNot);
                        if (j != 0) G.Commands.Add(CommandsCode.OperationAnd);
                    }
                    // k - количество коньюнкций
                    k++;
                    if (k > 1) G.Commands.Add(CommandsCode.OperationOr);
                }
                // Переходим к следующему набору
                FGet.GetNextVector(b);
            }
            return G; 
        }

        /// <summary>
        /// Составляет нормальную полную конъюнктивную форму для формулы
        /// </summary>
        static public Formula ToSCNF(ref Formula F)
        {
            Formula G = new Formula(ref F);

            if (F.VariableCount == 0) { G.Commands.Add(F.Commands[0]); return G; }

            bool[] b = new bool[F.VariableCount];
            int k = 0;
            for (int i = 0; i < (1 << F.VariableCount); i++)
            {
                // если значение функции на наборе 0 то добавляем коньюнкцию
                if (F.CaclOnBoolVector(b) == false)
                {
                    for (int j = 0; j < F.VariableCount; j++)
                    {
                        G.Commands.Add(CommandsCode.PushVariable);
                        G.Commands.Add(j);
                        if (b[j] == true) G.Commands.Add(CommandsCode.OperationNot);
                        if (j != 0) G.Commands.Add(CommandsCode.OperationOr);
                    }
                    // k - количество коньюнкций
                    k++;
                    if (k > 1) G.Commands.Add(CommandsCode.OperationAnd);
                }
                FGet.GetNextVector(b);
            }
            return G;
        }

        /// <summary>
        /// Составляет полином Жегалкина
        /// </summary>
        static public Formula ToJegalkinPoly(ref Formula F)
        {
            Formula G = new Formula(ref F);

            int VectorCount = 1 << F.VariableCount;

            // Текущий набор 
            bool[] b = new bool[F.VariableCount];

            // Делаем треугольную матрицу
            bool[][] c = new bool[VectorCount][];
            for (int i = 0; i < VectorCount; i++) c[i] = new bool[VectorCount - i];

            // Заполняем нижнюю строку вектором значений функции
            for (int i = 0; i < VectorCount; i++)
            {
                c[0][i] = F.CaclOnBoolVector(b);
                FGet.GetNextVector(b);
            }

            // Определяем коэффициенты
            for (int i = 1; i < VectorCount; i++)
                for (int j = 0; j < VectorCount - i; j++)
                    c[i][j] = c[i - 1][j] ^ c[i - 1][j + 1];

            // Собираем команды полинома
            int k = 0; // k - количество сложений по модулю 2
            int t = 0; // t число переменных в мономе чтобы добавить
            FGet.GetNextVector(b);
            if (c[0][0] == true) { G.Commands.Add(CommandsCode.PushTrue); k++; }
            for (int i = 1; i < VectorCount; i++)
            {
                // Коэффициент равер единице - включаем моном
                if (c[i][0] != false)
                {
                    t = 0;
                    for (int h = 0; h < F.VariableCount; h++)
                    {
                        if (b[h] == true)
                        {
                            G.Commands.Add(CommandsCode.PushVariable);
                            G.Commands.Add(h);
                            t++;
                            if (t > 1) G.Commands.Add(CommandsCode.OperationAnd);
                        }
                    }
                    k++;
                    if (k > 1) G.Commands.Add(CommandsCode.OperationXor);
                }
                FGet.GetNextVector(b);
            }
            return G;
        }
    }*/
}
