using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using UrielGuy.SyntaxHighlightingTextBox;
using BinaryCalc.Functions;
using System.Diagnostics;
using System.Xml;

namespace BinaryCalc
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Форма с просмотром переменных
        /// </summary>
        VariablesForms VariablesNoteForm;

        /// <summary>
        /// Список всех тетрадей
        /// </summary>
        public List<Note> NoteCollection;

        public MainForm()
        {
            InitializeComponent();
            LoadFont();// грузим математический шрифт

            // пишем имя главной формы
            Text = ProjectName.Name
                + ' ' + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()
                + '.' + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();

            // записываем версию при первом запуске после компиляции
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(System.IO.File.ReadAllText("versions.xml"));
            XmlNode N = Doc.GetElementsByTagName("Version")[0];
            if (N.Attributes[0].InnerText == "")
            {
                N.Attributes[0].InnerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
                N.Attributes[1].InnerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                N.Attributes[2].InnerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
                N.Attributes[3].InnerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();
            }
            Doc.Save("versions.xml"); 

            // устанавливаем директорию и фильтр файлов
            OpenFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Notes";
            SaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Notes";
            SaveFileDialog.Filter = OpenFileDialog.Filter = "XML " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString() 
                + " Note|*.xml";

            // создаем форму с переменными
            VariablesNoteForm = new VariablesForms(this);
            VariablesNoteForm.MdiParent = this;

            NoteCollection = new List<Note>(2);

            #region добавляем новый документ либо открываем последний сохраненный
            if (Properties.Settings.Default.OpenLastSavedNote == true)
            {
                if (System.IO.File.Exists(Properties.Settings.Default.LastSavedNote))
                {

                    string Name = Properties.Settings.Default.LastSavedNote;
                    Note O = new Note(ref VariablesNoteForm, System.IO.Path.GetFileNameWithoutExtension(Name),
                    SystemInformation.VirtualScreen.Width - 280,
                    SystemInformation.VirtualScreen.Height - 135, false, this);
                    NoteCollection.Add(O);
                    if (FormulaSpaceLoader.LoadFromFile(ref Name, NoteCollection[NoteCollection.Count - 1].BinarySpace, VariablesNoteForm) == false)
                    {
                        NoteCollection[NoteCollection.Count - 1].Dispose();
                        NoteCollection.RemoveAt(NoteCollection.Count - 1);
                        return;
                    }
                    OpenFileDialog.FileName = Name;
                    O.Show();
                }
                else NoteCollection.Add(new Note(ref VariablesNoteForm, "",
                    SystemInformation.VirtualScreen.Width - 280,
                    SystemInformation.VirtualScreen.Height - 135, true, this));
            }
            else if (Properties.Settings.Default.OpenNewNote == true)
                NoteCollection.Add(new Note(ref VariablesNoteForm, "",
                    SystemInformation.VirtualScreen.Width - 280,
                    SystemInformation.VirtualScreen.Height - 135, true, this));
            #endregion

            // инициализируем вывод таблиц и графиков в родительскую форму
            FGet.Initialize(this);

            // показываем окно переменных
            varsToolStripMenuItem_Click(varsToolStripMenuItem, null);
            VariablesNoteForm.Location = (Properties.Settings.Default.SaveWindowsLocation == true) ?
                Properties.Settings.Default.VarsWin : new Point(SystemInformation.VirtualScreen.Width - 275, 0);
            VariablesNoteForm.Height = SystemInformation.VirtualScreen.Height - 135;
#region 
#if DEBUG       
//             DEBUGaddline(@"GetScheme((!x1 + x2 + x3 + x4 + x5 + x6) *
// (x1 + !x2^x3 + x3>x4 + x5 + x6) *
// (x1 + x2 + !(!x3 + x4) + x5 + x6) *
// (x1 + x2 + x3 + !x4 + x5 + x6) *
// (x1 + x2 + x3 + x4 + !x5 + x6) *
// (x1 + x2 + x3 + x4 + !x5 + x6) *
// (x1 + x2 + x3 + x4 + x5 + !x6))");
//            DEBUGaddline("CreateFromVector(newfunction,0,1,1,0)");
//            DEBUGaddline("CalcOnVector(x^y,1,1)");
//            DEBUGaddline("GetFormulaVector(!x1*x2*!x3 + x1*!x2*!x3 + x1*!x2*x3 + x1*x2*!x3 + x1*x2*x3) // исходная");
//            DEBUGaddline("GetFormulaVector(x1  + x2*!x3) // сокращенная");
//            DEBUGaddline("MinimizeQuine(!a*!b*!c*d + !a*b*!c*d + a*!b*!c*d + !a*b*c*d + a*!b*c*d + !a*!b*c*d)");
//            DEBUGaddline("MinimizeQuine(!x1*x2*!x3+x1*!x2*!x3+x1*!x2*x3+x1*x2*!x3+x1*x2*x3)");
//            DEBUGaddline("w = x1 > x2^x3 | x1");
//             DEBUGaddline("ConvertToDual(x^y^z)");
//             DEBUGaddline("ConvertToSDNF(x^y^z)");
//             DEBUGaddline("ConvertToSCNF(x^y^z)");
//             DEBUGaddline("ConvertToJegalkinPoly(x1+x2)");
//             DEBUGaddline("CheckBelongTZero(x1+x2)");
//             DEBUGaddline("CheckBelongTOne(x1+x2)");
//             DEBUGaddline("CheckBelongS(x1+x2)");
//             DEBUGaddline("CheckBelongM(x1+x2)");
//             DEBUGaddline("CheckBelongL(x1+x2)");
//             DEBUGaddline("GetFictitiousVars(x1*0+x2)");
//             DEBUGaddline("CheckFullSystem(x|y)");
//             DEBUGaddline("CheckFullSystem(!x, x * y)");
//             DEBUGaddline("CheckBasis(!x, x+y, x*y)");
//             DEBUGaddline("CheckBasis(x * y, x ^ y)");
            DEBUGaddline("func = x^y;\nMinimizeQuine(func);\nCheckFullSystem(func,1,x*y);");
#endif
#endregion
            // максимизируем форму
            //WindowState = FormWindowState.Maximized;
        }

