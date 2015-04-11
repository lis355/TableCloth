using System.Collections;
using System.Collections.Generic;

namespace TableClothKernel
{
    /// <summary>
    /// Список переменных, основан на словаре для более быстрой проверки на существование элемента
    /// </summary>
    public class VariableList : IEnumerable<KeyValuePair<string, Expression>>
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
        public void Set( string name, Expression expression )
        {
            if ( !IsExist( name ) )
            {
                // добавляем
                _variablesTable.Add( name, expression );
            }
            else
            {
                _variablesTable[name] = expression;
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
				if ( IsExist( name ) )
				{
					return _variablesTable[name];
				}

				throw new TcException( "Can't find variable " + name );
            }
        }

	    public IEnumerator<KeyValuePair<string, Expression>> GetEnumerator()
	    {
		    return _variablesTable.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return _variablesTable.GetEnumerator();
	    }
    }
}
