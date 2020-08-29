using System;
using System.Text.Json.Serialization;

namespace HockeyApi.Features.Player
{
    public class PlayerStatusUpdateRequest
    {
        [JsonIgnore]
        public RosterTransactionType RosterTransactionType { get; set; }

        [JsonIgnore]
        public int PlayerId { get; set; }

        public string TeamCode { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
