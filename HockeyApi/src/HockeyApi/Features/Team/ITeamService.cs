using System.Collections.Generic;

namespace HockeyApi.Features.Team
{
    public interface ITeamService
    {
        IEnumerable<TeamModel> List();
        TeamDetailsModel GetDetails(string code);
    }
}
