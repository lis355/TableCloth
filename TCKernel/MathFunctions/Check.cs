using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel.MathFunctions
{
    static class FCheck
    {/*
        /// <summary>
        /// Проверка на принадлежность предполному классу T0
        /// </summary>
        static public string BelongTZero(ref Formula F)
        {
            if (F.CaclOnBoolVector(new bool[F.VariableCount]) == false) return "Yes"; else return "No";
        }

        /// <summary>
        /// Проверка на принадлежность предполному классу T1
        /// </summary>
        static public string BelongTOne(ref Formula F)
        {
            bool[] b = new bool[F.VariableCount];
            for (int i = 0; i < F.VariableCount; i++) b[i] = true;
            if (F.CaclOnBoolVector(b) == true) return "Yes"; else return "No";
        }

        /// <summary>
        /// Проверка на принадлежность предполному классу S
        /// </summary>
        static public string BelongS(ref Formula F)
        {
            // Набор
            bool[] b = new bool[F.VariableCount];

            bool f1, f2;

            // константа не самодвойственна
            if (F.VariableCount == 0) return "No";

            for (int i = 0, j = 0; i < (1<<(F.VariableCount - 1)) ; i++)
            {
                // Рассчитываем первое значение
                f1 = F.CaclOnBoolVector(b);
                // Инвертируем набор
                for (j = 0; j < F.VariableCount; j++) b[j] = !b[j];
                // Рассчитываем второе значение
                f2 = F.CaclOnBoolVector(b);
                // Если значения одинаковые - неудача
                if (f1 == f2) return "No";
                // Получаем предыдущий или следующий набор в зависимости от того,
                // в какой половине наборов мы находимся
                if (b[0] == false) FGet.GetNextVector(b);
                else FGet.GetPrevVector(b);
            }
            return "Yes";
        }

        /// <summary>
        /// Проверка на принадлежность предполному классу M
        /// </summary>
        static public string BelongM(ref Formula F)
        {
            bool[][] b = new bool[1 << F.VariableCount][];
            b[0] = new bool[F.VariableCount];
            for (int i = 1, j = 0; i < (1 << F.VariableCount); i++)
            {
                b[i] = new bool[F.VariableCount];
                for (j = 0; j < F.VariableCount; j++) b[i][j] = b[i - 1][j];
                FGet.GetNextVector(b[i]);
            }
            
            bool f;

            // Проверяем каждый набор
            for (int i = 1, j = 0, h = 0; i < (1 << F.VariableCount); i++)
            {
                f = F.CaclOnBoolVector(b[i]);
                // Для него рассматриваем все предыдущие наборы
                for (j = i - 1; j >= 0; j--)
                {
                    // Смотрим, подходит ли набор
                    f = true;
                    for (h = 0; h < F.VariableCount; h++)
                    {
                        if (b[i][h] == false && b[j][h] == true)
                        {
                            f = false;
                            break;
                        }
                    }
                    // Да, подходит
                    if (f != false)
                    {
                        if (F.CaclOnBoolVector(b[i]) == false && F.CaclOnBoolVector(b[j]) == true)
                            return "No";
                    }
                }
            }
            return "Yes";
        }

        /// <summary>
        /// Проверка на принадлежность предполному классу L
        /// </summary>
        static public string BelongL(ref Formula F)
        {
            int VectorCount = Convert.ToInt32(Math.Pow(2, F.VariableCount));
           
            // массив включения переменных 0 - фиктивная 1 - не фиктивная
            bool[] v = new bool[F.VariableCount];

            // набор
            bool[] b = new bool[F.VariableCount];

            // треугольник
            bool[][] c = new bool[VectorCount][];

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

            //теперь просматриваем каждый моном и проверяем чтобы в нем было не более 1й переменной
            int varcol = 0;
            //FGet.GetNextVector(b);
            for (int i = 0; i< VectorCount; i++)
            {
                //просматриваем существующий моном
                if (c[i][0] == true)
                {
                    varcol = 0;
                    for (int j = 0; j < F.VariableCount; j++)
                    {
                        if (b[j] == true) varcol++;
                    }
                    if (varcol > 1)
                    {
                        return "No";
                    }
                }
                FGet.GetNextVector(b);
            }
            return "Yes";
        }

        static public string FullSystem(ref Formula[] FArray)
        {
            bool belongt0 = true, belongt1 = true, belongs = true, belongm = true, belongl = true;

            for (int i = 0; i < FArray.Length; i++)
            {
                if (belongt0) if (BelongTZero(ref FArray[i]) != "Yes") belongt0 = false;
                if (belongt1) if (BelongTOne(ref FArray[i]) != "Yes") belongt1 = false;
                if (belongs) if (BelongS(ref FArray[i]) != "Yes") belongs = false;
                if (belongm) if (BelongM(ref FArray[i]) != "Yes") belongm = false;
                if (belongl) if (BelongL(ref FArray[i]) != "Yes") belongl = false;
                if ((belongt0 || belongt1 || belongs || belongm || belongl) == false) return "Yes";
            }
            return "No"; 
        }

        static public string Basis(ref Formula[] FArray)
        {
            if (FArray.Length > 4) return "No";

            bool belongt0 = true, belongt1 = true, belongs = true, belongm = true, belongl = true;

            bool[,] M = new bool[FArray.Length, 5];// t0 t1 s m l

            for (int i = 0; i < FArray.Length; i++)
            {
                if (BelongTZero(ref FArray[i]) == "Yes") M[i, 0] = true;
                else M[i, 0] = belongt0 = false;
                if (BelongTOne(ref FArray[i]) == "Yes") M[i, 1] = true;
                else M[i, 1] = belongt1 = false;
                if (BelongS(ref FArray[i]) == "Yes") M[i, 2] = true;
                else M[i, 2] = belongs = false;
                if (BelongM(ref FArray[i]) == "Yes") M[i, 3] = true;
                else M[i, 3] = belongm = false;
                if (BelongL(ref FArray[i]) == "Yes") M[i, 4] = true;
                else M[i, 4] = belongl = false;
            }

            if ((belongt0 || belongt1 || belongs || belongm || belongl) == false)
            {
                bool[] Using = new bool[FArray.Length];
                int columndelete = 0, tmp = 0, pos = 0;// число вычеркнутых столбцов

                for (int j = 0, i = 0; j < 5; j++)
                    for (i = 0; i < FArray.Length; i++)
                        M[i, j] = !M[i, j];

                //определяем ядро, те импликанты в столбцах только один + от этого импликанта
                for (int j = 0, i = 0; j < 5; j++)
                {
                    tmp = 0;
                    for (i = 0; i < FArray.Length; i++)
                        if (M[i, j]) { tmp++; pos = i; }
                    if (tmp == 1) Using[pos] = true;
                }

                // зануляем столбцы, которые помечены в ядре
                for (int i = 0, j = 0; i < FArray.Length; i++)
                {
                    if (Using[i])
                        for (j = 0; j < 5; j++)
                        {
                            if (M[i, j])
                            {
                                FMinimize.CleanColumn(M, j);
                                columndelete++;
                            }
                        }
                }

                // задача покрыть все столбцы
                while (columndelete < 5)//еще не все столбцы вычеркнуты
                {
                    tmp = FMinimize.GetMaximumLine(M);
                    Using[tmp] = true;
                    for (int h = 0; h < 5; h++)
                    {
                        if (M[tmp, h])
                        {
                            columndelete++;
                            FMinimize.CleanColumn(M, h);
                        }
                    }
                }

                bool f = true;
                foreach (bool x in Using) if (!x) f = false;
                return (f) ? "Yes" : "No";
            }
            else
            {
                return "No";
            }
        }
      */
    }
}
