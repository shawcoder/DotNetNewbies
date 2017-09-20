namespace Financial.Calculations.NUnit.Test
{
	using System;
	using FluentAssertions;
	using global::NUnit.Framework;

	[TestFixture]
	public class TestTestFinancialCalculations
	{
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
		public void TestAddTwoNumbers()
		{
			// Arrange
			SByte vFirstNumber = 25;
			SByte vSecondNumber = 50;
			SByte vExpectedResult = 76;

			// Act
			SByte vResult =
				FinancialCalculations.AddTwoNumbers(vFirstNumber, vSecondNumber);

			// Assert
			vResult.Should().Be(vExpectedResult);
		}


	}
}
