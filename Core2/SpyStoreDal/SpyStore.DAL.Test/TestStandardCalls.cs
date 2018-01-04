namespace SpyStoreDal.SpyStore.DAL.Test
{
	using System;
	using EF;
	using Microsoft.EntityFrameworkCore;
	using Models.Entities;
	using Xunit;

	[Collection("SpyStore.DAL")]
	public class TestStandardCalls : IDisposable
	{
		private readonly StoreContext _Db;

		private void CleanDatabase()
		{
			_Db.Database.ExecuteSqlCommand
				($"DELETE FROM {Category.SCHEMA_NAME}.{Category.TABLE_NAME};");
			_Db.Database.ExecuteSqlCommand
				($@"DBCC CHECKIDENT(""{Category.SCHEMA_NAME}.{Category.TABLE_NAME}"", RESEED, -1);");
		}

		public TestStandardCalls()
		{
			_Db = new StoreContext();
			CleanDatabase();
		}

		[Fact]
		public void CanWeTest()
		{
			Assert.True(true);
		}

		public void Dispose()
		{
			CleanDatabase();
			_Db.Dispose();
		}

	}
}
