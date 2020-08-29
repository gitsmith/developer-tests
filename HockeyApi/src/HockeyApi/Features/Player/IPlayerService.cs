﻿using System.Collections.Generic;

namespace HockeyApi.Features.Player
{
    public interface IPlayerService
    {
        IEnumerable<PlayerModel> Search(string q);
    }
}