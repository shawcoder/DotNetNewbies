namespace Prototype
{
	using Common;

	public class VehicleManagerLazy
	{
		const int _ENGINE_SIZE = 1300;
		private IVehicle _Saloon;
		private IVehicle _Coupe;
		private IVehicle _Sport;
		private IVehicle _BoxVan;
		private IVehicle _Pickup;

		public virtual IVehicle CreateSaloon()
		{
			_Saloon = _Saloon ?? new Saloon(new StandardEngine(_ENGINE_SIZE));
			return (IVehicle)_Saloon.Clone();
		}

		public virtual IVehicle CreateCoupe()
		{
			_Coupe = _Coupe ?? new Coupe(new StandardEngine(_ENGINE_SIZE));
			return (IVehicle)_Coupe.Clone();
		}

		public virtual IVehicle CreateSport()
		{
			_Sport = _Sport ?? new Sport(new StandardEngine(_ENGINE_SIZE));
			return (IVehicle)_Sport.Clone();
		}

		public virtual IVehicle CreateBoxVan()
		{
			_BoxVan = _BoxVan ?? new BoxVan(new StandardEngine(_ENGINE_SIZE));
			return (IVehicle)_BoxVan.Clone();
		}

		public virtual IVehicle CreatePickup()
		{
			_Pickup = _Pickup ?? new Pickup(new StandardEngine(_ENGINE_SIZE));
			return (IVehicle)_Pickup.Clone();
		}

	}
}
