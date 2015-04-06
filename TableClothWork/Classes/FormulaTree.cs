using System;
using System.Collections.Generic;

namespace BinaryCalc
{
    class FormulaTree
    {
        public Vertex Root;
        List<int> levels; // высота по уровням

        public FormulaTree()
        {
            Root = null;
            levels = null;
        }

        public FormulaTree(ref Formula F)
        {
            /*
            Stack<Vertex> St = new Stack<Vertex>(30);
            levels = new List<int>(5);
            Vertex t1, t2;

            string s = "";

            for (int i = 0; i < F.Commands.Count; i++)
            {
                if (F.Commands[i] == CommandsCode.PushVariable ||
                    F.Commands[i] == CommandsCode.PushFalse ||
                    F.Commands[i] == CommandsCode.PushTrue)
                {
                    if (F.Commands[i] == CommandsCode.PushVariable) s = F.VariableName[F.Commands[++i]];
                    else if (F.Commands[i] == CommandsCode.PushFalse) s = "0";
                    else if (F.Commands[i] == CommandsCode.PushTrue) s = "1";

                    St.Push(new Vertex(null, null, null, 0, false, s, 0));
                }
                else if (F.Commands[i] == CommandsCode.OperationNot)
                {
                    // если отрицание сразу от аргумента или 1/0 то нужно просто изменить его
                    if (St.Peek().IsOperation == false)
                    {
                        t1 = St.Pop();
                        if (t1.Str == "0") t1.Str = "1";
                        else if (t1.Str == "1") t1.Str = "0";
                        else t1.Str = "!" + t1.Str;
                        St.Push(t1);
                    }
                    else // это отрицание от сложного аргумента
                    {
                        t1 = St.Pop();
                        int l = t1.Level + 1;

                        St.Push(new Vertex(t1, null, null, l, true, "", F.Commands[i]));

                        t1.Parent = St.Peek();
                    }
                }
                else if (F.Commands[i] >= CommandsCode.OperationAnd &&
                         F.Commands[i] <= CommandsCode.OperationPirse)
                {
                    t1 = St.Pop();//преимущественно левый
                    t2 = St.Pop();
                    int l = Math.Max(t1.Level, t2.Level) + 1;

                    St.Push(new Vertex(t2, t1, null, l, true, "", F.Commands[i]));

                    t1.Parent = t2.Parent = St.Peek();
                }
            }
            Root = St.Pop();
            */
        }

        public void Clear()
        {
            ClearRecursion(Root);
        }

        void ClearRecursion(Vertex V)
        {
            if (V.L != null) ClearRecursion(V.L);
            if (V.R != null) ClearRecursion(V.R);
            V = null;
        }

        public void SolveTreeForScheme()
        {
            SetPoints(Root);
        }

        void SetPoints(Vertex V)
        {
            if (V.L != null && V.R != null)
                if (V.R.IsOperation == false && V.L.IsOperation == true)
                {
                    Vertex tmp = V.R;
                    V.R = V.L;
                    V.L = tmp;
                }

            if (V.R != null) SetPoints(V.R);
            if (V.L != null) SetPoints(V.L);

            if (V.IsOperation == false)
            {
                //если это первый лист то нужно добавить в список высот
                if (levels.Count == 0) levels.Add(12);

                if (V.Parent == null)
                {
                    // единственная вершина в дерева лист-корень
                    V.In.X = 110;
                    V.In.Y = levels[0] + 16;
                    V.Out.X = 160;
                    V.Out.Y = levels[0] + 16;
                    return;
                }

                if (V.Parent.R.IsOperation == false && V != V.Parent.R)
                {
                    V.Parent.R.In.Y += 16;
                    V.Parent.R.Out.Y += 16;

                    V.In.X = 110;
                    V.In.Y = levels[0] + 16;
                    V.Out.X = 160;
                    V.Out.Y = levels[0] + 16;

                    levels[0] += 16 * 2;

                    for (int j = 1; j < levels.Count; j++) levels[j] = levels[0] - 24;
                }
                else
                {
                    V.In.X = 110;
                    V.In.Y = levels[0];
                    V.Out.X = 160;
                    V.Out.Y = levels[0];
                }

                levels[0] += 16;
            }
            #region OperationNot
            else if (V.Code == CommandsCode.OperationNot)
            {
                // если отрицание сразу от аргумента или 1/0 то нужно просто изменить его
                if (V.R.IsOperation == false)
                {
                    if (V.R.Str == "0") V.R.Str = "1";
                    else if (V.R.Str == "1") V.R.Str = "0";
                    else V.R.Str = "!" + V.R.Str;
                }
                else // это отрицание от сложного аргумента
                {
                    int l = V.R.Level + 1;

                    //если это первая  то нужно добавить в список высот
                    if (levels.Count == l) levels.Add(36);

                    V.In.X = 160 + 60 * (l - 1);
                    V.Out.X = 190 + 60 * (l - 1);
                    V.In.Y = V.Out.Y = V.R.In.Y;

                    levels[l] += 64;
                }
            }
            #endregion

            #region OperationAnd - OperationPirse
            else if (V.Code >= CommandsCode.OperationAnd &&
                        V.Code <= CommandsCode.OperationEquivalence)
            {
                int l = Math.Max(V.L.Level, V.R.Level) + 1;

                //если это первая  то нужно добавить в список высот
                if (levels.Count == l) levels.Add(36);

                V.In.X = 160 + 60 * (l - 1);
                V.In.Y = levels[l];
                V.Out.X = 190 + 60 * (l - 1);
                V.Out.Y = levels[l];

                if (V.L != null && V.R != null)
                {
                    V.In.Y = V.Out.Y = V.R.In.Y;
                    if (V.L.IsOperation && !V.R.IsOperation)
                        V.In.Y = V.Out.Y = V.L.In.Y;
                }

                levels[l] += 64;
            }
            #endregion
        }

        public int Heigh
        {
            get
            {
                return levels[0];
            }
        }

        public int Width
        {
            get
            {
                return 220 + 60 * (levels.Count - 1);
            }
        }
    }
}
