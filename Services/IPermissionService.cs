using NamLao206.Models.ViewModels;
using NamLao206.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Services
{
    public interface IPermissionService
    {
        // Core permission methods
        UserPermission GetUserPermissions(int accountId);
        List<MenuItem> GetUserMenu(int accountId);
        bool HasPermission(int accountId, string permissionCode);
        bool HasPermissionForDepartment(int accountId, string permissionCode, int? departmentId = null);

        // Department methods
        int GetUserDepartment(int accountId);
        string GetUserDepartmentName(int accountId);
        List<MenuItem> GetUserMenuByDepartment(int accountId);

        // Multiple permissions check
        bool HasAnyPermission(int accountId, params string[] permissionCodes);
        bool HasAllPermissions(int accountId, params string[] permissionCodes);

        // Group methods
        bool IsInGroup(int accountId, string groupName);
        List<PermissionGroupVM> GetUserPermissionGroups(int accountId);
        List<PermissionGroupVM> GetAllPermissionGroups();

        // Permission management
        List<PermissionVM> GetAllPermissions();
        List<PermissionVM> GetPermissionsByModule(string module);

        // User permission management
        List<UserPermissionVM> GetUsersWithPermissions(int? departmentId = null);
        UserPermissionVM GetUserPermissionDetails(int accountId);
        bool UpdateUserGroups(int accountId, List<int> groupIds, int updatedBy);

        // Group permission management
        bool UpdateGroupPermissions(int groupId, List<int> permissionIds, int updatedBy);
        bool CreatePermissionGroup(PermissionGroupVM group, int createdBy);
        bool UpdatePermissionGroup(PermissionGroupVM group, int updatedBy);
        bool DeletePermissionGroup(int groupId, int deletedBy);

        // Menu management
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByDepartment(int? departmentId);
        bool UpdateMenuItem(MenuItem menuItem, int updatedBy);

        // Utility methods
        void ClearUserCache(int accountId);
        List<DepartmentVM> GetDepartments();
    }
}