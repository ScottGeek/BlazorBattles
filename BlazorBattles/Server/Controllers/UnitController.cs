using BlazorBattles.Server.Data;
using BlazorBattles.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly DataContext _context;

        #region Old Hardcoded before db added
        //public IList<Unit> Units => new List<Unit>
        //{
        //    new Unit { Id =1, Title = "Knight", Attack=10, BananaCost=100, Defense=10},
        //    new Unit { Id =2, Title = "Archer", Attack=15, BananaCost=150, Defense=5},
        //    new Unit { Id =3, Title = "Mage", Attack=20, BananaCost=200, Defense=1},
        //}; 
        #endregion

        public UnitController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnits()
        {

            var units = await _context.Units.ToListAsync();
            return Ok(units);
        }

        [HttpPost]
        public async Task<IActionResult> AddUnit(Unit unit)
        {
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            return Ok(await _context.Units.ToListAsync());

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            var result = await _context.Units.FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
            {
                return NotFound("Unit with the given Id does not exits");
            }

            result.Title = unit.Title;
            result.Defense = unit.Defense;
            result.Attack = unit.Attack;
            result.BananaCost = unit.BananaCost;
            result.HitPoints = unit.HitPoints;

            await _context.SaveChangesAsync();
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var result = await _context.Units.FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
            {
                return NotFound("Unit with the given Id does not exits");
            }

            _context.Units.Remove(result);

            await _context.SaveChangesAsync();
            return Ok(await _context.Units.ToListAsync());

        }


    }
}