#if DEBUG
        int nh = 0;
        public void DEBUGaddline(string s)
        {
            NoteCollection[0].BinarySpace.FSpaceList.AddBack();
            NoteCollection[0].BinarySpace.FSpaceList[nh].TextInput = s;
            NoteCollection[0].BinarySpace.FSpaceList[nh++].TextInput += "";
        }
#endif

        private void LoadFont()
        {
            MathFont.CourierMathFont = new PrivateFontCollection();
            if (System.IO.File.Exists(Application.StartupPath + @"\font\courier_math.ttf") == false)
            {
                MessageBox.Show("Отсутствует шрифт \\font\\courier_math.ttf","Ошибка");
                Environment.Exit(0);
            }
            MathFont.CourierMathFont.AddFontFile(Application.StartupPath + @"\font\courier_math.ttf");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ShowFormulaTable(ref Formula F)
        {
            if (F.VariableCount > 9)
            {
                MessageBox.Show(
@"На 10 и более переменных количество различных наборов
более 1024. Работа с таблицой будет затруднена. 
Если вы хотите увидеть вектор значений функции, используйте
функцию GetFormulaVector", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TableForm TableForm = new TableForm(F);
            TableForm.MdiParent = this;
            TableForm.Text = "F = " + F.ConvertToString();
            TableForm.Show();
        }

        public void ShowScheme(ref Formula F)
        {
            SchemeForm SForm = new SchemeForm(ref F, this);
            SForm.MdiParent = this;
            SForm.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NoteCollection.Add(new Note(ref VariablesNoteForm, "", 0, 0, true, this));
        }

        private void AuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Author A = new Author();
            A.AName.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
            A.AText.Text = "Описание:\tПрограммный комплекс\nдля работы с булевой алгеброй\n"+
"Версия:\t"+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()+'.'+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString()+'\n'+
"Сборка:\t" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString() + '.' + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString() + '\n' +
"Автор:\tЛарцов Иван\nГруппа: 8212";
            A.ShowDialog();
        }

        private void varsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (varsToolStripMenuItem.Checked == false)
            {
                VariablesNoteForm.Show();
                varsToolStripMenuItem.Checked = true;
            }
            else
            {
                VariablesNoteForm.Hide();
                varsToolStripMenuItem.Checked = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // сохраняем расположение дополнительных окон
            Properties.Settings.Default.VarsWin = VariablesNoteForm.Location;
            Properties.Settings.Default.Save();

            // сначала спросить о сохранении всех документов
            if (NoteCollection.Count > 0)
            {
                DialogResult v = MessageBox.Show("На форме находятся открытые тетради. Выйти?", this.Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (v == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else if (v == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else Environment.Exit(0);
        }

        public void opennewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog.ShowDialog();
            string Name = OpenFileDialog.FileName;
            if (IsOpening(System.IO.Path.GetFileNameWithoutExtension(Name)))
            {
                MessageBox.Show("Тетрадь с таким именем уже открыта", "Ошибка");
                return;
            }
            if (OpenFileDialog.FileName != "")
            {
                Note O = new Note(ref VariablesNoteForm, System.IO.Path.GetFileNameWithoutExtension(Name), 0, 0, false, this);
                NoteCollection.Add(O);
                if (FormulaSpaceLoader.LoadFromFile(ref Name, NoteCollection[NoteCollection.Count - 1].BinarySpace, VariablesNoteForm) == false)
                {
                    NoteCollection[NoteCollection.Count - 1].Dispose();
                    NoteCollection.RemoveAt(NoteCollection.Count - 1);
                    return;
                }
                OpenFileDialog.FileName = Name;
                O.Show();
            }
            else MessageBox.Show("Пустое имя файла", "Ошибка");
        }

        bool IsOpening(string s)
        {
            foreach (Note x in NoteCollection)
                if (x.Text == s) return true;
            return false;
        }

        private void opencurrentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog.ShowDialog();
            string Name = OpenFileDialog.FileName;
            if (IsOpening(System.IO.Path.GetFileNameWithoutExtension(Name)))
            {
                MessageBox.Show("Тетрадь с таким именем уже открыта", "Ошибка");
                return;
            }
            if (Name != "")
            {
                Note D = SelectedNote.Ref;
                VariablesNoteForm.DeleteVarSpace(D.BinarySpace.FVariables.VariablesSpaceName);
                D.BinarySpace.FVariables = new Variables(ref VariablesNoteForm, System.IO.Path.GetFileNameWithoutExtension(Name), D.BinarySpace);
                VariablesNoteForm.AddNewVarSpace(System.IO.Path.GetFileNameWithoutExtension(Name), ref D.BinarySpace.FVariables);
                D.Text = System.IO.Path.GetFileNameWithoutExtension(Name);
                FormulaSpaceLoader.LoadFromFile(ref Name, D.BinarySpace, VariablesNoteForm);
            }
            else MessageBox.Show("Пустое имя файла", "Ошибка");
        }

        public void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            if (NoteCollection.Count == 0) { MessageBox.Show("Нет открытых тетрадей", "Ошибка"); return; } 
            SaveFileDialog.ShowDialog();
            if (SaveFileDialog.FileName != "")
            {
                FormulaSpaceSaver.SaveToFile(SaveFileDialog.FileName, SelectedNote.Ref.BinarySpace);
                Properties.Settings.Default.LastSavedNote = SaveFileDialog.FileName;
                Properties.Settings.Default.Save();
                //MessageBox.Show("saved");
            }
            else MessageBox.Show("Пустое имя файла. Документ не сохранен.", "Ошибка");
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options A = new Options();
            A.ShowDialog();
        }

        private void справкаToolStripButton_Click(object sender, EventArgs e)
        {
            string sh = "help.pdf";
            if (File.Exists(sh)) Process.Start(sh);
            else MessageBox.Show("Файл справки отсутствует","Ошибка");
        }

        #region AddTextToSelectedInBox
        void AddTextToSelectedInBox(string s)
        {
            int t = SelectedInBox.Ref.SelectionStart;
            SelectedInBox.Ref.Text = SelectedInBox.Ref.Text.Insert(t, s);
            SelectedInBox.Ref.SelectionStart = t + s.Length;
            if (s[s.Length - 1] == ')') SelectedInBox.Ref.SelectionStart--;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("!");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("*");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("+");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("^");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox(">");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("|");
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("~");
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("1");
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("0");
        }

        private void toDualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("ConvertToDual()");
        }

        private void toSDNFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("ConvertToSDNF()");
        }

        private void toSCNFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("ConvertToSCNF()");
        }

        private void toJegalkinPolyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("ConvertToJegalkinPoly()");
        }

        private void belongT0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBelongTZero()");
        }

        private void belongT1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBelongTOne()");
        }

        private void belongSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBelongS()");
        }

        private void belongMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBelongM()");
        }

        private void belongLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBelongL()");
        }

        private void fullSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckFullSystem()");
        }

        private void basisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CheckBasis()");
        }

        private void fictitiousVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("GetFictitiousVars()");
        }

        private void formulaVectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("GetFormulaVector()");
        }

        private void truthTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("GetTruthTable()");
        }

        private void schemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("GetScheme()");
        }

        private void createFromVectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("CreateFromVector()");
        }

        private void minimizeQuineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTextToSelectedInBox("MinimizeQuine()");
        }
 #endregion

        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("update.exe");
        }
    }

    /// <summary>
    /// Класс для математического шрифта
    /// </summary>
    static class MathFont
    {
        public static PrivateFontCollection CourierMathFont;
    }

    static class ProjectName
    {
        public const string Name = "TableCloth Work";
    }
}
