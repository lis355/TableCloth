namespace TableClothKernel
{
	public abstract class Command : TcToken
	{
		public override string ToDebugString() { return GetType().Name; }
		public abstract void Validate();
	}
}
