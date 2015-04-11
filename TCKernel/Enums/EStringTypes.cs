namespace TableClothKernel
{
    /// <summary>
    /// Тип вывода на печать констант: числами 0 и 1
    /// или символьно True и False
    /// </summary>
    public enum EStringConstantType
    {
        Number,
        Word  
    }

    /// <summary>
    /// Тип вывода на печать операторов: символами ( напрмиер || && )
    /// или символьно ( например [not] [and] )
    /// </summary>
    public enum EStringCommandType
    {
        Symbol,
        Word  
    }
}
