namespace Singleton
{
	public class SerialNumberGenerator
	{
		private static volatile SerialNumberGenerator _Instance;
		private int _Count;

		public static SerialNumberGenerator Instance =>
			_Instance ?? (_Instance = new SerialNumberGenerator());

		private SerialNumberGenerator()
		{
		}

		public virtual int NextSerial => ++_Count;

	}
}
