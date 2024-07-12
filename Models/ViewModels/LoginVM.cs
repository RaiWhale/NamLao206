using System.ComponentModel.DataAnnotations;

namespace NamLao206.Models.ViewModels
{
    public class LoginVM
    {
        [Key]
        public string LoginName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}