using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Financial.Calculations.MSTest.Test
{
	[TestClass]
	public class TestFinancialCalculations
	{
		[TestMethod]
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
			Assert.AreEqual(vExpectedResult, vResult);
		}

	}
}
