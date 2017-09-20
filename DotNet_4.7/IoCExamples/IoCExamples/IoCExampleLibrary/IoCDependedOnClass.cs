namespace IoCExampleLibrary
{
	public interface IIoCDependedOnClass
	{
		string DoTheDependedOnStuff();
	}

	public class IoCDependedOnClass: IIoCDependedOnClass
	{
		public string DoTheDependedOnStuff()
		{
			string vResult = TypesAndConsts.REAL_STRING;
			return vResult;
			// Do some other stuff.
		}

	}
}
