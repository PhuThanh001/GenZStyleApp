using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ProjectParticipantManagement.BAL.Errors;

namespace ProjectParticipantManagement.WebAPI.Helpers
{
    public static class ErrorHelper
    {
        public static string GetErrorsString(ValidationResult validationResult)
        {
            List<ErrorDetail> errors = new List<ErrorDetail>();
            foreach (var error in validationResult.Errors)
            {
                ErrorDetail errorDetail = errors.FirstOrDefault(x => x.FieldNameError.Equals(error.PropertyName));
                if ( errorDetail == null)
                {
                    List<string> descriptionError = new List<string>();
                    descriptionError.Add(error.ErrorMessage);
                    ErrorDetail newErrorDetail = new ErrorDetail()
                    {
                        FieldNameError = error.PropertyName,
                        DescriptionError = descriptionError
                    };

                    errors.Add(newErrorDetail);
                } else
                {
                    errorDetail.DescriptionError.Add(error.ErrorMessage);
                }
            }

            var message = JsonConvert.SerializeObject(errors);
            return message;
        }
    }
}
