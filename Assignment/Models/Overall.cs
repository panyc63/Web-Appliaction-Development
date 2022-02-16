using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class Overall
    {
        [Display(Name = "Booking ID")]
        public int BookingID { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [Display(Name = "Schedule ID")]
        public int ScheduleID { get; set; }

        [Display(Name = "Passenger Name")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string PassengerName { get; set; }

        [Display(Name = "Passport Number")]
        [StringLength(20, ErrorMessage = "Passport number cannot be longer than 20 characters.")]
        public string PassportNumber { get; set; }

        [Display(Name = "Nationality")]
        [StringLength(50, ErrorMessage = "Nationality cannot be longer than 50 characters.")]
        public string Nationality { get; set; }

        [Display(Name = "Seat Class")]
        [StringLength(20, ErrorMessage = "Seat Class cannot be longer than 20 characters.")]
        public string SeatClass { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(3000, ErrorMessage = "Remarks cannot be longer than 3000 characters.")]
        public string? Remarks { get; set; }

        [Display(Name = "Date and Time Booking Created")]
        public DateTime DateTimeCreated { get; set; }

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

        [Display(Name = "Departure Date and Time")]
        public DateTime? DepartureDateTime { get; set; }

        [Display(Name = "Arrival Date and Time")]
        public DateTime? ArrivalDateTime { get; set; }
    }
}
