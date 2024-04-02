using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReduxLogin.Data;
using ReduxLogin.Dtos;
using ReduxLogin.Helpers;
using ReduxLogin.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReduxLogin.Controllers
{
    //http://localhost:8000/api/auth
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly JWTService _jwtService;
        public AuthController(IUserRepository repository, ApplicationDbContext db, JWTService jwtService , IConfiguration configuration)
        {
            _repository = repository;
            _db = db;
            _jwtService = jwtService;
            _configuration = configuration;
                
        }
        // GET: api/values
        [HttpGet]
        public IActionResult GetAll()
        {
            //get data from database
            var users = _db.Users.ToList();

            //Map user domain to dto 

            var userDtos = new List<UserDto>();
            foreach (var user in users)
                userDtos.Add(new UserDto()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email

                });
            
                    
            

            return Ok(users);
        }

        

        // POST api/values
        [HttpPost("register")]
        public IActionResult Register( RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword( registerDto.Password)
            };



            return Created("Success", _repository.Create(user));
        }

        // POST api/values
        [HttpPost("create")]
        public IActionResult Create(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            return Created("Success", _repository.Create(user));
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = _repository.GetByUserName(loginDto.UserName!);

            if(user ==  null)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.UserId);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            //var token = _jwtService.Generate(user.UserId);
            //   return Ok(new { jwt });

            return Ok(new
            {
                message = "success"


            });
        }

       

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);


                return Ok(user);
            }
            catch(Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

