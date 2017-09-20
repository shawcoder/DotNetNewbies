namespace BridgeClient
{
	using Common;
	using static System.Console;

	class Program
	{
		static void Main(string[] args)
		{
			IDrivableEngine vEngine = new StandardDrivableEngine(1500);
			StandardDriverControls vStandardDriver = 
				new StandardDriverControls(vEngine);
			WriteLine("Standard Driver");
			vStandardDriver.IgnitionOn();
			vStandardDriver.Accelarate();
			vStandardDriver.Brake();
			vStandardDriver.IgnitionOff();

			vEngine = new TurboDrivableEngine(2500);
			SportDriverControls vSportDriver = new SportDriverControls(vEngine);
			WriteLine("\nSport Driver");
			vSportDriver.IgnitionOn();
			vSportDriver.Accelarate();
			vSportDriver.AccelerateHard();
			vSportDriver.BrakeHard();
			vSportDriver.Brake();
			vSportDriver.IgnitionOff();

			ReadKey();
		}

	}
}
