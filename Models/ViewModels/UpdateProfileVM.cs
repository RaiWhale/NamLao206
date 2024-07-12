using System.ComponentModel.DataAnnotations;

namespace NamLao206.Models.ViewModels
{
    public class UpdateProfileVM
    {
        [Key]
        public string Name { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Avatar { get; set; }

    }
}