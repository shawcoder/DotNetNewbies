namespace IoCIntegrationTest
{
	using System;
	using System.Collections.Generic;
	using System.Transactions;
	using AWEFramework.NinjectInitializer;
	using NUnit.Framework;

	[TestFixture]
	public abstract class BASE_IntegrationTest
	{
		/// <summary>
		/// This is really cool in that one instantiates the transaction at the
		/// beginning of the tests and roll it back at the end. The only thing
		/// this won't clean up is changes in integer, auto-increment primary
		/// keys. For that one needs to track the tables in which inserts occurred
		/// and call RESEED for each one.
		/// NOTE: This only works if you are using a database in which you can
		/// promote your transaction into the respective MSDTC 
		/// (MS Distributed Transaction Coordinator which is usually always 
		/// available)
		/// </summary>
		private readonly TransactionScope _TransactionScope;

		protected List<string> TablesToReseed { get; set; }

		// Must convert this from [TestFixtureSetUp] to constructor because if we 
		// are using [TestCaseSource] (which fires BEFORE [TestFixtureSetUp]) 
		// this won't get executed in time.
		protected BASE_IntegrationTest()
		{
			InitializeNinject.StartUp();
			_TransactionScope = new TransactionScope();
			TablesToReseed = new List<string>();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			// Mostly empty because Ninject knows how to stop.
			_TransactionScope.Dispose();
			List<Exception> exs = new List<Exception>();
			foreach (string vTable in TablesToReseed)
			{
				try
				{
					string vCmd = string.Format
						(
							"dbcc CHECKIDENT ('{0}', RESEED, 1); dbcc CHECKIDENT ('{0}', RESEED);"
							, vTable
						);
					// Execute the sql command via some gadget.
				}
				catch (Exception ex)
				{
					exs.Add(ex);
				}
				if (exs.Count > 0)
				{
					throw new AggregateException(exs);
				}
			}
		}

	}
}
