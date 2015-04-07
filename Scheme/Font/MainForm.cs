using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Text.RegularExpressions;

namespace Scheme
{
    /// <summary> Главная форма приложения. </summary>
    public partial class MainForm : Form
    {
        // шрифт для рисования надписей
        Font DrawningOperationFont, DrawningArgumentFont;

        Graphics Scheme;

        SolidBrush Brush;

        Pen SBPen, SRPen, SYPen, G;

        Formula F;

        DrawTree T;

        Point SchemeSize;

        Bitmap MainImage;

        int H; // текущий оператор/операнд отображенный желтым цветом на схеме

        public MainForm(/*ref Formula F*/)
        {
            InitializeComponent();

            #region initialize
            //
            LoadFont();
            F = new Formula(@"(!x1 ~ x2 > x3 ^ x4 + x5 * x6) *
                                 (x1 + !x2 ^ x3 + x3 > x4 + x5 + x6) *
                                 ((X ~ Y) + !(!Z + E) + R + G)");
            //F = new Formula("(A ~ B + !(!C + D) + E + F)");
            Size = new System.Drawing.Size(SystemInformation.VirtualScreen.Width,
                SystemInformation.VirtualScreen.Height - 40);
            //
            #endregion

            // строим дерево по формуле
            T = new DrawTree(ref F);
            
            // получаем размер схемы
            SchemeSize = new Point(T.Width, Math.Max(T.Heigh, 30 + F.VariableCount*70));

            // инициализируем изображение и вывод схемы на него
             MainImage = new Bitmap(SchemeSize.X, SchemeSize.Y);
             Scheme = Graphics.FromImage(MainImage);

            // инициазируем графические примитивы
            DrawningOperationFont = new Font(MathFont.CourierMathFont.Families[0], 15F);
            DrawningArgumentFont = new Font(MathFont.CourierMathFont.Families[0], 10F);
            Brush = new SolidBrush(Color.Black);
            SBPen = new Pen(Color.Black);
            SRPen = new Pen(Color.OrangeRed, 3);
            SYPen = new Pen(Color.Gold, 3);

//             float[] dashValues = { 4, 2, 4, 2, 4, 2 };
//             SYPen.DashPattern = dashValues;
            
            H = -1;

            // рисуем схему в MainImage
            PaintScheme(H);

            // выводим схему на экран
            DrawOriginalSheme(MaxButton, null);
        }

        #region DrawItems
        /// <summary>
        /// Рисует левую часть схемы, где на вход подаются все переменные и вычисляется их отрицания
        /// </summary>
        void DrawOperands(int h)
        {
            int x0 = 10, y = 10;

            for (int i = 0; i < F.VariableCount; i++)
            {
                DrawText(x0 + 8, y, Convert.ToString(F.VariableName[i]));
                Scheme.DrawEllipse(SBPen, x0, y + 15, 6, 6);
                Scheme.DrawLine(SBPen, x0 + 7, y + 18, x0 + 100, y + 18);
                Scheme.DrawLine(SBPen, x0 + 30, y + 18, x0 + 30, y + 18 + 30);
                Scheme.DrawLine(SBPen, x0 + 30, y + 18 + 30, x0 + 30 + 20, y + 18 + 30);
                DrawOperator(x0 + 50, y + 48, CommandsCode.OperationNot, ref SBPen);
                Scheme.DrawLine(SBPen, x0 + 80, y + 48, x0 + 100, y + 48);
                y += 70;
            }
            x0 = 110;
            Scheme.FillRectangle(Brush, x0 - 2, 10, 5, Math.Max(h, y) - 10);
        }

        /// <summary>
        /// Рисует прямоугольник с операцией
        /// </summary>
        void DrawOperator(int x, int y, int Code , ref Pen S)
        {
            string codestring = "";
            switch (Code)
            {
                case CommandsCode.OperationNot: codestring = "!"; break;
                case CommandsCode.OperationAnd: codestring = "*"; break;
                case CommandsCode.OperationOr: codestring = "+"; break;
                case CommandsCode.OperationXor: codestring = "^"; break;
                case CommandsCode.OperationPirse: codestring = "~"; break;
                case CommandsCode.OperationSheffer: codestring = "|"; break;
                case CommandsCode.OperationImplication: codestring = ">"; break;
            }
            Scheme.DrawString(codestring, DrawningOperationFont, Brush, x, y - 22, new StringFormat());
            Scheme.DrawRectangle(S, x, y - 24, 30, 48);
            Scheme.FillEllipse(Brush, x + 27, y - 3, 6, 6);
        }

        /// <summary>
        /// Рисует прямоугольный путь соединяющий 2 точки
        /// </summary>
        void DrawLine(int x1, int y1, int x2, int y2, ref Pen S)
        {
            if (x1 == x2) return;
            int m = 1;
            if (y1 < y2) m = -1;
            if (y1 != y2 && x1 != x2)
            {         
                Scheme.DrawLine(S, x1, y1, x2 - 10, y1);
                Scheme.DrawLine(S, x2 - 10, y1, x2 - 10, y2 + m * 10);
                Scheme.DrawLine(S, x2 - 10, y2 + m * 10, x2, y2 + m * 10);
            }
            else Scheme.DrawLine(S, x1, y1, x2, y2);
        }

        /// <summary>
        /// Добавляет лист - переменная либо константа
        /// </summary>
        void DrawArgument(int x, int y, string s, ref Pen S)
        {
            DrawText(x + 8, y - 16, s);
            Scheme.DrawLine(S, x, y, x + 50, y);
        }

        /// <summary>
        /// Рисуем заключительную линию "F"
        /// </summary>
        void DrawFunc(int x, int y, int h)
        {
            Pen P = SBPen;
            if (h == T.VertexCount ) P = SYPen;
            if (h == T.VertexCount + 1) P = SRPen;
            DrawText(x + 38, y - 16, "F");
            Scheme.DrawLine(P, x + 2, y, x + 50, y);
            Scheme.DrawEllipse(P, x + 50, y - 3, 6, 6);
        }

        /// <summary>
        /// Пишет текст шрифтом 10F
        /// </summary>
        void DrawText(int x, int y, string s)
        {
            Scheme.DrawString(s, DrawningArgumentFont, Brush, x, y, new StringFormat());
        }
        #endregion

        /// <summary>
        /// Рисуем схему
        /// </summary>
        private void PaintScheme(int h)
        {
            // включаем сглаживание линий
            Scheme.SmoothingMode = SmoothingMode.HighQuality;

            // сбрасываем все нарисованное
            Scheme.FillRectangle(new SolidBrush(Color.White), 0, 0, SchemeSize.X, SchemeSize.Y);

            // рисуем схему
            DrawTreeRecursion(T.Root, h);
            DrawOperands(T.Heigh);
            DrawFunc(T.Root.Out.X, T.Root.Out.Y, h);
        }

        /// <summary>
        /// Функция рерурсивного рисования схемы
        /// </summary>
        void DrawTreeRecursion(Vertex V, int h)
        {
            if (V == null) return;

            #region Recursion for R
            if (V.R != null)
            {
                if (V.R.Number > h || V.R.IsOperation == false) G = SBPen;
                else if (V.R.Number == h) G = SYPen;
                else if (V.R.Number < h) G = SRPen;
                DrawLine(V.R.Out.X, V.R.Out.Y, V.In.X, V.In.Y, ref G);

                DrawTreeRecursion(V.R, h);
            }
            #endregion
            #region Recursion for L
            if (V.L != null)
            {
                if (V.L.Number > h || V.L.IsOperation == false) G = SBPen;
                else if (V.L.Number == h) G = SYPen;
                else if (V.L.Number < h) G = SRPen;
                DrawLine(V.L.Out.X, V.L.Out.Y, V.In.X, V.In.Y, ref G);

                DrawTreeRecursion(V.L, h);
            }
            #endregion

            if (V.Number < h) G = SRPen;
            else if (V.Number == h) G = SYPen;
            else if (V.Number > h) G = SBPen;

            if (V.IsOperation == false) DrawArgument(V.In.X, V.In.Y, V.Str, ref G);
            else DrawOperator(V.In.X, V.In.Y, V.Code, ref G);

            if (V.IsOperation == false)//вершина - лист, нужно нарисовать линию от нее
                DrawLine(V.Out.X, V.Out.Y, V.Parent.In.X, V.Parent.In.Y, ref G);
        }

        #region font
        //////////////////////////////////////////////////////////////////////////
        private void LoadFont()
        {
            MathFont.CourierMathFont = new PrivateFontCollection();
            MathFont.CourierMathFont.AddFontFile(Application.StartupPath + @"\font\courier_math.ttf");
        }
        //////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Сохранить изображение
        /// </summary>
        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SPicture.Image.Save(DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".bmp");
        }

