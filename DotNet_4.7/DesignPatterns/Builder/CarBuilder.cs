namespace Builder
{
	using Common;
	using static System.Console;

	public class CarBuilder: VehicleBuilder
	{
		private readonly AbstractCar _CarInProgress;

		public CarBuilder(AbstractCar aCar) { _CarInProgress = aCar; }

		public override void BuildBody() { WriteLine("Building Car Body"); }

		public override void BuildBoot() { WriteLine("Building Car Boot"); }

		public override void BuildChassis() { WriteLine("Building Car Chassis"); }

		public override void BuildPassengerArea() { WriteLine("Building Car PassengerArea"); }

		public override void BuildWindows() { WriteLine("Building Car Windows"); }

		public override IVehicle Vehicle => _CarInProgress;

	}
}
