namespace Common
{
	using System;
	using static System.Console;

	public abstract class AbstractDriverControls
	{
		private readonly IDrivableEngine _Engine;

		protected AbstractDriverControls(IDrivableEngine aEngine)
		{
			if (aEngine == null)
				throw new ArgumentNullException(nameof(aEngine));
			_Engine = aEngine;
		}

		public virtual void IgnitionOn() { _Engine.Start(); }

		public virtual void IgnitionOff() { _Engine.Stop(); }

		public virtual void Accelarate()
		{
			_Engine.IncreasePower();
			WriteLine("VROOOM!");
		}

		public virtual void Brake()
		{
			_Engine.DecreasePower(); 
			WriteLine("SCREEEEEECH!");
		}
	}
}
