using GenZStyleAPP.BAL.DTOs.ChatHistorys;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GenZStyleApp_API.Controllers
{
    public class ChatsController : ODataController
    {
        private readonly IChatRepository _chatRepository;
        

        public ChatsController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;

        }

        [HttpGet("odata/ChatHistory/{ReceiverId}")]
        public async Task<IActionResult> Get(int receiver) 
        {
               List<GetChatHistoryResponse> getChatHistories = await _chatRepository.GetChats(receiver ,HttpContext);
               return Ok(getChatHistories);
        }



    }
}
