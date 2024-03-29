using GenZStyleAPP.BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IEmailRepository
    {
        void SendEmail(Message message);
    }
}
