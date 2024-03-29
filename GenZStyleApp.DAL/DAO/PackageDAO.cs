using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class PackageDAO
    {
        private GenZStyleDbContext _dbContext;
        public PackageDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            try 
            { 
                return await _dbContext.Packages.Include(p => p.Invoices)
                                                .FirstOrDefaultAsync(p => p.PackageId == packageId);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        // Add new Package
        public async Task AddNewPackage(Package package)
        {
            try
            {
                await _dbContext.Packages.AddAsync(package);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Update Package
        public void UpdatePackage(Package package)
        {
            try
            {
                this._dbContext.Entry<Package>(package).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public async Task<List<Package>> GetAllPackages()
        {
            try
            {
                List<Package> packages = await _dbContext.Packages
                    .AsNoTracking()
                    .ToListAsync();

                return packages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
