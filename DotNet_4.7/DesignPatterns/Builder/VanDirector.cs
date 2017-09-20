namespace Builder
{
	using Common;

	public class VanDirector: VehicleDirector
	{

		public override IVehicle Build(VehicleBuilder aBuilder)
		{
			aBuilder.BuildChassis();
			aBuilder.BuildBody();
			aBuilder.BuildReinforcedStorageArea();
			aBuilder.BuildWindows();
			return aBuilder.Vehicle;
		}

	}
}
