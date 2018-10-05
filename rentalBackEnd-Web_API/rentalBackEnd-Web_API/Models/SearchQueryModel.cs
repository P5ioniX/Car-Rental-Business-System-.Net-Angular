using rentalBackEnd_Web_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class SearchQueryModel
    {
        public PublicVehicleViewModel _queryVehicle { get; set; }
        public RentalModel _dates { get; set; }
    }
}