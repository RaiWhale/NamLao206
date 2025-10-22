using NamLao206.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Filters
{
    public class DepartmentAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _permissionCode;
        private readonly int? _departmentId;
        private readonly bool _checkDepartment;
        private static readonly ObjectCache _cache = MemoryCache.Default;

        public DepartmentAuthorizeAttribute(int departmentId)
        {
            _departmentId = departmentId;
            _checkDepartment = true;
        }

        public DepartmentAuthorizeAttribute(string permissionCode, int departmentId)
        {
            _permissionCode = permissionCode;
            _departmentId = departmentId;
            _checkDepartment = true;
        }

        public DepartmentAuthorizeAttribute(string permissionCode)
        {
            _permissionCode = permissionCode;
            _checkDepartment = false;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            try
            {
                var accountId = int.Parse(httpContext.User.Identity.Name);
                var cacheKey = $"UserAuth_{accountId}_{_permissionCode}_{_departmentId}";

                // Kiểm tra cache
                if (_cache.Contains(cacheKey))
                {
                    return (bool)_cache.Get(cacheKey);
                }

                using (var permissionService = new PermissionService())
                {
                    bool isAuthorized = CheckAuthorization(permissionService, accountId);

                    // Cache kết quả trong 5 phút
                    _cache.Add(cacheKey, isAuthorized, DateTimeOffset.Now.AddMinutes(5));

                    return isAuthorized;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Authorization error: {ex.Message}");
                return false;
            }
        }

        private bool CheckAuthorization(PermissionService permissionService, int accountId)
        {
            var userDepartmentId = permissionService.GetUserDepartment(accountId);

            // Kiểm tra department
            if (_checkDepartment && _departmentId.HasValue)
            {
                if (userDepartmentId != _departmentId.Value)
                    return false;
            }

            // Kiểm tra permission
            if (!string.IsNullOrEmpty(_permissionCode))
            {
                if (!permissionService.HasPermission(accountId, _permissionCode))
                    return false;
            }

            return true;
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
                // Show access denied page
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Error" },
                        { "action", "AccessDenied" }
                    });
            }
        }
    }
}