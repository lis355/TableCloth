namespace TableClothKernel
{
    enum CommandsCode : byte
    {
        Not,
        And,
        Or,
        Xor,
        Equivalence,
        Implication,
        Sheffer,
        Pirse
    }

    enum BooleanConstants : byte
    {
        True,
        False
    }
}