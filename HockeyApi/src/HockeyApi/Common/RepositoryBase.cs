using System;
using System.Collections.Generic;
using System.Data;

namespace HockeyApi.Common
{
    public abstract class RepositoryBase
    {
        protected RepositoryBase(IDb db)
        {
            Db = db;
        }

        protected IDb Db { get; }

        protected IEnumerable<T> Get<T>(IDbCommand dbCommand, Func<IDataReader, T> mapper)
        {
            var items = new HashSet<T>();

            using (var dbConnection = Db.CreateConnection())
            {
                dbCommand.Connection = dbConnection;

                using (var dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        items.Add(mapper(dataReader));
                    }
                }

                dbCommand.Dispose();
            }

            return items;
        }
    }
}
