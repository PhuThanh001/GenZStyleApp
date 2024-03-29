using FluentValidation;
using GenZStyleApp.BAL.Helpers;
using GenZStyleAPP.BAL.DTOs.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Package
{
    public class UpdatePackageValidation : AbstractValidator<UpdatePackageRequest>
    {
        private const int MAX_BYTES = 2048000;

        public UpdatePackageValidation()
        {
            #region PackageName
            RuleFor(p => p.PackageName)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            #region Description
            RuleFor(p => p.Description)
                 .NotNull().WithMessage("{PropertyName} is null.")
                 .NotEmpty().WithMessage("{PropertyName} is empty.")
                 .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            #endregion

            //#region Status
            //RuleFor(p => p.Status)
            //     .NotNull().WithMessage("{PropertyName} is null.")
            //     .NotEmpty().WithMessage("{PropertyName} is empty.")
            //     .Length(5, 120).WithMessage("{PropertyName} from {MinLength} to {MaxLength} characters.");
            //#endregion

            #region Cost
            RuleFor(w => w.Cost)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .InclusiveBetween(100, 999999999).WithMessage("{PropertyName} min is 1.000 VNĐ and max is 999.999.999 VNĐ");
            #endregion

            //#region IsStatus
            //RuleFor(w => w.IsStatus)
            //    .NotNull().WithMessage("{PropertyName} is null.")
            //    .NotEmpty().WithMessage("{PropertyName} is empty.")
            //    .InclusiveBetween(1000, 999999999).WithMessage("{PropertyName} min is 1.000 VNĐ and max is 999.999.999 VNĐ");
            //#endregion



            #region Image
            /*RuleFor(p => p.Image)
                   .NotEmpty().WithMessage("{PropertyName} is empty.")
                   .NotNull().WithMessage("{PropertyName} is null.");
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.Length).ExclusiveBetween(0, MAX_BYTES)
                   .WithMessage($"Image is required file length greater than 0 and less than {MAX_BYTES / 1024 / 1024} MB"));
            RuleFor(p => p.Image)
                   .ChildRules(pro => pro.RuleFor(img => img.FileName).Must(FileHelper.HaveSupportedFileType).WithMessage("Avatar is required extension type .png, .jpg, .jpeg"));*/
            #endregion
        }
    }
}
