using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay
{
    public class PostTransactionZaloResponse
    {
        public int? return_code { get; set; }
        public string? return_message { get; set; }
        public int? sub_return_code { get; set; }
        public string? sub_return_message { get; set; }
        public string? zp_trans_token { get; set; }
        public string? order_url { get; set; }
        public string? order_token { get; set; }

        public static PostTransactionZaloResponse FromDictionary(Dictionary<string, object> dictionary)
        {
            var response = new PostTransactionZaloResponse();
            response.return_code = dictionary["return_code"] as int?;
            response.return_message = dictionary["return_message"] as string;
            response.sub_return_code = dictionary["sub_return_code"] as int?;
            response.sub_return_message = dictionary["sub_return_message"] as string;
            response.zp_trans_token = dictionary["zp_trans_token"] as string;
            response.order_url = dictionary["order_url"] as string;
            response.order_token = dictionary["order_token"] as string;

            return response;
        }
    }
}
