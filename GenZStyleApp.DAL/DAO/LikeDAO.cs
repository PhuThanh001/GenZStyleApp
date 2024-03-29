using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class LikeDAO
    {
        private GenZStyleDbContext _dbContext;

        public LikeDAO(GenZStyleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //get likes
        public async Task<List<Like>> GetAllAccountByLikes(int postId)
        {
            try
            {
                List<Like> likes = await _dbContext.Likes
                    .AsNoTracking()
                    .Include(l => l.Account).ThenInclude(l => l.User)
                    .Where(l => l.PostId == postId)
                    .ToListAsync();

                return likes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task AddLikeAsync(Like like)
        {
            try
            {
                var existingLike = await this._dbContext.Likes
                .FirstOrDefaultAsync(l => l.PostId == like.PostId && l.LikeBy == like.LikeBy);


                if (existingLike != null)
                {
                    // Cập nhật thông tin của existingToken nếu cần
                    this._dbContext.Entry(existingLike).CurrentValues.SetValues(like);
                }
                else
                {
                    // Thêm mới nếu không có đối tượng có cùng ID
                    await this._dbContext.Likes.AddAsync(like);

                }
                //await this._dbContext.Likes.AddAsync(like);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ChangeLike(Post post)
        {
            try
            {
                this._dbContext.Entry<Post>(post).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Like>> GetLikeByPostId(int PostId)
        {
            try
            {
                return await this._dbContext.Likes.Include(l => l.Post)
                .Where(l => l.PostId == PostId)
                .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<Like> GetLikeByPostIdAndAccount(int PostId, int accountid)
        {
            try
            {
                return await this._dbContext.Likes.Include(l => l.Post)
                .Where(l => l.PostId == PostId && l.LikeBy == accountid)
                .SingleOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<List<Like>> GetLikeByPostIdAysn(int PostId)
        {
            try
            {
                return await this._dbContext.Likes
                .Where(l => l.PostId == PostId)
                .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        

    }
}
