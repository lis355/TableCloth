using System;
using System.Collections.Generic;
using System.Text;

namespace TableClothKernel
{
    /// <summary>
    /// Основной класс для рассчета строки
    /// </summary>
    public static class Calc
    {
        // Основная функция рассчета строки
        public static bool CalcExpression( string S )
        {
            Parser Prs = new Parser( new Scanner( S ) );
            try
            {
                Prs.Parse();
                //Prs.errorString = Prs.errors.errorStream. .ToString();
                //Prs.Tree.ToDot();
            }
            catch// ( TCException Expt )
            {
                return false; // Expt.Message;
            }

            return ( Prs.errors.count != 0 );
        }
    }
}
