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
        public GameController( ILogger<GameController> logger)
        {            
            this.service = new GameService();
            this.logger = logger;
        }
        /// <summary>
        /// Save the game result
        /// </summary>
        /// <param name="model">game result to save</param>
        /// <returns></returns>
        /// <response code="200">if success save</response>
        /// <response code="400">If payload body is not valid</response>
        /// <response code="500">If any error occur</response>
        [HttpPost]
        public IActionResult GameResult(GameResultViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(400, "the result is not valid");
                service.Save((GameResultModel)model);

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
