using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFirstWebAPI.dtos;
using MyFirstWebAPI.Models;

namespace MyFirstWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions, ProductsContext context, TokenService tokenService)  : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> CreateUser(RegisterRequestDTO request)
        {
            // validation: Check if the username already exists in the database
            // AnyAsync returns true if it finds at least one match
            bool userExists = await context.Users.AnyAsync(u => u.Username == request.Username);

            if (userExists) return BadRequest("That username is already taken.");
            

            // map the request data to a new database User object
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,            
                Password = request.Password
            };

            context.Users.Add(newUser);

            // save the changes to the actual SQL database
            await context.SaveChangesAsync();

            return Ok("User successfully created!");
        }


        [HttpPost]
        [Route("auth")]
        public async Task<ActionResult<string>> AuthenticateUser(AuthenticationRequestDTO request)
        {
            var dbUser = await context.Users.SingleOrDefaultAsync(
                user => user.Username == request.Username &&
                user.Password == request.Password
            );

            // this means the user was not found in the database, so we return a 401 Unauthorized status code
            if (dbUser == null) return Unauthorized("Invalid username or password.");


            var accessToken = tokenService.GenerateToken(dbUser);

            return Ok(accessToken);
        }

        
    }
}
