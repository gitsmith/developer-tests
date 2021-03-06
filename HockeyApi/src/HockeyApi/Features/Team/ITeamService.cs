﻿using System.Collections.Generic;

namespace HockeyApi.Features.Team
{
    public interface ITeamService
    {
        TeamDetailsModel GetDetails(string code);
        IEnumerable<TeamModel> List();
    }
}
