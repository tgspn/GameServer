using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameServer.Models;
using GameServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GameServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> logger;
        GameService service;
        public GameController(IConfiguration configuration, ILogger<GameController> logger)
        {
            var persistenceTime = string.IsNullOrEmpty(configuration["persistenceTime"]) ? TimeSpan.FromMinutes(10) : TimeSpan.Parse(configuration["persistenceTime"]);
            this.service = new GameService(persistenceTime);
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GameResult(GameResultViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return StatusCode(400, "the result is not valid");
                await Task.Run(() => { service.Save((GameResultModel)model); });
                
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "unable to save");
                return StatusCode(500, "unable to save");
            }
        }
    }
}
