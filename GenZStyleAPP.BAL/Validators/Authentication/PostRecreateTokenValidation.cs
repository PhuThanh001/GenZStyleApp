using BMOS.BAL.DTOs.Authentications;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Authentication
{
    public class PostRecreateTokenValidation : AbstractValidator<PostRecreateTokenRequest>
    {
        public PostRecreateTokenValidation()
        {
            #region Access token
            RuleFor(rc => rc.AccessToken)
              .Cascade(CascadeMode.Continue)
              .NotNull().WithMessage("{PropertyName} is null.")
              .NotEmpty().WithMessage("{PropertyName} is empty.");
            #endregion

            #region Refresh Token
            RuleFor(rc => rc.RefreshToken)
              .Cascade(CascadeMode.Continue)
              .NotNull().WithMessage("{PropertyName} is null.")
              .NotEmpty().WithMessage("{PropertyName} is empty.");
            #endregion
        }
    }
}
