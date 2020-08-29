using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features.Player
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            this._playerService = playerService;
        }

        [HttpGet("[controller]")]
        public IActionResult Search(string q)
        {
            return Json(_playerService.Search(q));
        }
    }
}
