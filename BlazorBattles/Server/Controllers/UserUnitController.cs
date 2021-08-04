using BlazorBattles.Server.Data;
using BlazorBattles.Server.Services;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserUnitController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUtilityService _utilityService;

        public UserUnitController(DataContext dataContext, IUtilityService utilityService)
        {
            _dataContext = dataContext;
            _utilityService = utilityService;
        }


        [HttpPost]
        public async Task<IActionResult> BuildUserUnit([FromBody] int unitId)
        {
            var unit = await _dataContext.Units.FirstOrDefaultAsync<Unit>(u => u.Id == unitId);
            var user = await _utilityService.GetUser();

            if (user.Bananas < unit.BananaCost)
            {
                return BadRequest("Not Enough bananas!");
            }

            user.Bananas -= unit.BananaCost;

            var newUserUnit = new UserUnit {
             UnitId = unit.Id,
             UserId = user.Id,
             HitPoints = unit.HitPoints,
            };

            _dataContext.UserUnits.Add(newUserUnit);
            await _dataContext.SaveChangesAsync();
            return Ok(newUserUnit);

        }

        [HttpGet]
        public async Task<IActionResult> GetUserUnits()
        {
            var user = await _utilityService.GetUser();
            var userUnits = await _dataContext.UserUnits.Where(unit => unit.UserId == user.Id).ToListAsync();

            var response = userUnits.Select(
                unit => new UserUnitResponse
                { 
                    UnitId = unit.UnitId,
                    HitPoints = unit.HitPoints
                });

            return Ok(response);

        }


    }
}
