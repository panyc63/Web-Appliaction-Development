using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class CustomerFS
    {
        [Display(Name = "Schedule ID")]
        public int ScheduleID { get; set; }

        [Display(Name = "Departure City")]
        [MaxLength(50)]
        public string DepartureCity { get; set; }

        [Display(Name = "Departure Country")]
        [MaxLength(50)]
        public string DepartureCountry { get; set; }

        [Display(Name = "Arrival City")]
        [MaxLength(50)]
        public string ArrivalCity { get; set; }

        [Display(Name = "Arrival Country")]
        [MaxLength(50)]
        public string ArrivalCountry { get; set; }

        [Display(Name = "Flight Number")]
        [MaxLength(20)]
        public string FlightNumber { get; set; }

        [Display(Name = "Economy Class Price")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}", ApplyFormatInEditMode = true)]
        public decimal EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public decimal BusinessClassPrice { get; set; }

        [Display(Name = "Departure Date and Time")]
        public DateTime? DepartureDateTime { get; set; }

        [Display(Name = "Arrival Date and Time")]
        public DateTime? ArrivalDateTime { get; set; }
    }
}
