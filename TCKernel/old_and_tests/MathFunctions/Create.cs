namespace TableClothKernel.MathFunctions
{/*
	[CalcMethods]
    public static class FCreate
    {
        /// <summary>
        /// Создает формулу по вектору значений
        /// </summary>
        static public Operand CreateFromVector( Constant constant )
        {
			return Constant.True;

            Formula G = new Formula();

            // если функция константа
            if (B.Length == 1)
            {
                if (B[0] == true) G.Commands.Add(CommandsCode.PushTrue);
                if (B[0] == false) G.Commands.Add(CommandsCode.PushFalse);
                return G;
            }
            
            for (int i = 0; i < Syntax.GetNumberOfVariableFromFunctionVector(B.Length); i++)
            {
                G.AddVariableName("x"+Convert.ToString(i+1));
            }

            bool[] b = new bool[G.VariableCount];
            int k = 0;
            for (int i = 0; i < (1 << G.VariableCount); i++)
            {
                // если значение функции на наборе 1 то добавляем коньюнкцию
                if (B[i] == true)
                {
                    for (int j = 0; j < G.VariableCount; j++)
                    {
                        G.Commands.Add(CommandsCode.PushVariable);
                        G.Commands.Add(j);
                        if (b[j] == false) G.Commands.Add(CommandsCode.OperationNot);
                        if (j != 0) G.Commands.Add(CommandsCode.OperationAnd);
                    }
                    // k - количество коньюнкций
                    k++;
                    if (k > 1) G.Commands.Add(CommandsCode.OperationOr);
                }
                FGet.GetNextVector(b);
            }
            return G;
        }
    }*/
}
