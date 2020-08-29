using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features.Player
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("[controller]")]
        public IActionResult Create([FromBody] CreatePlayerRequest createPlayerRequest)
        {
            var playerId = _playerService.Create(createPlayerRequest);

            if (playerId == null)
            {
                return BadRequest();
            }

            return Created($"player/{playerId}", new { id = playerId });
        }

        [HttpGet("[controller]/{id}")]
        public IActionResult GetDetails(int id)
        {
            var playerDetails = _playerService.GetDetails(id);

            if (playerDetails == null)
            {
                return NotFound();
            }

            return Json(playerDetails);
        }

        [HttpGet("[controller]")]
        public IActionResult Search(string q)
        {
            return Json(_playerService.Search(q));
        }
    }
}
