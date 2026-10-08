using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Khabarov_Artem_AS2304.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("get-token")]
        public IActionResult GetToken([FromQuery] string role = "User") // генерируем
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) }; // для проверки

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyARTEMKA1175WOAH67SIXSEVEEEEENUWUOWO")); //обратно расшифровываем
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //чтобы не подделали токен

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds // роль, время и подпись
            );

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}