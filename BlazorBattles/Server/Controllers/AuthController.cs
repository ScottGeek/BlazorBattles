using BlazorBattles.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBattles.Shared;

namespace BlazorBattles.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {

            var response = await _authRepository.Register(
                new User
                {
                   Username = userRegister.Username,
                   Email = userRegister.Email,
                   Bananas = userRegister.Bananas,
                   DateOfBirth = userRegister.DateOfBirth,
                   IsConfirmed = userRegister.IsConfirmed
                },
                userRegister.Password,
                userRegister.StartUnitId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin request)
        {

            var response = await _authRepository.Login(
               request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

    }
}
