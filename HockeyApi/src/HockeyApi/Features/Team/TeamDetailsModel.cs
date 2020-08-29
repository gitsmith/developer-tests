using System.Collections.Generic;
using System.Linq;
using HockeyApi.Features.Player;

namespace HockeyApi.Features.Team
{
    public class TeamDetailsModel
    {
        public TeamDetailsModel(TeamModel teamModel, IEnumerable<PlayerModel> activePlayers, IEnumerable<PlayerModel> inactivePlayers)
        {
            TeamModel = teamModel;
            ActivePlayers = activePlayers ?? Enumerable.Empty<PlayerModel>();
            InactivePlayers = inactivePlayers ?? Enumerable.Empty<PlayerModel>();
        }

        public TeamModel TeamModel { get; }
        public IEnumerable<PlayerModel> ActivePlayers { get; }
        public IEnumerable<PlayerModel> InactivePlayers { get; }
    }
}