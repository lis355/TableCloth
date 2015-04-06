namespace TableClothKernel
{
    static class ExpressionString
    {
        static readonly string[] _stringCommandsCodeSymbol = { "!", "&&", "||", "^", "==", "=>", "[shef]", "[pirse]" };
        static readonly string[] _stringCommandsCodeWord = { "[not]", "[and]", "[or]", "[xor]", "[equ]", "[impl]", "[shef]", "[pirse]" };

        static readonly string[] _stringConstantNumber = { "0", "1" };
        static readonly string[] _stringConstantWord = { "false", "true" };

        public delegate string CommandsCodeToStringConverter( CommandsCode c );
        public static CommandsCodeToStringConverter CommandsCodeToString = CommandCodeToStringSymbol;

        static StringCommandType _commandCodeTypeVariable = StringCommandType.Symbol;
        static public StringCommandType CommandCodeType
        {
            set
            {
                _commandCodeTypeVariable = value;

                if ( value == StringCommandType.Symbol )
                {
                    CommandsCodeToString = CommandCodeToStringSymbol;
                }
                else
                {
                    CommandsCodeToString = CommandCodeToStringWord;
                }
            }
            get
            {
                return _commandCodeTypeVariable;
            }
        }

        static string CommandCodeToStringSymbol( CommandsCode c )
        {
            return _stringCommandsCodeSymbol[ ( byte )c ];
        }

        static string CommandCodeToStringWord( CommandsCode c )
        {
            return _stringCommandsCodeWord[ ( byte )c ];
        }

        public delegate string ConstantToStringConverter( bool c );
        public static ConstantToStringConverter ConstantToString = ConstantToStringNumber;

        static StringConstantType _constantTypeVariable = StringConstantType.Number;
        public static StringConstantType ConstantType
        {
            set
            {
                _constantTypeVariable = value;

                if ( value == StringConstantType.Number )
                {
                    ConstantToString = ConstantToStringNumber;

                }
                else
                {
                    ConstantToString = ConstantToStringWord;
                }
            }
            get
            {
                return _constantTypeVariable;
            }
        }

        static string ConstantToStringNumber( bool c )
        {
            return ( !c ) ? _stringConstantNumber[ 0 ] : _stringConstantNumber[ 1 ];
        }

        static string ConstantToStringWord( bool c )
        {
            return ( !c ) ? _stringConstantWord[ 0 ] : _stringConstantWord[ 1 ];
        }
    }
}
