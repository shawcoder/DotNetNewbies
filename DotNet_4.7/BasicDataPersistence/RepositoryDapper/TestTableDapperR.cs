namespace RepositoryDapper
{
	using Dapper;
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;

	public class TestTableDapperR
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
			const string SQL =
				"IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ApplicationStatus])"
					+ " SELECT 1 ELSE SELECT 0;";
			using (IDbConnection vDb = BuildConnection())
			{
				int vCount = vDb.Query<int>(SQL).Single();
				vDb.Close();
				bool vResult = (vCount == 1);
				return vResult;
			}
		}

		public bool Exists(Int32 aApplicationStatusId)

		{
			const string SQL =
				"IF EXISTS (SELECT TOP 1 42 FROM [dbo].[ApplicationStatus] WHERE"
					+ " ([dbo].[ApplicationStatus].[ApplicationStatusId] = @ApplicationStatusId)"
					+ ") SELECT 1 ELSE SELECT 0;";
			using (IDbConnection vDb = BuildConnection())
			{
				int vCount = vDb.Query<int>(SQL, new { ApplicationStatusId = aApplicationStatusId }).Single();
				vDb.Close();
				bool vResult = (vCount == 1);
				return vResult;
			}
		}


	}
}
