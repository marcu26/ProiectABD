using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Users;
using Core.Email;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public ProjectDbContext _dbContext { get; set; }

        public UsersRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region utils

        public static string Encode(string text)
        {
            if (text == null)
                return null;

            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        public async Task<UserDto> CreateUserAsync(string email, string fullName, string password, int role)
        {
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new Exception($"User cu emailul {email} exista deja");
            }

            password = Encode(password);

            var user = new User
            {
                Email = email,
                FullName = fullName,
                Password = password,
                Role = role
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Email = email,
                FullName = fullName,
                Role = role
            };
        }

        public async Task<UserDto> LoginByEmailAndPassword(string email, string password)
        {
            password= Encode(password);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new Exception("Email sau parola gresite");
            }

            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        public async Task GetResetPasswordEmail(string email) 
        {
            EmailService e = new EmailService();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception($"User cu email {email} nu exista.");

            do
            {
                user.ResetPasswordCode = RandomString(6);
            } while (await _dbContext.Users.AnyAsync(u=>u.ResetPasswordCode==user.ResetPasswordCode));

            await _dbContext.SaveChangesAsync();

            EmailDto dto = new EmailDto()
            {
                Body = "Hello!\n This is your code to reset your Science Time password: " + user.ResetPasswordCode,
                To = email,
                Subject = "ResetPasswordEmail"
            };

            await e.sendEmailAsync(dto);
        }

        public async Task ResetPasswordAsync(string code, string newPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.ResetPasswordCode==code);


            if (user == null)
                throw new Exception("The code is not right");

            user.Password = Encode(newPassword);
            user.ResetPasswordCode = null;

            await _dbContext.SaveChangesAsync();
        }


    }
}
