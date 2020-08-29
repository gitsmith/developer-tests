using System;

namespace HockeyApi.Features.Player
{
    public class RosterTransactionModel
    {
        public RosterTransactionModel(int id, RosterTransactionType rosterTransactionType, int playerId, string teamCode, DateTime effectiveDate)
        {
            Id = id;
            RosterTransactionType = rosterTransactionType;
            PlayerId = playerId;
            TeamCode = teamCode;
            EffectiveDate = effectiveDate;
        }

        public int Id { get; }
        public RosterTransactionType RosterTransactionType { get; }
        public int PlayerId { get; }
        public string TeamCode { get; }
        public DateTime EffectiveDate { get; }
    }
}