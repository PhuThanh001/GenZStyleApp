using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class MessageDAO
    {
        private GenZStyleDbContext _dbContext;
        public MessageDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Message>> GetMessageByUser(int receiver ,int sender)
        {
            try 
            {
                 return await _dbContext.Messages.
                 Where(c => (c.SenderId == sender && c.ReceiverId == receiver) || (c.SenderId == receiver && c.ReceiverId == sender))
                .OrderBy(c => c.CreateAt).ToListAsync();

            }            
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }



        
    }
}
