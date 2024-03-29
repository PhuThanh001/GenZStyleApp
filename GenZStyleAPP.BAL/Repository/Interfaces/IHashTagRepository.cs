using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IHashTagRepository
    {
        public Task<List<GetPostForSearch>> SearchByHashTagName(string hashtag);
        public Task<List<GetAllHashTag>> GetHashTagsAsync();

        public  Task<GetHashTagResponse> AddNewHashTag(FireBaseImage fireBaseImage, GetHashTagRequest hashTagRequest);
    }
}
