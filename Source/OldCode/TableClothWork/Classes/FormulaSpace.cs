using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using UrielGuy.SyntaxHighlightingTextBox;

namespace BinaryCalc
{
    /// <summary>
    /// Класс для хранения номера текущего пространства формулы
    /// </summary>
    static public class SelectedInBox
    {
        public static UrielGuy.SyntaxHighlightingTextBox.SyntaxHighlightingTextBox Ref;
    }

    /// <summary>
    /// Класс элемента - пространства ввода, на котором находятся поля ввода и вывода, лейблы "ввод" и  "результат"
    /// и кнопки сокрыть и удалить.
    /// </summary>
    public class FormulaSpace
    {
        public System.Windows.Forms.TableLayoutPanel Space;
        System.Windows.Forms.PictureBox VvodDelete;
        System.Windows.Forms.PictureBox MinButton;
        public UrielGuy.SyntaxHighlightingTextBox.SyntaxHighlightingTextBox InBox;
        public UrielGuy.SyntaxHighlightingTextBox.SyntaxHighlightingTextBox OutBox;
        System.Windows.Forms.Label Lresult;
        System.Windows.Forms.Label Lvvod;
        BCalcProgram Parent;

        public int Number;//порядковый номер созданного пространства
        public bool Minimized;

        public FormulaSpace(int id, ref BCalcProgram P)
        {
            Number = id;
            Minimized = false;
            Parent = P;

            #region Определение элементов
            // FTable
            Space = new System.Windows.Forms.TableLayoutPanel();
            Space.ColumnCount = 3;
            Space.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            Space.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            Space.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            Space.RowCount = 2;
            Space.RowStyles.Add(new System.Windows.Forms.RowStyle());
            Space.RowStyles.Add(new System.Windows.Forms.RowStyle());
            Space.Size = new System.Drawing.Size(600, 40);
            Space.Dock = DockStyle.Top;
            Space.AutoSize = true;

            #region InBox
            InBox = new UrielGuy.SyntaxHighlightingTextBox.SyntaxHighlightingTextBox();
            InBox.BackColor = System.Drawing.Color.White;
            InBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            InBox.CaseSensitive = false;
            InBox.FilterAutoComplete = false;
            InBox.Dock = System.Windows.Forms.DockStyle.Fill;
            InBox.Font = new System.Drawing.Font(MathFont.CourierMathFont.Families[0], 11.25F);
            InBox.Margin = new System.Windows.Forms.Padding(0);
            InBox.MaxUndoRedoSteps = 5;
            InBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            InBox.Size = new System.Drawing.Size(500, 34);

            InBox.Seperators.Add(' ');  InBox.Seperators.Add('\r');
            InBox.Seperators.Add('\n'); InBox.Seperators.Add(',');
            InBox.Seperators.Add('+');  InBox.Seperators.Add('*');
            InBox.Seperators.Add('!');  InBox.Seperators.Add('=');
            InBox.Seperators.Add('^');  InBox.Seperators.Add('~');
            InBox.Seperators.Add('|');  InBox.Seperators.Add('>');
            InBox.Seperators.Add('(');  InBox.Seperators.Add(')');
            InBox.Seperators.Add(';');

            InBox.HighlightDescriptors.Add(new HighlightDescriptor("ConvertToDual",         Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("ConvertToSDNF",         Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("ConvertToSCNF",         Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("ConvertToJegalkinPoly", Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("ConvertToBasis",        Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBelongTZero",      Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBelongTOne",       Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBelongS",          Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBelongM",          Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBelongL",          Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckFullSystem",       Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CheckBasis",            Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("GetFictitiousVars",     Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("GetFormulaVector",      Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("GetTruthTable",         Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("GetScheme",             Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CalcOnVector",          Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CreateBasis",           Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("CreateFromVector",      Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("MinimizeQuine",         Color.DodgerBlue, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("0", Color.Magenta, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("1", Color.Magenta, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            InBox.HighlightDescriptors.Add(new HighlightDescriptor("//", Color.Green,  null, DescriptorType.ToEOL, DescriptorRecognition.WholeWord, false));
            #endregion
 
            #region OutBox
            OutBox = new UrielGuy.SyntaxHighlightingTextBox.SyntaxHighlightingTextBox();
            OutBox.BackColor = System.Drawing.Color.White;
            OutBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            OutBox.CaseSensitive = false;
            OutBox.FilterAutoComplete = false;
            OutBox.Dock = System.Windows.Forms.DockStyle.Fill;
            OutBox.Font = new System.Drawing.Font(MathFont.CourierMathFont.Families[0], 11.25F);
            //OutBox.Font = new System.Drawing.Font("Courier New Math", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            OutBox.Margin = new System.Windows.Forms.Padding(0);
            OutBox.MaxUndoRedoSteps = 5;
            OutBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            OutBox.Size = new System.Drawing.Size(500, 34);
            OutBox.ReadOnly = true;
            OutBox.Visible = false;

            OutBox.Seperators.Add(' '); OutBox.Seperators.Add('\r');
            OutBox.Seperators.Add('\n'); OutBox.Seperators.Add(',');
            OutBox.Seperators.Add('+'); OutBox.Seperators.Add('*');
            OutBox.Seperators.Add('!'); OutBox.Seperators.Add('=');
            OutBox.Seperators.Add('^'); OutBox.Seperators.Add('~');
            OutBox.Seperators.Add('|'); OutBox.Seperators.Add('>');
            OutBox.Seperators.Add('('); OutBox.Seperators.Add(')');
            OutBox.Seperators.Add('{'); OutBox.Seperators.Add('}');
            OutBox.Seperators.Add(';');

            OutBox.HighlightDescriptors.Add(new HighlightDescriptor("0", Color.Magenta, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            OutBox.HighlightDescriptors.Add(new HighlightDescriptor("1", Color.Magenta, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            OutBox.HighlightDescriptors.Add(new HighlightDescriptor("Ошибка", Color.Red, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            OutBox.HighlightDescriptors.Add(new HighlightDescriptor("Позиция ошибки", Color.Red, null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
            #endregion

            foreach (HighlightDescriptor x in Parent.FVariables.VarsHighlight.Values)
            {
                InBox.HighlightDescriptors.Add(x); InBox.Text += "";
                OutBox.HighlightDescriptors.Add(x); OutBox.Text += "";
            }

            #region PuctureButtons
            // VvodDelete
            VvodDelete = new System.Windows.Forms.PictureBox();
            VvodDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            VvodDelete.Image = BinaryCalc.Properties.Resources.all0;
            VvodDelete.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            VvodDelete.Size = new System.Drawing.Size(12, 12);
            VvodDelete.TabStop = false;

            // MinButton
            MinButton = new System.Windows.Forms.PictureBox();
            MinButton.Cursor = System.Windows.Forms.Cursors.Hand;
            MinButton.Image = BinaryCalc.Properties.Resources.all0;
            MinButton.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            MinButton.Size = new System.Drawing.Size(12, 12);
            MinButton.TabStop = false;
            MinButton.Visible = false;
            #endregion

            // Lvvod
            Lvvod = new System.Windows.Forms.Label();
            Lvvod.AutoSize = true;
            Lvvod.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Lvvod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            Lvvod.Size = new System.Drawing.Size(63, 14);
            Lvvod.Text = "Ввод [" + Convert.ToString(id) + "]";

            // Lresult
            Lresult = new System.Windows.Forms.Label();
            Lresult.AutoSize = true;
            Lresult.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Lresult.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            Lresult.Size = new System.Drawing.Size(70, 14);
            Lresult.Text = "Результат";
            Lresult.Visible = false;

            Space.Controls.Add(InBox, 2, 0);
            Space.Controls.Add(OutBox, 2, 1);
            Space.Controls.Add(Lresult, 1, 1);
            Space.Controls.Add(MinButton, 0, 1);
            Space.Controls.Add(VvodDelete, 0, 0);
            Space.Controls.Add(Lvvod, 1, 0);

            InBox.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDownInInput);
            InBox.TextChanged += new EventHandler(InBoxTextChanged);
            InBox.Click += new EventHandler(InBoxMouseClick);
            OutBox.TextChanged += new EventHandler(OutBoxTextChanged);
            VvodDelete.Click += new EventHandler(VvodDeleteClick);
            VvodDelete.MouseEnter += new EventHandler(VvodDeleteMouseEnter);
            VvodDelete.MouseHover += new EventHandler(VvodDeleteMouseHover);
            VvodDelete.MouseLeave += new EventHandler(VvodDeleteMouseLeave);
            MinButton.MouseEnter += new EventHandler(MinButtonMouseEnter);
            MinButton.MouseHover += new EventHandler(MinButtonMouseHover);
            MinButton.MouseLeave += new EventHandler(MinButtonMouseLeave);
            MinButton.Click += new EventHandler(Minimize);

            #endregion
        }

        /// <summary>
        /// Рассчитать введенную строку
        /// </summary>
        private void CalcInputString()
        {
            CalcResult result = Calc.CalcExpression(InBox.Text, ref Parent.FVariables);

            SecondRowVisible = true;

            if (result.Success == true)
            {
//                 if (OutString == "") TextOutput = ""; 
//                 else if (OutString == "#") { TextOutput = ""; SecondRowVisible = false; }
//                 else TextOutput = OutString;
//                 TextOutputColor = Color.Black;
            }
            else
            {
                 TextOutput = "Ошибка : " + TCErrors.Err[result.ErrorCode] + "\nПозиция ошибки : " + result.ErrorPos.ToString();
                 TextOutput += "";
            }
        }

        #region События ввода в пространстве формулы, нажатие на кнопки удаления и минимизации
        /// <summary>
        /// Нажатие на кнопку "удалить"
        /// </summary>
        private void VvodDeleteClick(object sender, EventArgs e)
        {
            Parent.FSpaceList.Delete(Number);
        }

        /// <summary>
        /// Нажатие на кнопку "свернуть"
        /// </summary>
        private void Minimize(object sender, EventArgs e)
        {
            VvodDelete.Click -= new EventHandler(VvodDeleteClick);//отменяем предыдущее
            VvodDelete.Click += new EventHandler(Maximize);
            Lvvod.Text = " ... ";
            InBox.Visible = false;
            SecondRowVisible = false;
            Minimized = true;
        }

        /// <summary>
        /// Функция для сворачивания пространства функции
        /// </summary>
        public void Min()
        {
            Minimize(VvodDelete, null);
        }

        /// <summary>
        /// Нажатие на кнопку "развернуть"
        /// </summary>
        private void Maximize(object sender, EventArgs e)
        {
            VvodDelete.Image = BinaryCalc.Properties.Resources.delete2;
            VvodDelete.Click -= new EventHandler(Maximize);
            VvodDelete.Click += new EventHandler(VvodDeleteClick);
            Lvvod.Text = "Ввод [" + Convert.ToString(Number) + "]";
            InBox.Visible = true;
            SecondRowVisible = true;
            Minimized = false;
        }

        /// <summary>
        /// Нажатие клавиши в поле ввода формулы
        /// </summary>
        private void KeyDownInInput(object sender, KeyEventArgs e)
        {
            SelectedInBox.Ref = InBox;
            //нажатие Ctrl+Shift  - рассчитать значение в ячейке
            if (e.Control && e.Shift) 
            {
                CalcInputString();
                return;
            }
        }

        /// <summary>
        /// Нажатие мыши в поле ввода формулы
        /// </summary>
        private void InBoxMouseClick(object sender, EventArgs e)
        {
            SelectedInBox.Ref = InBox;
        }
        #endregion

        #region Динамическое изменение размеров полей
        /// <summary>
        /// Динамическое изменение размеров поля ввода
        /// </summary>
        private void InBoxTextChanged(object sender, EventArgs e)
        {
            InBox.Height = (InBox.GetLineFromCharIndex(InBox.Text.Length) + 2) * InBox.Font.Height;
        }

        /// <summary>
        /// Динамическое изменение размеров поля вывода
        /// </summary>
        private void OutBoxTextChanged(object sender, EventArgs e)
        {
            OutBox.Height = (OutBox.GetLineFromCharIndex(OutBox.Text.Length) + 2) * OutBox.Font.Height;
        }
        #endregion

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Delete()
        {
            Space.Dispose();
        }

//         public void MakeText()
//         {
//             InBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(KeyDownInInput);
//             InBox.TextChanged -= new EventHandler(InBoxTextChanged);
//             //InBox.Font = new System.Drawing.Font(MathFont.CourierMathFont.Families[0], 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(104)));
//             Lvvod.Text = new string(' ', Lvvod.Text.Length);
//             InBox.ReadOnly = true;
//             InBox.ForeColor = Color.Green;
//             InBox.Update();
//             InBox.Text += "";
//             OutBox.Text = "#";
//             SecondRowVisible = false;
//         }

        /// <summary>
        /// Показать или скрыть результат вычислений
        /// </summary>
        public bool SecondRowVisible
        {
            get
            {
                return OutBox.Visible;
            }
            set
            {
                MinButton.Visible = Lresult.Visible = OutBox.Visible = value;
            }
        }

        #region Свойства - текст полей ввода и вывода 
        /// <summary>
        /// Текст поля ввода
        /// </summary>
        public string TextInput
        {
            get
            {
                return InBox.Text;           
            }
            set
            {
                InBox.Text = value;
                InBoxTextChanged(InBox,null);
                InBox.Text += "";
            }
        }
      
        /// <summary>
        /// Текст поля вывода
        /// </summary>
        public string TextOutput
        {
            get
            {
                return OutBox.Text;
            }
            set
            {
                OutBox.Text = value;
                OutBoxTextChanged(OutBox,null);
                OutBox.Text += "";
            }
        }

        public Color TextOutputColor
        {
            set
            {
                OutBox.ForeColor = value;
            }
        }
        #endregion

        #region Fading Buttons
        private void VvodDeleteMouseEnter(object sender, EventArgs e)
        {
            VvodDelete.Image = (Minimized) ? BinaryCalc.Properties.Resources.min1 : BinaryCalc.Properties.Resources.delete1;
        }

        private void VvodDeleteMouseHover(object sender, EventArgs e)
        {
            VvodDelete.Image = (Minimized) ? BinaryCalc.Properties.Resources.min2 : BinaryCalc.Properties.Resources.delete2;
        }

        private void VvodDeleteMouseLeave(object sender, EventArgs e)
        {
            VvodDelete.Image = BinaryCalc.Properties.Resources.all0;
        }

        private void MinButtonMouseEnter(object sender, EventArgs e)
        {
            MinButton.Image = BinaryCalc.Properties.Resources.max1;
        }

        private void MinButtonMouseHover(object sender, EventArgs e)
        {
            MinButton.Image = BinaryCalc.Properties.Resources.max2;
        }

        private void MinButtonMouseLeave(object sender, EventArgs e)
        {
            MinButton.Image = BinaryCalc.Properties.Resources.all0;
        }
        #endregion
    }
}
