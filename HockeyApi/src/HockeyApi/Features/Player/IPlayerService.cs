using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public interface IPlayerService
    {
        int? Create(CreatePlayerRequest createPlayerRequest);
        PlayerDetailsModel GetDetails(int id);
        IEnumerable<PlayerModel> Search(string q);
    }
}