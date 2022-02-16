using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class ContactUs
    {
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]

        public string Lastname { get; set; }
        public string Description { get; set; }
    }
}
