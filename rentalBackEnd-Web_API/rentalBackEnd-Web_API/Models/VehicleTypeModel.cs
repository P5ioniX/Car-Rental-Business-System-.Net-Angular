using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class VehicleTypes
    {
        public string Make         { get; set; }
        public string Model        { get; set; }
        public int    Daily_Rate   { get; set; }
        public int    Penalty_Rate { get; set; }
        public int    Year         { get; set; }
        public string Gear_Type    { get; set; }

        /*
          [Vehicle_Type_ID] INT           IDENTITY (1, 1) NOT NULL, 

          [Make]            NVARCHAR (20) NOT NULL,
          [Model]           NVARCHAR (30) NOT NULL,
          [Daily_Rate]      INT           NOT NULL,
          [Penalty_Rate]    INT           NOT NULL,
          [Year]            INT           NOT NULL,
          [Gear_Type]       NVARCHAR (10) NOT NULL,
       */
    }
}