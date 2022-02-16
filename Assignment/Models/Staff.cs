using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class Staff
    {
        public int StaffID { get; set; }

        [Display(Name = "Staff name")]
        [MaxLength(50)]
        public string StaffName { get; set; }

        public char? Gender { get; set; }

        public DateTime? DateEmployed { get; set; }

        public string? Vocation { get; set; }

        [Display(Name = "Email address")]
        [MaxLength(50)]
        public string EmailAddr { get; set; }
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Wrong email format")]

        [Display(Name = "Password")]
        [MaxLength(255)]
        public string Password { get; set; }

        [Display(Name = "Status")]
        [MaxLength(50)]
        public string Status { get; set; }

    }
}
