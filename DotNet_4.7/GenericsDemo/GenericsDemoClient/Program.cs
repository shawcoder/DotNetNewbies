namespace GenericsDemoClient
{
	using GenericsDemo;
	using System.Collections.Generic;
	using static System.Console;

	class Program
	{
		private const string STRING_1 = "Item 1";
		private const string STRING_2 = "Item 2";
		private const string STRING_3 = "Item 3";
		public static void DoItTheHardWay()
		{
			TheHardWay vList = new TheHardWay();
			vList.Add(STRING_1);
			vList.Add(STRING_2);
			vList.Add(STRING_3);
			//vList.Add(1);
			for (int vLcv = 0; vLcv < vList.HowMany; vLcv++)
			{
				WriteLine((string)vList.GetByIndex(vLcv));
			}
		}

		public static void DoItTheEasyWay()
		{
			List<string> vList = new List<string>();
			vList.Add(STRING_1);
			vList.Add(STRING_2);
			vList.Add(STRING_3);
			foreach (string vItem in vList)
			{
				WriteLine(vItem);
			}
		}

		static void Main(string[] args)
		{
			DoItTheHardWay();
			WriteLine();
			DoItTheEasyWay();
			ReadKey();
		}

	}
}
