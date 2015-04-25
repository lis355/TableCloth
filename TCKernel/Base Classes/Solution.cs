namespace TableClothKernel
{
	public class Solution
	{
		/// <summary>
		/// Парсер ввода юзера
		/// </summary>
		public InputProcessor Input { get; private set; }

		/// <summary>
		/// Переменные в данном солюшене
		/// </summary>
		public VariableList Variables { get; private set; }

		/// <summary>
		/// Выполняет команды
		/// </summary>
		public CommandProcessor Commands { get; private set; }

		/// <summary>
		/// Рассчитывает выражения
		/// </summary>
		public Calculator Calculator { get; private set; }

		public Solution()
		{
			Input = new InputProcessor( this );
			Variables = new VariableList();
			Commands = new CommandProcessor( this );
			Calculator = new Calculator( Variables );
		}
	}
}
