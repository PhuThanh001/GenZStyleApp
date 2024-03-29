using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Validators.Comments
{
    public class GetCommentRequestValidator : AbstractValidator<GetCommentRequest>
    {
        private const int MAX_BYTES = 2048000;

        public GetCommentRequestValidator()
        {
            RuleFor(com => com.Content)
                .NotNull().WithMessage("{PropertyName} is null.")
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .MaximumLength(120).WithMessage("{PropertyName} max length is 120 characters.");
                
        }
    }
}
