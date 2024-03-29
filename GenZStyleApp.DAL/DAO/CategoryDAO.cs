using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class CategoryDAO
    {
        private GenZStyleDbContext _dbContext;
        public CategoryDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region Get category by name
        public async Task<Category> GetPostByNameAsync(string name)
        {
            try
            {
                return await _dbContext.Categories

                   .SingleOrDefaultAsync(p => p.CategoryName == name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task AddNewCategory(Category category)
        {
            try
            {
                await _dbContext.Categories.AddAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
