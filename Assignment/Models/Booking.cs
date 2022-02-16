using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class Booking
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

        [Display(Name = "Amount Payable (SGD)")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public SqlMoney AmtPayable { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(3000, ErrorMessage = "Remarks cannot be longer than 3000 characters.")]
        public string? Remarks { get; set; }

        [Display(Name = "Date and Time Booking Created")]
        public DateTime DateTimeCreated { get; set; }

    }
}
