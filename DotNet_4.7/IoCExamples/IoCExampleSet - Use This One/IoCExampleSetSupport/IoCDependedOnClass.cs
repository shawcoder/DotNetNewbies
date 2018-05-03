namespace IoCExampleSet.IoCExampleSetSupport
{
	public interface IIoCDependedOnClass
	{
		string DoTheDependedOnStuff();
	}

	public class IoCDependedOnClass: IIoCDependedOnClass
	{
		public string DoTheDependedOnStuff()
		{
			string vResult = Consts.REAL_STRING;
			return vResult;
			// Do some other stuff.
		}

	}
}
