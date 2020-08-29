using System;
using System.Collections.Generic;
using System.Data;

namespace HockeyApi.Common
{
    public abstract class RepositoryBase
    {
        private readonly IDb _db;

        protected RepositoryBase(IDb db)
        {
            _db = db;
        }

        protected IEnumerable<T> Get<T>(IDbCommand dbCommand, Func<IDataReader, T> mapper)
        {
            var items = new HashSet<T>();

            using (var dbConnection = _db.CreateConnection())
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
