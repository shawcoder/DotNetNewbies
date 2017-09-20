namespace Common
{
	using System;

	public interface IVehicle: ICloneable
	{
		void Paint(VehicleColour colour);

		IEngine Engine { get; }
		VehicleColour Colour { get; }

	}
}