using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZStyleApp.DAL.DAO
{
    public class HashTagDAO
    {
        private GenZStyleDbContext _dbContext;
        public HashTagDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region AllHashtag
        public async Task<List<Hashtag>> GetAllHashTag()
        {
            return await _dbContext.Hashtags
        .Include(h => h.HashPosts)
            .ThenInclude(J => J.Post)
                .ThenInclude(p => p.Likes)
        .Include(h => h.HashPosts)
            .ThenInclude(J => J.Post)
                .ThenInclude(p => p.Comments)
        .ToListAsync();

        }
        #endregion
        // Search By HashTagName
        public async Task<List<Post>> SearchByHashTagName(string hashtagname)
        {
            try
            {

                List<Post> hashtags = await _dbContext.Posts
                    .Include(h => h.Likes)
                    .Include(p => p.HashPosts).ThenInclude(hp => hp.Hashtag)
                    .Where(p => p.HashPosts.Any(hp => hp.Hashtag.Name.Contains(hashtagname)))
                    .ToListAsync();

                return hashtags;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<List<Hashtag>> SearchByHashTagNames(string hashtagname)
        {
            try
            {

                List<Hashtag> hashtags = await _dbContext.Hashtags
                        .Include(h => h.HashPosts).ThenInclude(J => J.Post)
                         .Where(a => a.Name.Contains(hashtagname))
                                .ToListAsync();
                return hashtags;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        #region Create Hashtag
        public async Task CreateHashTagAsync(Hashtag hashtag)
        {
            try
            {
                await this._dbContext.Hashtags.AddAsync(hashtag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get customer by name
        public async Task<Hashtag> GetHashTagByNameAsync(string name)
        {
            try
            {
                return await _dbContext.Hashtags.Include(c => c.HashPosts).ThenInclude(h => h.Post)
                                                 .SingleOrDefaultAsync(c => c.Name.Equals(name));
                                                 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


    }
}
