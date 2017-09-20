namespace AbstractFactory
{
	public abstract class AbstractVehicleFactory
	{
		public abstract IBody CreateBody();

		public abstract IChassis CreateChassis();

		public abstract IGlassware CreateGlassware();

	}
}
