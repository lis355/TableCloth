using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace TableClothGUI
{
    public partial class Note : Form
    {
        SortedDictionary<uint, InputSpace> IS = new SortedDictionary<uint, InputSpace>();

        uint AbsoluteCountOfInputSpace = 0;

        int OldWidth;
        int Delta = 0;
        int Mashtab, FrontSizeDeltaPercent;
        float CurrentFrontSize, FrontSizeDelta;

        public NoteLinkKernel NoteKernel;

        Timer MainTimer;

        public Note(Form MDIParent, string Name, string KernelPath, string KernelArgs, bool CreateInputSpace)
        {
            InitializeComponent();

            SuspendLayout();
            MainTimer = new Timer();
            BackColor = GUIVariables.BackgroundColor;
            Status.BackColor = GUIVariables.BackgroundColor;
            MdiParent = MDIParent;
            NoteName = Name;
            OldWidth = Width;
            CurrentFrontSize = GUIVariables.StdFontSize;
            FrontSizeDelta = 2.0f;
            Mashtab = 100;
            toolStripStatusLabelMashtab.Text = Mashtab.ToString() + "%";
            NoteKernel = new NoteLinkKernel(KernelPath, KernelArgs);
            MainTimer.Interval = 500;
            MainTimer.Start(); 
            ResumeLayout();

            if (CreateInputSpace) AddInputSpace();
        }

        public Note(Form MDIParent, string FileName)
        {
            bool isvisible = false;
            string intext = "";

            InitializeComponent();

            SuspendLayout();
            MainTimer = new Timer();
            BackColor = GUIVariables.BackgroundColor;
            Status.BackColor = GUIVariables.BackgroundColor;
            MdiParent = MDIParent;
            OldWidth = Width;
            Mashtab = 100;
            CurrentFrontSize = GUIVariables.StdFontSize;
            FrontSizeDelta = 2.0f;
            MainTimer.Interval = 500;
            MainTimer.Start();
            ResumeLayout();

            XmlTextReader D = new XmlTextReader(FileName);

            while (D.Read())
            {
                if (D.IsStartElement())
		        {
		            switch (D.Name)
		            {
                        case "Note":
                            NoteName = D.GetAttribute("Name");
                            int nMashtab = Convert.ToInt16(D.GetAttribute("Mashtab"));
                            int FrontSizeDeltaPercent = Convert.ToInt16(100 * FrontSizeDelta / GUIVariables.StdFontSize);
                            while (nMashtab > Mashtab)
                            {
                                CurrentFrontSize += FrontSizeDelta;
                                Mashtab += FrontSizeDeltaPercent;
                            }
                            while (nMashtab < Mashtab)
                            {
                                CurrentFrontSize -= FrontSizeDelta;
                                Mashtab -= FrontSizeDeltaPercent;
                            }
                            break;
                        case "Space":
                            isvisible = Convert.ToBoolean(D.GetAttribute("OutVisible"));
			                break;
                        case "InText":
                            D.Read();
                            intext = D.Value;
                            break;
                        case "OutText":
                            D.Read();
                            AddInputSpace(intext, D.Value, isvisible);
                            break;
		            }
		        }
            }

            D.Close();

            toolStripStatusLabelMashtab.Text = Mashtab.ToString() + "%";
        }

#region public methods

        public void AddInputSpace(string InStartText = "", string OutStartText = "", bool OutVisible = false)
        {
            SuspendLayout();
            if (IS.Count == 0) AbsoluteCountOfInputSpace = 0;
            InputSpace sp = new InputSpace(this, MainTimer, Width - 40);
            sp.SpaceNumber = AbsoluteCountOfInputSpace;
            sp.FontSize = CurrentFrontSize;
            sp.InText = InStartText;
            sp.OutText = OutStartText;
            if (OutVisible) sp.ShowOut();
            NoteMainPanel.Controls.Add(sp);
            sp.BringToFront();
            ResumeLayout();
            IS.Add(AbsoluteCountOfInputSpace, sp);
            AbsoluteCountOfInputSpace++;
        }

        public bool RemoveInputSpace(uint Key)
        {
            InputSpace deleteobj;
            if (IS.TryGetValue(Key, out deleteobj))
            {
                IS.Remove(Key);
                NoteMainPanel.Controls.Remove(deleteobj);
                deleteobj.Dispose();
                return true;
            }
            return false;
        }

        public void SetInputSpaceInText(uint NumSpace, string S)
        {
            InputSpace obj;
            if (IS.TryGetValue(NumSpace, out obj))
            {
                obj.InText = S;
            }
        }

        public string GetInputSpaceInText(uint NumSpace)
        {
            InputSpace obj;
            if (IS.TryGetValue(NumSpace, out obj))
            {
                return obj.InText;
            }
            return null;
        }

        public string GetInputSpaceOutText(uint NumSpace)
        {
            InputSpace obj;
            if (IS.TryGetValue(NumSpace, out obj))
            {
                return obj.OutText;
            }
            return null;
        }

        public void SaveNote(string FileName)
        {
            XmlTextWriter D = new XmlTextWriter(FileName, Encoding.UTF8);
            D.Formatting = Formatting.Indented;

            D.WriteStartDocument();

            D.WriteComment(Assembly.GetExecutingAssembly().GetName().Name + " version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());

            D.WriteStartElement("Note");
            D.WriteAttributeString("Name", NoteName);
            D.WriteAttributeString("Mashtab", Mashtab.ToString());
            foreach (InputSpace sp in IS.Values)
            {
                D.WriteStartElement("Space");
                D.WriteAttributeString("OutVisible", sp.IsOutVisible().ToString());

                D.WriteStartElement("InText");
                D.WriteWhitespace("");
                D.WriteString(sp.InText);
                D.WriteEndElement();

                D.WriteStartElement("OutText");
                D.WriteWhitespace("");
                D.WriteString(sp.OutText);
                D.WriteEndElement();

                D.WriteEndElement();
            }
            D.WriteEndElement();

            D.WriteEndDocument();
            D.Close();
        }

        public string NoteName
        {
            set { Text = value; }
            get { return Text; }
        }

#endregion

        private void toolStripMashtabPlus_Click(object sender, EventArgs e)
        {
            FrontSizeDeltaPercent = Convert.ToInt16(100 * FrontSizeDelta / GUIVariables.StdFontSize);
            CurrentFrontSize += FrontSizeDelta;
            foreach (KeyValuePair<uint, InputSpace> s in IS)
                s.Value.FontSize = CurrentFrontSize; 
            Mashtab+=FrontSizeDeltaPercent;
            toolStripStatusLabelMashtab.Text = Mashtab.ToString()+"%";
        }

        private void toolStripMashtabLess_Click(object sender, EventArgs e)
        {
            FrontSizeDeltaPercent = Convert.ToInt16(100 * FrontSizeDelta / GUIVariables.StdFontSize);
            CurrentFrontSize -= FrontSizeDelta;
            foreach (KeyValuePair<uint, InputSpace> s in IS)
                s.Value.FontSize = CurrentFrontSize;
            Mashtab -= FrontSizeDeltaPercent;
            toolStripStatusLabelMashtab.Text = Mashtab.ToString() + "%";
        }

        private void Note_Resize(object sender, EventArgs e)
        {
            Delta = Width - OldWidth;
            OldWidth = Width;
            foreach (KeyValuePair<uint, InputSpace> s in IS)
                s.Value.ReSize(Delta);
        }

        private void NoteMainPanel_DoubleClick(object sender, EventArgs e)
        {
            AddInputSpace();
        }

        private void Note_FormClosed(object sender, FormClosedEventArgs e)
        {
            NoteKernel.KillKernel();
        }
    }
}
