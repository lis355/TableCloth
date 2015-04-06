using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TableClothGUI
{
    internal partial class CommandBox : UserControl
    {
        public CommandBox()
        {
            InitializeComponent();
        }

        public override string Text
        {
            set
            {
                myText.Clear();
                for (int i = 0; i < value.Length; i++)
                    myText.Add(new Letter(value[i], GUIVariables.TextStdColor));
                CursorPos = value.Length;
                mySText = value;
                /**/TCSyntaxHighlighter.LightAllText(myText, Text);
                ReSize();
            }
            get
            {
                return mySText;
            }
        }

        public int TextLength
        {
            get { return mySText.Length; }
        }

        public void Initialize(Timer T, ExternalReSize ParentExReSize, int W = 0)
        {
            SuspendLayout();

            ParentTimer = T;
            TimerEvent = new EventHandler(Timer_Tick);

            ExReSize = ParentExReSize;

            Cursor = System.Windows.Forms.Cursors.IBeam;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            GetFontSize();

            StartSelection = EndSelection = -1;

            CursorPos = 0;

            Height = (int)(CommandBoxFontSize.Height * GUIVariables.FirstLineCoefficient);

            Width = (W == 0) ? Height * 5 : W;

            // первоначальное построение графики
            InitializeGraphics(false);

            myGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            ReDraw();

            ResumeLayout();
        }

        private void InitializeGraphics(bool isReDraw = true)
        {
            BackgroundImage = new Bitmap(Width, Height);
            myGraphics = Graphics.FromImage(BackgroundImage);

            GetFontSize();
            GetSymbolCount();

            if (isReDraw) ReDraw();
        }

        // Определяет размеры текущего шрифта, так же заполняет переменные как количество символов в строке и столбце
        private void GetFontSize()
        {
            CommandBoxFontSize = new Size(TextRenderer.MeasureText("AB", CommandBoxFont).Width -
                TextRenderer.MeasureText("A", CommandBoxFont).Width,
                TextRenderer.MeasureText("A", CommandBoxFont).Height);
        }

        private void GetSymbolCount()
        {
            SymbolsInLine = Width / CommandBoxFontSize.Width - 1;
            SymbolsInColumn = Height / CommandBoxFontSize.Height;
            SymbolsMaximum = SymbolsInLine * SymbolsInColumn;
        }

        private void ReDraw()
        {
            // чистим все ранее нарисованное
            myGraphics.FillRectangle(GUIVariables.BackgroundBrush, 0, 0, Width, Height);

            // узнаем, есть ли выделение или нет
            bool isCorrectSelection = (EndSelection != -1);
            int tStartSelection = StartSelection, tEndSelection = EndSelection;
            if (isCorrectSelection && StartSelection > EndSelection)
            {
                int tmp = tStartSelection;
                tStartSelection = tEndSelection;
                tEndSelection = tmp;
            }

            // отображение текста посимвольно
            int  WPos = 0, HPos = 0;
            for (int i = 0; i < TextLength; i++)
            {
                if (WPos >= SymbolsInLine)
                {
                    WPos = 0;
                    HPos++;
                }
                if (isCorrectSelection && (tStartSelection <= i) && (i <= tEndSelection))
                {
                    myGraphics.DrawString(myText[i].Symbol, CommandBoxFont,
                        GUIVariables.HighlightBrush, WPos * CommandBoxFontSize.Width, HPos * CommandBoxFontSize.Height);
                }
                else
                {
                    myGraphics.DrawString(myText[i].Symbol, CommandBoxFont,
                        myText[i].Brush, WPos * CommandBoxFontSize.Width, HPos * CommandBoxFontSize.Height);
                }
                WPos++;
            }

            if (isCursor)
            {
                myGraphics.DrawString("_", CommandBoxFont, GUIVariables.TextStdBrush,
                    (CursorPos % SymbolsInLine) * CommandBoxFontSize.Width,
                    (CursorPos / SymbolsInLine) * CommandBoxFontSize.Height);
            }

            /////////////////////////
            /*if (false)
            {
                string tststr = DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() +
                         " SIL=" + SymbolsInLine.ToString() + " SIC=" + SymbolsInColumn.ToString()
                         + " s=" + tStartSelection.ToString() + "e=" + tEndSelection.ToString();
                int tstx = 0, tsty = 0;// Height - CommandBoxFontSize.Height;
                for (float i = CommandBoxFontSize.Width + 1; i <= (SymbolsInLine + 2) * CommandBoxFontSize.Width;
                    i += CommandBoxFontSize.Width) myGraphics.DrawLine(Pens.LightSalmon, i, 0, i, CommandBoxFontSize.Height * SymbolsInColumn);
                for (int i = 0; i <= SymbolsInColumn; i++)
                    myGraphics.DrawLine(Pens.LightSalmon,
                        0, i * CommandBoxFontSize.Height, (SymbolsInLine + 1) * CommandBoxFontSize.Width, i * CommandBoxFontSize.Height);
                myGraphics.DrawEllipse(Pens.Gray, 0, 0, Width, Height);
                myGraphics.FillRectangle(Brushes.Gray, tstx, tsty, TextRenderer.MeasureText(tststr, CommandBoxFont).Width, CommandBoxFontSize.Height);
                myGraphics.DrawString(tststr, CommandBoxFont, new SolidBrush(Color.DarkOrange), tstx, tsty);
            }*/
            ////////////////////////

            // нужно чтобы контрол сам обновился
            Refresh();
        }

        public void ReSize(int WidthDelta = 0)
        {
            SuspendLayout();

            Width += WidthDelta;

            GetSymbolCount();

            int LinesCount = (int)Math.Ceiling((double)TextLength / (double)SymbolsInLine);

            Height = (int)(CommandBoxFontSize.Height * GUIVariables.FirstLineCoefficient);

            if (LinesCount > 1)
                Height += CommandBoxFontSize.Height * (LinesCount - 1);
            
            InitializeGraphics();

            ExReSize();

            ResumeLayout();
        }

        public float FontSize
        {
            get
            {
                return CommandBoxFont.Size;
            }
            set
            {
                CommandBoxFont = GUIVariables.GetStdFontWithSize(value);

                // определяем новую высоту компонента
                GetFontSize();

                ReSize();
            }
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool isCorrectSelection = (EndSelection != -1);
            int tStartSelection = StartSelection, tEndSelection = EndSelection;
            if (isCorrectSelection && StartSelection > EndSelection)
            {
                int tmp = tStartSelection;
                tStartSelection = tEndSelection;
                tEndSelection = tmp;
            }

            switch (keyData)
            {
                case Keys.ShiftKey | Keys.Shift: break;
                case Keys.ControlKey | Keys.Control: break;
                //case Keys.Enter: AddSymbolWithReSizeOrReDraw('\n'); break;
                case Keys.Enter | Keys.Control: if (!ReadOnly) ExecuteCommand(Text); break;
                #region cursor change pos
                case Keys.Left:  
                {
                    if (CursorPos != 0)
                        CursorPos--;
                    ReDraw();
                    break;
                }
                case Keys.Right:
                {
                    if (CursorPos != TextLength)
                        CursorPos++;
                    ReDraw();
                    break;
                }
                case Keys.Up:
                {
                    if (CursorPos - SymbolsInLine >= 0)
                        CursorPos -= SymbolsInLine;
                    ReDraw();
                    break;
                }
                case Keys.Down:
                {
                    if (CursorPos + SymbolsInLine < TextLength)
                        CursorPos += SymbolsInLine;
                    ReDraw();
                    break;
                }
                #endregion
                #region CNTRL + SOME
                case Keys.V | Keys.Control:
                {
                    if (Clipboard.ContainsText(TextDataFormat.Text))
                    {
                        // нужно удалить если было выделение
                        if (isCorrectSelection)
                        {
                            for (int i = tStartSelection; i <= tEndSelection; i++)
                            {
                                myText.RemoveAt(tStartSelection);
                                mySText.Remove(tStartSelection, 1);
                            }
                            StartSelection = EndSelection = -1;
                        }
                        string frombuf = Clipboard.GetText();
                        for (int i = 0; i < frombuf.Length; i++)
                        {
                            myText.Insert(CursorPos, new Letter(frombuf[i], GUIVariables.TextStdColor));
                            if (CursorPos > TextLength)
                                mySText += frombuf[i].ToString();
                            else
                                mySText = mySText.Insert(CursorPos, frombuf[i].ToString());
                            CursorPos++;
                        }
                        /**/
                        TCSyntaxHighlighter.LightAllText(myText, mySText);
                        ReSize();
                    }
                    break;
                }
                case Keys.X | Keys.Control:
                {
                    if (isCorrectSelection)
                    {
                        string tobuf = "";
                        for (int i = tStartSelection; i <= tEndSelection; i++)
                        {
                            tobuf += myText[i].Symbol;
                        }
                        Clipboard.SetText(tobuf);
                        // нужно удалить
                        for (int i = tStartSelection; i <= tEndSelection; i++)
                        {
                            myText.RemoveAt(tStartSelection);
                            mySText.Remove(tStartSelection, 1);
                        }
                        CursorPos = StartSelection;
                        StartSelection = EndSelection = -1;
                        ReSize();
                    }
                    break;
                }
                case Keys.C | Keys.Control:
                {
                    if (isCorrectSelection)
                    {
                        string tobuf = "";
                        for (int i = tStartSelection; i <= tEndSelection; i++)
                            tobuf += myText[i].Symbol;
                        Clipboard.SetText(tobuf);
                    }
                    break;
                }
                case Keys.A | Keys.Control:
                {
                    if (TextLength == 0) break;
                    StartSelection = 0;
                    EndSelection = TextLength - 1;
                    ReDraw();
                    break;
                }
                #endregion
                #region backspace and delete
                case Keys.Back:
                {
                    if (0 < CursorPos && CursorPos <= TextLength)
                    {
                        CursorPos--;
                        myText.RemoveAt(CursorPos);
                        mySText = mySText.Remove(CursorPos, 1);
                        /**/
                        TCSyntaxHighlighter.LightAllText(myText, mySText);
                        if (TextLength != 0 && TextLength <= SymbolsMaximum - SymbolsInLine)
                            ReSize();
                        else
                            ReDraw();
                    }
                    break;
                }
                case Keys.Delete:
                {
                    if (0 <= CursorPos && CursorPos < TextLength)
                    {
                        myText.RemoveAt(CursorPos);
                        mySText = mySText.Remove(CursorPos, 1);
                        if (TextLength != 0 && TextLength <= SymbolsMaximum - SymbolsInLine)
                            ReSize();
                        else
                            ReDraw();
                    }
                    break;
                }
                #endregion
                case Keys.D1: AddSymbolWithReSizeOrReDraw('1'); break;
                case Keys.D2: AddSymbolWithReSizeOrReDraw('2'); break;
                case Keys.D3: AddSymbolWithReSizeOrReDraw('3'); break;
                case Keys.D4: AddSymbolWithReSizeOrReDraw('4'); break;
                case Keys.D5: AddSymbolWithReSizeOrReDraw('5'); break;
                case Keys.D6: AddSymbolWithReSizeOrReDraw('6'); break;
                case Keys.D7: AddSymbolWithReSizeOrReDraw('7'); break;
                case Keys.D8: AddSymbolWithReSizeOrReDraw('8'); break;
                case Keys.D9: AddSymbolWithReSizeOrReDraw('9'); break;
                case Keys.D0: AddSymbolWithReSizeOrReDraw('0'); break;
                case Keys.Space: AddSymbolWithReSizeOrReDraw(' '); break;
                case Keys.D1 | Keys.Shift: AddSymbolWithReSizeOrReDraw('!'); break;
                case Keys.D2 | Keys.Shift: AddSymbolWithReSizeOrReDraw('@'); break;
                case Keys.D3 | Keys.Shift: AddSymbolWithReSizeOrReDraw('#'); break;
                case Keys.D4 | Keys.Shift: AddSymbolWithReSizeOrReDraw('$'); break;
                case Keys.D5 | Keys.Shift: AddSymbolWithReSizeOrReDraw('%'); break;
                case Keys.D6 | Keys.Shift: AddSymbolWithReSizeOrReDraw('^'); break;
                case Keys.D7 | Keys.Shift: AddSymbolWithReSizeOrReDraw('&'); break;
                case Keys.D8 | Keys.Shift: AddSymbolWithReSizeOrReDraw('*'); break;
                case Keys.D9 | Keys.Shift: AddSymbolWithReSizeOrReDraw('('); break;
                case Keys.D0 | Keys.Shift: AddSymbolWithReSizeOrReDraw(')'); break;
                case Keys.Oemtilde: AddSymbolWithReSizeOrReDraw('~'); break;
                case Keys.OemMinus | Keys.Shift: AddSymbolWithReSizeOrReDraw('-'); break;
                case Keys.Oemplus: AddSymbolWithReSizeOrReDraw('='); break;
                case Keys.Oemplus | Keys.Shift: AddSymbolWithReSizeOrReDraw('+'); break;
                case Keys.OemOpenBrackets: AddSymbolWithReSizeOrReDraw('['); break;
                case Keys.Oem6: AddSymbolWithReSizeOrReDraw(']'); break;
                case Keys.Oem1: AddSymbolWithReSizeOrReDraw(';'); break;
                case Keys.Oemcomma: AddSymbolWithReSizeOrReDraw(','); break;
                case Keys.OemPeriod: AddSymbolWithReSizeOrReDraw('.'); break;
                case Keys.Oem1 | Keys.Shift: AddSymbolWithReSizeOrReDraw(':'); break;
                case Keys.Oemcomma | Keys.Shift: AddSymbolWithReSizeOrReDraw('<'); break;
                case Keys.OemPeriod | Keys.Shift: AddSymbolWithReSizeOrReDraw('>'); break;
                case Keys.OemQuestion: AddSymbolWithReSizeOrReDraw('/'); break;
                case Keys.OemQuestion | Keys.Shift: AddSymbolWithReSizeOrReDraw('?'); break;
                case Keys.Divide: AddSymbolWithReSizeOrReDraw('/'); break;
                case Keys.Multiply: AddSymbolWithReSizeOrReDraw('*'); break;
                case Keys.Subtract: AddSymbolWithReSizeOrReDraw('-'); break;
                case Keys.Add: AddSymbolWithReSizeOrReDraw('+'); break;
                default:
                {
                    //MessageBox.Show(keyData.ToString());
                    bool isShift = ((keyData & Keys.Modifiers) == Keys.Shift);
                    char c = Convert.ToChar((int)keyData % 128);
                    if (Char.IsLetter(c))
                    {
                        c = (isShift) ? Char.ToUpper(c) : Char.ToLower(c);
                        AddSymbolWithReSizeOrReDraw(c);
                    }
                    break;
                }
            }
            return true;
        }

        private void ExecuteCommand(string Text)
        {
            ParentInputSpace.ExecuteCommand(Text);
        }

        // нажатие на клавишу
        private void CommandBox_KeyDown(object sender, KeyEventArgs e)
        {   
            char c = Convert.ToChar(e.KeyValue);
            if (Char.IsLetter(c))
            {
                c = (e.Shift) ? Char.ToUpper(c) : Char.ToLower(c);
                AddSymbolWithReSizeOrReDraw(c);
            }
        }

        private void AddSymbolWithReSizeOrReDraw(char c)
        {
            if (!ReadOnly)
            {
                myText.Insert(CursorPos, new Letter(c, GUIVariables.TextStdColor));
                if (CursorPos > TextLength)
                    mySText += c.ToString();
                else
                    mySText = mySText.Insert(CursorPos, c.ToString());
                CursorPos++;
                /**/TCSyntaxHighlighter.LightAllText(myText, mySText);
                if (TextLength > SymbolsMaximum)
                    ReSize();
                else
                    ReDraw();
            }
        }

        // Активируем и деактивируем таймер при выходе/входе мыши на компонент
        private void CommandBox_Enter(object sender, EventArgs e)
        {
            ParentTimer.Tick += TimerEvent;
        }

        private void CommandBox_Leave(object sender, EventArgs e)
        {
            ParentTimer.Tick -= TimerEvent;
            isCursor = false;
            ReDraw();
        }

        // тик таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            isCursor = !isCursor;
            ReDraw();
        }

        // выделение текста
        private void CommandBox_MouseDown(object sender, MouseEventArgs e)
        {
            // определяем координаты мыши и букву
            StartSelection = EndSelection = -1;
            Point Coords = new Point(e.X / CommandBoxFontSize.Width, e.Y / CommandBoxFontSize.Height);
            if (Coords.X > SymbolsInLine) return;

            // устанавливаем курсор
            CursorPos = (Coords.X + Coords.Y * SymbolsInLine >= TextLength)
                ? TextLength : (Coords.X + Coords.Y * SymbolsInLine);
            if (Coords.X + Coords.Y * SymbolsInLine < TextLength)
            {
                StartSelection = Coords.X + Coords.Y * SymbolsInLine;
                isSelection = true;
            }
            ReDraw();
        }

        private void CommandBox_MouseMove(object sender, MouseEventArgs e)
        {
            // определяем координаты мыши и букву
            if (isSelection)
            {
                Point Coords = new Point(e.X / CommandBoxFontSize.Width, e.Y / CommandBoxFontSize.Height);
                if (Coords.X > SymbolsInLine) return;
                if (Coords.X + Coords.Y * SymbolsInLine < TextLength)
                    EndSelection = Coords.X + Coords.Y * SymbolsInLine;
                ReDraw();
            }
        }

        private void CommandBox_MouseUp(object sender, MouseEventArgs e)
        {
            isSelection = false;
        }
    }
}
