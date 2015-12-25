using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BinaryCalc
{
    public partial class SchemeForm : Form
    {
        // шрифт для рисования надписей
        Font DrawningOperationFont, DrawningArgumentFont;

        Graphics Scheme;

        SolidBrush Brush;

        Pen SBPen;

        Formula F;

        FormulaTree T;

        Point SchemeSize;

        Bitmap MainImage;

        MainForm MF;// для SavePanel

        Stack<bool> CalcStack;

        public SchemeForm(ref Formula D, MainForm M)
        {
            InitializeComponent();
            MF = M;

            // строим дерево по формуле
            F = D;
            T = new FormulaTree(ref D);
            
            // рассчитываем координаты схемы
            T.SolveTreeForScheme();

            // получаем размер схемы
            SchemeSize = new Point(T.Width, Math.Max(T.Heigh, 30 + F.VariableCount * 70));

            // инициализируем изображение и вывод схемы на него
            MainImage = new Bitmap(SchemeSize.X, SchemeSize.Y);
            Scheme = Graphics.FromImage(MainImage);

            // инициазируем графические примитивы
            DrawningOperationFont = new Font(MathFont.CourierMathFont.Families[0], 15F);
            DrawningArgumentFont = new Font(MathFont.CourierMathFont.Families[0], 10F);
            Brush = new SolidBrush(Color.Black);
            SBPen = new Pen(Color.Black);

            CalcStack = new Stack<bool>(10);

            // рисуем схему в MainImage
            PaintScheme();

            // выводим схему на экран
            NormView_Click(MaxButton, null);
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
                DrawOperator(x0 + 50, y + 48, CommandsCode.OperationNot);
                Scheme.DrawLine(SBPen, x0 + 80, y + 48, x0 + 100, y + 48);
                y += 70;
            }
            x0 = 110;
            Scheme.FillRectangle(Brush, x0 - 2, 10, 5, Math.Max(h, y) - 10);
        }

        /// <summary>
        /// Рисует прямоугольник с операцией
        /// </summary>
        void DrawOperator(int x, int y, int Code)
        {
            string codestring = "";
            switch (Code)
            {
                case CommandsCode.OperationNot: codestring = "!"; break;
                case CommandsCode.OperationAnd: codestring = "*"; break;
                case CommandsCode.OperationOr: codestring = "+"; break;
                case CommandsCode.OperationXor: codestring = "^"; break;
                case CommandsCode.OperationEquivalence: codestring = "~"; break;
                case CommandsCode.OperationSheffer: codestring = "|"; break;
                case CommandsCode.OperationImplication: codestring = ">"; break;
            }
            Scheme.DrawString(codestring, DrawningOperationFont, Brush, x, y - 22, new StringFormat());
            Scheme.DrawRectangle(SBPen, x, y - 24, 30, 48);
            Scheme.FillEllipse(Brush, x + 27, y - 3, 6, 6);
        }

        /// <summary>
        /// Рисует прямоугольный путь соединяющий 2 точки
        /// </summary>
        void DrawLine(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2) return;
            int m = 1;
            if (y1 < y2) m = -1;
            if (y1 != y2 && x1 != x2)
            {
                Scheme.DrawLine(SBPen, x1, y1, x2 - 10, y1);
                Scheme.DrawLine(SBPen, x2 - 10, y1, x2 - 10, y2 + m * 10);
                Scheme.DrawLine(SBPen, x2 - 10, y2 + m * 10, x2, y2 + m * 10);
            }
            else Scheme.DrawLine(SBPen, x1, y1, x2, y2);
        }

        /// <summary>
        /// Добавляет лист - переменная либо константа
        /// </summary>
        void DrawArgument(int x, int y, string s)
        {
            DrawText(x + 8, y - 16, s);
            Scheme.DrawLine(SBPen, x, y, x + 50, y);
        }

        /// <summary>
        /// Рисуем заключительную линию "F"
        /// </summary>
        void DrawFunc(int x, int y)
        {
            DrawText(x + 38, y - 16, "F");
            Scheme.DrawLine(SBPen, x + 2, y, x + 50, y);
            Scheme.DrawEllipse(SBPen, x + 50, y - 3, 6, 6);
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
        /// Отображаем схему
        /// </summary>
        private void PaintScheme()
        {
            // включаем сглаживание линий
            Scheme.SmoothingMode = SmoothingMode.HighQuality;

            // сбрасываем все нарисованное
            Scheme.FillRectangle(new SolidBrush(Color.White), 0, 0, SchemeSize.X, SchemeSize.Y);

            // рисуем схему
            DrawTreeRecursion(T.Root);
            DrawOperands(T.Heigh);
            DrawFunc(T.Root.Out.X, T.Root.Out.Y);
        }

        /// <summary>
        /// Функция рерурсивного рисования схемы
        /// </summary>
        void DrawTreeRecursion(Vertex V)
        {
            if (V == null) return;

            if (V.R != null)
            {
                DrawLine(V.R.Out.X, V.R.Out.Y, V.In.X, V.In.Y);
                DrawTreeRecursion(V.R);
            }

            if (V.L != null)
            {
                DrawLine(V.L.Out.X, V.L.Out.Y, V.In.X, V.In.Y);
                DrawTreeRecursion(V.L);
            }

            if (V.IsOperation == false) DrawArgument(V.In.X, V.In.Y, V.Str);
            else DrawOperator(V.In.X, V.In.Y, V.Code);

            if (V.IsOperation == false && V.Parent != null)//вершина - лист, нужно нарисовать линию от нее
                DrawLine(V.Out.X, V.Out.Y, V.Parent.In.X, V.Parent.In.Y);
        }

        /// <summary>
        /// Сохранить изображение
        /// </summary>
        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            MF.SaveFileDialog.Filter = "BMP|*.bmp";
            MF.SaveFileDialog.ShowDialog();
            if (MF.SaveFileDialog.FileName != "") SPicture.Image.Save(MF.SaveFileDialog.FileName);
            else MessageBox.Show("Пустое имя файла", "Ошибка");
            MF.SaveFileDialog.Filter = "XML " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString() 
                + " Note|*.xml";
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
        private void Preview_Click(object sender, EventArgs e)
        {
            int k = 50;

            // если размер схемы меньше размера формы
            if (SchemeSize.X < ClientSize.Width && SchemeSize.Y < ClientSize.Height) return;

            // меняем размер рисунка
            SPicture.Size = new System.Drawing.Size(ClientSize.Width - k, ClientSize.Height - k);

            // рисуем новую схему
            SPicture.Image = new Bitmap(ClientSize.Width - k, ClientSize.Height - k);
            Scheme = Graphics.FromImage(SPicture.Image);

            // делаем превью с MainImage
            if (SPicture.Size.Width > SPicture.Size.Height)
                Scheme.DrawImage(MainImage, new Rectangle((ClientSize.Width - k) / 2 - (SchemeSize.X * (ClientSize.Height - k) / SchemeSize.Y) / 2,
                    5, SchemeSize.X * (ClientSize.Height - k) / SchemeSize.Y, ClientSize.Height - k),
                    new Rectangle(0, 0, SchemeSize.X, SchemeSize.Y), GraphicsUnit.Pixel);
            else Scheme.DrawImage(MainImage, new Rectangle(10, (ClientSize.Height - k) / 2 - (SchemeSize.Y * (ClientSize.Width - k) / SchemeSize.X) / 2,
                    ClientSize.Width - k, SchemeSize.Y * (ClientSize.Width - k) / SchemeSize.X),
                    new Rectangle(0, 0, SchemeSize.X, SchemeSize.Y), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Вернуть нормальный вид
        /// </summary>
        private void NormView_Click(object sender, EventArgs e)
        {
            SPicture.Size = new System.Drawing.Size(SchemeSize.X, SchemeSize.Y);
            SPicture.Image = MainImage;
            Scheme = Graphics.FromImage(MainImage);
        }
    }
}
