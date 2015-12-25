using System;
using System.CodeDom.Compiler;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TableClothWork
{
	public partial class Note
	{
		NoteViewModel _model;

		public Note()
		{
			InitializeComponent();

			_model = DataContext as NoteViewModel;
		}

		void StackPanel_MouseUp( object sender, System.Windows.Input.MouseButtonEventArgs e )
		{
			var model = DataContext as NoteViewModel;
			if ( model != null 
				&& model.AddNewUserInputCommand  != null 
				&& model.AddNewUserInputCommand.CanExecute( null ) )
			{
				model.AddNewUserInputCommand.Execute( null );
			}
		}

		//public static Note CreateNote( string name )
		//{
		//	if ( name.IndexOfAny( System.IO.Path.GetInvalidFileNameChars() ) >= 0 )
		//		throw new FormatException();
		//
		//	var note = new Note();
		//	note.NoteName = name;
		//
		//	return note;
		//}
	}
}
