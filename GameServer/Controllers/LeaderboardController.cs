using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameServer.Models;
using GameServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILogger<LeaderboardController> logger;
        private readonly LeaderboardService service;

        public LeaderboardController(ILogger<LeaderboardController> logger)
        {
            this.logger = logger;
            this.service = LeaderboardService.Instance;
        }

        [HttpGet]
        [Route("{gameId}")]
        public  ActionResult<IEnumerable<LeaderboardViewModel>> LeaderBoard(long gameId)
        {
            try
            {
                if (!service.ContainsGame(gameId))
                    return StatusCode(404, "No game found");

                return StatusCode(200, service[gameId]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "unable to list");
                return StatusCode(500, "unable to list");
            }
        }
    }
}
