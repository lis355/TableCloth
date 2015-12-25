using System;
using System.Collections.Generic;

using GJson;

namespace TableClothWork
{
	[Name( "Note" )]
	public class NoteData
	{
		[Name( "Space" )]
		public class UserInputData
		{
			[Name( "Min" )]
			public bool Minimized;

			[Name( "In" )]
			public string In;

			[Name( "Out" )]
			public string Out;
		}

		[Name( "Name" )]
		public string Name = String.Empty;

		[Name( "Spaces" )]
		public List<UserInputData> Spaces = new List<UserInputData>();
	}
}
