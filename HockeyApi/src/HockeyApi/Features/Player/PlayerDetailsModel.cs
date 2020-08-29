using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HockeyApi.Features.Player
{
    public class PlayerDetailsModel
    {
        public PlayerDetailsModel(PlayerModel playerModel, IEnumerable<RosterTransactionModel> rosterTransactions)
        {
            PlayerModel = playerModel;
            RosterTransactions = rosterTransactions ?? Enumerable.Empty<RosterTransactionModel>();
        }

        public PlayerModel PlayerModel { get; }
        public IEnumerable<RosterTransactionModel> RosterTransactions { get; }

        [JsonIgnore]
        public RosterTransactionModel LastRosterTransaction => RosterTransactions.OrderByDescending(x => x.EffectiveDate).First();

        [JsonIgnore]
        public string TeamCode => RosterTransactions.OrderByDescending(x => x.EffectiveDate).First().TeamCode;
    }
}