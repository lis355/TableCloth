using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel
{
    static class ExpressionString
    {
        static string[] StringCommandsCodeSymbol = new string[] { "!", "&&", "||", "^", "==", "=>", "[shef]", "[pirse]" };
        static string[] StringCommandsCodeWord = new string[] { "[not]", "[and]", "[or]", "[xor]", "[equ]", "[impl]", "[shef]", "[pirse]" };

        static string[] StringConstantNumber = new string[] { "0", "1" };
        static string[] StringConstantWord = new string[] { "false", "true" };

        public delegate string CommandsCodeToStringConverter( CommandsCode C );
        public static CommandsCodeToStringConverter CommandsCodeToString = CommandCodeToStringSymbol;

        static StringCommandType CommandCodeTypeVariable = StringCommandType.Symbol;
        static public StringCommandType CommandCodeType
        {
            set
            {
                CommandCodeTypeVariable = value;
                CommandsCodeToString = ( value == StringCommandType.Symbol ) ? ( CommandsCodeToStringConverter )CommandCodeToStringSymbol : ( CommandsCodeToStringConverter )CommandCodeToStringWord;
            }
            get
            {
                return CommandCodeTypeVariable;
            }
        }

        static string CommandCodeToStringSymbol( CommandsCode C ) { return StringCommandsCodeSymbol[ ( byte )C ]; }
        static string CommandCodeToStringWord( CommandsCode C ) { return StringCommandsCodeWord[ ( byte )C ]; }

        public delegate string ConstantToStringConverter( bool C );
        public static ConstantToStringConverter ConstantToString = ConstantToStringNumber;

        static StringConstantType ConstantTypeVariable = StringConstantType.Number;
        public static StringConstantType ConstantType
        {
            set
            {
                ConstantTypeVariable = value;
                ConstantToString = ( ConstantToStringConverter )( ( value == StringConstantType.Number ) ? ( ConstantToStringConverter )ConstantToStringNumber : ( ConstantToStringConverter )ConstantToStringWord );
            }
            get
            {
                return ConstantTypeVariable;
            }
        }

        static string ConstantToStringNumber( bool C ) { return ( !C ) ? StringConstantNumber[ 0 ] : StringConstantNumber[ 1 ]; }
        static string ConstantToStringWord( bool C ) { return ( !C ) ? StringConstantWord[ 0 ] : StringConstantWord[ 1 ]; }
    }
}
