    using BCrypt.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.NetworkInformation;
    using System.Security.Claims;
    using System.Text;

    namespace CarRental.API.Models
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            public static User user = new User();
            private IConfiguration _configuration;

            public AuthController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            [HttpPost("Register")]
            public ActionResult<User> Register(UserDto request)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                user.Username = request.Username;
                user.PasswordHash = passwordHash;

                return Ok(user);
            }

            [HttpPost("Login")]
            public ActionResult<User> Login(UserDto request)
            {
                if(user.Username!= request.Username)
                {
                    return BadRequest("User not found.");
                }

                if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest("Wrong password.");
                }

                string token = CreateToken(user);

                return Ok(token);
            }

            private string CreateToken(User user)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: cred
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
        }
    }
