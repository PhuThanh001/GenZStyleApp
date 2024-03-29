using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.UserRelations
{
    public class GetFollowResponse
    {
        /*public int Follower { get; set; }

        public int Followering  { get; set; }*/

        public ICollection<GetAccountResponse> Followers { get; set; }
        public ICollection<GetAccountResponse> Following { get; set; }
    }
}
