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

            if (teamDetails != null && teamDetails.ActivePlayers.Count() < 10)
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

        public bool UpdateStatusToInjured(PlayerStatusUpdateRequest playerStatusUpdateRequest)
        {
            var playerDetails = _playerRepository.GetDetails(playerStatusUpdateRequest.PlayerId);

            if (playerDetails?.LastRosterTransaction.RosterTransactionType == RosterTransactionType.Healthy ||
                playerDetails?.LastRosterTransaction.RosterTransactionType == RosterTransactionType.Signed ||
                playerDetails?.LastRosterTransaction.RosterTransactionType == RosterTransactionType.Traded)
            {
                playerStatusUpdateRequest.RosterTransactionType = RosterTransactionType.Injured;
                playerStatusUpdateRequest.TeamCode = playerDetails.TeamCode;

                return _playerRepository.UpdateStatus(playerStatusUpdateRequest);
            }

            return false;
        }

        public bool UpdateStatusToHealthy(PlayerStatusUpdateRequest playerStatusUpdateRequest)
        {
            var playerDetails = _playerRepository.GetDetails(playerStatusUpdateRequest.PlayerId);

            var teamDetails = _teamRepository.GetDetails(playerDetails.TeamCode);

            if (teamDetails.ActivePlayers.Count() < 10 &&
                playerDetails?.LastRosterTransaction.RosterTransactionType == RosterTransactionType.Injured)
            {
                playerStatusUpdateRequest.RosterTransactionType = RosterTransactionType.Healthy;
                playerStatusUpdateRequest.TeamCode = playerDetails.TeamCode;

                return _playerRepository.UpdateStatus(playerStatusUpdateRequest);
            }

            return false;
        }

        public bool UpdateStatusToTraded(PlayerStatusUpdateRequest playerStatusUpdateRequest)
        {
            var playerDetails = _playerRepository.GetDetails(playerStatusUpdateRequest.PlayerId);

            var currentTeamDetails = _teamRepository.GetDetails(playerDetails.TeamCode);

            var destinationTeamDetails = _teamRepository.GetDetails(playerStatusUpdateRequest.TeamCode);

            if (currentTeamDetails.ActivePlayers.Count() > 4 &&
                destinationTeamDetails.ActivePlayers.Count() < 10 &&
                playerDetails?.LastRosterTransaction.RosterTransactionType != RosterTransactionType.Injured)
            {
                playerStatusUpdateRequest.RosterTransactionType = RosterTransactionType.Traded;

                return _playerRepository.UpdateStatus(playerStatusUpdateRequest);
            }

            return false;
        }
    }
}
