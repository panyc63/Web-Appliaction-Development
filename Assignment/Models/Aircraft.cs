using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class Aircraft
    {
        public int AircraftID { get; set; }

        public string MakeModel { get; set; }

        public int? NumEconomySeat { get; set; }

        public int? NumBusinessSeat { get; set; }

        public DateTime? DateLastMaintenance { get; set; }

        public string Status { get; set; }

    }
}
