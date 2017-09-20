namespace SpyStore.DAL.Test
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using FluentAssertions;
	using FluentAssertions.Types;
	using NUnit.Framework;

	[TestFixture]
	public class TestTheTests
	{
		// Version 8 of TestTheTests

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
			// Is everything in the assembly either:
			// - a setup?
			// - a test fixture?
			// - ignored?
			// - abstract
			int vAbstractClassCount = 0;
			foreach (var vClass in AllTypes.From(_Assembly))
			{
				// Abstract classes must be extended to be useful and static classes
				// are both abstract and sealed (can't be extended) thus the test
				// for both abstract and sealed yields abstract classes.
				if (vClass.IsAbstract && !vClass.IsSealed)
				{
					vAbstractClassCount++;
				}
			}
			int vTestFixtureCount =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<TestFixtureAttribute>()
					.Count();
			int vIgnoredClassCount =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<IgnoreAttribute>()
					.Count();
			int vIgnoredAndFixtureCount =
				AllTypes
					.From(_Assembly)
					.ThatAreDecoratedWith<IgnoreAttribute>()
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
			int vClassCount =
				AllTypes
					.From(_Assembly)
					.Count
					(
					 aAssembly =>
						 aAssembly.IsPublic
						 && aAssembly.IsVisible
						 && !aAssembly.IsNested
					);
			vClassCount
				.Should()
				.Be
				(
				 _SetupFixtureCount + vTestFixtureCount + vIgnoredClassCount + vAbstractClassCount - vIgnoredAndFixtureCount
				 , $"Abstract Classes: {vAbstractClassCount}, SetupFixtures: {_SetupFixtureCount}, TestFixtures: {vTestFixtureCount}, Ignored Classes: {vIgnoredClassCount}, Ignored TestFixtures: {vIgnoredAndFixtureCount}"
				);
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
				int vMethodCount = vType.Methods().ThatArePublicOrInternal.Count();
				int vTestCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<TestAttribute>()
							 .Count();
				vTestCount
					.Should()
					.BeGreaterThan(0, "because {0} should have tests", vType.FullName);
				int vSetupCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<SetUpAttribute>().Count();
				vSetupCount.Should().BeInRange(0, 1);
				int vTearDownCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<TearDownAttribute>().Count();
				vTearDownCount.Should().BeInRange(0, 1);
				int vOneTimeSetupCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<OneTimeSetUpAttribute>().Count();
				vOneTimeSetupCount.Should().BeInRange(0, 1);
				int vOneTimeTearDownCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<OneTimeTearDownAttribute>().Count();
				vOneTimeTearDownCount.Should().BeInRange(0, 1);
				int vTheoryCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<TheoryAttribute>().Count();
				int vDataPointSourceCount =
					vType.Methods()
							 .ThatArePublicOrInternal
							 .ThatAreDecoratedWith<DatapointSourceAttribute>().Count();
				vTheoryCount.Should().BeInRange(0, 1);
				int vExpected =
					vSetupCount
					+ vTearDownCount
					+ vTestCount
					+ vOneTimeSetupCount
					+ vOneTimeTearDownCount
					+ vTheoryCount
					+ vDataPointSourceCount;
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
