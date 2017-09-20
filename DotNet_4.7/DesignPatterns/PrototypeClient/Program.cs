namespace PrototypeClient
{
	using Common;
	using Prototype;
	using static System.Console;

	class Program
	{
		static void Main()
		{
			VehicleManager vManager = new VehicleManager();
			IVehicle vSaloon1 = vManager.CreateSaloon();
			IVehicle vSaloon2 = vManager.CreateSaloon();
			IVehicle vPickup1 = vManager.CreatePickup();
			WriteLine(vSaloon1);
			WriteLine(vSaloon2);
			WriteLine(vPickup1);

			VehicleManagerLazy vManagerLazy = new VehicleManagerLazy();
			IVehicle vSaloon3 = vManagerLazy.CreateSaloon();
			IVehicle vSaloon4 = vManagerLazy.CreateSaloon();
			IVehicle vPickup3 = vManagerLazy.CreatePickup();
			WriteLine(vSaloon3);
			WriteLine(vSaloon4);
			WriteLine(vPickup3);

			ReadKey();
		}

	}
}
