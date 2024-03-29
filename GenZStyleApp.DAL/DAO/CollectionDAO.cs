using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class CollectionDAO
    {
        private GenZStyleDbContext _dbContext;
        public CollectionDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Add new collection
        public async Task AddNewCollection(Collection collection)
        {
            try
            {
                await _dbContext.Collections.AddAsync(collection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Collection>> GetAllCollections(int accountId)
        {
            try
            {
                List<Collection> collections = await _dbContext.Collections
                    .Include(c => c.Post)
                        .ThenInclude(p => p.HashPosts)
                        .ThenInclude(hp => hp.Hashtag)
                    .Include(c => c.Post)
                        .ThenInclude(p => p.Likes)
                    .Include(c => c.Post)
                        .ThenInclude(p => p.Comments)
                    .Where( c => c.AccountId == accountId && c.IsSaved == true)
                    .ToListAsync();

                return collections;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Collection> GetAllCollectionsByCollectionId(int collectionId)
        {
            try
            {
                Collection collections = await _dbContext.Collections
                    .SingleOrDefaultAsync(p => p.Id == collectionId);

                return collections;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Collection> GetAllCollectionsByPostId(int postId)
        {
            try
            {
                Collection collections = await _dbContext.Collections.Include(u => u.Post)
                    .SingleOrDefaultAsync(p => p.PostId == postId);

                return collections;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
