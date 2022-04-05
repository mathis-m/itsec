using System.Security.Cryptography;
using System.Text;
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
        public AppUser? GetUserFromLogin(string userName, string password)
        {
            var user = _context.AppUser.SingleOrDefault(user => user.UserName == userName);
            if (user is null)
                return null;

            // A2:2017-Broken Authentication
            // Implemented Authentication incorrectly
            // fix: if(password != user.Password) return false; else return user;
            // even better use SHA256 to store not clear password :)
            // fixed by Robert & Mathis

            var hashedPwd = CalculateSHA256(password);
            return hashedPwd != user.Password 
                ? null 
                : user;
        }

        public async Task<AppUser> RegisterUser(string userName, string password)
        {
            var user = await _context.AppUser.AddAsync(new AppUser
            {
                UserName = userName,
                Password = CalculateSHA256(password)
            });

            await _context.SaveChangesAsync();

            return user.Entity;
        }

        private string CalculateSHA256(string str)
        {
            var sha256 = SHA256.Create();
            var objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

            return objUtf8.GetString(hashValue);
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _context.AppUser.SingleOrDefaultAsync(x => x.UserName == userName);
            if (user is null)
                throw new InvalidOperationException("User does not exist");
            _context.AppUser.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
