namespace BuilderClient
{
	using System;
	using Common;
	using Builder;
	using static System.Console;

	class Program
	{
		static void Main()
		{
			/*
				- Builder class contains all possible build methods for all possible
					vehicle types.
			*/
			AbstractCar vCar = new Saloon(new StandardEngine(1300));
			VehicleBuilder vBuilder = new CarBuilder(vCar);
			VehicleDirector vDirector = new CarDirector();
			Common.IVehicle vVehicle = vDirector.Build(vBuilder);
			WriteLine(vVehicle);
			ReadKey();
		}
	}
}
