namespace NuGetHandler.Infrastructure
{
	using System;

	/// <summary>
	/// Usage of this exception is as in the following example:
	/// throw new UnhandledSwitchCaseException
	///		(nameof(someparameter), someparametervalue))
	/// </summary>
	public class UnhandledSwitchCaseException: Exception
	{
		private const string _MESSAGE =
			"Switch parameter {0}, value {1} not handled";

		public UnhandledSwitchCaseException()
		{
			// Nothing else needed
		}

		public UnhandledSwitchCaseException(string aMessage) : base(aMessage)
		{
			// Nothing else needed here, either
		}

		public UnhandledSwitchCaseException
			(string aMessage, Exception aInner) : base(aMessage, aInner)
		{
			// And nothing else needed at this point, either.
		}

		public UnhandledSwitchCaseException(string aSwitchOn, string aSwitchValue)
			: base(string.Format(_MESSAGE, aSwitchOn, aSwitchValue))
		{
			// ...and the fun still continues.
		}

		public UnhandledSwitchCaseException
			(string aSwitchOn, string aSwitchValue, Exception aInner)
			: base(string.Format(_MESSAGE, aSwitchOn, aSwitchValue), aInner)
		{
			// ...and the fun still continues even more.
		}

	}
}
