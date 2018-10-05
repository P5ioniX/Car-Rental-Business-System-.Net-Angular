using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class RentalModel
    {
        public int?      Rental_ID      { get; set; }
        public int       User_ID        { get; set; }
        public int       Vehicle_ID     { get; set; }
        public DateTime  Start_Date     { get; set; }
        public DateTime  Return_Date    { get; set; }
        public DateTime? Returned_Date  { get; set; }

        public int?      Total_Price    { get; set; }
    }

}