namespace BasicDataPersistence
{
	using FluentAssertions;
	using NUnit.Framework;
	using System;
	using System.Data;
	using System.Data.SqlClient;

	[TestFixture]
	public class TestAdoDotNet
	{
		private SqlConnection _Connection;

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
			_Connection = new SqlConnection(BuildConnectionString());
			_Connection.Open();
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
			_Connection.Close();
			_Connection = null;
		}

		private string BuildConnectionString()
		{
			const string SERVER = @".\SqlExpress";
			const string DATABASE = "MetaData";
			const bool INTEGRATED_SECURITY = true;
			const string USER = "";
			const string PASSWORD = "";

			SqlConnectionStringBuilder vCsb = new SqlConnectionStringBuilder();
			vCsb.DataSource = SERVER;
			vCsb.InitialCatalog = DATABASE;
			vCsb.IntegratedSecurity = INTEGRATED_SECURITY;
			bool vTest = INTEGRATED_SECURITY;
			if (!vTest)
			{
				vCsb.UserID = USER;
				vCsb.Password = PASSWORD;
			}
			string vResult = vCsb.ConnectionString;
			return vResult;
		}

		[Test]
		public void TestBasicConnection()
		{
			// Arrange
			SqlConnection vConnection = new SqlConnection(BuildConnectionString());

			// Act
			vConnection.Open();

			// Assert
			vConnection.State.Should().Be(ConnectionState.Open);
			vConnection.Close();
		}

		[Test]
		public void TestSqlReader()
		{
			// Arrange
			string vSql = "SELECT * FROM TestTable";
			SqlCommand vCommand = new SqlCommand(vSql, _Connection);
			Guid vResult = Guid.Empty;

			// Act
			using (SqlDataReader vReader = vCommand.ExecuteReader())
			{
				while (vReader.Read())
				{
					vResult = (Guid)vReader["TestTableId"];
				}
			}

			// Assert
			vResult.Should().NotBeEmpty();
		}

		[Test]
		public void TestInsertCommand()
		{
			// Arrange
			string vSql =
				"INSERT INTO TestTable (TestTableId, SomeInteger, SomeString) VALUES (NEWID(), 5, 'A String')";
			int vCount;

			// Act
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCount = vCommand.ExecuteNonQuery();
			}

			// Assert
			vCount.Should().Be(1);
		}

		private Guid FetchId()
		{
			string vSql = "SELECT * FROM TestTable";
			SqlCommand vCommand = new SqlCommand(vSql, _Connection);
			Guid vResult = Guid.Empty;
			using (SqlDataReader vReader = vCommand.ExecuteReader())
			{
				while (vReader.Read())
				{
					vResult = (Guid)vReader["TestTableId"];
				}
			}
			return vResult;
		}

		[Test]
		public void TestDeleteCommand()
		{
			// Arrange
			string vSql = $"DELETE FROM TestTable WHERE TestTableId = '{FetchId()}'";
			int vCount;

			// Act
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCount = vCommand.ExecuteNonQuery();
			}

			// Assert
			vCount.Should().Be(1);
		}

		private void DeleteAllRecords()
		{
			string vSql = $"DELETE FROM TestTable";

			// Act
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCommand.ExecuteNonQuery();
			}
		}

		private void InsertARecord()
		{
			string vSql =
				"INSERT INTO TestTable (TestTableId, SomeInteger, SomeString) VALUES (NEWID(), 5, 'A String')";
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCommand.ExecuteNonQuery();
			}
		}

		[Test]
		public void TestUpdateCommand()
		{
			// Arrange
			DeleteAllRecords();
			InsertARecord();
			string vSql = "UPDATE TestTable SET SomeInteger = 5";
			int vCount;

			// Act
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCount = vCommand.ExecuteNonQuery();
			}

			// Assert
			vCount.Should().Be(1);
		}

		[Test]
		public void TestParamaterizedUpdate()
		{
			// Arrange
			DeleteAllRecords();
			InsertARecord();
			string vSql =
				"UPDATE TestTable SET SomeInteger=@SomeInteger, SomeString=@SomeString WHERE TestTableId = @TestTableId";
			int vCount;
			SqlParameter vIdParam =
				new SqlParameter
				{
					ParameterName = "@TestTableId"
					, DbType = DbType.Guid
					, Value = FetchId()
					, Direction = ParameterDirection.Input
				};
			SqlParameter vIntParam =
				new SqlParameter
				{
					ParameterName = "@SomeInteger"
					, DbType = DbType.Int16
					, Value = 42
					, Direction = ParameterDirection.Input
				};

			SqlParameter vStringParam =
				new SqlParameter
				{
					ParameterName = "@SomeString"
					, DbType = DbType.String
					, Size = 10
					, Value = "A New String"
					, Direction = ParameterDirection.Input
				};
			// Act
			using (SqlCommand vCommand = new SqlCommand(vSql, _Connection))
			{
				vCommand.Parameters.Add(vIdParam);
				vCommand.Parameters.Add(vIntParam);
				vCommand.Parameters.Add(vStringParam);
				vCount = vCommand.ExecuteNonQuery();
			}

			// Assert
			vCount.Should().Be(1);
		}

	}
}
