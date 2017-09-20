namespace IoCUnitTestNUnit.Test
{
	using FluentAssertions;
	using IoCExampleSet.IoCExampleSetSupport;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class TestIoCExampleClass: MoqTestBase
	{
		[Test]
		public void TestDoTheStuffActual()
		{
			// Arrange
			IIoCExampleClass vClass =
				MockInstanceFactory.GetActual<IoCExampleClass>();

			//The following line not only returns an instance of the MOCK of the
			//depended on class,it alsoregistersthe mockasthe REAL THING.
			//So...if one were to create an instance of IoCExampleClass, the class
			//that it depends on WOULD BE THIS MOCK, NOT THE REAL THING. If, for
			//some reason, access to the actual Real Thing is needed, use
			//vMockClass.Object;
			Mock<IIoCDependedOnClass> vMockDependedOnClass =
				MockInstanceFactory.GetMock<IIoCDependedOnClass>();
			vMockDependedOnClass.Setup
				(m => m.DoTheDependedOnStuff()).Returns(Consts.REAL_STRING);

			// Act
			string vResult = vClass.DoTheStuff();

			// Assert
			vResult.Should().Be(Consts.REAL_STRING);
		}

		[Test]
		public void TestDoTheStuffMoq()
		{
			// Arrange
			IIoCExampleClass vClass =
				MockInstanceFactory.GetActual<IoCExampleClass>();
			Mock<IIoCDependedOnClass> vMockDependedOnClass =
				MockInstanceFactory.GetMock<IIoCDependedOnClass>();
			vMockDependedOnClass.Setup
				(m => m.DoTheDependedOnStuff()).Returns(Consts.MOCK_STRING);

			// Act
			string vResult = vClass.DoTheStuff();

			// Assert
			vResult.Should().Be(Consts.MOCK_STRING);
		}

	}
}
