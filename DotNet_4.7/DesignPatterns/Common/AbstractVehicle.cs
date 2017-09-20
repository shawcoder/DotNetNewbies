namespace Common
{

	public abstract class AbstractVehicle: IVehicle
	{
		private readonly IEngine _Engine;
		private VehicleColour _Colour;

		protected AbstractVehicle(IEngine engine)
			: this(engine, VehicleColour.Unpainted)
		{
		}

		protected AbstractVehicle(IEngine engine, VehicleColour colour)
		{
			_Engine = engine;
			_Colour = colour;
		}

		public virtual IEngine Engine => _Engine;

		public virtual VehicleColour Colour => _Colour;

		public virtual void Paint(VehicleColour colour) { _Colour = colour; }

		public override string ToString()
		{
			return GetType().Name + " (" + _Engine + ", " + _Colour + ")";
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public virtual object Clone() { return MemberwiseClone(); }

	}

}