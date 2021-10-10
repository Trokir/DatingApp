using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
            {
                return BadRequest("UserName is taken");
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
           

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
           if(user == null)
            {
                return Unauthorized("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password") ;
                }
            }
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            var result = await _context.Users.AnyAsync(x => x.UserName == userName.ToLowerInvariant());
            return result;
        }
        private async Task<bool> UserExistsById(int id)
        {
            var result = await _context.Users.AnyAsync(x => x.Id == id);
            return result;
        }

        [HttpDelete("removeuser")]
        public async Task<ActionResult<AppUser>> DeleteUser(int id)
        {
            if (await UserExistsById(id)==false)
            {
                return BadRequest("User can't found");
            }
            var user = await _context.Users.FindAsync(id);
            _context.Users.Attach(user);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}