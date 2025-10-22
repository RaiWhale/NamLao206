using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class PermissionGroupVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhóm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên nhóm không vượt quá 100 ký tự")]
        public string GroupName { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả không vượt quá 255 ký tự")]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public List<PermissionVM> Permissions { get; set; }
        public List<int> SelectedPermissionIds { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}