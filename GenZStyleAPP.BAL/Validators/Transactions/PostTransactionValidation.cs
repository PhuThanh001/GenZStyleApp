using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Transactions
{
    public class PostTransactionValidation : AbstractValidator<PostTransactionRequest>
    {
        public PostTransactionValidation()
        {
            #region Email
            RuleFor(w => w.Email)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .EmailAddress().WithMessage("{PropertyName} is invalid format email.");
            #endregion

            #region Amount
            RuleFor(w => w.Amount)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .InclusiveBetween(1000, 999999999).WithMessage("{PropertyName} min is 1.000 VNĐ and max is 999.999.999 VNĐ");
            #endregion

            #region RedirectUrl
            RuleFor(w => w.RedirectUrl)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.");
            #endregion
        }
    }
}
