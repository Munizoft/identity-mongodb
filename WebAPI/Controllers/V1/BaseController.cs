using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Munizoft.Identity.MongoDB.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController<TController> : ControllerBase
    {
        private readonly ILogger<TController> _logger;

        #region Properties
        protected Guid UserId
        {
            get
            {
                Guid userId;
                var id = this.User.Claims.Where(w => w.Type == "user_id").FirstOrDefault().Value;

                if (Guid.TryParse(id, out userId))
                {
                    return userId;
                }

                throw new Exception("Invalid user");
            }
        }

        protected String Role
        {
            get
            {
                var claim = this.User.Claims.Where(w => w.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(claim))
                {
                    return claim;
                }

                throw new Exception("Invalid Role");
            }
        }

        protected Boolean IsSuperAdmin
        {
            get
            {
                var claim = this.User.Claims.Where(w => w.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(claim) && string.Equals(claim, "super admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        protected Boolean IsAdmin
        {
            get
            {
                var claim = this.User.Claims.Where(w => w.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(claim) && string.Equals(claim, "admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        protected Boolean IsUser
        {
            get
            {
                var claim = this.User.Claims.Where(w => w.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(claim) && string.Equals(claim, "user", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }
        #endregion Properties

        public BaseController(ILogger<TController> logger)
        {
            _logger = logger;
        }

        protected ObjectResult GenericError(Exception ex)
        {
            return Problem(ex.Message);
        }

        protected ObjectResult GenericError(String error)
        {
            return Problem(error);
        }
    }
}