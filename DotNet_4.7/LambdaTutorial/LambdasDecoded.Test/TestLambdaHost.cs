namespace LambdasDecoded.Test
{
	using FluentAssertions;
	using LambdaTutorial.LambdasDecoded;
	using NUnit.Framework;
	using static LambdaTutorial.LambdasDecoded.LambdaHost;

	[TestFixture]
	public class TestLambdaHost
	{
		private LambdaHost _LambdaHost;

		/// <summary>
		/// This attribute is used inside a TestFixture to provide a common set of 
		/// functions that are performed just before each test method is called.
		/// SetUp methods may be either static or instance methods and you may 
		/// define more than one of them in a fixture.Normally, multiple SetUp 
		/// methods are only defined at different levels of an inheritance 
		/// hierarchy, as explained below.
		/// If a SetUp method fails or throws an exception, the test is not 
		/// executed and a failure or error is reported.
		/// The SetUp attribute is inherited from any base class. Therefore, if a 
		/// base class has defined a SetUp method, that method will be called 
		/// before each test method in the derived class.
		/// You may define a SetUp method in the base class and another in the 
		/// derived class. NUnit will call base class SetUp methods before those 
		/// in the derived classes.
		/// Notes:
		///     Although it is possible to define multiple SetUp methods in the same 
		///     class, you should rarely do so. Unlike methods defined in separate 
		///     classes in the inheritance hierarchy, the order in which they are 
		///     executed is not guaranteed.
		/// SetUp methods may be async if running under .NET 4.0 or higher.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			_LambdaHost = new LambdaHost(); ;
		}

		/// <summary>
		/// This attribute is used inside a TestFixture to provide a common set of 
		/// functions that are performed after each test method.
		/// TearDown methods may be either static or instance methods and you may 
		/// define more than one of them in a fixture.Normally, multiple TearDown 
		/// methods are only defined at different levels of an inheritance 
		/// hierarchy, as explained below.
		/// So long as any SetUp method runs without error, the TearDown method is 
		/// guaranteed to run.It will not run if a SetUp method fails or throws an 
		/// exception.
		/// The TearDown attribute is inherited from any base class. Therefore, if 
		/// a base class has defined a TearDown method, that method will be called 
		/// after each test method in the derived class.
		/// You may define a TearDown method in the base class and another in the 
		/// derived class. NUnit will call base class TearDown methods after those 
		/// in the derived classes.
		/// Notes:
		/// 1.Although it is possible to define multiple TearDown methods in the 
		///     same class, you should rarely do so.Unlike methods defined in 
		///     separate classes in the inheritance hierarchy, the order in which 
		///     they are executed is not guaranteed.
		/// 2.TearDown methods may be async if running under .NET 4.0 or higher.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
		}

		/* The CORRECT way to perform a unit test that is EXPECTED to throw an
						exception follows:

						// Act
						Action vResult = () => vTest.IsMember<TestEnum>();

						// Assert
						vResult.ShouldThrow<Exception>();

		*/

		[Test]
		public void TestAddTheTwoNumbersAndReturnTheResultWithADelegate()
		{
			// Arrange
			int vExpected = FIRST_NUMBER + SECOND_NUMBER;

			// Act
			int vResult =
				_LambdaHost.AddTheTwoNumbersAndReturnTheResultWithADelegate
					(FIRST_NUMBER, SECOND_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestTheSimplestLambdaExpressionAsAFunction()
		{
			// Arrange
			int vExpected = FIRST_NUMBER;

			// Act
			int vResult = _LambdaHost.TheSimplestLambdaExpressionAsAFunction();

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestTheSimplestLambdaExpression()
		{
			// Arrange
			int vExpected = FIRST_NUMBER;

			// Act
			int vResult = _LambdaHost.TheSimplestLambdaExpression();

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample2CallsAreEquivalent()
		{
			// Arrange
			int vExpected = _LambdaHost.TheSimplestLambdaExpressionAsAFunction();

			// Act
			int vResult = _LambdaHost.TheSimplestLambdaExpression();

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample3MethodEquivalent()
		{
			// Arrange
			int vExpected = FIRST_NUMBER + SECOND_NUMBER;

			// Act
			int vResult =
				_LambdaHost.Example3MethodEquivalent(FIRST_NUMBER, SECOND_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}


		[Test]
		public void TestExample3Equivalency()
		{
			// Arrange
			int vExpected =
				_LambdaHost.Example3MethodEquivalent(FIRST_NUMBER, SECOND_NUMBER);

			// Act
			int vResult =
				_LambdaHost.Example3LambdaWithParameters(FIRST_NUMBER, SECOND_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample4MethodEquivalent()
		{
			// Arrange
			int vExpected = TEST_STRING.Length + FIRST_NUMBER;

			// Act
			int vResult =
				_LambdaHost.Example4MethodEquivalent(TEST_STRING, FIRST_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample4Lambda()
		{
			// Arrange
			int vExpected = TEST_STRING.Length + FIRST_NUMBER;

			// Act
			int vResult = _LambdaHost.Example4Lambda(TEST_STRING, FIRST_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample4Equivalent()
		{
			// Arrange
			int vExpected =
				_LambdaHost.Example4MethodEquivalent(TEST_STRING, FIRST_NUMBER);

			// Act
			int vResult = _LambdaHost.Example4Lambda(TEST_STRING, FIRST_NUMBER);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample5SumTheListOfNumbersThatAreEvenMethod()
		{
			// Arrange
			int vExpected = 2 + 4 + 6 + 8 + 10;

			// Act
			int vResult =
				_LambdaHost.Example5SumTheListOfNumbersThatAreEvenMethod
					(_LambdaHost.OneThruTen);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample5Lambda()
		{
			// Arrange
			int vExpected = 2 + 4 + 6 + 8 + 10;

			// Act
			int vResult = _LambdaHost.Example5Lambda(_LambdaHost.OneThruTen);

			// Assert
			vResult.Should().Be(vExpected);
		}

		[Test]
		public void TestExample5Equivalent()
		{
			// Arrange
			int vExpected =
				_LambdaHost.Example5SumTheListOfNumbersThatAreEvenMethod
					(_LambdaHost.OneThruTen);

			// Act
			int vResult = _LambdaHost.Example5Lambda(_LambdaHost.OneThruTen);

			// Assert
			vResult.Should().Be(vExpected);
		}

	}
}
