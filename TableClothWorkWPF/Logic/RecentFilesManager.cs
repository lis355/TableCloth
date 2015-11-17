using System;
using System.Collections.Generic;
using GJson;

namespace TableClothWork
{
	public class FileInfo
	{
		[Name( "path" )]
		public string Path;

		public FileInfo()
		{
			Path = String.Empty;
		}

		public FileInfo( string absolutePath )
		{
			Path = absolutePath;
		}

		public virtual bool IsExists
		{
			get
			{
				return Setting.Instance.IsAbsoluteFileExists( Path );
			}
		}
	}

	public class RecentFilesManager
	{
		public class OpenFileInfo : FileInfo
		{
			public enum EFileStatus
			{
				Saved,
				NotSaved
			}

			[Name( "status" )]
			public EFileStatus Status;

			public override bool IsExists
			{
				get
				{
					switch ( Status )
					{
						case EFileStatus.NotSaved: return Setting.Instance.IsFileExists( Path );
						case EFileStatus.Saved: return Setting.Instance.IsAbsoluteFileExists( Path );
						default: return false;
					}
				}
			}
		}

		public class RecentFiles
		{
			[Name( "recentFiles" )]
			public List<FileInfo> RecentFileList = new List<FileInfo>();

			[Name( "openFiles" )]
			public List<OpenFileInfo> OpenFileList = new List<OpenFileInfo>();
		}

		public RecentFiles Data { get; private set; }

		public event EventHandler<RecentFiles> OnRecentFilesChanged;

		public RecentFilesManager()
		{
		}

		int _maximumRecentFilesAmount = 10;
		public int MaximumRecentFilesAmount
		{
			get
			{
				return _maximumRecentFilesAmount;
			}
			set
			{
				value = Math.Max( 0, value );

				if ( _maximumRecentFilesAmount == value )
					return;

				while ( Data.RecentFileList.Count > value )
				{
					Data.RecentFileList.RemoveAt( 0 );
				}

				_maximumRecentFilesAmount = value;

				if ( OnRecentFilesChanged != null )
				{
					OnRecentFilesChanged( this, Data );
				}
			}
		}

		public void Load()
		{
			Data = Serializator.TryDeserialize<RecentFiles>( Preferences.Instance["recentFiles"] );

			Data.RecentFileList.RemoveAll( x => !x.IsExists );
			Data.OpenFileList.RemoveAll( x => !x.IsExists );

			if ( OnRecentFilesChanged != null )
			{
				OnRecentFilesChanged( this, Data );
			}
		}

		public void Save()
		{
			Preferences.Instance["recentFiles"] = Serializator.Serialize( Data );
		}

		public void AddRecentFile( FileInfo file )
		{
			Data.RecentFileList.Add( file );

			while ( Data.RecentFileList.Count > MaximumRecentFilesAmount )
			{
				Data.RecentFileList.RemoveAt( 0 );
			}

			if ( OnRecentFilesChanged != null )
			{
				OnRecentFilesChanged( this, Data );
			}
		}
	}
}
