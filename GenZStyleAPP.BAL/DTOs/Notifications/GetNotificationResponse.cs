using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Notifications
{
    public class GetNotificationResponse
    {
        [Key]

        public int NotificationId { get; set; }

        public int AccountId { get; set; }

        public string Message { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public GetAccountResponse Account { get; set; }
    }
}
