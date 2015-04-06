using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryCalc.Functions
{
    static class FGet
    {
        /// <summary>
        /// Указатель на главную форму, нужен чтобы показывать таблицы и схемы
        /// </summary>
        static MainForm MForm;

        /// <summary>
        /// Инициализация класса для отображения форм схем и таблиц истинности
        /// </summary>
        static public void Initialize(MainForm ApplicationForm)
        {
            MForm = ApplicationForm;
        }

        /// <summary>
        /// Получает строку фиктивных переменных
        /// </summary>
        static public string GetFictitiousVars(ref Formula F)
        {
            string Out = "{";

            Formula G = new Formula(ref F);

            int VectorCount = 1 << F.VariableCount;

            bool[] b = new bool[F.VariableCount];

            bool[][] c = new bool[VectorCount][];// треугольник

            for (int i = 0; i < VectorCount; i++) c[i] = new bool[VectorCount - i];

            // заполняем нижнюю строку вектором значений функции
            for (int i = 0; i < VectorCount; i++)
            {
                c[0][i] = F.CaclOnBoolVector(b);
                FGet.GetNextVector(b);
            }

            // определяем коэффициенты
            for (int i = 1; i < VectorCount; i++)
                for (int j = 0; j < VectorCount - i; j++)
                    c[i][j] = c[i - 1][j] ^ c[i - 1][j + 1];

            // массив включения переменных 0 - фиктивная 1 - не фиктивная
            bool[] v = new bool[F.VariableCount];

            // теперь нужно пройтись по массиву коэффициентов и получить список активных
            int usablevars = 0;
            FGet.GetPrevVector(b);
            for (int i = VectorCount - 1; i >= 0; i--)
            {
                if (usablevars == F.VariableCount) break;
                if (c[i][0] == true)
                {
                    for (int j = 0; j < F.VariableCount; j++)
                    {
                        if (b[j] == true)
                        {
                            usablevars++;
                            v[j] = true;
                        }
                    }
                }
                FGet.GetPrevVector(b);
            }

            // заполняем вывод
            for (int i = 0; i < F.VariableCount; i++)
            {
                if (v[i] == false)
                {
                    Out += " " + F.VariableName[i] + ",";
                }
            }

            if (F.VariableCount == 0) Out = "{ }";
            else Out = Out.Remove(Out.Length - 1) + " }";

            return Out;
        }

        /// <summary>
        /// Получает вектор значений функции
        /// </summary>
        static public bool[] GetFormulaVector(ref Formula F)
        {
            bool[] b = new bool[F.VariableCount];
            bool[] result = new bool[1 << F.VariableCount];

            for (int i = 0; i < (1 << F.VariableCount); i++)
            {
                result[i] = F.CaclOnBoolVector(b);
                GetNextVector(b);
            }
            return result;
        }

        /// <summary>
        /// Получает строковое представление вектора значений функции
        /// </summary>
        static public string GetFormulaStringVector(ref Formula F)
        {
            bool[] b; 

            b = GetFormulaVector(ref F);

            string Out = "{ ";

            // заполняем вывод
            for (int i = 0; i < (1 << F.VariableCount); i++)
                Out += ((b[i] == true) ? "1" : "0") + " , ";

            return Out.Remove(Out.Length - 2) + "}";
        }

        static public void GetTruthTable(ref Formula F)
        {
            MForm.ShowFormulaTable(ref F);
        }

        static public void GetScheme(ref Formula F)
        {
            MForm.ShowScheme(ref F);
        }

        /// <summary>
        /// Получает следующий в лексикографическом порядке вектор для двоичного массива
        /// </summary>
        static public void GetNextVector(bool[] b)
        {
            GetNextVectorRecursion(b, b.Length);
        }

        static void GetNextVectorRecursion(bool[] b, int t)
        {
            if (t == 0) return;
            if (b[t - 1] == false)
            {
                b[t - 1] = true;
            }
            else
            {
                b[t - 1] = false;
                GetNextVectorRecursion(b, t - 1);
            }
        }
        
        /// <summary>
        /// Получает предыдущий в лексикографическом порядке вектор для двоичного массива
        /// </summary>
        static public void GetPrevVector(bool[] b)
        {
            GetPrevVectorRecursion(b, b.Length);
        }

        static void GetPrevVectorRecursion(bool[] b, int t)
        {
            if (t == 0) return;
            if (b[t - 1] == true)
            {
                b[t - 1] = false;
            }
            else
            {
                b[t - 1] = true;
                GetPrevVectorRecursion(b, t - 1);
            }
        }
    }
}