        #region Прокрутка
        bool f = false;
        int xstart, ystart;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            xstart = e.X;
            ystart = e.Y;
            f = true;
            SPicture.Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (f)
            {
                int x = SPanel.HorizontalScroll.Value, y = SPanel.VerticalScroll.Value;

                if (e.X > xstart) x = (x + SPanel.HorizontalScroll.SmallChange) % SPanel.HorizontalScroll.Maximum;
                else if (x > SPanel.HorizontalScroll.SmallChange - 1) x = x - SPanel.HorizontalScroll.SmallChange;
                SPanel.HorizontalScroll.Value = x;

                if (e.Y > ystart) y = (y + SPanel.VerticalScroll.SmallChange) % SPanel.VerticalScroll.Maximum;
                else if (y > SPanel.VerticalScroll.SmallChange - 1) y = y - SPanel.VerticalScroll.SmallChange;
                SPanel.VerticalScroll.Value = y;

                SPicture.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            f = false;
            SPicture.Cursor = Cursors.Default;
        }
        #endregion

        /// <summary>
        /// Показать превью
        /// </summary>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int k = 50;

            // меняем размер рисунка
            SPicture.Size = new System.Drawing.Size(ClientSize.Width - k, ClientSize.Height - k);
            
            // рисуем новую схему
            SPicture.Image = new Bitmap(ClientSize.Width - k, ClientSize.Height - k);
            Scheme = Graphics.FromImage(SPicture.Image);

