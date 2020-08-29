using System;
using System.Collections.Generic;
using System.Linq;
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

        public PlayerDetailsModel GetDetails(int id)
        {
            var playerSqlCommand = new SqlCommand(@"
                                                SELECT
                                                    *
                                                FROM
                                                    player
                                                WHERE
                                                    player.player_id = @player_id");

            playerSqlCommand.Parameters.Add("@player_id", System.Data.SqlDbType.VarChar).Value = id;

            var player = Get(
                playerSqlCommand,
                sqlDataReader =>
                    new PlayerModel(
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("player_id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("first_name")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("last_name")))).FirstOrDefault();

            var rosterTransactionsSqlCommand = new SqlCommand(@"
                                                            SELECT
                                                                TOP 10
                                                                roster_transaction_id,
                                                                roster_transaction_type_id,
                                                                player_id,
                                                                team_code,
                                                                effective_date
                                                            FROM
                                                                roster_transaction
                                                            WHERE
                                                                roster_transaction.player_id = @player_id
                                                            ORDER BY
                                                                roster_transaction.effective_date DESC");

            rosterTransactionsSqlCommand.Parameters.Add("@player_id", System.Data.SqlDbType.VarChar).Value = id;

            var rosterTransactions = Get(
                rosterTransactionsSqlCommand,
                sqlDataReader =>
                    new RosterTransactionModel(
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("roster_transaction_id")),
                        (RosterTransactionType)sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("roster_transaction_type_id")),
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("player_id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("team_code")),
                        sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("effective_date"))));

            return new PlayerDetailsModel(player, rosterTransactions);
        }

        public string Create(CreatePlayerRequest createPlayerRequest)
        {
            return $"{createPlayerRequest.FirstName}-{createPlayerRequest.LastName}-{createPlayerRequest.TeamCode}-{createPlayerRequest.EffectiveDate}";
        }
    }
}
