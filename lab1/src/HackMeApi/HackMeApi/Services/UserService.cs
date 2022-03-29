using HackMeApi.Infrastructure;
using HackMeApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackMeApi.Services
{
    public class UserService
    {
        private readonly HackMeContext _context;

        public UserService(HackMeContext context)
        {
            _context = context;
        }
        public User? GetUserFromLogin(string userName, string password)
        {
            var user = _context.Users.SingleOrDefault(user => user.UserName == userName);
            if (user is null)
                return null;

            var dbPwdChars = user.Password.ToCharArray();

            var inputPwChars = password.ToCharArray();

            // A2:2017-Broken Authentication
            // Implemented Authentication incorrectly
            // fix: if(password != user.Password) return false; else return user;
            // even better use SHA256 to store not clear password :)
            for (var i = 0; i < inputPwChars.Length; i++)
            {
                var pwChar = inputPwChars[i];
                
                if (i >= dbPwdChars.Length) 
                    return null;
                if (pwChar != dbPwdChars[i])
                    return null;
            }

            return user;
        }

        public async Task<User> RegisterUser(string userName, string password)
        {
            var user = await _context.Users.AddAsync(new User
            {
                UserName = userName,
                Password = password
            });

            await _context.SaveChangesAsync();

            return user.Entity;
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);
            if (user is null)
                throw new InvalidOperationException("User does not exist");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
