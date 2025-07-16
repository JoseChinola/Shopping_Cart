using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping_Cart.Data;
using Shopping_Cart.DTOs.Auth;
using Shopping_Cart.Interfaces;
using Shopping_Cart.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shopping_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository _userRepo)
        {
           this._userRepo = _userRepo;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest user)
        {
            if (user == null)
            {
                return BadRequest(new
                {
                    message = "Invalid user data",
                    error = true,
                    success = false
                });
            }

            var existingUser = await _userRepo.ExistsAsync(user.Username);
            if (existingUser)
            {
                return BadRequest(new
                {
                    message = "Username already exists",
                    error = true,
                    success = false
                });
            }

            var newUser = new User
            {
                Name = user.Name,
                Username = user.Username,
                PasswordHash = ComputeSha256Hash(user.Password)
            };

            await _userRepo.RegisterAsync(newUser);

           var response = new UserResponse
            {
                Name = newUser.Name,
                UserName = newUser.Username,
                Id = newUser.Id
            };

            return Ok(new 
            {
                message = "user successfully registered ",
                data = response,
                error = false,
                success = true
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userRepo.GetByUsernameAsync(request.Username);

            if (user == null || user.PasswordHash != ComputeSha256Hash(request.Password))
            {
                return BadRequest(new 
                {
                    message="Invalid credentials",
                    error = true,
                    success = false
                });
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.Username
            };

            return Ok(new
            {
               
                message = "Login successfully",
                data = response,
                error = false,
                success = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var users = await _userRepo.GetAllAsync();
            if (users == null)
            {
                return NotFound(new
                {
                    message = "User not found",
                    error = true,
                    success = false
                });
            }


            var response = users.Select( u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.Username
            }).ToList();

            return Ok(new
            {
                message = "Users",
                data = response,
                error = false,
                success = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found",
                    error = true,
                    success = false
                });
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.Username
            };

            return Ok(new
            {
                message= "User found ",
                data = response,
                error = false,
                success = true
            });
        }
      
        public static string ComputeSha256Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-","").ToLower();
        }
    }
}
