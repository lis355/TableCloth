namespace TableClothKernel
{
	public class Function : Operand
	{
	    public string Name { set; get; }
	    public OperandList Operands { set; get; }
	
	    public Function():
	        base()
	    {
	        Operands = new OperandList();
	    }

		public override void Validate()
		{
			Operands.Validate();
		}

		public override Operand Simplify()
		{
			Operands = Operands.Simplify() as OperandList;

			return this;
		}
	}
}
