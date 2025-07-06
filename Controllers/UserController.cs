using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using MyDotNetApp.Dtos;
using MyDotNetApp.Models;
using MyDotNetApp.Services;

namespace MyDotNetApp.Controllers
{
    public class UserController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /*
        this is the controller which will handle the error and the role based auth of the user registrations
        */

        public IActionResult RegisterUser(User user)
        {
            var result = _userService.RegisterUser(user);
            if (!result.success)
            {
                Console.WriteLine(result);
                return new BadRequestObjectResult(new { message = result.message });
            }

            return new OkObjectResult(new { message = result.message, user = result.user });
        }

        /*
       this is the controller which will handle the user login
       */

        public IActionResult LoginUser(LoginDto loginDto)
        {
            var result = _userService.LoginUser(loginDto);
            if (!result.success)
                return new BadRequestObjectResult(new { message = result.message });

            return new OkObjectResult(new { message = result.message, token = result.token });
        }
    }
}
