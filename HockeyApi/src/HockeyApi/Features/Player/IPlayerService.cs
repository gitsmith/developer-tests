using System;
using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public interface IPlayerService
    {
        IEnumerable<PlayerModel> Search(string q);
        PlayerDetailsModel GetDetails(int id);
        string Create(CreatePlayerRequest createPlayerRequest);
    }
}