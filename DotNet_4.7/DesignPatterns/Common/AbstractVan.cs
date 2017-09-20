namespace Common
{
	public abstract class AbstractVan: AbstractVehicle
	{

		protected AbstractVan(IEngine engine)
			: this(engine, VehicleColour.Unpainted)
		{ }

		protected AbstractVan(IEngine engine, VehicleColour colour)
			: base(engine, colour)
		{ }
	}
}