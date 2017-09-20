namespace AWE.Testing.NUnit
{
	using FluentAssertions;
	using FluentAssertions.Types;
	using global::NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	[TestFixture]
	public class TestTheTests
	{
		private const int _ALLOWED_SETUP_FIXTURE_COUNT = 1;
		private Assembly _Assembly;
		private int _SetupFixtureCount;

		[SetUp]
		public void Setup()
		{
			_Assembly = Assembly.GetAssembly(typeof(TestTheTests));
			_SetupFixtureCount =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<SetUpFixtureAttribute>()
					.Count();
		}

		[TearDown]
		public void TearDown()
		{
			_Assembly = null;
		}

		[Test]
		public void ValidateFixtures()
		{
			// Arrange
			// Is everything in the assembly either a setup or a test fixture?
			int vTestFixtureCount =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<TestFixtureAttribute>()
					.Count();
			// Act

			// Assert
			_SetupFixtureCount
				.Should()
				.BeInRange
					(
						0
						, _ALLOWED_SETUP_FIXTURE_COUNT
						, "there is supposed to be only {0} SetupFixture"
						, _ALLOWED_SETUP_FIXTURE_COUNT
					);
			int vFixtureCount = AllTypes.From(_Assembly).Count();
			vFixtureCount.Should().Be(_SetupFixtureCount + vTestFixtureCount);
		}

		[Test]
		public void ValidateSetupFixture()
		{
			// Arrange
			if (_SetupFixtureCount == 0)
			{
				Assert.True(true); // Don't need to perform this test
			}

			// Act

			// Assert
			_SetupFixtureCount
				.Should()
				.BeInRange
					(
						0
						, _ALLOWED_SETUP_FIXTURE_COUNT
						, "there is supposed to be only {0} SetupFixture"
						, _ALLOWED_SETUP_FIXTURE_COUNT
					);
			AllTypes
				.From(_Assembly)
				.ThatAreDecoratedWith<SetUpFixtureAttribute>()
				.Methods()
				.ThatAreDecoratedWith<OneTimeSetUpAttribute>()
				.Count()
				.Should()
				.BeInRange(0, 1);
			AllTypes
				.From(_Assembly)
				.ThatAreDecoratedWith<SetUpFixtureAttribute>()
				.Methods()
				.ThatAreDecoratedWith<OneTimeTearDownAttribute>()
				.Count()
				.Should()
				.BeInRange(0, 1);
		}

		[Test]
		public void ValidateTests()
		{
			// Arrange
			List<Type> vTestFixtures =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<TestFixtureAttribute>()
					.ToList();

			// Act

			// Assert
			foreach (Type vType in vTestFixtures)
			{
				string vTypeName = vType.Name;
				// BEWARE! static methods are NOT counted by the following
				int vMethodCount = vType.Methods().ThatArePublicOrInternal.Count();
				int vTestCount =
					vType.Methods().ThatAreDecoratedWith<TestAttribute>().Count();
				int vSetupCount =
					vType.Methods().ThatAreDecoratedWith<SetUpAttribute>().Count();
				vSetupCount.Should().BeInRange(0, 1);
				int vTearDownCount =
					vType.Methods().ThatAreDecoratedWith<TearDownAttribute>().Count();
				vTearDownCount.Should().BeInRange(0, 1);
				int vExpected = vSetupCount + vTearDownCount + vTestCount;
				vMethodCount
					.Should()
					.Be
						(
							vExpected
							, " method(s) in {0} should be marked as Test, Setup or TearDown"
							, vType.Name
						);
			}
		}

	}
}
