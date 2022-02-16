using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Models
{
    public class PilotAttendant
    {
        public List<Staff> Staff1 { get; set; }
        public List<Staff> Staff2 { get; set; }

        public PilotAttendant()
        {
            Staff1 = new List<Staff>();
            Staff2 = new List<Staff>();
        }
    }
}
