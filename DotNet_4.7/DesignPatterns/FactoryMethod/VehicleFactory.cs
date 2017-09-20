namespace FactoryMethod
{
	using Common;

	public abstract class VehicleFactory
	{
		public enum DrivingStyle
		{
			Economical
			, Midrange
			, Powerful
		}

		public enum BuildWhat
		{
			Car
			, Van
		}

		public static VehicleFactory Factory { get; private set; }

		public virtual IVehicle Build
			(DrivingStyle aDrivingStyle, VehicleColour aVehicleColour)
		{
			IVehicle vVehicle = SelectVehicle(aDrivingStyle);
			vVehicle.Paint(aVehicleColour);
			return vVehicle;
		}

		public static IVehicle Make
		(
			BuildWhat aBuildWhat
			, DrivingStyle aDrivingStyle
			, VehicleColour aVehicleColour
		)
		{
			switch (aBuildWhat)
			{
				case BuildWhat.Car:
				{
					Factory = new CarFactory();
					break;
				}
				case BuildWhat.Van:
				{
					Factory = new VanFactory();
					break;
				}
			}
			return Factory.Build(aDrivingStyle, aVehicleColour);
		}

		/// <summary>
		/// </summary>
		protected internal abstract IVehicle SelectVehicle
			(DrivingStyle aDrivingStyle);
	}
}
