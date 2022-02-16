using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class FlightSchedule
    {
        [Display(Name ="Schedule ID")]
        public int ScheduleID { get; set; }

        [Display(Name ="Flight Number")]
        [MaxLength(20)]
        public string FlightNumber { get; set; }

        [Display(Name = "Route ID")]
        public int RouteID { get; set; }

        [Display(Name = "Aircraft ID")]
        public int? AircraftID { get; set; }

        [Display(Name = "Departure Date and Time")]
        public DateTime? DepartureDateTime { get; set; }

        [Display(Name = "Arrival Date and Time")]
        public DateTime? ArrivalDateTime { get; set; }

        [Display(Name = "Economy Class Price")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}",ApplyFormatInEditMode = true)]
        public decimal EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public decimal BusinessClassPrice { get; set; }

        [Display(Name ="Status of Flight Schedule")]
        [MaxLength(20)]
        public string Status { get; set; }

    }
}
