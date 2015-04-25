using System.Collections;
using System.Collections.Generic;

namespace TableClothKernel
{
	public sealed class OperandList : Operand, IList<Operand>
	{
		readonly List<Operand> _operands;
 
		public OperandList()
		{
			_operands = new List<Operand>();
		}

		public override void Validate()
		{
			foreach ( var operand in _operands )
			{
				operand.Validate();
			}
		}

		public override Operand Simplify()
		{
			for ( int i = 0; i < _operands.Count; ++i )
			{
				_operands[i] = _operands[i].Simplify();
			}

			return this;
		}

		public IEnumerator<Operand> GetEnumerator()
		{
			return _operands.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( _operands as IEnumerable ).GetEnumerator();
		}

		public void Add( Operand item )
		{
			_operands.Add( item );
		}

		public void Clear()
		{
			_operands.Clear();
		}

		public bool Contains( Operand item )
		{
			return _operands.Contains( item );
		}

		public void CopyTo( Operand[] array, int arrayIndex )
		{
			_operands.CopyTo( array, arrayIndex );
		}

		public bool Remove( Operand item )
		{
			return _operands.Remove( item );
		}

		public int Count
		{
			get
			{
				return _operands.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public int IndexOf( Operand item )
		{
			return _operands.IndexOf( item );
		}

		public void Insert( int index, Operand item )
		{
			_operands.Insert( index, item );
		}

		public void RemoveAt( int index )
		{
			_operands.RemoveAt( index );
		}

		public Operand this[int index]
		{
			get
			{
				return _operands[index];
			}
			set
			{
				_operands[index] = value;
			}
		}
	}
}
