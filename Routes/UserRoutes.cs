using Microsoft.AspNetCore.Mvc;
using MyDotNetApp.Dtos;
using MyDotNetApp.Models;
using MyDotNetApp.Services;

namespace MyDotNetApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserRoutes : ControllerBase
    {
        private readonly UserController _userController;

        public UserRoutes(UserService userService)
        {
            _userController = new UserController(userService);
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] User user) =>
            _userController.RegisterUser(user);

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginDto loginDto) =>
            _userController.LoginUser(loginDto);
    }
}
