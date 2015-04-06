namespace TableClothKernel.MathFunctions
{
    static class FMinimize
    {
        /// <summary>
        /// Минимизация логической формулы методом Квайна из СДНФ
        /// </summary>
        /*static public Formula MinimizeQuine(ref Formula F)
        {
            Formula G = new Formula(ref F);

            // Наборы, на которых функция истина. Будут преобразовываться в импликанты.
            // 0 - x == 0
            // 1 - x == 1
            // 2 == z не учавствует в импликанте
            List<short[]> Implicants = new List<short[]>(F.VariableCount / 2);
            List<short[]> SimplyImplicants = new List<short[]>(F.VariableCount / 2);
            List<bool> Using = new List<bool>(Implicants.Count);

            // Наборы, на которых функция истина.
            List<short[]> TrueNabors = new List<short[]>(F.VariableCount / 2);
            
            // Текущий набор
            bool[] b = new bool[F.VariableCount];

            // Длина вектора функции
            int VectorCount = 1 << F.VariableCount;

            #region Определение наборов, на которых функция истина
            for (int i = 0, j = 0; i < VectorCount; i++)
            {
                if (F.CaclOnBoolVector(b))
                {
                    short[] h = new short[F.VariableCount];
                    short[] k = new short[F.VariableCount];
                    for (j = 0; j < F.VariableCount; j++)
                        k[j] = h[j] = (short)((b[j]) ? 1 : 0);
                    Implicants.Add(h); 
                    TrueNabors.Add(k);
                }
                FGet.GetNextVector(b);
            }
            // Рассматриваем вырожденные случаи, когда функция тождественная константа
            if (Implicants.Count == 0)
            {
                G.Commands.Add(CommandsCode.PushFalse);
                return G;
            }
            else if (Implicants.Count == VectorCount)
            {
                G.Commands.Add(CommandsCode.PushTrue);
                return G;
            }
            #endregion

            #region Составляем таблицу по увеличению единиц в наборах
            List<int> ImplicantsNumbers = new List<int>(Implicants.Count);
            for (int j = 0; j < Implicants.Count; j++) ImplicantsNumbers.Add(NumberOfTrue(Implicants[j]));

            // Сортируем по возрастанию.
            int pos = 0, min, tmp;
            short[] tmpa;
            for (int j = 0, k = 0; j < Implicants.Count - 1; j++)
            {
                min = int.MaxValue;
                for (k = j; k < Implicants.Count; k++)
                    if (ImplicantsNumbers[k] < min) { min = ImplicantsNumbers[k]; pos = k; }
                if (pos != j && ImplicantsNumbers[pos] != ImplicantsNumbers[j])
                {
                    tmpa = Implicants[j];
                    Implicants[j] = Implicants[pos];
                    Implicants[pos] = tmpa;
                    tmp = ImplicantsNumbers[j];
                    ImplicantsNumbers[j] = ImplicantsNumbers[pos];
                    ImplicantsNumbers[pos] = tmp;
                }
            }

            // теперь ImplicantsNumbers говорит сколько элементов в группах
            pos = ImplicantsNumbers.Count;
            for (int j = 0; j < pos; )
            {
                tmp = j;
                while ((j < pos) && (ImplicantsNumbers[j] == ImplicantsNumbers[tmp])) j++;
                ImplicantsNumbers.Add(j - tmp);
            }
            ImplicantsNumbers.RemoveRange(0, pos);
            #endregion

            #region Пока есть сложные импликанты, делаем операции сливания
            while (true)
            {
                for (int i = 0; i < Implicants.Count; i++) Using.Add(false);
                // для кадого блока (0 - 1) (1 - 2) ...
                int icount = Implicants.Count, ncount = ImplicantsNumbers.Count;
                for (int i = 0, j = 0, k = 0, start = 0, h = 0; i < ncount - 1; i++)//блок
                {
                    tmp = 0;
                    for (j = start; j < start + ImplicantsNumbers[i]; j++)//импликант в блоке
                    {
                        for (k = start + ImplicantsNumbers[i];
                            k < start + ImplicantsNumbers[i] + ImplicantsNumbers[i + 1]; k++)//импликант в соседнем следующем блоке
                        {
                            if (IsDifferentInOneCoord(Implicants[j], Implicants[k], ref pos))
                            {
                                tmpa = new short[G.VariableCount];
                                for (h = 0; h < G.VariableCount; h++) tmpa[h] = Implicants[j][h];
                                tmpa[pos] = 2;
                                Implicants.Add(tmpa); Using.Add(false);
                                //Implicants[Implicants.Count - 1][pos] = 2;
                                Using[j] = true;
                                Using[k] = true;
                                tmp++;
                            }
                        }
                    }
                    start += ImplicantsNumbers[i];
                    ImplicantsNumbers.Add(tmp);
                }
                pos = 0;
                for (tmp = 0; tmp < icount; tmp++)
                    if (Using[tmp] == false)
                    {
                        pos++;
                        //if (!(tmp >= icount - ImplicantsNumbers[ncount - 1]))//последний блик не в счет
                        SimplyImplicants.Add(Implicants[tmp]);// эта переменная не использовалась, значит она простой импликант
                    }
                // корректируем данные о группах
                Implicants.RemoveRange(0, icount);
                Using.RemoveRange(0, icount);
                ImplicantsNumbers.RemoveRange(0, ncount);
                if (pos == icount)//все не использовались
                    break;
            }
            foreach (short[] x in Implicants) SimplyImplicants.Add(x);
            #endregion

            #region  составляем таблицу импликантов
            bool[,] ImpMatrix = new bool[SimplyImplicants.Count, TrueNabors.Count];
            Using.Clear();
            for (int i = 0, j = 0; i < SimplyImplicants.Count; i++)
            {
                Using.Add(false);
            	for (j = 0; j < TrueNabors.Count; j++)
            	{
                    ImpMatrix[i,j] = IsBelongToVector(SimplyImplicants[i], TrueNabors[j]);
            	}
            }

            int columndelete = 0;// число вычеркнутых столбцов

            //определяем ядро, те импликанты в столбцах только один + от этого импликанта
            for (int j = 0, i = 0; j < TrueNabors.Count; j++)
            {
                tmp = 0;
                for (i = 0; i < SimplyImplicants.Count; i++)
                    if (ImpMatrix[i, j]) { tmp++; pos = i; }
                if (tmp == 1) Using[pos] = true;
            }

            // зануляем столбцы, которые помечены в ядре
            for (int i = 0, j = 0; i < SimplyImplicants.Count; i++)
            {
                if (Using[i]) 
                    for (j = 0; j < TrueNabors.Count; j++)
                    {
                        if (ImpMatrix[i, j])
                        {
                            CleanColumn(ImpMatrix, j);
                            columndelete++;
                        }
                    }
            }

            // задача покрыть все столбцы
            while (columndelete < TrueNabors.Count)//еще не все столбцы вычеркнуты
            {
                tmp = GetMaximumLine(ImpMatrix);
                Using[tmp] = true;
                for (int h = 0; h < TrueNabors.Count; h++)
                {
                    if (ImpMatrix[tmp, h])
                    {
                        columndelete++;
                        CleanColumn(ImpMatrix, h);// ImpMatrix[tmp, h] = false;
                    }
                }
            }
            #endregion

            #region по выбранным импликантам собираем функцию
            for (int i = SimplyImplicants.Count - 1, j = 0, k = 0, h = 0; i >= 0; i--)
            {
                if (Using[i])
                {
                    h = 0;
                    for (j = 0; j < F.VariableCount; j++)
                    {
                        if (SimplyImplicants[i][j] == 1 || SimplyImplicants[i][j] == 0)
                        {
                            G.Commands.Add(CommandsCode.PushVariable);
                            G.Commands.Add(j);//номер переменной
                            if (SimplyImplicants[i][j] == 0)
                                G.Commands.Add(CommandsCode.OperationNot);
                            h++;
                            if (h > 1) G.Commands.Add(CommandsCode.OperationAnd);
                        }
                    }
                    // k - количество коньюнкций
                    k++;
                    if (k > 1) G.Commands.Add(CommandsCode.OperationOr);
                }
            }
            #endregion

            return G; 
        }
        */
        /// <summary>
        /// Возвращает строку где число единиц больше всех
        /// </summary>
        static public int GetMaximumLine( bool[ , ] m )
        {
            int pos = 0, max = int.MinValue;
            for ( int i = 0, j = 0, cout = 0; i < m.GetLength( 0 ); i++ )
            {
                cout = 0;
                for ( j = 0; j < m.GetLength( 1 ); j++ )
                    if ( m[ i, j ] ) cout++;
                if ( cout > max ) { max = cout; pos = i; }

            }
            return pos;
        }

        /// <summary>
        /// Зануляет выбранный столбец
        /// </summary>
        static public void CleanColumn( bool[ , ] m, int n )
        {
            for ( int i = 0; i < m.GetLength( 0 ); i++ )
                m[ i, n ] = false;
        }

        /// <summary>
        /// Зануляет выбранную строку
        /// </summary>
        static public void CleanRow( bool[ , ] m, int n )
        {
            for ( int i = 0; i < m.GetLength( 1 ); i++ )
                m[ n, i ] = false;
        }

        /// <summary>
        /// Определяет лежит ли вектор v в векторе b
        /// </summary>
        static bool IsBelongToVector( short[] b, short[] v )
        {
            //  0 1  z 
            //  0 1  0/1
            for ( int i = 0; i < b.Length; i++ )
                if ( ( b[ i ] != 2 ) && ( v[ i ] != b[ i ] ) ) return false;
            return true;
        }

        /// <summary>
        /// Считает количество единиц в наборе.
        /// Набор должен быть только из 0 и 1.
        /// </summary>
        static int NumberOfTrue( short[] b )
        {
            int sum = 0;
            for ( int i = 0; i < b.Length; i++ ) sum += b[ i ];
            return sum;
        }

        /// <summary>
        /// Возращает true если наборы различаются в одной 
        /// координате и возвращает номер этой координаты в out параметре
        /// </summary>
        /// <param name="b">Первый набор</param>
        /// <param name="v">Второй набор</param>
        /// <param name="pos">Номер координаты, где отличие</param>
        /// <returns>Истина, если различие только в 1 координате</returns>
        static bool IsDifferentInOneCoord( short[] b, short[] v, ref int pos )
        {
            int onediff = 0;
            for ( int i = 0; i < b.Length; i++ )
            {
                if ( b[ i ] != v[ i ] )
                {
                    pos = i;
                    onediff++;
                }
                if ( onediff > 1 ) return false;
            }
            return true;
        }
    }
}
