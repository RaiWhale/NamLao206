using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Models.ViewModels
{
    public class MenuItemVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên menu là bắt buộc")]
        public string MenuName { get; set; }

        public string MenuUrl { get; set; }
        public int? ParentId { get; set; }
        public string IconClass { get; set; }
        public string PermissionCode { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> AvailablePermissions { get; set; }
        public List<SelectListItem> AvailableDepartments { get; set; }
        public List<SelectListItem> ParentMenus { get; set; }
    }
}