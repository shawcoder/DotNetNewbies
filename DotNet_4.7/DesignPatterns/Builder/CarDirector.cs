namespace Builder
{
	using Common;

	public class CarDirector: VehicleDirector
	{

		public override IVehicle Build(VehicleBuilder aBuilder)
		{
			aBuilder.BuildChassis();
			aBuilder.BuildBody();
			aBuilder.BuildPassengerArea();
			aBuilder.BuildBoot();
			aBuilder.BuildWindows();
			return aBuilder.Vehicle;
		}

	}
}
