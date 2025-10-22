using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class PermissionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên quyền là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên quyền không vượt quá 100 ký tự")]
        public string PermissionName { get; set; }

        [Required(ErrorMessage = "Mã quyền là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã quyền không vượt quá 50 ký tự")]
        [RegularExpression(@"^[A-Z_]+$", ErrorMessage = "Mã quyền chỉ chứa chữ hoa và dấu gạch dưới")]
        public string PermissionCode { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả không vượt quá 255 ký tự")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Module không vượt quá 50 ký tự")]
        public string Module { get; set; }

        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}