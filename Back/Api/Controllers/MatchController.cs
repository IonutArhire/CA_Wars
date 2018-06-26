using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Services.MatchResourcesService;
using Services.MatchesManagerService;
using Services.Models;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private IMatchesManagerService _matchesManagerService;

        private IMatchResourcesService _gameResourcesService;

        public MatchController(IMatchesManagerService matchesManagerService,
                                IMatchResourcesService gameResourcesService) {

            this._matchesManagerService = matchesManagerService;
            this._gameResourcesService = gameResourcesService;
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] MatchCreateResource input)
        {
            var newGameModel = this._gameResourcesService.GetMatchResources(
                new DimensionsModel(input.Rows, input.Cols), input.NrPlayers, input.MaxIters, input.RuleSet
            );

            var id = Guid.NewGuid();
            this._matchesManagerService.Create(id, newGameModel);

            return Created("", new MatchCreateResponseDto(id));
        }
    }
}
