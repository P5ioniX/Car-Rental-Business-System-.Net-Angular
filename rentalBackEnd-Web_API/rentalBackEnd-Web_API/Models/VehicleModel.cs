using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class VehicleModel 
    {
        public int              Vehicle_ID      { get; set; }
        public int              Branch_ID       { get; set; }
        public int              Vehicle_Type_ID { get; set; }
        public int              Mileage         { get; set; }
        public byte[]           Picture         { get; set; }
        public bool             Rentable        { get; set; }
        public bool             Available       { get; set; }

        public VehicleTypes     Type            { get; set; }

    }
}