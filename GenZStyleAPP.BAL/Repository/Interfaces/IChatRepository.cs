using GenZStyleAPP.BAL.DTOs.ChatHistorys;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IChatRepository
    {
        public Task<List<GetChatHistoryResponse>> GetChats(int receiver, HttpContext httpContext);
    }
}
