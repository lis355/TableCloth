using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BinaryCalc.Functions;

namespace BinaryCalc
{
    public partial class TableForm : Form
    {
        public TableForm(Formula F)
        {
            InitializeComponent();
            Grid.ColumnCount = F.VariableCount + 1;
            Grid.RowCount = Convert.ToInt32(Math.Pow(2, F.VariableCount)) + 1;
            for (int i = 0; i < Grid.ColumnCount - 1; i++) Grid[i, 0].Value = "x" + Convert.ToString(i+1);
            Grid[Grid.ColumnCount-1,0].Value="F";
            
            bool[] b = new bool[F.VariableCount];
            for (int i = 1; i < Grid.RowCount; i++)
            {
                for (int j = 0; j < Grid.ColumnCount - 1; j++) Grid[j, i].Value = (b[j] == true) ? "1" : "0";
                Grid[Grid.ColumnCount - 1, i].Value = (F.CaclOnBoolVector(b)==true) ? "1" : "0";
                FGet.GetNextVector(b);
            }

            for (int i = 0; i < Grid.RowCount; i++) Grid.Rows[i].Height = 18;
            for (int i = 0; i < Grid.ColumnCount; i++) Grid.Columns[i].Width = 50;
            Height = Grid.Height = 18 * (Grid.RowCount + 2) + 10;
            Width = Grid.Width = 50 * (Grid.ColumnCount + 1) + 10;  
        }
    }
}
