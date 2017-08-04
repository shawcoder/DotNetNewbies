namespace SpyStoreDal.SpyStore.DAL.Test.xUnit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using EF;
	using FluentAssertions;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Storage;
	using Models.Entities;
	using Xunit;

	[Collection("SpyStore.DAL")]
	public class TestStandardCalls : IDisposable
	{
		private const string _FOO = "Foo";
		private const string _BAR = "Bar";

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
			true.Should().BeTrue();
		}

		[Fact]
		public void TestAddACategory()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = "Foo"
				};

			// Act
			_Db.Categories.Add(vCategory);

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Added);
			vCategory.Id.Should().BeLessThan(0);
			vCategory.TimeStamp.Should().BeNull();

			// Act
			_Db.SaveChanges();

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Unchanged);
			vCategory.Id.Should().Be(0);
			vCategory.TimeStamp.Should().NotBeNull();
			_Db.Categories.Count().Should().Be(1);
		}

		[Fact]
		public void TestAddACategoryFromContext()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};

			// Act
			_Db.Add(vCategory);

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Added);
			vCategory.Id.Should().BeLessThan(0);
			vCategory.TimeStamp.Should().BeNull();

			// Act
			_Db.SaveChanges();

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Unchanged);
			vCategory.Id.Should().Be(0);
			vCategory.TimeStamp.Should().NotBeNull();
			_Db.Categories.Count().Should().Be(1);
		}

		[Fact]
		public void TestFetchAllCategories()
		{
			// Arrange
			_Db.Categories.Add(new Category { CategoryName = _FOO });
			_Db.Categories.Add(new Category { CategoryName = _BAR });
			_Db.SaveChanges();

			// Act
			List<Category> vCategories =
				_Db.Categories.OrderBy(o => o.CategoryName).ToList();

			// Assert
			vCategories.Count.Should().Be(2);
			vCategories[0].CategoryName.Should().Be(_BAR);
			vCategories[1].CategoryName.Should().Be(_FOO);
		}

		[Fact]
		public void TestUpdateACategory()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};
			_Db.Categories.Add(vCategory);
			_Db.SaveChanges();
			vCategory.CategoryName = _BAR;

			// Act
			_Db.Update(vCategory);

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Modified);

			// Act
			_Db.SaveChanges();

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Unchanged);

			// Arrange
			StoreContext vContext;
			string vExpected;

			// Act
			using (vContext = new StoreContext())
			{
				vExpected = vContext.Categories.First().CategoryName;
			}

			// Assert
			vExpected.Should().Be(_BAR);
		}

		[Fact]
		public void TestShouldNotUpdateANonAttachedCategory()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};
			_Db.Categories.Add(vCategory);
			vCategory.CategoryName = _BAR;

			// Act
			Action vResult = () => _Db.Categories.Update(vCategory);

			// Assert
			vResult.ShouldThrow<InvalidOperationException>();
		}

		[Fact]
		public void TestShouldDeleteACategoryWithTimestampData()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};
			_Db.Categories.Add(vCategory);
			_Db.SaveChanges();
			StoreContext vContext = new StoreContext();
			Category vCategoryToDelete =
				new Category
				{
					Id = vCategory.Id
					, TimeStamp = vCategory.TimeStamp
				};
			vContext.Entry(vCategoryToDelete).State = EntityState.Deleted;

			// Act
			int vResult = vContext.SaveChanges();

			// Assert
			vResult.Should().Be(1);
		}

		[Fact]
		public void TestShouldNotDeleteACategoryWithoutTimestapData()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};
			_Db.Categories.Add(vCategory);
			_Db.SaveChanges();
			StoreContext vContext = new StoreContext();
			Category vCategoryToDelete =
				new Category
				{
					Id = vCategory.Id
				};
			vContext.Categories.Remove(vCategoryToDelete);

			// Act
			RetryLimitExceededException vResult =
				Assert.Throws<RetryLimitExceededException>
					(() => vContext.SaveChanges());
			DbUpdateConcurrencyException vInnerException =
				vResult.InnerException as DbUpdateConcurrencyException;

			// Assert
			vInnerException?.Entries.Count.Should().Be(1);
			((Category)vInnerException?.Entries[0].Entity)?.Id.Should().Be(vCategory.Id);
		}

		[Fact]
		public void TestShouldDeleteACategory()
		{
			// Arrange
			Category vCategory =
				new Category
				{
					CategoryName = _FOO
				};

			// Act
			_Db.Categories.Add(vCategory);
			_Db.SaveChanges();

			// Assert
			_Db.Categories.Count().Should().Be(1);

			// Act
			_Db.Categories.Remove(vCategory);

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Deleted);

			// Act
			_Db.SaveChanges();

			// Assert
			_Db.Entry(vCategory).State.Should().Be(EntityState.Detached);
			_Db.Categories.Count().Should().Be(0);
		}

		public void Dispose()
		{
			CleanDatabase();
			_Db.Dispose();
		}

	}
}