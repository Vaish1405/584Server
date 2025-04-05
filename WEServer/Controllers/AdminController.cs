using System.IdentityModel.Tokens.Jwt;
using cleanModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEServer.Dtos;
using LoginRequest = WEServer.Dtos.LoginRequest;

namespace WEServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldCitiesUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginRequest login)
        {
            WorldCitiesUser user = await userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                return Unauthorized("Unknown user");
            }
            bool success = await userManager.CheckPasswordAsync(user, login.Password);
            if (!success)
            {
                return Unauthorized("Wrong Password");
            }

            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponse { 
                Success = true, 
                Message = "works", 
                Token = tokenString
            });
        }
    }
}
