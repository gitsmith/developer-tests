using System;
namespace HockeyApi.Features.Player
{
    public class CreatePlayerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamCode { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}