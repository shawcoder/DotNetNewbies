namespace AdapterClient
{
	using Common;
	using System.Collections;
	using System.Collections.Generic;
	using Adapter;
	using static System.Console;

	class Program
	{
		static void Main(string[] args)
		{
			IList<IEngine> vEngines = new List<IEngine>();
			vEngines.Add(new StandardEngine(1300));
			vEngines.Add(new StandardEngine(1600));
			vEngines.Add(new TurboEngine(2000));
			SuperGreenEngine vSuperGreenEngine = new SuperGreenEngine(2500);
			vEngines.Add(new SuperGreenEngineAdapter(vSuperGreenEngine));
			foreach (IEngine vEngine in vEngines)
			{
				WriteLine(vEngine);
			}
			ReadKey();
		}

	}
}
