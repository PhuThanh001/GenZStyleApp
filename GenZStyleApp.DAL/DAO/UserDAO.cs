using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class UserDAO
    {
        private GenZStyleDbContext _dbContext;
        
        public UserDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region Get User by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Users.Include(c => c.Account)
                                                 .SingleOrDefaultAsync(c => c.Account.Email.Equals(email)
                                                 && c.Account.IsActive == true)
                                                  ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get User by phone
        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            try
            {
                return await _dbContext.Users.Include(c => c.Account)
                                                 .SingleOrDefaultAsync(c => c.Phone.Equals(phone));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task AddNewUser(User User)
        {
            try
            {
                await _dbContext.Users.AddAsync(User);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private readonly GenZStyleDbContext _context = new GenZStyleDbContext();

        //get User
        public async Task<List<User>> GetAllUser()
        {
            try
            {
                List<User> users = await _dbContext.Users
                    .AsNoTracking()
                    .Include(u => u.Account)
                    .Where(u => u.UserId != 1)
                    .ToListAsync();
                return users;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByAccountIdAsync(int accountId)
        {
            try
            {

                return await _dbContext.Users.Include(u => u.Account)

                                             .SingleOrDefaultAsync(u => u.Account.AccountId == accountId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                User users = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserId == userId);
                return users;



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Update user
        public void UpdateUser(User user)
        {
            try
            {
                this._dbContext.Entry<User>(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region DeleteUser
        public void DeleteUser(User user)
        {
            try
            {
                this._dbContext.Users.Remove(user);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //ban user by updating status
        public void BanUser(User user)
        {
            try
            {
                this._dbContext.Entry<User>(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<User> GetUserById(int id)
        {
            try
            {
                return await _context.Users
                                       .Include(u => u.Role)
                                      .Include(u => u.Account).ThenInclude(a => a.Invoices)
                                      .Include(u => u.Account).ThenInclude(a => a.IsActive == true)
                                      .Include(u => u.Account).ThenInclude(a => a.Likes).ThenInclude(a => a.Post)
                                      .SingleOrDefaultAsync(u => u.UserId == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<Token?> UpdateRefreshTokenAsync(
        int userId,
            string newRefreshToken,
            DateTime date
        )
        {
            try
            {
                var token = await _context.Tokens.FirstOrDefaultAsync(u => u.ID == userId);
                if (token != null)
                {
                    // Update the user's refresh token in the database
                    token.RefreshToken = newRefreshToken;
                    token.ExpiredDate = date;
                    _context.Tokens.Update(token);
                    await _context.SaveChangesAsync();
                    return token;
                }
                }
            catch
            {
                return null;
            }
            return null;
        }
        /*public async Task<User?> UpdateAccessTokenAsync(int userId, string newAccessToken)
        {
            try
            {
                var user = await _context.Tokens.FirstOrDefaultAsync(u => u.ID == userId);
                if (user != null)
                {
                    // Update the user's refresh token in the database

                    user. = newAccessToken;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return null;
            }
            return null;
        }*/
        public async Task<User> GetUserByUserNameAsync(string username)
        {
            try
            {
                return await _dbContext.Users.Include(c => c.Account)
                                                 .SingleOrDefaultAsync(c => c.Account.Username.Equals(username)
                                                 && c.Account.IsActive == true)
                                                 
                                                  ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
    
