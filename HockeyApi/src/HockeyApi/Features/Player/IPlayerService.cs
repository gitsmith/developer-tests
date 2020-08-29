using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public interface IPlayerService
    {
        bool UpdateStatusToInjured(PlayerStatusUpdateRequest assignToInjuredReserveRequest);
        bool UpdateStatusToHealthy(PlayerStatusUpdateRequest assignToInjuredReserveRequest);
        bool UpdateStatusToTraded(PlayerStatusUpdateRequest assignToInjuredReserveRequest);
        int? Create(CreatePlayerRequest createPlayerRequest);
        PlayerDetailsModel GetDetails(int id);
        IEnumerable<PlayerModel> Search(string q);
    }
}