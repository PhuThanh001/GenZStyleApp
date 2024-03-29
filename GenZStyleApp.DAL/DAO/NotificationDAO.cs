using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class NotificationDAO
    {
        private GenZStyleDbContext _dbContext;
        public NotificationDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddNotiAsync(Notification newNoti)
        {
            await this._dbContext.Notifications.AddAsync(newNoti);
        }
        //get notifications by createat new
        public async Task<List<Notification>> GetNotifications(int accountId)
        {
            try
            {
                List<Notification> notifications = await _dbContext.Notifications
                    .OrderByDescending(n => n.CreateAt)
                    .Where(u => u.AccountId == accountId)
                    .ToListAsync();

                return notifications;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
