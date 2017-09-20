namespace Builder
{
	using Common;
	using static System.Console;

	public class VanBuilder: VehicleBuilder
	{
		private readonly AbstractVan _VanInProgress;

		public VanBuilder(AbstractVan aVan) { _VanInProgress = aVan; }

		public override void BuildBody() { WriteLine("Building Van Body"); }

		public override void BuildBoot() { WriteLine("Building Van Boot"); }

		public override void BuildChassis() { WriteLine("Building Van Chassis"); }

		public override void BuildPassengerArea() { WriteLine("Building Van PassengerArea"); }

		public override void BuildWindows() { WriteLine("Building Van Windows"); }

		public override void BuildReinforcedStorageArea() { WriteLine("Building Van ReinforcedStorageArea"); }

		public override IVehicle Vehicle => _VanInProgress;

	}
}
