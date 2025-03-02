using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TechnomediaTestTask.DTO;
using TechnomediaTestTask.DTOs;

namespace TechnomediaTestTask.Services
{
    public class AuthService
    {
        private readonly TechnomediaTestTaskDBEntities _context;

        private readonly string issuer;
        private readonly byte[] secret;

        public AuthService(TechnomediaTestTaskDBEntities context)
        {
            issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Jwt:Key"]);
            _context = context;
        }

        public string GenerateToken(string username, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateUser(LoginDTO loginDTO, out IEnumerable<string> userRoles)
        {
            userRoles = new List<string>();

            try
            {
                var user = _context.users.Include("roles").SingleOrDefault(u => u.username == loginDTO.Username);
                if (user != null && user.password == loginDTO.Password)
                {
                    userRoles = new List<string> { user.roles?.name ?? "User" };
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the user.", ex);
            }

            return false;
        }

        public bool RegisterUser(RegisterDTO registerDTO)
        {
            if (_context.users.Any(u => u.username == registerDTO.Username))
            {
                return false;
            }

            try
            {
                var role = _context.roles.SingleOrDefault(r => r.name == registerDTO.RoleName);
                if (role == null)
                {
                    return false;
                }

                var newUser = new users
                {
                    username = registerDTO.Username,
                    password = registerDTO.Password, 
                    role_id = role.id
                };

                _context.users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }

        public IEnumerable<UserDTO> GetAllUsers() 
        {
            return _context.users
                .Select( u => new UserDTO
                {
                    Username = u.username,
                    RoleName = u.roles.name
                });
        }
    }
}