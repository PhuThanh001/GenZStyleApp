using GenZStyleAPP.BAL.DTOs.Comments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<GetCommentResponse>> GetCommentByPostId(int id);


        public Task UpdateCommentByPostId(GetCommentRequest commentRequest, int PostId, HttpContext httpContext);

        public  Task DeleteCommentById(int id);


    }
}
