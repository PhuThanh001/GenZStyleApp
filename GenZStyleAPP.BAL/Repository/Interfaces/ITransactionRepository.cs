using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface  ITransactionRepository
    {
        public Task<GetTransactionResponse> CreateWalletTransactionAsync(PostTransactionRequest model,int packageId, MomoConfigModel _config);

        public Task<GetTransactionResponse> PaymentNotificationAsync(string id, MomoConfigModel _config);

        public Task<PostTransactionZaloResponse> CreateZaloTransactionAsync(PostTransactionRequest model,int packageId, ZaloConfigModel config);
        public Task<GetTransactionResponse> PaymentNotificationAsync(string id, ZaloConfigModel _config);
    }
}
