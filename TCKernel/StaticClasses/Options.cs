using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableClothKernel
{
    public static class Options
    {
        static StringConstantType _ConstantOutType;
        public static StringConstantType ConstantOutType
        {
            set
            {
                _ConstantOutType = value;
                ExpressionString.ConstantType = value;
            }
            get
            {
                return _ConstantOutType;
            }
        }
        //  public static StringCommandType CommandOutType = StringCommandType.Symbol;
    }
}
