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
    /// Тип вывода на печать операторов
    /// </summary>
    public enum EStringOperatorType
    {
        Symbol, // символами ( напрмиер || && )
        Word, // зарезервированными словами ( например [not] [and] )
  		Function // функциями Not( ... ) And( ..., ... )
    }
}
