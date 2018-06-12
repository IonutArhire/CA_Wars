using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MatchCreateController : ControllerBase
    {
        private ILifeLikeRepo _lifeLikeRepo;

        public MatchCreateController(ILifeLikeRepo lifeLikeRepo) {
            this._lifeLikeRepo = lifeLikeRepo;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpGet]
        public IActionResult Get() {
            var result = this._lifeLikeRepo.GetByName("GOF");
            return Ok();
        }
    }
}
