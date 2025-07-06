using System.Text.Json;
using MyDotNetApp.Data;
using MyDotNetApp.Dtos;
using MyDotNetApp.Helpers;
using MyDotNetApp.Models;

namespace MyDotNetApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        this below function is use to register a user in the database
        response to the frontend is success, message, user details
        function name is - RegisterUser
        */

        public (bool success, string message, User? user) RegisterUser(User userDto)
        {
            try
            {
                if (userDto == null)
                    return (false, "Invalid registration data!", null);

                // Name required
                if (string.IsNullOrWhiteSpace(userDto.Name))
                    return (false, "Name is required!", null);

                // Email validation
                if (string.IsNullOrWhiteSpace(userDto.Email))
                    return (false, "Email is required!", null);
                if (!userDto.Email.Contains("@") || !userDto.Email.Contains("."))
                    return (false, "Invalid email format!", null);

                // Password match
                if (userDto.Password != userDto.ConfirmPassword)
                    return (false, "Password and Confirm Password do not match!", null);

                // Password length
                if (userDto.Password.Length < 8)
                    return (false, "Password must be at least 8 characters long!", null);

                // Password strength: uppercase, lowercase, number, special character
                bool hasUpper = userDto.Password.Any(char.IsUpper);
                bool hasLower = userDto.Password.Any(char.IsLower);
                bool hasDigit = userDto.Password.Any(char.IsDigit);
                bool hasSpecial = userDto.Password.Any(ch => !char.IsLetterOrDigit(ch));

                if (!(hasUpper && hasLower && hasDigit && hasSpecial))
                    return (
                        false,
                        "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character!",
                        null
                    );

                // Contact number validation
                if (string.IsNullOrWhiteSpace(userDto.ContactNumber))
                    return (false, "Contact number is required!", null);
                if (userDto.ContactNumber.Length != 10 || !userDto.ContactNumber.All(char.IsDigit))
                    return (false, "Contact number must be exactly 10 digits!", null);

                // Email uniqueness
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
                if (existingUser != null)
                    return (false, "Email already registered!", null);

                // Hash password
                var hashedPassword = PasswordHelper.HashPassword(userDto.Password);

                // Create user
                var newUser = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    Password = hashedPassword,
                    ContactNumber = userDto.ContactNumber,
                    Address = userDto.Address ?? "",
                    PinCode = userDto.PinCode ?? "",
                    ProfileImage = userDto.ProfileImage ?? "",
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return (true, "User registered successfully!", newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registrations Error: " + ex.Message);
                return (false, "Something went wrong during registrations!", null);
            }
        }

        /*
        this below function is used to login a user to the database
        */

        public (bool success, string message, string? token) LoginUser(LoginDto loginDto)
        {
            try
            {
                Console.WriteLine("Incoming login data: " + JsonSerializer.Serialize(loginDto));

                if (
                    loginDto == null
                    || string.IsNullOrWhiteSpace(loginDto.Email)
                    || string.IsNullOrWhiteSpace(loginDto.Password)
                )
                {
                    return (false, "Email and Password are required!", null);
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    return (false, "User not found!", null);
                }

                bool isPasswordCorrect = PasswordHelper.VerifyPassword(
                    loginDto.Password,
                    user.Password
                );
                if (!isPasswordCorrect)
                {
                    return (false, "Incorrect password!", null);
                }

                // Generate JWT token
                var token = JwtHelper.GenerateJwtToken(user);

                return (true, "Login successful!", token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error" + ex.Message);
                return (false, "something went wrong during login", null);
            }
        }
    }
}
