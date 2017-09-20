namespace Common
{

	public abstract class AbstractCar: AbstractVehicle
	{

		protected AbstractCar(IEngine engine)
			: this(engine, VehicleColour.Unpainted)
		{ }

		protected AbstractCar(IEngine engine, VehicleColour colour)
			: base(engine, colour)
		{ }
	}
}