using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.ViewModels
{
    public class PublicVehicleViewModel
    {
        public string Make         { get; set; }
        public string Model        { get; set; }
        public int    Daily_Rate   { get; set; }
        public int    Penalty_Rate { get; set; }
        public int    Year         { get; set; }
        public string Gear_Type    { get; set; }
        public int    Vehicle_ID   { get; set; }
        public int    Mileage      { get; set; }
        public bool?  Available    { get; set; }
        public byte[] Picture      { get; set; }
    }
}