using System;
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

        [HttpGet("[controller]/{id}")]
        public IActionResult GetDetails(int id)
        {
            return Json(_playerService.GetDetails(id));
        }

        [HttpPost("[controller]")]
        public IActionResult Create([FromBody] CreatePlayerRequest createPlayerRequest)
        {
            var playerId = _playerService.Create(createPlayerRequest);

            if (playerId == null)
            {
                return BadRequest();
            }

            return Created($"player/{playerId}", new { id = playerId});
        }
    }
}
