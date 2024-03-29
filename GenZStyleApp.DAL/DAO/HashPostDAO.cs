using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class HashPostDAO
    {
        private GenZStyleDbContext _dbContext;
        public HashPostDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        #region DeleteHashPost
        public async Task DeleteHashPost(HashPost post)
        {
            try
            {
                this._dbContext.HashPosts.Remove(post);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<List<HashPost>> GetHashPostByPostIdAsync(int id)
        {
            try
            {
                return await _dbContext.HashPosts.Where(p => p.PostId == id)
                                                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

