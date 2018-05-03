namespace IoCUnitTestNUnit
{
	using Moq;
	using Ninject;
	using Ninject.MockingKernel.Moq;
	using NUnit.Framework;

	public class MoqTestBase: TestBase
	{
		protected IMockInstanceFactory MockInstanceFactory { get; private set; }

		[SetUp]
		public void Setup()
		{
			// MockBehavior.Strict requires all methods of mocked class to be
			// implemented.
			// DefaultValue.Mock when mock is created with properties, always create
			// mocks of the necessary items instead of just setting them to null.
			NinjectSettings vSettings = new NinjectSettings();
			vSettings.SetMockBehavior
				(IoCUnitTestNUnit.MockInstanceFactory.DEFAULT_MOCK_BEHAVIOR);
			MoqMockingKernel vKernel = new MoqMockingKernel(vSettings);
			MockRepository vMockRepository = vKernel.MockRepository;
			vMockRepository.DefaultValue = DefaultValue.Mock;
			MockInstanceFactory = new MockInstanceFactory(vKernel);
		}

		[TearDown]
		public void TearDown()
		{
			MockInstanceFactory.VerifyAll();
			MockInstanceFactory = null;
		}

	}
}
