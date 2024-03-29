using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class RoleDAO
    {
        private GenZStyleDbContext _dbContext;
        public RoleDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            try
            {
                return await this._dbContext.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
