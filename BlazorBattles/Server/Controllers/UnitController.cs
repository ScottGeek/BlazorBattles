using BlazorBattles.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        public IList<Unit> Units => new List<Unit>
        {
            new Unit { Id =1, Title = "Knight", Attack=10, BananaCost=100, Defense=10},
            new Unit { Id =2, Title = "Archer", Attack=15, BananaCost=150, Defense=5},
            new Unit { Id =3, Title = "Mage", Attack=20, BananaCost=200, Defense=1},
        };

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {
            return Ok(Units);
        }

    }
}
