using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HockeyApi.Common
{
	public class Db : IDb
	{
		readonly string _connStr;

		public Db(string connStr)
		{
			_connStr = connStr ?? throw new ArgumentNullException(nameof(connStr));
		}

		public IDbConnection CreateConnection()
		{
			var conn = new SqlConnection(_connStr);
			conn.Open();
			return conn;
		}
	}
}
