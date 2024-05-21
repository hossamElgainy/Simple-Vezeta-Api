using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vezeta.Application.DTO.AccountDtos;
using Vezeta.Application.Interfaces;
using VezetaApi.ApiResponse;
using VezetaCore.Models;

namespace VezetaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtToken _jwtToken;


        public AccountController(IJwtToken jwtToken,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _jwtToken = jwtToken;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm]dtoLogin dto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await _userManager.FindByNameAsync(dto.UserName);
                if(appUser != null)
                {
                    if(await _userManager.CheckPasswordAsync(appUser, dto.Password))
                    {
                        var Role =_userManager.GetRolesAsync(appUser).Result.FirstOrDefault();
                         var token =_jwtToken.GenerateToken(appUser,Role);
                        var expireAt = _jwtToken.ExtractValidToDateFromToken(token);
                        return Ok(new OkResponse<AuthDto>( new AuthDto
                        {
                            Id = appUser.Id,
                            Email = appUser.Email,
                            FullName = appUser.FullName,
                            Token = token,
                            ExpireAt = expireAt
                        }));
                       
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User...");
                }
            }
            return BadRequest();
        }
    }
}
