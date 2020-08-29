using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HockeyApi.Common;
using Microsoft.Data.SqlClient;

namespace HockeyApi.Features
{
    public class TeamService : ITeamService
	{
		private readonly IDb _db;

		public TeamService(IDb db)
		{
			_db = db;
		}

        public IEnumerable<TeamModel> List()
		{
			return Get(new SqlCommand(@"
									SELECT
										team_code,
										team_name
									FROM
										team"),
									dataReader =>
										new TeamModel(
											dataReader.GetString(dataReader.GetOrdinal("team_code")),
												dataReader.GetString(dataReader.GetOrdinal("team_name"))));
		}

		public TeamDetailsModel Get(string code)
		{
            var teamSqlCommand = new SqlCommand(@"
                                            SELECT
                                                *
                                            FROM
                                                team
                                            WHERE
                                                team_code = @team_code");
            teamSqlCommand.Parameters.Add("@team_code", System.Data.SqlDbType.VarChar).Value = code;

            var teamModel = Get(
                teamSqlCommand,
                sqlDataReader =>
                    new TeamModel(
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("team_code")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("team_name")))).FirstOrDefault();

            var activePlayersSqlCommand = new SqlCommand(@$"
                                                        SELECT
                                                            player.player_id,
                                                            player.first_name,
                                                            player.last_name
                                                        FROM
                                                            (
                                                            SELECT
                                                                player_id,
                                                                MAX(effective_date) AS effective_date
                                                            FROM
                                                                roster_transaction
                                                            GROUP BY
                                                                player_id
                                                            ) AS most_recent_transaction
                                                        INNER JOIN
                                                            roster_transaction
                                                        ON
                                                            most_recent_transaction.player_id = roster_transaction.player_id
                                                            AND
                                                            most_recent_transaction.effective_date = roster_transaction.effective_date
                                                        INNER JOIN
                                                            player
                                                        ON
                                                            most_recent_transaction.player_id = player.player_id
                                                        WHERE
                                                            roster_transaction.team_code = @team_code
                                                            AND
                                                            roster_transaction.roster_transaction_type_id IN ({(int)RosterTransactionType.Signed}, {(int)RosterTransactionType.Healthy}, {(int)RosterTransactionType.Traded})");
            activePlayersSqlCommand.Parameters.Add("@team_code", System.Data.SqlDbType.VarChar).Value = code;

            var activePlayers = Get(
                activePlayersSqlCommand,
                sqlDataReader =>
                    new PlayerModel(
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("player_id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("first_name")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("last_name"))));

            var inactivePlayersSqlCommand = new SqlCommand(@$"
                                                    SELECT
                                                        player.player_id,
                                                        player.first_name,
                                                        player.last_name
                                                    FROM
                                                        (
                                                        SELECT
                                                            player_id,
                                                            MAX(effective_date) AS effective_date
                                                        FROM
                                                            roster_transaction
                                                        GROUP BY
                                                            player_id
                                                        ) AS most_recent_transaction
                                                    INNER JOIN
                                                        roster_transaction
                                                    ON
                                                        most_recent_transaction.player_id = roster_transaction.player_id
                                                        AND
                                                        most_recent_transaction.effective_date = roster_transaction.effective_date
                                                    INNER JOIN
                                                        player
                                                    ON
                                                        most_recent_transaction.player_id = player.player_id
                                                    WHERE
                                                        roster_transaction.team_code = @team_code
                                                        AND
                                                        roster_transaction.roster_transaction_type_id IN ({(int)RosterTransactionType.Injured})");
            inactivePlayersSqlCommand.Parameters.Add("@team_code", System.Data.SqlDbType.VarChar).Value = code;

            var inactivePlayers = Get(
                    inactivePlayersSqlCommand,
                    sqlDataReader =>
                        new PlayerModel(
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("player_id")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("first_name")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("last_name"))));

            return new TeamDetailsModel(teamModel, activePlayers, inactivePlayers);
        }

		private IEnumerable<T> Get<T>(IDbCommand dbCommand, Func<IDataReader, T> mapper)
        {
			var items = new HashSet<T>();

			using (var dbConnection = _db.CreateConnection())
            {
				dbCommand.Connection = dbConnection;

				using(var dataReader = dbCommand.ExecuteReader())
                {
					while(dataReader.Read())
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
