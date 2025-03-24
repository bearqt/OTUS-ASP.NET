using Microsoft.AspNetCore.Mvc;
using OTUS_CI.Data;

namespace OTUS_CI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(EfDbContext context) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = context.Users.ToList();
            return Ok(result);
        }

        [HttpGet("mocked")]
        public async Task<IActionResult> GetMockedUsers()
        {
            return Ok(Summaries);
        }
    }
}
