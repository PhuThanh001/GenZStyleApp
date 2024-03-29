using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.Enums
{
    public static class TransactionEnum
    {   

        public enum RechangeStatus
        {
            PENDING = 0,
            SUCCESSED = 1,
            FAILED = 2,
        }
        public enum TransactionType
        {
            DEPOSIT,
            SEND,
            RECEIVE,

        }
    }
}
