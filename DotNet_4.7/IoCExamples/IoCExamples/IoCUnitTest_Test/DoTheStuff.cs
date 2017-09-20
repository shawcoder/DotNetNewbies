namespace IoCUnitTest_Test
{
	using IoCUnitTest;
	using Moq;
	using NUnit.Framework;
	using IoCExampleLibrary;

	public class DoTheStuff: BASE_MoqTest
	{
		[Test]
		public void DoTheStuffBasicTest()
		{
			// Arrange
			const string EXPECTED_RESULT = TypesAndConsts.REAL_STRING;

			// Act
			IoCExampleClass vClass = MockInstanceFactory.GetActual<IoCExampleClass>();
			// The following line not only returns an instance of a MOCK of the
			// depended on class, it also registers the mock as the REAL THING
			// So...if one were to create an instance of IoCExampleClass, the
			// class that it depends on WOULD BE THIS MOCK, NOT THE REAL THING!
			// If one wants to access the Real Thing, use:
			// vMockClass.Object; is the Real IoCDependedOnClass
			Mock<IIoCDependedOnClass> vMockDependedOnClass =
				MockInstanceFactory.GetMock<IIoCDependedOnClass>();
			// Now, fixup the method in the depended on class such that it will
			// return something OTHER than the TypesAndConsts.REAL_STRING value.
			// This will demonstrate that the mock and it's new method are what
			// are being executed and tested against.
			vMockDependedOnClass.Setup
				(m => m.DoTheDependedOnStuff()).Returns(EXPECTED_RESULT);
			string vRealResult = vClass.DoTheStuff();

			// Assert
			Assert.That(vRealResult, Is.EqualTo(EXPECTED_RESULT));
		}

		[Test]
		public void DoTheStuffMocked()
		{
			// Arrange
			const string EXPECTED_RESULT = TypesAndConsts.MOCK_STRING;

			// The following line not only returns an instance of a MOCK of the
			// depended on class, it also registers the mock as the REAL THING
			// So...if one were to create an instance of IoCExampleClass, the
			// class that it depends on WOULD BE THIS MOCK, NOT THE REAL THING!
			// If one wants to access the Real Thing, use:
			// vMockClass.Object; is the Real IoCDependedOnClass
			Mock<IIoCDependedOnClass> vMockDependedOnClass =
				MockInstanceFactory.GetMock<IIoCDependedOnClass>();
			// Now, fixup the method in the depended on class such that it will
			// return something OTHER than the TypesAndConsts.REAL_STRING value.
			// This will demonstrate that the mock and it's new method are what
			// are being executed and tested against.
			vMockDependedOnClass.Setup
				(m => m.DoTheDependedOnStuff()).Returns(EXPECTED_RESULT);

			// Act
			IoCExampleClass vClass = MockInstanceFactory.GetActual<IoCExampleClass>();
			string vRealResult = vClass.DoTheStuff();

			// Assert
			Assert.That(vRealResult, Is.EqualTo(EXPECTED_RESULT));
		}

	}
}