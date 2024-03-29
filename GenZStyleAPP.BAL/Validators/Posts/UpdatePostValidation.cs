using FluentValidation;
using GenZStyleApp.BAL.Helpers;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Posts
{
    public class UpdatePostValidation : AbstractValidator<UpdatePostRequest>
    {
        private const int MAX_BYTES = 2048000;

        public UpdatePostValidation()
        {
            #region Content
            RuleFor(p => p.Content)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            /*#region CreateTime
            RuleFor(p => p.CreateTime)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Custom((expiredDate, context) =>
                   {
                       if (expiredDate.Date.CompareTo(DateTime.Now.Date) < 0)
                       {
                           context.AddFailure("CreateTime must be greater than or equal today.");
                       }
                   });
            #endregion*/

            /*#region UpdateTime
            RuleFor(p => p.UpdateTime)
                   .NotNull().WithMessage("{PropertyName} is null.")
                   .Custom((expiredDate, context) =>
                   {
                       if (expiredDate.Date.CompareTo(DateTime.Now.Date) < 0)
                       {
                           context.AddFailure("UpdateTime must be greater than or equal today.");
                       }
                   });
            #endregion*/

            #region Image
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                   .WithMessage($"Image is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));
            #endregion



        }
    }
}
