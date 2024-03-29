using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMOS.BAL.Validators.Accounts
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidation()
        {
            #region OldPassWord
            RuleFor(c => c.OldPassword)
                 .Cascade(CascadeMode.Continue)
                 .NotEmpty().WithMessage("{PropertyName} is empty!")
                 .NotNull().WithMessage("{PropertyName} is null!")
                 .Length(5, 10).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion

            #region NewPassWord
            RuleFor(c => c.NewPassword)
                 .Cascade(CascadeMode.Continue)
                 .NotEmpty().WithMessage("{PropertyName} is empty!")
                 .NotNull().WithMessage("{PropertyName} is null!")
                 .Length(5, 10).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion

            #region ConFirmPassword
            RuleFor(c => c.ConfirmPassword)
                 .Cascade(CascadeMode.Continue)
                 .NotEmpty().WithMessage("{PropertyName} is empty!")
                 .NotNull().WithMessage("{PropertyName} is null!")
                 .Length(5, 10).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters!");
            #endregion
        }
    }
}
