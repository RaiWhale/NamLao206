using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class CopyQuyenViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn đơn vị nguồn")]
        [Display(Name = "Đơn vị nguồn")]
        public int DonViNguon_Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đơn vị đích")]
        [Display(Name = "Đơn vị đích")]
        public int DonViDich_Id { get; set; }

        [Display(Name = "Ghi đè dữ liệu cũ")]
        public bool GhiDe { get; set; }

        public List<MenuItem> MenuItemsNguon { get; set; }
        public List<MenuItem> MenuItemsDich { get; set; }
    }
}