            // делаем превью с MainImage
            if (SPicture.Size.Width > SPicture.Size.Height)
            Scheme.DrawImage(MainImage, new Rectangle((ClientSize.Width - k) / 2 - (SchemeSize.X * (ClientSize.Height - k) / SchemeSize.Y ) / 2,
                5, SchemeSize.X * (ClientSize.Height - k) / SchemeSize.Y, ClientSize.Height - k),
                new Rectangle(0, 0, SchemeSize.X, SchemeSize.Y), GraphicsUnit.Pixel);
            else Scheme.DrawImage(MainImage, new Rectangle(10, (ClientSize.Height - k) / 2 - (SchemeSize.Y * (ClientSize.Width - k) / SchemeSize.X) / 2,
                    ClientSize.Width - k, SchemeSize.Y * (ClientSize.Width - k) / SchemeSize.X),
                    new Rectangle(0, 0, SchemeSize.X, SchemeSize.Y), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Вернуть нормальный вид
        /// </summary>
        private void DrawOriginalSheme(object sender, EventArgs e)
        {
            SPicture.Size = new System.Drawing.Size(SchemeSize.X, SchemeSize.Y);
            SPicture.Image = MainImage;
            Scheme = Graphics.FromImage(MainImage);
        }

        #region Кнопки прокрутки визуализации
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            H--;
            if (H <= -1) H = -1;
            PaintScheme(H);
            SPicture.Image = MainImage;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            H = -1;
            PaintScheme(H);
            SPicture.Image = MainImage;
            Timer.Enabled = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            H++;
            if (H > T.VertexCount) H = T.VertexCount + 1;
            PaintScheme(H);
            SPicture.Image = MainImage;
        }
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            toolStripButton4_Click(toolStripButton4, null);
            if (H == T.VertexCount + 1) Timer.Enabled = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) toolStripButton4_Click(toolStripButton4, null);
            if (e.KeyCode == Keys.Left) toolStripButton2_Click(toolStripButton2, null);
            if (e.KeyCode == Keys.Space) toolStripButton3_Click(toolStripButton3, null);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Timer.Enabled = !Timer.Enabled;
        }
    }
    //////////////////////////////////////////////////////////////////////////
