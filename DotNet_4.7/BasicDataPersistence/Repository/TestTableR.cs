namespace Repository
{
	using Entity;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;

	public class TestTableR
	{
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

		private SqlConnection BuildConnection()
		{
			SqlConnection vResult = new SqlConnection(BuildConnectionString());
			return vResult;
		}

		public bool Any()
		{
			string vSql =
				"IF EXISTS (SELECT TOP 1 1 FROM TestTable) SELECT 1 ELSE SELECT 0 AS Result";
			SqlCommand vCommand = new SqlCommand(vSql, BuildConnection());
			int vCount = 0;
			bool vResult;
			using (SqlDataReader vReader = vCommand.ExecuteReader())
			{
				while (vReader.Read())
				{
					vCount = (int)vReader["Result"];
				}
				vResult = (vCount > 0);
			}
			return vResult;
		}

		public bool Exists(Guid aTestTableId)
		{
			string vSql =
				"IF EXISTS (SELECT TOP 1 1 FROM TestTable WHERE TestTableId=@TestTableId) SELECT 1 ELSE SELECT 0 AS Result";
			int vCount;
			SqlParameter vIdParam =
				new SqlParameter
				{
					ParameterName = "@TestTableId"
					, DbType = DbType.Guid
					, Value = aTestTableId
					, Direction = ParameterDirection.Input
				};
			using (SqlCommand vCommand = new SqlCommand(vSql, BuildConnection()))
			{
				vCommand.Parameters.Add(vIdParam);
				vCount = vCommand.ExecuteNonQuery();
			}
			bool vResult = (vCount > 0);
			return vResult;
		}

		public List<TestTable> GetAll()
		{
			string vSql = "SELECT * FROM TestTable";
			SqlCommand vCommand = new SqlCommand(vSql, BuildConnection());
			List<TestTable> vResult = new List<TestTable>();
			using (SqlDataReader vReader = vCommand.ExecuteReader())
			{
				while (vReader.Read())
				{
					TestTable vRec = new TestTable();
					vRec.TestTableId = (Guid)vReader["TestTableId"];
					vRec.SomeInteger = (int)vReader["SomeInteger"];
					vRec.SomeString = (string)vReader["SomeSyr=tring"];
					vResult.Add(vRec);
				}
			}
			return vResult;
		}

		public TestTable Get(Guid aTestTableId)
		{
			string vSql = "SELECT TOP 1 * FROM TestTable WHERE TestTableId=@TestTableId";
			SqlParameter vIdParam =
				new SqlParameter
				{
					ParameterName = "@TestTableId"
					, DbType = DbType.Guid
					, Value = aTestTableId
					, Direction = ParameterDirection.Input
				};
			SqlCommand vCommand = new SqlCommand(vSql, BuildConnection());
			vCommand.Parameters.Add(vIdParam);
			TestTable vResult = new TestTable();
			using (SqlDataReader vReader = vCommand.ExecuteReader())
			{
				while (vReader.Read())
				{
					vResult.TestTableId = (Guid)vReader["TestTableId"];
					vResult.SomeInteger = (int)vReader["SomeInteger"];
					vResult.SomeString = (string)vReader["SomeSyr=tring"];
				}
			}
			return vResult;
		}

		public void Insert(TestTable aTestTable)
		{
			string vSql =
				"INSERT INTO TestTable (TestTableId, SomeInteger, SomeString) VALUES (NEWID(), 5, 'A String')";
			int vCount;
			using (SqlCommand vCommand = new SqlCommand(vSql, BuildConnection()))
			{
				vCount = vCommand.ExecuteNonQuery();
			}
			if (vCount != 1)
			{
				throw new Exception("Invalid Insert");
			}
		}

		public void Update(TestTable aTestTable)
		{
			string vSql =
				"UPDATE TestTable SET SomeInteger=@SomeInteger, SomeString=@SomeString WHERE TestTableId = @TestTableId";
			int vCount;
			SqlParameter vIdParam =
				new SqlParameter
				{
					ParameterName = "@TestTableId"
					, DbType = DbType.Guid
					, Value = aTestTable.TestTableId
					, Direction = ParameterDirection.Input
				};
			SqlParameter vIntParam =
				new SqlParameter
				{
					ParameterName = "@SomeInteger"
					, DbType = DbType.Int16
					, Value = aTestTable.SomeInteger
					, Direction = ParameterDirection.Input
				};

			SqlParameter vStringParam =
				new SqlParameter
				{
					ParameterName = "@SomeString"
					, DbType = DbType.String
					, Size = 10
					, Value = aTestTable.SomeString
					, Direction = ParameterDirection.Input
				};
			using (SqlCommand vCommand = new SqlCommand(vSql, BuildConnection()))
			{
				vCommand.Parameters.Add(vIdParam);
				vCommand.Parameters.Add(vIntParam);
				vCommand.Parameters.Add(vStringParam);
				vCount = vCommand.ExecuteNonQuery();
			}
			if (vCount != 1)
			{
				throw new Exception("Invalid Insert");
			}
		}

		public void Delete(Guid aTestTableId)
		{
			string vSql = $"DELETE FROM TestTable WHERE TestTableId = '{aTestTableId}'";
			int vCount;
			using (SqlCommand vCommand = new SqlCommand(vSql, BuildConnection()))
			{
				vCount = vCommand.ExecuteNonQuery();
			}
			if (vCount != 1)
			{
				throw new Exception("Invalid Insert");
			}
		}

		public void DeleteAll()
		{
			string vSql = "DELETE FROM TestTable";
			int vCount;
			using (SqlCommand vCommand = new SqlCommand(vSql, BuildConnection()))
			{
				vCount = vCommand.ExecuteNonQuery();
			}
		}

	}
}
