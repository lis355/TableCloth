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

		public delegate void VariableEvent( VariableList list, string variableName );

		public event VariableEvent VariableChanged;
		public event VariableEvent VariableDeleted;

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

	        if ( VariableChanged != null )
	        {
		        VariableChanged( this, name );
	        }
        }

        /// <summary>
        /// Удаление переменной
        /// </summary>
        public void Delete( string name )
        {
            if ( !IsExist( name ) )
				return;

			_variablesTable.Remove( name );

			if ( VariableDeleted != null )
	        {
		        VariableDeleted( this, name );
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

	    public Expression Find( string name )
	    {
			Expression expression;
			_variablesTable.TryGetValue( name, out expression );
			return expression;
	    }

	    public void Clear()
        {
            _variablesTable.Clear();
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
