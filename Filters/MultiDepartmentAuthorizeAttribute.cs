using NamLao206.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Filters
{
    public class MultiDepartmentAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly string _permissionCode;
        private readonly int[] _departmentIds;

        public MultiDepartmentAuthorizeAttribute(string permissionCode, params int[] departmentIds)
        {
            _permissionCode = permissionCode;
            _departmentIds = departmentIds;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            try
            {
                var accountId = int.Parse(httpContext.User.Identity.Name);
                using (var permissionService = new PermissionService())
                {
                    var userDepartmentId = permissionService.GetUserDepartment(accountId);

                    // Kiểm tra user có thuộc một trong các department được phép không
                    if (!_departmentIds.Contains(userDepartmentId))
                        return false;

                    // Kiểm tra permission
                    if (!string.IsNullOrEmpty(_permissionCode))
                    {
                        return permissionService.HasPermission(accountId, _permissionCode);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Authorization error: {ex.Message}");
                return false;
            }
        }
    }
}