namespace Common
{

	public class Sport: AbstractCar
	{

		public Sport(IEngine engine)
			: this(engine, VehicleColour.Unpainted) { }

		public Sport(IEngine engine, VehicleColour colour)
			: base(engine, colour) { }
	}
}