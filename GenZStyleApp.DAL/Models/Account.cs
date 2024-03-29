using GenZStyleApp.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZStyleApp.DAL.Models
{
    public class Account 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int AccountId { get; set; }
        public int UserId { get; set; }

        public int? InboxId { get; set; }

       
        public string Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsVip { get; set; }
        public bool IsActive { get; set; }

        public Inbox Inbox { get; set; }
        public User User { get; set; }
        
        

        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<Token>? Tokens { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

        public virtual ICollection<UserRelation>? UserRelations { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<Message>? Messageee { get; set; }


        public virtual ICollection<InboxPaticipant>? InboxPaticipants { get; set; }
        public virtual ICollection<Style>? Styles { get; set; }
        [NotMapped]
        public ICollection<PackageRegistration>? PackageRegistrations { get; set; }

        

    }
}
