using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class PackageRegistrationDAO
    {
        private GenZStyleDbContext _dbContext;
        public PackageRegistrationDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public async Task AddNewPackageRegistration(PackageRegistration packageRegistration)
        {
            try
            {
                await _dbContext.packageRegistrations.AddAsync(packageRegistration);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
