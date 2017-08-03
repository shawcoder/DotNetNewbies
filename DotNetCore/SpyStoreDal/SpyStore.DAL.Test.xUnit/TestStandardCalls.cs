namespace SpyStoreDal.SpyStore.DAL.Test.xUnit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using EF;
	using FluentAssertions;
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
					CategoryName = "Foo"
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
			const string FOO = "Foo";
			const string BAR = "Bar";
			_Db.Categories.Add(new Category { CategoryName = FOO });
			_Db.Categories.Add(new Category { CategoryName = BAR });
			_Db.SaveChanges();

			// Act
			List<Category> vCategories =
				_Db.Categories.OrderBy(o => o.CategoryName).ToList();

			// Assert
			vCategories.Count.Should().Be(2);
			vCategories[0].CategoryName.Should().Be(BAR);
			vCategories[1].CategoryName.Should().Be(FOO);
		}

		[Fact]
		public void TestUpdateACategory()
		{
			// Arrange
			const string FOO = "Foo";
			const string BAR = "Bar";

			Category vCategory =
				new Category
				{
					CategoryName = FOO
				};
			_Db.Categories.Add(vCategory);
			_Db.SaveChanges();
			vCategory.CategoryName = BAR;

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
			vExpected.Should().Be(BAR);
		}

		public void Dispose()
		{
			CleanDatabase();
			_Db.Dispose();
		}

	}
}