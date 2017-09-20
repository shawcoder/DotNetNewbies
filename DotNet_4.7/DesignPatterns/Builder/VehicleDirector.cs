namespace Builder
{
	using Common;

	public abstract class VehicleDirector
	{
		public abstract IVehicle Build(VehicleBuilder aBuilder);

	}
}
