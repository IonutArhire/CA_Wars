using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Services.GameResourcesService;
using Services.MatchesManagerService;
using Services.Models;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private ILifeLikeRepo _lifeLikeRepo;

        private IMatchesManagerService _matchesManagerService;

        private IGameResourcesService _gameResourcesService;

        public MatchController(ILifeLikeRepo lifeLikeRepo,
                                IMatchesManagerService matchesManagerService,
                                IGameResourcesService gameResourcesService) {

            this._lifeLikeRepo = lifeLikeRepo;
            this._matchesManagerService = matchesManagerService;
            this._gameResourcesService = gameResourcesService;
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] MatchCreateResource input)
        {
            var newGameModel = this._gameResourcesService.GetGameResources(
                new DimensionsModel(input.Rows, input.Cols), input.NrPlayers, input.MaxIters, input.RuleSet
            );

            var id = Guid.NewGuid();
            this._matchesManagerService.Create(id, newGameModel);

            return Created("", new { id=id });
        }

        [HttpGet]
        public IActionResult Get() {
            var result = this._lifeLikeRepo.GetByName("GOF");
            return Ok();
        }
    }
}
