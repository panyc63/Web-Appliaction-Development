using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class Customer
    {
        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }


        [Display(Name = "Customer Name")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string CustomerName { get; set; }

        
        [Display(Name = "Nationality")]
        [StringLength(50, ErrorMessage = "Natonality cannot be longer than 50 characters.")]
        public string? Nationality { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Telephone Number")]
        [StringLength(50, ErrorMessage = "Phone number cannot be longer than 50 characters.")]
        [RegularExpression(@"^\+[1-9]{1}[0-9]{3,14}$")]
        public string? TelNo { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email Address cannot be longer than 50 characters.")]
        [RegularExpression( @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")]
        [ValidateEmailExists]
        public string EmailAddr { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(255, ErrorMessage = "Password cannot be longer than 255 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [StringLength(255, ErrorMessage = "Password cannot be longer than 255 characters.")]
        public string OldPassword { get; set; }

        // Change password fields
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(255, ErrorMessage = "Password cannot be longer than 255 characters.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [StringLength(255, ErrorMessage = "Password cannot be longer than 255 characters.")]
        [Compare("NewPassword", ErrorMessage = "This does not match your new password.")]
        public string ConfirmPassword { get; set; }
    }
}
