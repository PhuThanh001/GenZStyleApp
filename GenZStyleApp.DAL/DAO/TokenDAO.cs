using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMOS.DAL.DAOs
{
    public class TokenDAO
    {
        private GenZStyleDbContext _dbContext;
        public TokenDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region Get token by refresh token
        public async Task<Token> GetTokenByRefreshTokenAsync(string token)
        {
            try
            {
                return await _dbContext.Tokens.Include(t => t.Account)
                                              .ThenInclude(a => a.User).ThenInclude(u => u.Role)
                                              .SingleOrDefaultAsync(t => t.RefreshToken.Equals(token));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
        public async Task<Token> GetLastToken()
        {
            try
            {
                List<Token> tokens = await _dbContext.Tokens.Include(p => p.Account)                                                                 
                                                                  .ToListAsync();
                Token lastToken = tokens.LastOrDefault();
                return lastToken;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Token> GetTokenByIdAsync(int Id)
        {
            try
            {
                Token tokens = await _dbContext.Tokens.SingleOrDefaultAsync(p => p.ID == Id);
                                                                  
                
                return tokens;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Create token
        public async Task CreateTokenAsync(Token token)
        {
            try
            {
                var existingToken = await this._dbContext.Tokens.FindAsync(token.ID);


                if (existingToken != null)
                {
                    // Cập nhật thông tin của existingToken nếu cần
                    this._dbContext.Entry(existingToken).CurrentValues.SetValues(token);
                }
                else
                {
                    // Thêm mới nếu không có đối tượng có cùng ID
                    await this._dbContext.Tokens.AddAsync(token);
                    
                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Token
        public void UpdateToken(Token token)
        {
            try
            {
                this._dbContext.Entry<Token>(token).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
