namespace FactoryMethodClient
{
	using Common;
	using FactoryMethod;
	using static System.Console;

	class Program
	{
		static void Main()
		{
			// Econical car, blue
			VehicleFactory vCarFactory = new CarFactory();
			IVehicle vCar = vCarFactory.Build
				(VehicleFactory.DrivingStyle.Economical, VehicleColour.Blue);
			WriteLine(vCar);

			// White van
			VehicleFactory vVanFactory = new VanFactory();
			IVehicle vVan = vVanFactory.Build
				(VehicleFactory.DrivingStyle.Powerful, VehicleColour.White);
			WriteLine(vVan);

			// Red Sports car using static factory
			IVehicle vSportsVehicle = 
				VehicleFactory.Make
				(
					VehicleFactory.BuildWhat.Car
					, VehicleFactory.DrivingStyle.Powerful
					, VehicleColour.Red
				);
			WriteLine(vSportsVehicle);

			ReadKey();
		}
	}
}
