using FluentValidation;
using GenZStyleAPP.BAL.DTOs.HashTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Hashtags
{
    public class PostHashTagRequestValidation : AbstractValidator<GetHashTagRequest>
    {
        private const int MAX_BYTES = 2048000;
        
        public PostHashTagRequestValidation() 
        {
            RuleFor(hash => hash.Name)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .MaximumLength(20).WithMessage("{PropertyName} max length is 20 characters.")
                .Must(name => name != null && name.StartsWith("#")).WithMessage("{PropertyName} must start with '#'.");
        }

    }
}
