using System.Collections.Generic;

namespace HockeyApi.Features
{
    public interface ITeamService
    {
        IEnumerable<TeamModel> List();
        TeamDetailsModel Get(string code);
    }
}
