using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.UserRelations
{
    public class GetUserRelationResponse
    {
        public int FollowerId { get; set; }

        public int FollowingId { get; set; }

        public bool isFollow { get; set; }

        public GetAccountResponse Account { get; set; }

        
    }
}
