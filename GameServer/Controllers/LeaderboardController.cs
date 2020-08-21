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
        /// <summary>
        /// return the leaderboard from the game
        /// </summary>
        /// <param name="gameId">Game Id to get leaderboard</param>
        /// <returns>A Top 100 leaderboard</returns>
        /// <response code="200">Returns the top 100 leaderboard</response>
        /// <response code="404">If game Id not found</response>
        /// <response code="500">If any error occur</response>
        [HttpGet]
        [Route("{gameId}")]        
        public  ActionResult<IEnumerable<LeaderboardViewModel>> Leaderboard(long gameId)
        {
            try
            {
                if (!service.ContainsGame(gameId))
                    return StatusCode(404, "Game not found");

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
