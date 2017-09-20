namespace Common
{
	public abstract class AbstractDrivableEngine : AbstractEngine, IDrivableEngine
	{
		private bool _Running;
		private int _Power;

		protected AbstractDrivableEngine(int aSize, bool aTurbo)
			: base(aSize, aTurbo)
		{
			_Power = 0;
		}

		public virtual void Start() { _Running = true; }

		public virtual void Stop()
		{
			_Running = false;
			_Power = 0;
		}

		public virtual void IncreasePower()
		{
			if (_Running && (Power < 10))
			{
				_Power++;
			}
		}

		public virtual void DecreasePower()
		{
			if (_Running && (_Power > 0))
			{
				_Power--;
			}
		}

		public override string ToString() => $"{GetType().Name} ({Size})";

		public bool Running => _Running;

		public int Power => _Power;

	}
}
