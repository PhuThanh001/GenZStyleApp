using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Inboxs;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Accounts
{
    public class GetAccountResponse
    {
        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }

        public int? InboxId { get; set; }

        public int WalletId { get; set; }

        public int follower { get; set; }

        public int following { get; set; }//chỉnh sửa chỗ này bên hàm getlistsuggestion
        public bool isfollow { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool? Gender { get; set; }
        public decimal? Height { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsVip { get; set; }
        public bool IsActive { get; set; }
        /*public Inbox Inbox { get; set; }*/
        public GetUserPost User { get; set; }
        
        public string? AvatarUrl { get; set; }

        public virtual ICollection<GetPostResponse> Posts { get; set; }



    }
}