#region 

    public class CommandsCode
    {
        public const int
        PushFalse = 0,
        PushTrue = 1,
        PushVariable = 2, // После данной команды идет порядковый номер переменной
        OperationNot = 3,
        OperationAnd = 4,
        OperationOr = 5,
        OperationXor = 6,
        OperationImplication = 7,
        OperationSheffer = 8,
        OperationPirse = 9;
    }

    public static class Syntax
    {
        /// <summary>
        /// Регулярные выражения для переменной и операции
        /// </summary>
        public static string Variable = @"^([a-zA-Z]+\d*)+";
        public static string Operation = @"^[~!\+\*^>\|]$";

        #region Правильные имена функций
        public static string[] FunctionNames =
        {
/*0         Formula*/"ConvertToDual",/*(Formula)*/           
/*1         Formula*/"ConvertToSDNF",/*(Formula)*/
/*2         Formula*/"ConvertToSCNF",/*(Formula)*/
/*3         Formula*/"ConvertToJegalkinPoly",/*(Formula)*/
/*4         Formula*/"ConvertToBasis",/*(BasisName,Formula)*/
/*5      True/False*/"CheckBelongTZero",/*(Formula)*/
/*6      True/False*/"CheckBelongTOne",/*(Formula)*/
/*7      True/False*/"CheckBelongS",/*(Formula)*/
/*8      True/False*/"CheckBelongM",/*(Formula)*/
/*9      True/False*/"CheckBelongL",/*(Formula)*/
/*10     True/False*/"CheckFullSystem",/*(Formula1,Formula2,...)*/
/*11     True/False*/"CheckBasis",/*(Formula1,Formula2,...)*/
/*12    {x1,x2,...}*/"GetFictitiousVars",/*(Formula)*/
/*13  {0/1,0/1,...}*/"GetFormulaVector",/*(Formula)*/
/*14               */"GetTruthTable",/*(Formula)*/
/*15               */"GetScheme",/*(Formula)*/
/*16            0/1*/"CalcOnVector",/*(Formula,0/1,0/1,...)*/
/*17     True/False*/"CreateBasis",/*(BasisName,Formula1,Formula2,...)*/
/*18        Formula*/"MinimizeQuine"/*(Formula)*/
        };
        #endregion

        /// <summary>
        /// Проверка строки на корректный идентификатор
        /// </summary>
        public static bool IsVariable(string S)
        {
            return Regex.IsMatch(S, Variable, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Проверка строки на корректную операцию
        /// </summary>
        public static bool IsOperation(string S)
        {
            return Regex.IsMatch(S, Operation);
        }

        /// <summary>
        /// Проверка символа на операнд из скриптовой строки
        /// </summary>
        public static bool IsOperand(char C)
        {
            return (C == '$' || C == '%' || C == '0' || C == '1');
        }

        /// <summary>
        /// Проверка строки на корректный идентификатор функции
        /// </summary>
        public static bool IsFunction(string S)
        {
            bool result = false;
            for (int i = 0; i < FunctionNames.Length; i++)
            {
                if (S == FunctionNames[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Проверка строки на корректный идентификатор функции
        /// с переменным числом параметров
        /// </summary>
        public static bool IsFuncWithManyParameters(string S)
        {
            return (S == FunctionNames[4] ||
                    S == FunctionNames[10] ||
                    S == FunctionNames[11] ||
                    S == FunctionNames[16] ||
                    S == FunctionNames[17]) ? true : false;
        }

        /// <summary>
        /// Проверка идентификатора на функцию, возвращающую формулы
        /// </summary>
        public static bool IsFuncCanUseInExpression(string S)
        {
            return (S == FunctionNames[0] ||
                    S == FunctionNames[1] ||
                    S == FunctionNames[2] ||
                    S == FunctionNames[3] ||
                    S == FunctionNames[4]) ? true : false;
        }

        /// <summary>
        /// Проверка корректности скобок
        /// </summary>
        public static bool BracketsCheck(string S)
        {
            Stack<char> D = new Stack<char>();
            for (int i = 0; i < S.Length; i++)
            {
                if (S[i] == '(')
                {
                    D.Push('(');
                }
                else if (S[i] == ')')
                {
                    if (D.Count == 0) return false;
                    if (D.Pop() != '(') return false;
                }
                else continue;
            }
            if (D.Count != 0) return false;
            return true;
        }

        public static int GetPriority(char C)
        {
            if (C == '(') return 0;
            else if (C == '*') return 1;
            else if (C == '+') return 2;
            else if (C == '^') return 3;
            else if (C == '>') return 4;
            else if (C == '|') return 5;
            else if (C == '~') return 6;
            else return -1;
        }
    }

    public class Formula
    {
        /// <summary>
        /// Формула в текстовой записи, обновляется при вызове функции СonvertToString
        /// </summary>
        string FormulaString;

        /// <summary>
        /// Стек команд
        /// </summary>
        public List<int> Commands;

        /// <summary>
        /// Количество переменных в формуле
        /// </summary>
        public int VariableCount
        {
            get
            {
                return VariableName.Count;
            }
        }

        /// <summary>
        /// Список имен переменных
        /// </summary>
        public List<string> VariableName;

        /// <summary>
        /// Стек для рассчета формулы
        /// </summary>
        Stack<bool> CalcStack;

        /// <summary>
        /// Конструкторы
        /// </summary>
        public Formula(string S = "")
        {
            Commands = new List<int>(30);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
            FormulaString = S;
            string O = "";
            Formula F = this;
            Calc.CalcExpression(S, ref O, ref F);
        }

        public Formula(ref Formula F)
        {
            Commands = new List<int>(30);
            for (int i = 0; i < F.Commands.Count; i++) Commands.Add(F.Commands[i]);
            VariableName = new List<string>(5);
            CalcStack = new Stack<bool>(20);
            FormulaString = F.FormulaString;
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
                if (Commands[i] == CommandsCode.PushFalse)
                {
                    CalcStack.Push(false);
                }
                else if (Commands[i] == CommandsCode.PushTrue)
                {
                    CalcStack.Push(true);
                }
                else if (Commands[i] == CommandsCode.PushVariable)
                {
                    // После данной команды идет порядковый номер переменной
                    CalcStack.Push(Values[Commands[i + 1]]);
                    i++;
                }
                else if (Commands[i] == CommandsCode.OperationNot)
                {
                    t1 = !CalcStack.Pop();
                    CalcStack.Push(t1);
                }
                else if (Commands[i] == CommandsCode.OperationAnd)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 && t2);
                }
                else if (Commands[i] == CommandsCode.OperationOr)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 || t2);
                }
                else if (Commands[i] == CommandsCode.OperationXor)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!t1 && t2 || t1 && !t2);
                }
                else if (Commands[i] == CommandsCode.OperationSheffer)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!(t1 && t2));
                }
                else if (Commands[i] == CommandsCode.OperationPirse)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(!(t1 || t2));
                }
                else if (Commands[i] == CommandsCode.OperationImplication)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    CalcStack.Push(t1 || !t2);
                }
            }
            return CalcStack.Pop();
        }

        /// <summary>
        /// Парсинг строки в формулу
        /// </summary>
        public bool CreateFromString(string S)
        {
            string O = "";
            Formula F = this;
            return Calc.CalcExpression(S, ref O, ref F);
        }

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

        /// <summary>
        /// Конвертирование представления формулы в строку
        /// </summary>
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
                else if (Commands[i] == CommandsCode.OperationPirse)
                {
                    t1 = CalcStack.Pop();
                    t2 = CalcStack.Pop();
                    if (t1.Priority >= Syntax.GetPriority('~')) t1.Data = "(" + t1.Data + ")";
                    if (t2.Priority >= Syntax.GetPriority('~')) t2.Data = "(" + t2.Data + ")";
                    t1.Data = t2.Data + "~" + t1.Data; t1.Priority = Syntax.GetPriority('~');
                    CalcStack.Push(t1);
                }
            }
            return FormulaString = CalcStack.Pop().Data;
        }

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

        public string GetString
        {
            get
            {
                return FormulaString;
            }
        }
    }

    public static class Parse
    {
        /// <summary>
        /// Удаляет пробельные символы в строке и комментарии
        /// </summary>
        public static void DeleteSpace(ref string S)
        {
            S = Regex.Replace(S, @"/{2}.*\n", "");
            if (S.IndexOf("//") != -1) S = S.Remove(S.IndexOf("//"));
            for (int i = 1; i < S.Length - 1; i++)
            {
                if (S[i] == ' ' || S[i] == '\n' || S[i] == '\r')
                {
                    if (!(Char.IsLetterOrDigit(Convert.ToChar(S[i - 1])) && Char.IsLetterOrDigit(Convert.ToChar(S[i + 1]))))
                    {
                        S = S.Remove(i, 1);
                        i--;
                    }
                }
            }
            // еще пробелы могут остаться в конце и в начале строки
            if (S[0] == ' ') S = S.Remove(0, 1);
            if (S[S.Length - 1] == ' ') S = S.Remove(S.Length - 1, 1);
        }

        /// <summary>
        /// Получает первый идентификатор из строки
        /// </summary>
        public static string GetIdentificator(string S)
        {
            Match P = Regex.Match(S, Syntax.Variable);
            return S.Substring(P.Index, P.Length);
        }

        /// <summary>
        /// Для текущей скобки '(' на ходит парную
        /// </summary>
        public static int GetNextBracket(string S, int h)
        {
            if (S[h] != '(') return -1;
            int Deep = 0;
            for (int i = h + 1; i < S.Length; i++)
            {
                if (S[i] == ')')
                {
                    if (Deep == 0) return i;
                    else Deep--;
                }
                if (S[i] == '(') Deep++;
            }
            return -1;
        }
    }

    public static class Calc
    {
        /// <summary>
        /// Класс для обслуживания алгоритма парсинга формулы
        /// </summary>
        public class SpecialSymbol
        {
            public char Symbol;
            public int Number;
            public bool IsInverse;
            public SpecialSymbol(char S, int N, bool I)
            {
                Symbol = S;
                Number = N;
                IsInverse = I;
            }
        }

        /// <summary>
        /// Ссылка на список переменных
        /// </summary>
        //static Variables V;

        // Функция принимает введенную строку, проводит нужные действия
        // (добаляет новые переменные, и т.д.) и формирует отчет: ответ или ошибка, 
        // в форме строки которую впоследствии нужно записать в OutBox

        /// <summary>
        /// Инициализация класса, для передачи ссылки на список переменных в программе
        /// </summary>
        //         public static void SetVariableReference(ref Variables Var)
        //         {
        //             V = Var;
        //         }

        /// <summary>
        /// Рассчитывает строку, формируя переменные
        /// </summary>
        /// <param name="S">Входная строка</param>
        /// <param name="OutString">Выходная строка</param>
        /// <param name="Ans">Входная функция, для дальнейшего заполнения</param>
        /// <returns>Корректность выполнения функции</returns>
        public static bool CalcExpression(string S, ref string OutString, ref Formula Ans)
        {
            //удаляем пробелы
            Parse.DeleteSpace(ref S);

            if (S.IndexOf('=') != -1)
            {
                #region <строка> ::= <переменная> = <выражение>
                string VariableName = S.Substring(0, S.IndexOf('=')),
                        NewS = S.Substring(S.IndexOf('=') + 1);
                if (Syntax.IsVariable(VariableName) == false)
                {
                    //error
                    OutString = "Неправильный синтаксис идентификатора [" + VariableName + "]";
                    return false;
                };
                if (NewS == "")
                {
                    //error
                    OutString = "Пустое выражение";
                    return false;
                };

                //теперь считаем строку после '=' и рассчитываем ее
                if (CalcExpression(NewS, ref OutString, ref Ans) == false) return false;

                //присваиваем переменной результат
                //V.Set(VariableName, Ans);

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








                    if (MainFunctionName == Syntax.FunctionNames[4])
                    {
                        /*4         Formula ConvertToBasis(BasisName,Formula)*/
                    }
                    else if (MainFunctionName == Syntax.FunctionNames[16])
                    {
                        /*16            0/1"CalcOnVector(0/1,0/1,...)*/
                    }

                    //теперь нужно получить формулу в командном представлении для каждого аргумента
                    Formula[] CalculatedArguments = new Formula[Arguments.Length];
                    for (int i = 0; i < Arguments.Length; i++)
                    {
                        CalculatedArguments[i] = new Formula();
                        if (CalcSimplyString(Arguments[i], ref OutString, ref CalculatedArguments[i]) == false) return false;
                    }

                    if (MainFunctionName == Syntax.FunctionNames[10])
                    {
                        /*10     True/False"CheckFullSystem(Formula1,Formula2,...)*/
                    }
                    else if (MainFunctionName == Syntax.FunctionNames[11])
                    {
                        /*11     True/False"CheckBasis(Formula1,Formula2,...)*/
                    }

                    else if (MainFunctionName == Syntax.FunctionNames[17])
                    {
                        /*17     True/False"CreateBasis(BasisName,Formula1,Formula2,...)*/
                    }

                    return true;
                    #endregion
                }
                else
                {
                    #region <выражение> ::= <простое выражение[со скобками и функциями с 1 параметром]>
                    return CalcSimplyString(S, ref OutString, ref Ans);
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// Рассчет строки, состоящей из идентификаторов переменных, скобок и констант 
        /// Для корректности на вход подается пустая формула
        /// </summary>
        public static bool CalcSimplyString(string S, ref string OutString, ref Formula Ans)
        {
            #region Переменные
            List<SpecialSymbol> Symbols = new List<SpecialSymbol>(); //список из символов для упрощенного анализа
            string Identificator;
            string SyntaxScript = "";
            bool MeetNot = false;//встретили операцию !
            #endregion

            #region Формирование строк для алгоритма

            // унарный оператор может применяться несколько раз
            while (S.IndexOf("!!") != -1) S = S.Replace("!!", "");

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
                        //bool SingleFunction = false;

                        // Если встретили функцию, то есть 4 вида функций, которые могут встретиться в
                        // простом выражении и которые возвращают формулу
                        if (Syntax.IsFuncCanUseInExpression(Identificator) == false)
                        {
                            // если это функция которая возвращает НЕ формулу, то ее идентификатор
                            // должен идти с начала строки. иначе - ошибка

                            // так как функция на первом месте то последнее вхождение это 0й символ
                            if (S.LastIndexOf(Identificator) == 0)
                            {
                                // ставим флаг обрабатываем функцию
                                //SingleFunction = true;
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
                        if (CalcSimplyString(InherentlyString, ref OutString, ref InherentlyFormula) == false) return false;
                        S = S.Remove(startf, endf - startf + 1);

                        //
                        // применяем функцию для полученного внутреннего представления аргумента
                        //

                        Formula T = new Formula();
                        //                         if (Identificator == Syntax.FunctionNames[0]) T = FConvert.ToDual(ref InherentlyFormula);
                        //                         else if (Identificator == Syntax.FunctionNames[1]) T = FConvert.ToSDNF(ref InherentlyFormula);
                        //                         else if (Identificator == Syntax.FunctionNames[2]) T = FConvert.ToSCNF(ref InherentlyFormula);
                        //                         else if (Identificator == Syntax.FunctionNames[3]) T = FConvert.ToJegalkinPoly(ref InherentlyFormula);
                        //                         else if (Identificator == Syntax.FunctionNames[18]) T = FMinimize.MinimizeQuine(ref InherentlyFormula);

                        //                         else if (SingleFunction == true)
                        //                         {
                        //                             if (Identificator == Syntax.FunctionNames[14]) { FGet.GetTruthTable(ref InherentlyFormula); OutString = "#"; }
                        //                             else if (Identificator == Syntax.FunctionNames[15]) { FGet.GetScheme(ref InherentlyFormula); OutString = "#"; }
                        //                             else if (Identificator == Syntax.FunctionNames[5]) OutString = FCheck.BelongTZero(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[6]) OutString = FCheck.BelongTOne(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[7]) OutString = FCheck.BelongS(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[8]) OutString = FCheck.BelongM(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[9]) OutString = FCheck.BelongL(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[12]) OutString = FGet.GetFictitiousVars(ref InherentlyFormula);
                        //                             else if (Identificator == Syntax.FunctionNames[13]) OutString = FGet.GetFormulaStringVector(ref InherentlyFormula);
                        //                             return true;
                        /*                        }*/

                        S = S.Insert(startf, "(" + T.ConvertToString() + ")");

                        //теперь нужно заного попасть на этот символ
                        i--;
                        continue;
                    }
                    #endregion

                    #region Проверим идентификатор на формулу из списка переменных
                    // сначала проверим, не является ли обнаруженный идентификатор
                    // глобальной переменной - формулой
                    //if ( !V.IsExist(Identificator) )
                    //{
                    //    S = S.Remove(i, Identificator.Length);
                    //    S = S.Insert(i, "(" /*+ V[Identificator].ConvertToString()*/ + ")");
                    //    //теперь нужно заного попасть на этот символ
                    //    i--;
                    //    continue;
                    //}
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
            }

            // если строка пустая, функция тождественна 0
            else if (Symbols.Count == 0)
            {
                AddCommands(new SpecialSymbol('0', 0, false), ref Ans);
            }
            #endregion

            #region Упрощения выражения основанные на аксиомах

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
            return true;
        }

        /// <summary>
        /// Добавляет команды в формулу, по данному ключу
        /// </summary>
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
    }

    static class MathFont
    {
        public static PrivateFontCollection CourierMathFont;
    }
}
#endregion
