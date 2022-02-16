using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class FlightRoute
    {
        [Display(Name ="Route ID")]
        public int RouteID { get; set; }

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

        [Display(Name = "Flight Duration")]
        public int FlightDuration { get; set; }

    }
}
