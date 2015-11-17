using System.Collections.Generic;
using System.Collections.Specialized;

namespace TableClothWork
{
	public class ObservableListWrapper<T> : IList<T>, INotifyCollectionChanged
	{
		readonly IList<T> _list;

		public ObservableListWrapper( IList<T> list )
		{
			_list = list;
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		protected void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
		{
			if ( CollectionChanged != null )
			{
				CollectionChanged( this, e );
			}
		}

		public void Insert( int index, T item )
		{
			_list.Insert( index, item );
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index );
			OnCollectionChanged( e );

		}

		public void Add( T item )
		{
			_list.Add( item );
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item );
			OnCollectionChanged( e );
		}

		public void Clear()
		{
			_list.Clear();
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset );
			OnCollectionChanged( e );
		}

		public bool Remove( T item )
		{
			var result= _list.Remove( item );
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item );
			OnCollectionChanged( e );

			return result;
		}
		public void RemoveAt( int index )
		{
			_list.RemoveAt( index );
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, this[index], index );
			OnCollectionChanged( e );
		}

		public int IndexOf( T item )
		{
			return _list.IndexOf( item );
		}

		public T this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		public bool Contains( T item )
		{
			return _list.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex )
		{
			_list.CopyTo( array, arrayIndex );
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{

			return ( ( System.Collections.IEnumerable )_list ).GetEnumerator();
		}
	}
}
