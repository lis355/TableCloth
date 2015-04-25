namespace TableClothKernel
{
	public class DefineVariableCommand : Command
	{
		public Variable Variable { get; set; }
		public Expression Expression { get; set; }

		public override void Validate()
		{
			Variable.Validate();
			Expression.Validate();
		}
	}
	
	public class DeleteVariableCommand : Command
	{
		public Variable Variable { get; set; }

		public override void Validate()
		{
			Variable.Validate();
		}
	}
}
