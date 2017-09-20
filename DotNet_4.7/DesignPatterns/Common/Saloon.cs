namespace Common
{

	public class Saloon: AbstractCar
	{

		public Saloon(IEngine engine)
			: this(engine, VehicleColour.Unpainted) { }

		public Saloon(IEngine engine, VehicleColour colour)
			: base(engine, colour) { }
	}
}