using System.Collections.Generic;
using System.Linq;
using HockeyApi.Features.Team;

namespace HockeyApi.Features.Player
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        public int? Create(CreatePlayerRequest createPlayerRequest)
        {
            var teamDetails = _teamRepository.GetDetails(createPlayerRequest.TeamCode);

            if (teamDetails != null & teamDetails.ActivePlayers.Count() < 10)
            {
                return _playerRepository.Create(createPlayerRequest);
            }

            return null;
        }

        public PlayerDetailsModel GetDetails(int id)
        {
            return _playerRepository.GetDetails(id);
        }

        public IEnumerable<PlayerModel> Search(string q)
        {
            return _playerRepository.Search(q);
        }
    }
}
