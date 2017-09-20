namespace AbstractFactoryClient
{
	using System;
	using AbstractFactory;
	using static System.Console;

	public enum WhatToMake
	{
		Car
		, Van
	}

	class Program
	{
		/*
			Implementation details:
			- Start with interface
			- Add "functionality" using virtual properties in the abstract class

			- Downside of Abstract Factory pattern:
			- If new functionality is needed (e.g. Trailer Hitch parts), then that
				functionality must be added using the same (pardon the pun) pattern
				e.g. Add the interface, then add the abstract class, then add the
				functionality where necessary by modifying the appropriate factory
				method.
		*/
		static void Main()
		{
			WhatToMake WHAT_VEHICLE_TO_MAKE = WhatToMake.Car;
			AbstractVehicleFactory vFactory;

			switch (WHAT_VEHICLE_TO_MAKE)
			{
				case WhatToMake.Car:
				{
					vFactory = new CarFactory();
					break;
				}
				case WhatToMake.Van:
				{
					vFactory = new VanFactory();
					break;
				}
				default:
				{
					throw new Exception("Unhandled vehicle type selected!");
				}
			}
			IBody vBody = vFactory?.CreateBody();
			IChassis vChassis = vFactory?.CreateChassis();
			IGlassware vGlassware = vFactory?.CreateGlassware();
			WriteLine(vBody.BodyParts);
			WriteLine(vChassis.ChassisParts);
			WriteLine(vGlassware.GlasswareParts);
			ReadKey();
		}
	}
}
