using System.Windows.Forms;

namespace BinaryCalc
{
    // представляет собой класс хранящий все поля ввода и переменные данной формы
    public class BCalcProgram
    {
        /// <summary>
        /// Список из пространств формул, отображаемых на форме
        /// </summary>
        public FormulaSpaceList FSpaceList;

        /// <summary>
        /// Переменные
        /// </summary>
        public Variables FVariables;

        public BCalcProgram(ref TableLayoutPanel T, ref VariablesForms A, string S)
        {
            FSpaceList = new FormulaSpaceList(ref T, this);
            FVariables = new Variables(ref A, S, this);

            // создаем пространство ввода
            FSpaceList.AddBack();
        }
    }
}
