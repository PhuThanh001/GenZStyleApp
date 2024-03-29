using FluentValidation;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Accounts
{
    public class LoginValidation : AbstractValidator<GetLoginRequest>
    {
        public LoginValidation()
        {
            #region Email
            RuleFor(a => a.UserName)
               .Cascade(CascadeMode.Continue)
               .NotEmpty().WithMessage("{PropertyName} is empty.")
               .NotNull().WithMessage("{PropertyName} is null.");
               /*.EmailAddress().WithMessage("{PropertyName} is invalid.");*/
            #endregion

            #region Password
            RuleFor(a => a.PasswordHash)
              .Cascade(CascadeMode.Continue)
              .NotEmpty().WithMessage("Password is empty.")
              .NotNull().WithMessage("Password is null.");
            #endregion
        }
    }
}
