using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Munizoft.Identity.Infrastructure.Models
{
    public class ServiceResult<T> : Tuple<T, Boolean, IEnumerable<ServiceError>>
    {
        public ServiceResult(T data, Boolean succeeded = true, IEnumerable<ServiceError> errors = null)
            : base(data, succeeded, errors)
        {

        }

        /// <summary>
        ///     Data property
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data
        {
            get { return this.Item1; }
        }

        /// <summary>
        ///     Succeeded property
        /// </summary>
        [JsonProperty("succeeded", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean Succeeded
        {
            get { return this.Item2; }
        }

        /// <summary>
        ///     Errors property
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ServiceError> Errors
        {
            get { return this.Item3; }
        }

        #region Helpers
        /// <summary>
        ///     Success helper method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> OK(T data)
        {
            return new ServiceResult<T>(data, true, null);
        }

        /// <summary>
        ///     Failed helper method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> Fail(String code, String message)
        {
            return new ServiceResult<T>(Activator.CreateInstance<T>(), false, new List<ServiceError>() { new ServiceError(code, message, String.Empty) });
        }

        /// <summary>
        ///     Failed helper method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> Fail(ServiceError error)
        {
            return new ServiceResult<T>(Activator.CreateInstance<T>(), false, new List<ServiceError>() { error });
        }

        /// <summary>
        ///     Failed helper method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> Fail(IEnumerable<ServiceError> errors)
        {
            return new ServiceResult<T>(Activator.CreateInstance<T>(), false, errors);
        }
        #endregion Helpers
    }
}