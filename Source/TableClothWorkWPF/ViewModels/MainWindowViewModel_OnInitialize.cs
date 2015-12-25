namespace TableClothWork
{
	public partial class MainWindowViewModel
	{
		void OnInitialize( object parameter )
		{
			Setting.Instance.Initialaze();
			RecentFiles.Load();

			ViewModelTitle = Information.CurrentAssemblyTitleName;

			LoadOpenFiles();

			// test
			
		}

		void LoadOpenFiles()
		{
			foreach ( var openFileInfo in RecentFiles.Data.OpenFileList )
			{
				var data = Setting.Instance.DeserializeFile<NoteData>( openFileInfo.Path );
				var note = new NoteViewModel( data );
				Notes.Add( note );
			}

			if ( Notes.Count == 0 )
			{
				AddAndSelectNewNote( GetNextNewNoteName() );
			}
		}
	}
}
