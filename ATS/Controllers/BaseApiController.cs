using ATS.Infrastructure;
using ATS.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ATS.Service.Google;

namespace ATS.Controllers
{
    public class BaseApiController : ApiController
    {

        #region ApplicationUserManager
        private ApplicationUserManager _AppUserManager = null;
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        #endregion

        #region ApplicationRoleManager
        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        #endregion

        #region ModelFactory
        private ModelFactory _modelFactory;
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }
        #endregion

        #region GetErrorResult
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        #endregion

        #region IAuthenticationManager Authentication
        protected IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }  
        #endregion

        #region AuthRepository
        private AuthRepository _authRepository = null;
        protected AuthRepository AuthRepository
        {
            get
            {
                return _authRepository ?? new AuthRepository();
            }
        }
        #endregion
    }
}
