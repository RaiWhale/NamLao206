using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Models.ViewModels
{
    public class AccountVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
        public string Username { get; set; }

        [Display(Name = "Họ tên nhân viên")]
        public string EmployeeName { get; set; }

        [Display(Name = "Đơn vị")]
        public int DepartmentId { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public List<int> SelectedGroupIds { get; set; }
        public List<SelectListItem> AvailableGroups { get; set; }
    }
}