using System;
using System.Collections.Generic;
using HockeyApi.Common;
using Microsoft.Data.SqlClient;

namespace HockeyApi.Features.Player
{
    public class PlayerService : RepositoryBase, IPlayerService
    {
        public PlayerService(IDb db)
            : base(db)
        {
        }

        public IEnumerable<PlayerModel> Search(string q)
        {
            var sqlCommand = new SqlCommand(@"
                                            SELECT
                                                TOP 10
                                                *
                                            FROM
                                                player
                                            WHERE
                                                first_name LIKE @query + '%'
                                            OR
                                                last_name LIKE @query + '%'");

            //TODO: should make q required...consider separating service from repository
            sqlCommand.Parameters.Add("@query", System.Data.SqlDbType.VarChar).Value = q ?? string.Empty;

            return Get(
                sqlCommand,
                sqlDataReader =>
                    new PlayerModel(
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("player_id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("first_name")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("last_name"))));
        }
    }
}
