namespace NuGetHandler.Infrastructure
{
	public static class IntHelper
	{
		public static bool IsOdd(this int aInt)
		{
			bool vResult = (aInt % 2) == 1;
			return vResult;
		}

	}
}
