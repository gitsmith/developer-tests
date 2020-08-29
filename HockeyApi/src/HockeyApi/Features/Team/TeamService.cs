using System.Collections.Generic;

namespace HockeyApi.Features.Team
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public TeamDetailsModel GetDetails(string code)
        {
            return _teamRepository.GetDetails(code);
        }

        public IEnumerable<TeamModel> List()
        {
            return _teamRepository.List();
        }
    }
}
