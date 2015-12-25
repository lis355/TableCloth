using System.Collections.Generic;
using System.Windows.Forms;
using UrielGuy.SyntaxHighlightingTextBox;

namespace BinaryCalc
{
    /// <summary>
    /// Класс списка всех пространств ввода
    /// </summary>
    public class FormulaSpaceList
    {
        public Dictionary<int,FormulaSpace> FormulaSpaceVector;
        int TotalAmount;//общее число всех элементов нужно для индексации ячеек, не уменьшается при удалении ячейки
        TableLayoutPanel LayoutPanel;
        BCalcProgram Parent;

        /// <summary>
        /// Конструктор, явно нужно задать TableLayoutPanel на которую впоследствии будут добавляться пространства
        /// </summary>
        public FormulaSpaceList(ref TableLayoutPanel T, BCalcProgram P)
        {
            FormulaSpaceVector = new Dictionary<int, FormulaSpace>(5);
            TotalAmount = 0;
            LayoutPanel = T;
            Parent = P;
        }

        /// <summary>
        /// Добавление нового пространства на форму 
        /// </summary>
        public void AddBack()
        {
            FormulaSpaceVector.Add(TotalAmount,
                new FormulaSpace(TotalAmount, ref Parent));
            TotalAmount++;

            LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            LayoutPanel.RowCount++;

            LayoutPanel.Controls.Add(FormulaSpaceVector[TotalAmount - 1].Space);
        }

        /// <summary>
        /// Удаление h того элемента с формы
        /// </summary>
        public void Delete(int h)
        {
            LayoutPanel.Controls.Remove(FormulaSpaceVector[h].Space);
            FormulaSpaceVector.Remove(h);
            LayoutPanel.RowCount--;
        }

        /// <summary>
        /// Удаляем все пространства
        /// </summary>
        public void ClearTotalAmount()
        {
            while (FormulaSpaceVector.Count > 0) Delete(FormulaSpaceVector.Count - 1);
            TotalAmount = 0;
        }

        /// <summary>
        /// Добавление подсветки переменной
        /// </summary>
        public void AddHighlight(HighlightDescriptor D)
        {
            foreach (FormulaSpace x in FormulaSpaceVector.Values)
            {
                x.InBox.HighlightDescriptors.Add(D); x.InBox.Text += "";
                x.OutBox.HighlightDescriptors.Add(D); x.OutBox.Text += "";
            }
        }

        /// <summary>
        /// Удаление подстветки переменной
        /// </summary>
        public void RemoveHighlight(HighlightDescriptor D)
        {
            foreach (FormulaSpace x in FormulaSpaceVector.Values)
            {
                x.InBox.HighlightDescriptors.Remove(D); x.InBox.Text += "";
                x.OutBox.HighlightDescriptors.Remove(D); x.OutBox.Text += "";
            }
        }

        /// <summary>
        /// Индексатор
        /// </summary>
        public FormulaSpace this[int h]
        {
            get
            {
                return FormulaSpaceVector[h];
            }
            set
            {
                FormulaSpaceVector[h] = value;
            }
        }

        /// <summary>
        /// Число пространств
        /// </summary>
        public int Count
        {
            get
            {
                return FormulaSpaceVector.Count;
            }
        }
    }
}
