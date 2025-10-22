using NamLao206.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace NamLao206.Filters
{
    public class DynamicAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly string _permissionCode;
        private static readonly ObjectCache _cache = MemoryCache.Default;

        public DynamicAuthorizeAttribute(string permissionCode = "")
        {
            _permissionCode = permissionCode;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            try
            {
                var accountId = int.Parse(httpContext.User.Identity.Name);
                var cacheKey = $"UserPermissions_{accountId}";

                // Kiểm tra cache trước
                if (_cache.Contains(cacheKey))
                {
                    var cachedPermissions = _cache.Get(cacheKey) as List<string>;
                    if (cachedPermissions != null)
                    {
                        return string.IsNullOrEmpty(_permissionCode) ||
                               cachedPermissions.Contains(_permissionCode);
                    }
                }

                // Nếu không có cache, query database
                using (var permissionService = new PermissionService())
                {
                    var userPermissions = permissionService.GetUserPermissions(accountId);

                    // Lưu vào cache (5 phút)
                    _cache.Add(cacheKey, userPermissions.PermissionCodes,
                              DateTimeOffset.Now.AddMinutes(5));

                    return string.IsNullOrEmpty(_permissionCode) ||
                           userPermissions.PermissionCodes.Contains(_permissionCode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Authorization error: {ex.Message}");
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Redirect to login
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" },
                        { "returnUrl", filterContext.HttpContext.Request.Url?.PathAndQuery }
                    });
            }
            else
            {
                // Access denied
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "AccessDenied" }
                    });
            }
        }
    }
}