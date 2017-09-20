namespace Common
{
	public class SportDriverControls : AbstractDriverControls
	{

		public SportDriverControls(IDrivableEngine aEngine)
			: base(aEngine)
		{
			// Nothing else needed.
		}

		public virtual void AccelerateHard()
		{
			Accelarate();
			Accelarate();
		}

		public virtual void BrakeHard()
		{
			Brake();
			Brake();
		}

	}
}
