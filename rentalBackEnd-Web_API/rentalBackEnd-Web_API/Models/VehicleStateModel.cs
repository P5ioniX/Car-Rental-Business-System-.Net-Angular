using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class VehicleStateModel
    {
        public bool Rentable { get; set; }
        public bool Available { get; set; }
        public int Vehicle_ID { get; set; }
    }
}