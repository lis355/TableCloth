using System.Collections.Generic;

namespace TableClothKernel
{
    /// <summary>
    /// Список переменных, основан на словаре для более быстрой проверки на существование элемента
    /// </summary>
    class VariableList
    {
        /// <summary>
        /// Список переменных
        /// </summary>
        Dictionary<string, Expression> VariablesTable;

        public VariableList()
        {
            VariablesTable = new Dictionary<string, Expression>();
        }

        /// <summary>
        /// Проверка существования переменной
        /// </summary>
        public bool IsExist( string Name )
        {
            return VariablesTable.ContainsKey( Name );
        }

        /// <summary>
        /// Установить новое значение существующей переменной либо
        /// при ее отсутствии создать новую
        /// </summary>
        public void Set( string Name, Expression F )
        {
            if ( !IsExist( Name ) )
            {
                // добавляем
                VariablesTable.Add( Name, F );
            }
            else
            {
                VariablesTable[ Name ] = F;
            }
        }

        /// <summary>
        /// Удаление переменной
        /// </summary>
        public void Delete( string Name )
        {
            if ( IsExist( Name ) )
            {
                VariablesTable.Remove( Name );
            }
        }

        public Expression this[ string Name ]
        {
            get { return VariablesTable[ Name ]; }
        }

        public string[] GetAllVariablesNames()
        {
            string[] res = new string[ VariablesTable.Keys.Count ];
            VariablesTable.Keys.CopyTo( res, 0 );
            return res;
        }
    }
}
