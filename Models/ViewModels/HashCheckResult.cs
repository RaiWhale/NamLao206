using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class HashCheckResult
    {
        public bool? IsSafe { get; set; } // null = hash không tồn tại
        public string Message { get; set; }
    }
}