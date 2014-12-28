namespace Users.ViewModels.IdentityViewModels
{
    using System.Threading.Tasks;
    using Models.IdentityModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }

        [Display(Name = "Електронна поща")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Нова парола")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public static EditUserViewModel FromUser(AppUser user)
        {
            return new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
        }
    }
}