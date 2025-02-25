using HotelManagement.Application.Interfaces;
using HotelManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace HotelManagement.Application.Services
{
    /// <summary>
    /// Provides authentication services, including user registration and login.
    /// </summary>
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user data management.</param>
        /// <param name="jwtService">The service for generating JWT tokens.</param>
        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="fullname">The full name of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plain-text password of the user.</param>
        /// <param name="role">The role assigned to the user.</param>
        /// <returns>A success message indicating the user was registered.</returns>
        /// <exception cref="Exception">Thrown when the email is already registered.</exception>
        public async Task<string> RegisterAsync(string fullname, string email, string password, string role)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("The email is already registered.");
            }

            var user = new User
            {
                Fullname = fullname,
                Email = email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            await _userRepository.AddAsync(user);

            return "User successfully registered.";
        }


        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The plain-text password of the user.</param>
        /// <returns>A JWT token if authentication is successful; otherwise, null.</returns>
        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Passwordhash))
            {
                return null;
            }

            return _jwtService.GenerateToken(user, user.Role);
        }
    }
}
