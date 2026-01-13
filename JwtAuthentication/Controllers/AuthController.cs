using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    //Controller
    public class AuthController : ControllerBase
    {
        private readonly JwtContext jwtContext;
        private readonly IConfiguration configuration;
        public AuthController( JwtContext _jwtContext,IConfiguration _configuration)
        {
            jwtContext = _jwtContext;
            configuration=_configuration;
            
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await jwtContext.Employees.ToListAsync());
        }
        [HttpGet("id")]
        [Authorize]
        public async Task<IActionResult> GetbyId( int id)
        {
          return Ok ( await jwtContext.Employees.FindAsync(id));

        }
        [HttpPost]
        public async Task<ActionResult> Login(Login login) 
        {
           
            
                var data = await jwtContext.Employees.FirstOrDefaultAsync(x=>x.Email==login.Email && x.Password==login.Password);
            // return Ok(data);
            if (data != null)
            {
                var Claims = new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier,data.Email),
                    new Claim(ClaimTypes.Name,data.FirstName),
                    new Claim(ClaimTypes.Role,data.Roles)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
                var cred=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: Claims,
                    expires: DateTime.Now.AddMinutes(50),
                    signingCredentials: cred);
                var tokenstr=new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenstr);
            }
           
            return BadRequest("invalid user");

        }
        
    }
}
