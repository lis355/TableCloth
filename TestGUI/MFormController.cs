using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using TableClothGUI;

namespace TestGUI
{
    class MFormController
    {
        MForm _form;

        public MFormController( MForm form )
        {
            _form = form;
        }

        public void CreateNewNote()
        {
            // brushes which defines words color
            Color WordColor = Color.OrangeRed, TypeLexColor = Color.LightGreen;

            // define keywords with they colors
            SortedDictionary<string, Color> keys = new SortedDictionary<string, Color>();
            keys.Add("MonomialOrder", WordColor);
            keys.Add("SyzygyPolynomial", WordColor);
            keys.Add("Reduce", WordColor);
            keys.Add("Contains", WordColor);
            keys.Add("GroebnerBasis", WordColor);
            keys.Add("GroebnerBasisM", WordColor);
            keys.Add("GroebnerBasisR", WordColor);
            keys.Add("lex", TypeLexColor);
            keys.Add("grlex", TypeLexColor);
            keys.Add("grevlex", TypeLexColor);

            // create new syntax
            TCSyntax snt = new TCSyntax();
            snt.RoundBracketColor = Color.Crimson;
            snt.SquareBracketColor = Color.Crimson;
            snt.TriangleBracketColor = Color.Crimson;
            snt.GroupBracketColor = Color.Crimson;
            snt.NumberColor = Color.Blue;
            snt.SymbolColor = Color.Black;
            snt.KeyWords = keys;
            snt.StandartColor = Color.Black;

            snt.CreateSyntax();

            // set syntax
            TCSyntaxHighlighter.SetSyntax(snt);

            // add new note to form
            Note NT = new Note(
                _form,
                "NewNote1",
                @"C:\cygwin\bin\bash.exe",
                "--login -i Singular",
                false);

            NT.WindowState = FormWindowState.Maximized;

            // add some input spaces
            NT.AddInputSpace("p1 = 2*x(3,1)+3*x(2,5)+7");
            NT.AddInputSpace("p2 = 8*x(3,1)+7*x(2,5)+3");
            NT.AddInputSpace("p1 {+} p2");
            NT.AddInputSpace("SyzygyPolynomial [p1; p2]");
            NT.AddInputSpace(@"ring r = 0, (x,y,z), Dp; ideal i = x3-2xy, x2y-2y2+x; ideal g; g = groebner(i); g;");
            NT.Show();
        }

        public void SaveActiveNote()
        {
            var activeNote = _form.ActiveMdiChild as TableClothGUI.Note;
            if (activeNote == null)
                return;

            _form.SaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            _form.SaveFileDialog.Filter = "Groebner Basis Computations Files (*.gbf)|*.gbf";
            if ( _form.SaveFileDialog.ShowDialog() == DialogResult.OK )
            {
                activeNote.SaveNote( _form.SaveFileDialog.FileName );
            }
        }

        public void OpenNote()
        {
            _form.OpenFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            _form.OpenFileDialog.Filter = "Groebner Basis Computations Files (*.gbf)|*.gbf";
            if ( _form.OpenFileDialog.ShowDialog() == DialogResult.OK )
            {
                var note = new TableClothGUI.Note( _form, _form.OpenFileDialog.FileName );
                note.Show();
            }
        }
    }
}
