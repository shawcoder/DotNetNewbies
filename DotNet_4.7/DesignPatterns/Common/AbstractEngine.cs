namespace Common
{

	public abstract class AbstractEngine: IEngine
	{
		private readonly int _Size;
		private readonly bool _Turbo;

		protected AbstractEngine(int size, bool turbo)
		{
			_Size = size;
			_Turbo = turbo;
		}

		public virtual int Size => _Size;

		public virtual bool Turbo => _Turbo;

		public override string ToString()
		{
			return GetType().Name + " (" + _Size + ")";
		}

	}
}