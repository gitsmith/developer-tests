using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public interface IPlayerRepository
    {
        bool UpdateStatus(PlayerStatusUpdateRequest assignToInjuredReserveRequest);
        int? Create(CreatePlayerRequest createPlayerRequest);
        PlayerDetailsModel GetDetails(int id);
        IEnumerable<PlayerModel> Search(string q);
    }
}