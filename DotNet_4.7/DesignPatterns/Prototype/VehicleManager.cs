namespace Prototype
{
	using Common;

	public class VehicleManager
	{
		const int _ENGINE_SIZE = 1300;
		private IVehicle _Saloon;
		private IVehicle _Coupe;
		private IVehicle _Sport;
		private IVehicle _BoxVan;
		private IVehicle _Pickup;

		public VehicleManager()
		{
			// To make it simple all vehicles use the same engine
			_Saloon =
				new Saloon(new StandardEngine(_ENGINE_SIZE), VehicleColour.Black);
			_Coupe = new Coupe(new StandardEngine(_ENGINE_SIZE), VehicleColour.Green);
			_Sport = new Sport(new StandardEngine(_ENGINE_SIZE), VehicleColour.Blue);
			_BoxVan =
				new BoxVan(new StandardEngine(_ENGINE_SIZE), VehicleColour.Green);
			_Pickup =
				new Pickup(new StandardEngine(_ENGINE_SIZE), VehicleColour.Silver);
		}

		public virtual IVehicle CreateSaloon() { return (IVehicle)_Saloon.Clone(); }
		public virtual IVehicle CreateCoupe() { return (IVehicle)_Coupe.Clone(); }
		public virtual IVehicle CreateSport() { return (IVehicle)_Sport.Clone(); }
		public virtual IVehicle CreateBoxVan() { return (IVehicle)_BoxVan.Clone(); }
		public virtual IVehicle CreatePickup() { return (IVehicle)_Pickup.Clone(); }
	}
}
