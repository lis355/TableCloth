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
            var prs = new Parser( new Scanner( S ) );
            try
            {
                prs.Parse();
                //Prs.errorString = Prs.errors.errorStream. .ToString();
                //Prs.Tree.ToDot();
            }
            catch // ( TCException Expt )
            {
                return false; // Expt.Message;
            }

            return prs.errors.count != 0;
        }
    }
}
