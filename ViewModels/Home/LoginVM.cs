using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class LoginVM
    {

        [Required(ErrorMessage = "This field is required!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
