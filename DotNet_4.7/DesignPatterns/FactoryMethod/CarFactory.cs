namespace FactoryMethod
{
	using Common;

	public class CarFactory: VehicleFactory
	{
		#region Overrides of VehicleFactory

		/// <summary>
		/// This is the builder factory method that is called.
		/// </summary>
		protected internal override IVehicle SelectVehicle(DrivingStyle aDrivingStyle)
		{
			switch (aDrivingStyle)
			{
				case DrivingStyle.Economical:
				{
					return new Saloon(new StandardEngine(1300));
				}		
				case DrivingStyle.Midrange:
				{
					return new Coupe(new StandardEngine(1600));
				}
				case DrivingStyle.Powerful:
				{
					return new Sport(new TurboEngine(2000));
				}
				default:
				{
					return new Sport(new TurboEngine(2000));
				}
			}
		}

		#endregion
	}
}
