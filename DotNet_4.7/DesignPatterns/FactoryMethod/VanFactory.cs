namespace FactoryMethod
{
	using Common;

	public class VanFactory: VehicleFactory
	{
		#region Overrides of VehicleFactory

		/// <summary>
		/// </summary>
		protected internal override IVehicle SelectVehicle(DrivingStyle aDrivingStyle) {
			switch (aDrivingStyle)
			{
				case DrivingStyle.Economical:
				{
					return new Pickup(new StandardEngine(2200));
				}
				case DrivingStyle.Midrange:
				{
					return new Pickup(new StandardEngine(2200));
				}
				case DrivingStyle.Powerful:
				{
					return new BoxVan(new TurboEngine(2500));
				}
				default:
				{
					return new BoxVan(new TurboEngine(2500));
				}
			}
		}

		#endregion
	}
}
