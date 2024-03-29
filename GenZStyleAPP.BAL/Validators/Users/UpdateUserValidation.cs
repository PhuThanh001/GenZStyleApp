using BMOS.BAL.Helpers;
using FluentValidation;
using GenZStyleApp.BAL.Helpers;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Users
{

    public class UpdateUserValidation : AbstractValidator<UpdateUserRequest>
    {
        private const int MAX_BYTES = 2048000;
        public UpdateUserValidation()
        {
            #region City
            RuleFor(p => p.City)
                 .MaximumLength(120).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.")
                 .When(p => p.City != null); // Điều kiện kiểm tra giá trị không null
            #endregion

            #region Address
            RuleFor(p => p.Address)
                 .MaximumLength(1000).WithMessage("{PropertyName} cannot exceed {MaxLength} characters.")
                 .When(p => p.Address != null); // Điều kiện kiểm tra giá trị không null
            #endregion

            #region Height
            RuleFor(p => p.Height)
                 .ExclusiveBetween(0, 1000000000).WithMessage("{PropertyName} must be greater than 0 cm.")
                 .When(p => p.Height != null); // Điều kiện kiểm tra giá trị không null
            #endregion

            /*#region Phone
            RuleFor(c => c.Phone)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Length(10).WithMessage("Phone must be 10 characters.");
            #endregion*/

            #region Gender
            RuleFor(c => c.Gender)
                .Must(x => x == true || x == false).WithMessage("{PropertyName} must be either true or false.");
            #endregion

            #region Dob
            RuleFor(c => c.Dob)
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .Must(Dob => DateHelper.IsValidBirthday(Dob)).WithMessage("{PropertyName} must greater than 16 year old.")
                 .Must(Dob => DateHelper.IsValidBirthday(Dob)).WithMessage("{PropertyName} must greater than 16 year old.");
            #endregion

            #region Avatar
            RuleFor(p => p.Avatar)
                 .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                 .WithMessage($"Avatar is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            RuleFor(p => p.Avatar)
                 .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));
            #endregion
        }
    }
}
