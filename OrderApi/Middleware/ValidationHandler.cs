using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace OrderApi.Middleware
{
    public class ValidationHandler : IValidatorInterceptor
    {
        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext, ValidationResult result)
        {
            var modifyResult = result.Errors.Select(x => new ValidationFailure(x.PropertyName, $"Property: {x.PropertyName}, AttemptedValue: {x.AttemptedValue}, ErrrorMessage: {x.ErrorMessage}"));

            return new ValidationResult(modifyResult);
        }

        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
