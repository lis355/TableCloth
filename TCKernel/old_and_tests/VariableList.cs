using System.Collections.Generic;

namespace TableClothKernel
{
    /// <summary>
    /// Список переменных, основан на словаре для более быстрой проверки на существование элемента
    /// </summary>
    public class VariableList
    {
        readonly Dictionary<string, Expression> _variablesTable;

        public VariableList()
        {
            _variablesTable = new Dictionary<string, Expression>();
        }

        /// <summary>
        /// Проверка существования переменной
        /// </summary>
        public bool IsExist( string name )
        {
            return _variablesTable.ContainsKey( name );
        }

        /// <summary>
        /// Установить новое значение существующей переменной либо
        /// при ее отсутствии создать новую
        /// </summary>
        public void Set( string name, Expression f )
        {
            if ( !IsExist( name ) )
            {
                // добавляем
                _variablesTable.Add( name, f );
            }
            else
            {
                _variablesTable[name] = f;
            }
        }

        /// <summary>
        /// Удаление переменной
        /// </summary>
        public void Delete( string name )
        {
            if ( IsExist( name ) )
            {
                _variablesTable.Remove( name );
            }
        }

        public Expression this[string name]
        {
            get
            {
                return _variablesTable[name];
            }
        }

        public string[] GetAllVariablesNames()
        {
            var res = new string[_variablesTable.Keys.Count];
            _variablesTable.Keys.CopyTo( res, 0 );
            return res;
        }
    }
}
