using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features
{
    public class TeamController : Controller
	{
		private readonly ITeamService _teamService;

		public TeamController(ITeamService teamService)
		{
			_teamService = teamService;
		}

		[HttpGet("[controller]")]
		public IActionResult Index() => 
			Json(_teamService.List());

		[HttpGet("[controller]/{code}")]
		public IActionResult Get(string code)
		{
			return Json(_teamService.Get(code));
		}
	}
}
