using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMOS.BAL.Helpers;
using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Users;

namespace GenZStyleAPP.BAL.Validators.Users
{
    public class RegisterValidation : AbstractValidator<RegisterRequest>
    {
        private const int MAX_BYTES = 2048000;
        public RegisterValidation()
        {
            /*#region FullName
            RuleFor(c => c.FullName)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(5, 80).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion*/

            /*#region Address
            RuleFor(c => c.Address)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion*/

            #region Email
            RuleFor(c => c.Email)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(12, 100).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.")
                 .EmailAddress().WithMessage("{PropertyName} is in valid email format.");
            #endregion

            #region Phone
            RuleFor(c => c.Phone)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(10).WithMessage("Phone must be 10 characters.");
            #endregion

            /*#region PasswordHash
            RuleFor(c => c.PasswordHash)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(8, 10).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion*/

            /*#region Gender
            RuleFor(c => c.Gender)
                .Must(x => x == true || x == false).WithMessage("{PropertyName} must be either true or false.");
            #endregion*/

            #region BirthDate
            RuleFor(c => c.Dob)
                   .NotEmpty().WithMessage("{PropertyName} is empty.")
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Must(date => DateHelper.IsValidBirthday(date)).WithMessage("{PropertyName} must greater than 13 years old.")
                   .Must(date => DateHelper.IsValidBirthday(date)).WithMessage("{PropertyName} must greater than 13 years old.");
            #endregion

            /*#region Avatar
            RuleFor(p => p.Avatar)
                   .NotEmpty().WithMessage("{PropertyName} is empty.")
                   .NotNull().WithMessage("{PropertyName} is null.");
            RuleFor(p => p.Avatar)
                   .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                   .WithMessage($"Image is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            *//*RuleFor(p => p.Avatar)
                   .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));*//*
            #endregion*/
        }
    }
}
