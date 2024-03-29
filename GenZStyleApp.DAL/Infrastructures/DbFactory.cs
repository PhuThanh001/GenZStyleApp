using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.DAL.Infrastructures
{
    public class DbFactory
    {
        private GenZStyleDbContext _dbContext;

        private DbFactory()
        {

        }
        private static DbFactory instance = null;
        private static readonly Object objectLock = new Object();
        public static DbFactory Instance
        {
            get
            {
                lock (objectLock)
                {
                    if (instance == null)
                    {
                        instance = new DbFactory();
                    }
                    return instance;
                }
            }
        }

        public GenZStyleDbContext InitDbContext()
        {
            if (_dbContext == null)
            {
                _dbContext = new GenZStyleDbContext();
            }
            return _dbContext;
        }
    }
}
