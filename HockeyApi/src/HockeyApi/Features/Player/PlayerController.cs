using Microsoft.AspNetCore.Mvc;

namespace HockeyApi.Features.Player
{
    public class PlayerController : Controller
    {
        [HttpGet("[controller]")]
        public IActionResult Search(string q)
        {
            return Json($"query: {q}");
        }
    }
}
