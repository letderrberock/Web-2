using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class CreateBook
    {
        [Required(ErrorMessage = "This field is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Description { get; set; }
        
    }
}
