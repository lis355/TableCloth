using System.Collections.Generic;

namespace TableClothKernel
{
	public class Function : Operand
	{
	    public string Name { set; get; }
	    public List<Operand> Operands { set; get; }
	
	    public Function():
	        base()
	    {
	        Operands = new List<Operand>();
	    }

		public override void Validate()
		{
			foreach ( var operand in Operands )
			{
				operand.Validate();
			}
		}
	}
}
