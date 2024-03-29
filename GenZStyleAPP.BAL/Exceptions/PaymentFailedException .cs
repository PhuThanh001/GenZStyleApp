using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Exceptions
{
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException() : base("Payment failed.")
        {
            
        }

        public PaymentFailedException(string message) : base(message)
        {
            
        }

        public PaymentFailedException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
