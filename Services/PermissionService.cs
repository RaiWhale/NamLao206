// Services/PermissionService.cs
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NamLao206.Services
{
    public class PermissionService : IPermissionService, IDisposable
    {
        private readonly namlao206_websiteEntities _db;
        private bool _disposed = false;

        public PermissionService()
        {
            _db = new namlao206_websiteEntities();
        }

        public PermissionService(namlao206_websiteEntities dbContext)
        {
            _db = dbContext;
        }

        #region Core Permission Methods

        public UserPermission GetUserPermissions(int accountId)
        {
            try
            {
                var permissionCodes = _db.UserPermissionGroups
                    .Where(ug => ug.AccountId == accountId && ug.PermissionGroup.IsActive)
                    .SelectMany(ug => ug.PermissionGroup.GroupPermissions)
                    .Where(gp => gp.Permission.IsActive)
                    .Select(gp => gp.Permission.PermissionCode)
                    .Distinct()
                    .ToList();

                return new UserPermission
                {
                    AccountId = accountId,
                    PermissionCodes = permissionCodes
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting permissions for user {accountId}: {ex.Message}");
                return new UserPermission { AccountId = accountId, PermissionCodes = new List<string>() };
            }
        }

        public List<MenuItem> GetUserMenu(int accountId)
        {
            var userPermissions = GetUserPermissions(accountId);
            var permissionCodes = userPermissions.PermissionCodes;

            var menuItems = _db.MenuItems
                .Where(m => m.IsActive &&
                           (string.IsNullOrEmpty(m.PermissionCode) ||
                            permissionCodes.Contains(m.PermissionCode)))
                .OrderBy(m => m.DisplayOrder)
                .ToList();

            return BuildMenuHierarchy(menuItems, null);
        }

        public bool HasPermission(int accountId, string permissionCode)
        {
            if (string.IsNullOrEmpty(permissionCode))
                return true;

            var userPermissions = GetUserPermissions(accountId);
            return userPermissions.PermissionCodes.Contains(permissionCode);
        }

        public bool HasPermissionForDepartment(int accountId, string permissionCode, int? departmentId = null)
        {
            if (!HasPermission(accountId, permissionCode))
                return false;

            if (departmentId.HasValue)
            {
                var userDepartment = GetUserDepartment(accountId);
                return userDepartment == departmentId;
            }

            return true;
        }

        #endregion

        #region Department Methods

        public int GetUserDepartment(int accountId)
        {
            return _db.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.Employee.DM_PhongBans.DM_DonVis.Id)
                .FirstOrDefault();
        }

        public string GetUserDepartmentName(int accountId)
        {
            return _db.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.Employee.DM_PhongBans.DM_DonVis.TenDonVi)
                .FirstOrDefault();
        }

        public List<MenuItem> GetUserMenuByDepartment(int accountId)
        {
            var userDepartment = GetUserDepartment(accountId);
            var userPermissions = GetUserPermissions(accountId);

            var menuItems = _db.MenuItems
                .Where(m => m.IsActive &&
                           (m.DepartmentId == null || m.DepartmentId == userDepartment) &&
                           (string.IsNullOrEmpty(m.PermissionCode) ||
                            userPermissions.PermissionCodes.Contains(m.PermissionCode)))
                .OrderBy(m => m.DisplayOrder)
                .ToList();

            return BuildMenuHierarchy(menuItems, null);
        }

        #endregion

        #region Multiple Permissions Check

        public bool HasAnyPermission(int accountId, params string[] permissionCodes)
        {
            if (permissionCodes == null || !permissionCodes.Any())
                return true;

            var userPermissions = GetUserPermissions(accountId);
            return permissionCodes.Any(pc => userPermissions.PermissionCodes.Contains(pc));
        }

        public bool HasAllPermissions(int accountId, params string[] permissionCodes)
        {
            if (permissionCodes == null || !permissionCodes.Any())
                return true;

            var userPermissions = GetUserPermissions(accountId);
            return permissionCodes.All(pc => userPermissions.PermissionCodes.Contains(pc));
        }

        #endregion

        #region Group Methods

        public bool IsInGroup(int accountId, string groupName)
        {
            return _db.UserPermissionGroups
                .Any(ug => ug.AccountId == accountId &&
                          ug.PermissionGroup.GroupName == groupName &&
                          ug.PermissionGroup.IsActive);
        }

        public List<PermissionGroupVM> GetUserPermissionGroups(int accountId)
        {
            return _db.UserPermissionGroups
                .Where(ug => ug.AccountId == accountId && ug.PermissionGroup.IsActive)
                .Select(ug => new PermissionGroupVM
                {
                    Id = ug.PermissionGroup.Id,
                    GroupName = ug.PermissionGroup.GroupName,
                    Description = ug.PermissionGroup.Description,
                    IsActive = ug.PermissionGroup.IsActive,
                    Permissions = ug.PermissionGroup.GroupPermissions
                        .Where(gp => gp.Permission.IsActive)
                        .Select(gp => new PermissionVM
                        {
                            Id = gp.Permission.Id,
                            PermissionName = gp.Permission.PermissionName,
                            PermissionCode = gp.Permission.PermissionCode,
                            Description = gp.Permission.Description,
                            Module = gp.Permission.Module,
                            IsActive = gp.Permission.IsActive
                        }).ToList()
                })
                .ToList();
        }

        public List<PermissionGroupVM> GetAllPermissionGroups()
        {
            return _db.PermissionGroups
                .Where(pg => pg.IsActive)
                .Select(pg => new PermissionGroupVM
                {
                    Id = pg.Id,
                    GroupName = pg.GroupName,
                    Description = pg.Description,
                    IsActive = pg.IsActive,
                    CreatedDate = pg.CreatedDate.Value,
                    Permissions = pg.GroupPermissions
                        .Where(gp => gp.Permission.IsActive)
                        .Select(gp => new PermissionVM
                        {
                            Id = gp.Permission.Id,
                            PermissionName = gp.Permission.PermissionName,
                            PermissionCode = gp.Permission.PermissionCode,
                            Description = gp.Permission.Description,
                            Module = gp.Permission.Module,
                            IsActive = gp.Permission.IsActive,
                            IsSelected = true
                        }).ToList()
                })
                .OrderBy(pg => pg.GroupName)
                .ToList();
        }

        #endregion

        #region Permission Management

        public List<PermissionVM> GetAllPermissions()
        {
            return _db.Permissions
                .Where(p => p.IsActive)
                .Select(p => new PermissionVM
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    PermissionCode = p.PermissionCode,
                    Description = p.Description,
                    Module = p.Module,
                    IsActive = p.IsActive,
                    CreatedDate = p.CreatedDate.Value
                })
                .OrderBy(p => p.Module)
                .ThenBy(p => p.PermissionName)
                .ToList();
        }

        public List<PermissionVM> GetPermissionsByModule(string module)
        {
            return _db.Permissions
                .Where(p => p.IsActive && p.Module == module)
                .Select(p => new PermissionVM
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    PermissionCode = p.PermissionCode,
                    Description = p.Description,
                    Module = p.Module,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.PermissionName)
                .ToList();
        }

        #endregion

        #region User Permission Management

        public List<UserPermissionVM> GetUsersWithPermissions(int? departmentId = null)
        {
            var query = _db.Accounts
                .Where(a => a.Employee.DM_PhongBans.DM_DonVis.IsActive);

            if (departmentId.HasValue)
            {
                query = query.Where(a => a.Employee.DM_PhongBans.DM_DonVis.Id == departmentId);
            }

            var users = query.Select(a => new UserPermissionVM
            {
                AccountId = a.Id,
                Username = a.LoginName,
                EmployeeName = a.Employee.Name,
                DepartmentName = a.Employee.DM_PhongBans.DM_DonVis.TenDonVi,
                DepartmentId = a.Employee.DM_PhongBans.DM_DonVis.Id
            }).ToList();

            // Lấy groups của từng user
            var userGroups = _db.UserPermissionGroups
          .Where(ug => ug.PermissionGroup.IsActive)
          .GroupBy(ug => ug.AccountId)
          .ToDictionary(
              g => g.Key,
              g => g.Select(ug => ug.GroupId)
                    .Where(id => id.HasValue) // Lọc bỏ null
                    .Select(id => id.Value)   // Convert int? thành int
                    .ToList()
          );

            foreach (var user in users)
            {
                user.SelectedGroupIds = userGroups.ContainsKey(user.AccountId)
                    ? userGroups[user.AccountId]
                    : new List<int>();

                user.UserPermissions = GetUserPermissions(user.AccountId).PermissionCodes;
            }

            return users;
        }

        public UserPermissionVM GetUserPermissionDetails(int accountId)
        {
            var user = _db.Accounts
     .Where(a => a.Id == accountId)
     .Select(a => new UserPermissionVM
     {
         AccountId = a.Id,
         Username = a.LoginName,
         EmployeeName = a.Employee.Name,
         DepartmentName = a.Employee.DM_PhongBans.DM_DonVis.TenDonVi,
         DepartmentId = a.Employee.DM_PhongBans.DM_DonVis.Id,
         SelectedGroupIds = a.UserPermissionGroups
             .Where(ug => ug.PermissionGroup.IsActive && ug.GroupId.HasValue)
             .Select(ug => ug.GroupId.Value)  // Sử dụng .Value
             .ToList(),
         UserPermissions = GetUserPermissions(accountId).PermissionCodes
     })
     .FirstOrDefault();
            if (user != null)
            {
                user.AvailableGroups = GetAllPermissionGroups();
            }

            return user;
        }

        public bool UpdateUserGroups(int accountId, List<int> groupIds, int updatedBy)
        {
            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        // Xóa groups cũ
                        var existingGroups = _db.UserPermissionGroups.Where(ug => ug.AccountId == accountId);
                        _db.UserPermissionGroups.RemoveRange(existingGroups);

                        // Thêm groups mới
                        foreach (var groupId in groupIds)
                        {
                            _db.UserPermissionGroups.Add(new UserPermissionGroup
                            {
                                AccountId = accountId,
                                GroupId = groupId,
                                CreatedBy = updatedBy,
                                CreatedDate = DateTime.Now
                            });
                        }

                        _db.SaveChanges();
                        transaction.Commit();

                        // Clear cache
                        ClearUserCache(accountId);

                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating user groups for {accountId}: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Group Permission Management

        public bool UpdateGroupPermissions(int groupId, List<int> permissionIds, int updatedBy)
        {
            try
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        // Xóa permissions cũ
                        var existingPermissions = _db.GroupPermissions.Where(gp => gp.GroupId == groupId);
                        _db.GroupPermissions.RemoveRange(existingPermissions);

                        // Thêm permissions mới
                        foreach (var permissionId in permissionIds)
                        {
                            _db.GroupPermissions.Add(new GroupPermission
                            {
                                GroupId = groupId,
                                PermissionId = permissionId,
                                CreatedBy = updatedBy,
                                CreatedDate = DateTime.Now
                            });
                        }

                        _db.SaveChanges();
                        transaction.Commit();

                        // Clear cache của tất cả user trong group này
                        var userIds = _db.UserPermissionGroups
                            .Where(ug => ug.GroupId == groupId)
                            .Select(ug => ug.AccountId)
                            .ToList();

                        foreach (var userId in userIds)
                        {
                            ClearUserCache(userId);
                        }

                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating group permissions for {groupId}: {ex.Message}");
                return false;
            }
        }

        public bool CreatePermissionGroup(PermissionGroupVM group, int createdBy)
        {
            try
            {
                var newGroup = new PermissionGroup
                {
                    GroupName = group.GroupName,
                    Description = group.Description,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                };

                _db.PermissionGroups.Add(newGroup);
                _db.SaveChanges();

                // Thêm permissions nếu có
                if (group.SelectedPermissionIds != null && group.SelectedPermissionIds.Any())
                {
                    UpdateGroupPermissions(newGroup.Id, group.SelectedPermissionIds, createdBy);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating permission group: {ex.Message}");
                return false;
            }
        }

        public bool UpdatePermissionGroup(PermissionGroupVM group, int updatedBy)
        {
            try
            {
                var existingGroup = _db.PermissionGroups.Find(group.Id);
                if (existingGroup == null)
                    return false;

                existingGroup.GroupName = group.GroupName;
                existingGroup.Description = group.Description;
                existingGroup.IsActive = group.IsActive;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating permission group {group.Id}: {ex.Message}");
                return false;
            }
        }

        public bool DeletePermissionGroup(int groupId, int deletedBy)
        {
            try
            {
                var group = _db.PermissionGroups.Find(groupId);
                if (group == null)
                    return false;

                // Soft delete
                group.IsActive = false;
                _db.SaveChanges();

                // Clear cache của tất cả user trong group này
                var userIds = _db.UserPermissionGroups
                    .Where(ug => ug.GroupId == groupId)
                    .Select(ug => ug.AccountId)
                    .ToList();

                foreach (var userId in userIds)
                {
                    ClearUserCache(userId);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting permission group {groupId}: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Menu Management

        public List<MenuItem> GetAllMenuItems()
        {
            return _db.MenuItems
                .OrderBy(m => m.DisplayOrder)
                .ToList();
        }

        public List<MenuItem> GetMenuItemsByDepartment(int? departmentId)
        {
            var query = _db.MenuItems.AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(m => m.DepartmentId == null || m.DepartmentId == departmentId);
            }

            return query.OrderBy(m => m.DisplayOrder).ToList();
        }

        public bool UpdateMenuItem(MenuItem menuItem, int updatedBy)
        {
            try
            {
                var existingItem = _db.MenuItems.Find(menuItem.Id);
                if (existingItem == null)
                    return false;

                existingItem.MenuName = menuItem.MenuName;
                existingItem.MenuUrl = menuItem.MenuUrl;
                existingItem.ParentId = menuItem.ParentId;
                existingItem.IconClass = menuItem.IconClass;
                existingItem.PermissionCode = menuItem.PermissionCode;
                existingItem.DisplayOrder = menuItem.DisplayOrder;
                existingItem.IsActive = menuItem.IsActive;
                existingItem.DepartmentId = menuItem.DepartmentId;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating menu item {menuItem.Id}: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Utility Methods

        public void ClearUserCache(int accountId)
        {
            // Implementation for cache clearing
            // Ví dụ: _cache.Remove($"user_permissions_{accountId}");
            // Ví dụ: _cache.Remove($"user_menu_{accountId}");
        }

        public List<DepartmentVM> GetDepartments()
        {
            return _db.DM_DonVis
                .Where(d => d.IsActive)
                .Select(d => new DepartmentVM
                {
                    Id = d.Id,
                    TenDonVi = d.TenDonVi,              
                    IsActive = d.IsActive
                })
                .OrderBy(d => d.TenDonVi)
                .ToList();
        }

        #endregion

        #region Private Methods

        private List<MenuItem> BuildMenuHierarchy(List<MenuItem> allItems, int? parentId)
        {
            return allItems
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new MenuItem
                {
                    Id = x.Id,
                    MenuName = x.MenuName,
                    MenuUrl = x.MenuUrl,
                    IconClass = x.IconClass,
                    PermissionCode = x.PermissionCode,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    DepartmentId = x.DepartmentId,
                    MenuItems1 = BuildMenuHierarchy(allItems, x.Id)
                })
                .ToList();
        }

        #endregion

        public List<MenuItem> GetUserMenuByUserDepartment(int accountId)
        {
            try
            {
                // Lấy departmentId của user
                var userDepartmentId = GetUserDepartment(accountId);
                var userPermissions = GetUserPermissions(accountId);

                // Lấy menu items: 
                // - IsActive = true
                // - DepartmentId = null (cho tất cả) HOẶC DepartmentId = userDepartmentId
                // - Có PermissionCode thì phải có quyền, không có PermissionCode thì hiển thị luôn
                var menuItems = _db.MenuItems
                    .Where(m => m.IsActive &&
                               (m.DepartmentId == null || m.DepartmentId == userDepartmentId) &&
                               (string.IsNullOrEmpty(m.PermissionCode) ||
                                userPermissions.PermissionCodes.Contains(m.PermissionCode)))
                    .OrderBy(m => m.DisplayOrder)
                    .ToList();

                return BuildMenuHierarchy(menuItems, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting department menu for user {accountId}: {ex.Message}");
                return new List<MenuItem>();
            }
        }

        // Lấy tên đơn vị của user
        public string GetUserFullDepartmentInfo(int accountId)
        {
            return _db.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.Employee.DM_PhongBans.DM_DonVis.TenDonVi)
                .FirstOrDefault() ?? "Không xác định";
        }

        // Kiểm tra xem user có thuộc đơn vị cụ thể không
        public bool IsUserInDepartment(int accountId, int departmentId)
        {
            var userDeptId = GetUserDepartment(accountId);
            return userDeptId == departmentId;
        }
        #region IDisposable Implementation

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}