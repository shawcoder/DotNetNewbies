namespace Adapter
{
	public class SuperGreenEngine
	{
		public SuperGreenEngine(int aEngineSize)
		{
			EngineSize = aEngineSize;
		}

		public int EngineSize { get; }

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() { return "SUPER ENGINE" + EngineSize; }

	}
}
