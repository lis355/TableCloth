using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Scheme
{
    class Vertex
    {
        public Vertex R, L, Parent;
        public bool IsOperation;
        public string Str;//если это лист то в нем записан идентификатор операнда
        public Point In, Out;
        public int Level, Code, Number;

        /// <summary>
        /// Создать вершину
        /// </summary>
        /// <param name="Rv">Правый сын</param>
        /// <param name="Lv">Левый сын</param>
        /// <param name="P">Отец</param>
        /// <param name="Lev">Уровень в дереве от листа</param>
        /// <param name="Op">Является ли операцией либо лист</param>
        /// <param name="xin">Координато точки входа по x</param>
        /// <param name="yin">Координато точки входа по y</param>
        /// <param name="xout">Координато точки выхода поx</param>
        /// <param name="yout">Координато точки выхода по y</param>
        /// <param name="Name">Имя переменной</param>
        /// <param name="C">Код операции</param>
        /// <param name="N">Номер действия, как вычисляется выражение</param>
        public Vertex(Vertex Rv, Vertex Lv, Vertex P, int Lev, bool Op, int xin, int yin, int xout, int yout, string Name, int C, int N)
        {
            R = Rv;
            L = Lv;
            Parent = P;
            IsOperation = Op;
            Str = Name;
            In = new Point(xin, yin);
            Out = new Point(xout, yout);
            Level = Lev;
            Code = C;
            Number = N;
        }
    }

    class DrawTree
    {
        public Vertex Root;

        public int Heigh, Width, VertexCount;

        List<int> levels = new List<int>(5); // высота по уровням

        Vertex t1, t2;

        int Num;

        public DrawTree(ref Formula F)
        {
            Stack<Vertex> St = new Stack<Vertex>(30);

            string s = "";


            Num = VertexCount = 0;

            // сначала построим дерево без связей
            for (int i = 0; i < F.Commands.Count; i++)
            {
                if (F.Commands[i] == CommandsCode.PushVariable ||
                    F.Commands[i] == CommandsCode.PushFalse ||
                    F.Commands[i] == CommandsCode.PushTrue)
                {
                    if (F.Commands[i] == CommandsCode.PushVariable) s = F.VariableName[F.Commands[++i]];
                    else if (F.Commands[i] == CommandsCode.PushFalse) s = "0";
                    else if (F.Commands[i] == CommandsCode.PushTrue) s = "1";

                    St.Push(new Vertex(null, null, null, 0, false, 0, 0, 0, 0, s, 0, 0));
                    VertexCount++;
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

                        St.Push(new Vertex(t1, null, null, l, true, 0, 0, 0, 0, "", F.Commands[i], 0));
                        VertexCount++;

                        t1.Parent = St.Peek();
                    }
                }
                else if (F.Commands[i] >= CommandsCode.OperationAnd &&
                         F.Commands[i] <= CommandsCode.OperationPirse)
                {
                    t1 = St.Pop();//преимущественно левый
                    t2 = St.Pop();
                    int l = Math.Max(t1.Level, t2.Level) + 1;

                    St.Push(new Vertex(t2, t1, null, l, true, 0, 0, 0, 0, "", F.Commands[i], 0));
                    VertexCount++;

                    t1.Parent = t2.Parent = St.Peek();
                }
            }
            Root = St.Pop();

            // теперь балансируем дерево
            //Balanse(Root);

            // теперь нужно назначить точки входа и выхода
            levels = new List<int>(5); // высота по уровням

            SetPoints(Root);

            Heigh = levels[0];
            Width = 220 + 60 * (levels.Count - 1);
        }
        
        void Balanse(Vertex V)
        {
            if (V.R == null) return;

            if (V.L != null)
                if (V.R.IsOperation == false && V.L.IsOperation == true)
                {
                    Vertex tmp = V.R;
                    V.R = V.L;
                    V.L = tmp;
                }

            if (V.R != null) Balanse(V.R);
            if (V.L != null) Balanse(V.L);
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

                if (V.Parent.R.IsOperation == false && V != V.Parent.R)
                {
                    V.Parent.R.In.Y += 16;
                    V.Parent.R.Out.Y += 16;
                
                    V.In.X = 110;
                    V.In.Y = levels[0] + 16;
                    V.Out.X = 160;
                    V.Out.Y = levels[0] + 16;
                    V.Number = Num++;
                
                    levels[0] += 16 * 2;

                    for (int j = 1; j < levels.Count; j++) levels[j] = levels[0] - 24;
                }
                else
                {
                    V.In.X = 110;
                    V.In.Y = levels[0];
                    V.Out.X = 160;
                    V.Out.Y = levels[0];
                    V.Number = Num++;

                   // levels[0] += 16;
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
                    V.In.Y = levels[l - 1] - 64;
                    V.Out.X = 190 + 60 * (l - 1);
                    V.Out.Y = levels[l - 1] - 64;
                    V.Number = Num++;

                    levels[l] += 64;
                }
            }
            #endregion
            #region OperationAnd - OperationPirse
            else if (V.Code >= CommandsCode.OperationAnd &&
                        V.Code <= CommandsCode.OperationPirse)
            {
                int l = Math.Max(V.L.Level, V.R.Level) + 1;

                //если это первая  то нужно добавить в список высот
                if (levels.Count == l) levels.Add(36);

                V.In.X = 160 + 60 * (l - 1);
                V.In.Y = levels[l];
                V.Out.X = 190 + 60 * (l - 1);
                V.Out.Y = levels[l];
                V.Number = Num++;

                levels[l] += 64;
            }
            #endregion
        }
    }
}
