using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Users.ViewModels.IdentityViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name="Потребителско име")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Електронна поща")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name="Повторно въвеждане на паролата")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Въведените пароли трябва да съвпадат !!!")]
        public string ConfirmPassword { get; set; }
    }
}