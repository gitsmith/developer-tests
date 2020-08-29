using System;
using System.Collections.Generic;
using System.Linq;
using HockeyApi.Common;
using Microsoft.Data.SqlClient;

namespace HockeyApi.Features.Player
{
    public class PlayerRepository : RepositoryBase, IPlayerRepository
    {
        public PlayerRepository(IDb db)
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
                                                first_name LIKE @q + '%'
                                            OR
                                                last_name LIKE @q + '%'");

            //TODO: should make q required...consider separating service from repository
            sqlCommand.Parameters.Add("@q", System.Data.SqlDbType.VarChar).Value = q ?? string.Empty;

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

        public int? Create(CreatePlayerRequest createPlayerRequest)
        {
            using (var dbConnection = Db.CreateConnection())
            {
                var dbTransaction = dbConnection.BeginTransaction();

                using (var dbCommand = dbConnection.CreateCommand())
                {
                    try
                    {
                        dbCommand.Transaction = dbTransaction;

                        dbCommand.CommandText = @"
                                        INSERT INTO
                                            player(first_name, last_name)
                                            VALUES (@first_name, @last_name);
                                            SELECT SCOPE_IDENTITY()";

                        var firstNameParameter = dbCommand.CreateParameter();
                        firstNameParameter.DbType = System.Data.DbType.String;
                        firstNameParameter.ParameterName = "@first_name";
                        firstNameParameter.Value = createPlayerRequest.FirstName;
                        dbCommand.Parameters.Add(firstNameParameter);

                        var lastNameParameter = dbCommand.CreateParameter();
                        lastNameParameter.DbType = System.Data.DbType.String;
                        lastNameParameter.ParameterName = "@last_name";
                        lastNameParameter.Value = createPlayerRequest.LastName;
                        dbCommand.Parameters.Add(lastNameParameter);

                        var playerId = Convert.ToInt32(dbCommand.ExecuteScalar());

                        dbCommand.Parameters.Clear();

                        dbCommand.CommandText = @"
                                        INSERT INTO
                                            roster_transaction(roster_transaction_type_id, player_id, team_code, effective_date)
                                            VALUES (@roster_transaction_type_id, @player_id, @team_code, @effective_date)";

                        var rosterTransactionTypeParameter = dbCommand.CreateParameter();
                        rosterTransactionTypeParameter.DbType = System.Data.DbType.Int32;
                        rosterTransactionTypeParameter.ParameterName = "@roster_transaction_type_id";
                        rosterTransactionTypeParameter.Value = RosterTransactionType.Signed;
                        dbCommand.Parameters.Add(rosterTransactionTypeParameter);

                        var playerIdParameter = dbCommand.CreateParameter();
                        playerIdParameter.DbType = System.Data.DbType.Int32;
                        playerIdParameter.ParameterName = "@player_id";
                        playerIdParameter.Value = playerId;
                        dbCommand.Parameters.Add(playerIdParameter);

                        var teamCodeParameter = dbCommand.CreateParameter();
                        teamCodeParameter.DbType = System.Data.DbType.String;
                        teamCodeParameter.ParameterName = "@team_code";
                        teamCodeParameter.Value = createPlayerRequest.TeamCode;
                        dbCommand.Parameters.Add(teamCodeParameter);

                        var effectiveDateParameter = dbCommand.CreateParameter();
                        effectiveDateParameter.DbType = System.Data.DbType.DateTime;
                        effectiveDateParameter.ParameterName = "@effective_date";
                        effectiveDateParameter.Value = createPlayerRequest.EffectiveDate;
                        dbCommand.Parameters.Add(effectiveDateParameter);

                        dbCommand.ExecuteNonQuery();

                        dbTransaction.Commit();

                        return playerId;
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            dbTransaction.Rollback();
                        }
                        catch (Exception exception2)
                        {
                            //TODO: something smart
                        }
                    }

                }
            }

            return null;
        }
    }
}
