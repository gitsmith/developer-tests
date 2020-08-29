using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public class PlayerDetailsModel
    {
        public PlayerDetailsModel(PlayerModel playerModel, IEnumerable<RosterTransactionModel> rosterTransactions)
        {
            PlayerModel = playerModel;
            RosterTransactions = rosterTransactions;
        }

        public PlayerModel PlayerModel { get; }
        public IEnumerable<RosterTransactionModel> RosterTransactions { get; }
    }
}