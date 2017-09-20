namespace IoCExampleSet.IoCExampleSetSupport
{
	using System;

	public interface IIoCExampleClass
	{
		string DoTheStuff();
	}

	/// <summary>
	/// This class depends on an instance of the IIoCDependedOnClass. We are
	/// doing this in the example to demonstrate that if the constructor of this
	/// class succeeds, then we know that the depended on class has been 
	/// successfully instantiated as well. Hence Dependency Injection!
	/// </summary>
	public class IoCExampleClass: IIoCExampleClass
	{
		private readonly IIoCDependedOnClass _IoCDependedOnClass;

		public IoCExampleClass(IIoCDependedOnClass aIoCDependedOnClass)
		{
			if (aIoCDependedOnClass == null)
			{
				throw new ArgumentNullException(nameof(aIoCDependedOnClass));
			}
			_IoCDependedOnClass = aIoCDependedOnClass;
			// Could take in other stuff, but this is an example.
		}

		public string DoTheStuff()
		{
			string vResult = _IoCDependedOnClass.DoTheDependedOnStuff();
			return vResult;
			// Put some meaningful code here.
		}

	}
}
