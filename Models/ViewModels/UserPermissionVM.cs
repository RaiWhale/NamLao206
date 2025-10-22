using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class UserPermissionVM
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public List<PermissionGroupVM> AvailableGroups { get; set; }
        public List<int> SelectedGroupIds { get; set; }
        public List<string> UserPermissions { get; set; }
    }
}