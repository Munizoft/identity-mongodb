using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Munizoft.Identity.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace Munizoft.Identity.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<ServiceError> ToServiceError(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            List<ServiceError> errors = new List<ServiceError>();

            errors.Add(new ServiceError(exception.Message));

            return errors;
        }

        public static IEnumerable<ServiceError> ToServiceError(this IdentityError exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            List<ServiceError> errors = new List<ServiceError>();

            errors.Add(new ServiceError(exception.Code, exception.Description));

            return errors;
        }

        public static IEnumerable<ServiceError> ToServiceError(this IEnumerable<IdentityError> identityErrors)
        {
            if (identityErrors == null)
                throw new ArgumentNullException(nameof(identityErrors));

            List<ServiceError> errors = new List<ServiceError>();

            foreach (var error in identityErrors)
            {
                errors.Add(new ServiceError(error.Code, error.Description));
            }

            return errors;
        }

        public static IEnumerable<ServiceError> ToServiceError(this ValidationResult validationResult)
        {
            if (validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));

            List<ServiceError> errors = new List<ServiceError>();

            foreach (var error in validationResult.Errors)
            {
                errors.Add(new ServiceError(error.ErrorCode, error.ErrorMessage));
            }

            return errors;
        }

        public static IEnumerable<ServiceError> ToServiceError(this String error)
        {
            if (String.IsNullOrEmpty(error))
                throw new ArgumentNullException(nameof(error));

            List<ServiceError> errors = new List<ServiceError>()
            {
                new ServiceError(error)
            };

            return errors;
        }
    }
}