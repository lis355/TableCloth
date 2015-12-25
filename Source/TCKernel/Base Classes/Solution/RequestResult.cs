using System;
using System.Collections.Generic;

namespace TableClothKernel
{
    public class RequestResult
    {
        public class CommandResult
        {
            public string Output;

            /// <summary>
            /// Может быть null, это для кеширования
            /// </summary>
            public Expression Expression;

            public CommandResult()
            {
                Output = String.Empty;
            }
        }

        public bool Success;
        public TcException Exception;

        public UserInput Input;
        public List<CommandResult> Output;

        public RequestResult()
        {
            Success = false;
            Output = new List<CommandResult>();
        }
    }
}
