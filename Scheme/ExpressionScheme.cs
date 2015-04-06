using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace TableClothKernel
{
    internal partial class ExpressionScheme : UserControl
    {
        Graphics myGraphics;

        Expression Exp;

        Pen SPen = Pens.Black;
        Brush SBrush = Brushes.Black;
        Font DrawningFont;
        int[] TreeLevelsFill;

        const int
            BoxWidth = 30,
            BoxHeight = 48,
            BoxDistanceHorisontal = 50,
            BoxDistanceVertical = 20,
            BoxPointDiam = 8,
            UpIndent = 10, // отступ сверху
            RightIndent = 10, // отступ справа
            DownIndent = 10, // отступ сверху
            LeftIndent = 10, // отступ справа
            BoxLine = 10,
            SchemeArgs = 50,
            SchemeStart = RightIndent + SchemeArgs + BoxWidth + LeftIndent,
            TextHeight = 15;

        int SHeight = 0;

        public ExpressionScheme(Expression E)
        {
            Exp = E;
            int SchemeDeep = ExpressionVertexHeightWithBalance(Exp.Root);
            Size SchemeSize = new Size(SchemeStart + SchemeArgs +
                ((SchemeDeep > 0) ? (BoxWidth * SchemeDeep + BoxDistanceHorisontal * (SchemeDeep - 1)) : 0 )
                + SchemeArgs + LeftIndent,
                UpIndent + BoxHeight * SHeight + BoxDistanceVertical * (SHeight - 1) + DownIndent);
            DrawningFont = new System.Drawing.Font("Courier New", 12.0F, FontStyle.Bold, GraphicsUnit.Point);
            DrawScheme(SchemeSize, SchemeDeep);
            SuspendLayout();
            BackColor = System.Drawing.Color.White;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            Name = "Scheme";
            Size = SchemeSize;
            ResumeLayout(false);
        }

        /// <summary>
        /// Получает высоту поддерева с вершиной в V
        /// </summary>
        int ExpressionVertexHeightWithBalance(ExpressionVertex V)
        {
            if (V == null)
                return 0;

            int res = 0;
            if (V.IsOperand())
            {
                ExpressionVertex l = ((OperandVertex)V).L, r = ((OperandVertex)V).R;

                if (r != null)
                {
                    // балансировка дерева в левую сторону
                    if (r.IsOperand() && !l.IsOperand())
                    {
                        ((OperandVertex)V).L = r;
                        ((OperandVertex)V).R = l;
                    }
                }
                
                res = 1 + Math.Max(ExpressionVertexHeightWithBalance(l), ExpressionVertexHeightWithBalance(r));

                // подсчитываем ширину дерева
                if (!l.IsOperand())
                    SHeight++;
            }
            return res;
        }

        void DrawScheme(Size SchemeSize, int SchemeDeep)
        {
            BackgroundImage = new Bitmap(SchemeSize.Width, SchemeSize.Height);
            myGraphics = Graphics.FromImage(BackgroundImage);
            myGraphics.SmoothingMode = SmoothingMode.HighQuality;
            //myGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            myGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            myGraphics.FillRectangle(Brushes.White, 0, 0, SchemeSize.Width - 1, SchemeSize.Height - 1);

            /**/myGraphics.DrawRectangle(SPen, 0, 0, SchemeSize.Width - 1, SchemeSize.Height - 1);

            DrawLeftOperands(SchemeSize.Height);
            TreeLevelsFill = new int[SchemeDeep];
            DrawRecursion(Exp.Root, SchemeDeep, SchemeSize.Width - LeftIndent, UpIndent + BoxHeight / 2);
            DrawCircle(SchemeSize.Width - LeftIndent - BoxPointDiam / 2, UpIndent + BoxHeight / 2 - BoxPointDiam / 2, false);
        }

        /// <summary>
        /// Рекурсивное рисование схемы
        /// </summary>
        void DrawRecursion(ExpressionVertex V, int Depth, int parentx, int parenty)
        {
            if (V == null)
                return;

            if (V.IsOperand())
            {
                int xop = SchemeStart + SchemeArgs + (Depth - 1) * (BoxWidth + BoxDistanceHorisontal),
                    yop = UpIndent + TreeLevelsFill[Depth - 1] * (BoxHeight + BoxDistanceVertical) + BoxHeight / 2;

                DrawOperator(xop, yop, ((OperandVertex)V).OperationCode);
                DrawLine(xop + BoxWidth, yop, parentx, parenty);
                TreeLevelsFill[Depth - 1]++;

                DrawRecursion(((OperandVertex)V).L, Depth - 1, xop, yop);
                DrawRecursion(((OperandVertex)V).R, Depth - 1, xop, yop);

                if (((OperandVertex)V).OperationCode == CommandsCode.Not && !((OperandVertex)V).L.IsOperand())
                {
                    // если оператор not то нужно всю остальную схему сдвинуть вниз
                    for (int h = 0; h < Depth - 1; h++)
                        TreeLevelsFill[h]++;
                    DrawConstantOrVariable(((OperandVertex)V).L, xop, parenty);
                }
                else if (!((OperandVertex)V).L.IsOperand() && !((OperandVertex)V).R.IsOperand())
                {
                    DrawConstantOrVariable(((OperandVertex)V).L, xop, parenty - TextHeight / 2 - 2);
                    DrawConstantOrVariable(((OperandVertex)V).R, xop, parenty + TextHeight / 2 + 2);
                }
            }
        }

        void DrawConstantOrVariable(ExpressionVertex V, int x, int y)
        {
            DrawLine(SchemeStart, y, x, y);
            if (V.IsConstant())
            {
                DrawText(SchemeStart + 5, y - TextHeight - 2, ((ConstantVertex)V).ToString());
            }
            else if (V.IsVariable())
            {
                DrawText(SchemeStart + 5, y - TextHeight - 5, ((VariableVertex)V).Name);
            }
        }

        /// <summary>
        /// Пишет текст
        /// </summary>
        void DrawText(int x, int y, string s)
        {
            myGraphics.DrawString(s, DrawningFont, SBrush, x, y);
        }

        /// <summary>
        /// Рисует левую часть схемы, где на вход подаются все переменные и вычисляется их отрицания
        /// </summary>
        void DrawLeftOperands(int Height)
        {
            for (int i = 0, y; i < Exp.VariableCount; i++)
            {
                y = UpIndent + BoxHeight / 2 + i * 30/** (BoxDistanceVertical * 2 + BoxHeight)*/;
                myGraphics.DrawLine(SPen, RightIndent, y, SchemeStart, y);
                DrawCircle(RightIndent - BoxPointDiam / 2, y - BoxPointDiam / 2, false);
                DrawText(RightIndent + 5, y - TextHeight - 5, Exp.VariableNames[i]);
            }
            // вертикальная черная линия
            myGraphics.FillRectangle(SBrush, SchemeStart - 2, UpIndent, 4, Height - UpIndent - DownIndent);
        }
        
        /// <summary>
        /// Рисует прямоугольник с операцией
        /// </summary>
        void DrawOperator(int x, int y, CommandsCode Code)
        {
            DrawText(x, y - BoxHeight / 2, ExpressionString.CommandsCodeToString(Code));
            myGraphics.DrawRectangle(SPen, x, y - BoxHeight / 2, BoxWidth, BoxHeight);
            DrawCircle(x + BoxWidth - BoxPointDiam / 2, y - BoxPointDiam / 2);
        }

        void DrawCircle(int x, int y, bool fill = true)
        {
            if (fill)
            {
                myGraphics.FillEllipse(SBrush, x, y, BoxPointDiam, BoxPointDiam);
            }
            else
            {
                myGraphics.FillEllipse(Brushes.White, x, y, BoxPointDiam, BoxPointDiam);
                myGraphics.DrawEllipse(SPen, x, y, BoxPointDiam, BoxPointDiam);
            }
        }

        /// <summary>
        /// Рисует прямоугольный путь соединяющий 2 точки
        /// </summary>
        void DrawLine(int x1, int y1, int x2, int y2)
        {
            if (y1 == y2)
            {
                myGraphics.DrawLine(SPen, x1, y1, x2, y2);
            }
            else
            {
                myGraphics.DrawLine(SPen, x1, y1, x2 - BoxLine, y1);
                myGraphics.DrawLine(SPen, x2 - BoxLine, y1, x2 - BoxLine, y2 + BoxLine);
                myGraphics.DrawLine(SPen, x2 - BoxLine, y2 + BoxLine, x2, y2 + BoxLine);
            }
        }

        /*
        private void LoadFont()
        {
            MathFont.CourierMathFont = new PrivateFontCollection();
            MathFont.CourierMathFont.AddFontFile(Application.StartupPath + @"\font\courier_math.ttf");
        }
        */
        
        /// <summary>
        /// Сохранить изображение
        /// </summary>
        public void SaveScheme(string filename)
        {
            BackgroundImage.Save(filename + ".PNG");
        }
    }
}
