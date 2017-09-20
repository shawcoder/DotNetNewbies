namespace Common
{
	public interface IDrivableEngine : IEngine
	{
		void Start();

		void Stop();

		void IncreasePower();

		void DecreasePower();

	}
}
