using System.Collections.Generic;

namespace HockeyApi.Features.Team
{
    public interface ITeamRepository
    {
        TeamDetailsModel GetDetails(string code);
        IEnumerable<TeamModel> List();
    }
}
