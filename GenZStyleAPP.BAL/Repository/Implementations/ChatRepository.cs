using AutoMapper;
using GenZStyleAPP.BAL.DTOs.ChatHistorys;
using GenZStyleAPP.BAL.Helpers;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class ChatRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ChatRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetChatHistoryResponse>> GetChats(int receiver ,HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                var messages = _unitOfWork.MessageDAO.GetMessageByUser(receiver, accountStaff.AccountId);
                return this._mapper.Map<List<GetChatHistoryResponse>>(messages);

            }
            catch (Exception ex) 
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        } 
    }
}
