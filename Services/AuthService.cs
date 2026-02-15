using System.Linq;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services;


public class UserService
    {
        private readonly FootballForumDbContext _context;

        public UserService(FootballForumDbContext context)
        {
            _context = context;
        }

        public void Register(RegisterRequest dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username || u.Email == dto.Email))
            {
                throw new ArgumentException("Username or Email already taken!");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password 
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }


        public bool DeleteUser(int targetUserId, int currentUserId)
        {
                if (targetUserId != currentUserId) return false;

                var user = _context.Users.Find(targetUserId);
                if (user == null) return false;

                _context.Users.Remove(user);
                _context.SaveChanges();
    
                return true;
        }
    }
