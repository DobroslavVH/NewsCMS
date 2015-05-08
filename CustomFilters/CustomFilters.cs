using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCMS.CustomFilters
{
    public class AuthLogAttribute : AuthorizeAttribute
    {
        public AuthLogAttribute()
        {
            View = "AuthorizeFailed";
        }

        public string View { get; set; }

        // Check for authorization
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        // Method to check if the user is authorized or not
        // is yes continue the action else redirect to error message
        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            // If the result returns null then the user is authorized
            if (filterContext.Result == null)
                return;

            // If the user is Un-Authorized then navigate to auth failed view
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // var result = new viewresult {viewname = view}
                var vr = new ViewResult();
                vr.ViewName = View;

                ViewDataDictionary dict = new ViewDataDictionary();
                dict.Add("Message", "Sorry you are not authorized to do this...");

                vr.ViewData = dict;
                var result = vr;

                filterContext.Result = result;
            }
        }
    }
}