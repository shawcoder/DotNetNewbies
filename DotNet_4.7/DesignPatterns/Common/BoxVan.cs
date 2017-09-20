namespace Common
{

	public class BoxVan: AbstractVan
	{

		public BoxVan(IEngine engine)
			: this(engine, VehicleColour.Unpainted) { }

		public BoxVan(IEngine engine, VehicleColour colour)
			: base(engine, colour) { }
	}
}