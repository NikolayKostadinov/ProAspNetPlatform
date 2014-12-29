using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Users.ViewModels.IdentityViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Достъпна за администратори")]
        public bool IsAvailableForAdministrators { get; set; }
    }
}