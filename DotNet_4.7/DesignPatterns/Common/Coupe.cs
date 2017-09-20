namespace Common
{

	public class Coupe: AbstractCar
	{

		public Coupe(IEngine engine)
			: this(engine, VehicleColour.Unpainted) { }

		public Coupe(IEngine engine, VehicleColour colour)
			: base(engine, colour) { }
	}
}