using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using rentalBackEnd_Web_API.EntityModel;
using rentalBackEnd_Web_API.Models;
using rentalBackEnd_Web_API.StoreClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace rentalBackEnd_Web_API.Controllers
{
    [RoutePrefix("api/Rentals")]
    public class RentalsController : ApiController
    {
        RentalStore store = new RentalStore();
        VehicleStore carStore = new VehicleStore();

        #region [Route("GetAllRentals")]
        [HttpGet]
        [Route("GetAllRentals")]
        public async Task<IHttpActionResult> GetAllRentals()
        {
            List<RentalModel> rentals = new List<RentalModel>();
            string userContext = RequestContext.Principal.Identity.GetUserId();

            int ID = int.Parse(userContext);

            rentals = await store.getAll(ID);



            return Content(HttpStatusCode.OK, rentals);

        }
        #endregion

        #region [Route("GetRental")]
        [HttpGet]
        [Route("GetRental")]
        public async Task<IHttpActionResult> GetRentalByID(int vehicleID)
        {


            string userContext = RequestContext.Principal.Identity.GetUserId();

            try
            {
                Vehicle_Types vehicle = await carStore.GetVehicleType(vehicleID);

                var rental = await store.getRentalByVehicleNumber(vehicleID);

                if (rental != null && rental.Returned_Date == null)
                {
                    rental.Total_Price = store.CalculateCurrentTotalSum(vehicle, rental);
                    return Ok(rental);
                }
                else
                {
                    return Content(HttpStatusCode.OK, "No Rental Found");
                }
            }
            catch (Exception)
            {

                return Content(HttpStatusCode.NotFound, "No Rental Found");
            }

        }

        #endregion

        #region [Route("GetOpenRental")]
        [HttpGet]
        [Route("GetOpenRental")]
        public async Task<IHttpActionResult> GetOpenRentalByID(int vehicleID)
        {


            string userContext = RequestContext.Principal.Identity.GetUserId();

            try
            {
                Vehicle_Types vehicle = await carStore.GetVehicleType(vehicleID);

                var rental = await store.getOpenByVehicleNumber(vehicleID);

                if (rental != null && rental.Returned_Date == null)
                {
                    rental.Total_Price = store.CalculateCurrentTotalSum(vehicle, rental);
                    return Ok(rental);
                }
                else
                {
                    return Content(HttpStatusCode.OK, "No Rental Found");
                }
            }
            catch (Exception)
            {

                return Content(HttpStatusCode.NotFound, "No Rental Found");
            }

        }

        #endregion

        #region [Route("Rent")]
        [HttpPost]
        [Route("Rent")]
        public async Task<IHttpActionResult> Rent(RentalModel _rental)
        {

            string userContext = RequestContext.Principal.Identity.GetUserId();
            int ID = int.Parse(userContext);

            if (userContext != null)
            {
                Rental returnRental = new Rental();

                Tuple<string, Vehicle> vehicleAndDate = store.getVehicle(_rental);

                if (vehicleAndDate.Item2.Available)
                {
                    returnRental = await store.rentVehicle(ID, _rental, vehicleAndDate.Item2);
                    return Ok(returnRental);
                }
                else
                {
                    if (vehicleAndDate == null)
                    {
                        return Content(HttpStatusCode.Forbidden, "Vehicle is not available");
                    }
                    else
                    {
                        return Content(HttpStatusCode.Forbidden, vehicleAndDate.Item1);
                    }

                }

            }
            else
            {
                return BadRequest("Unknown User");
            }


        }

        #endregion

        #region [Route("ReturnRental")]
        [HttpPut]
        [Route("ReturnRental")]
        public async Task<IHttpActionResult> Return(int id, int mileage)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var Role = identity.FindFirst("Role").Value;

            if (Role == "Employee" || Role == "Manager")
            {
                Tuple<Status,string> returning = await store.ReturnRental(id, mileage);

                if (returning.Item1 == Status.Success)
                {
                    return Content(HttpStatusCode.OK, returning.Item2);
                }
                else
                {
                    return Content(HttpStatusCode.Forbidden, returning.Item2);
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "You Are Not Authorized For That");
            }

        }

        #endregion

        #region [Route("CancelRental")]
        [HttpPut]
        [Route("CancelRental")]
        public async Task<IHttpActionResult> CancelRental(int vehicleID)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var Role = identity.FindFirst("Role").Value;

            if (Role == "Employee" || Role == "Manager")
            {
                Tuple<Status, string> returning = await store.CancelRental(vehicleID);

                if (returning.Item1 == Status.Success)
                {
                    return Content(HttpStatusCode.OK, returning.Item2);
                }
                else
                {
                    return Content(HttpStatusCode.Forbidden, returning.Item2);
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "You Are Not Authorized For That");
            }

        }

        #endregion

        #region [Route("GetUserRentals")]
        [HttpGet]
        [Route("GetUserRentals")]
        public async Task<IHttpActionResult> GetUserRentals(int userId)
        {

            string userContext = RequestContext.Principal.Identity.GetUserId();
            Tuple<Status,List<RentalModel>,string> vehicle = await store.GetUsersRentals(userId);

            if (vehicle.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, vehicle.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "No Rentals Found");
            }

        }

        #endregion

        #region [Route("DeleteRental")]
        [HttpDelete]
        [Route("DeleteRental")]
        public async Task<IHttpActionResult> DeleteRental(int rentalId)
        {
            Tuple<Status,string> rentalResponce = await store.DeleteRental(rentalId);

            if (rentalResponce.Item1== Status.Success)
            {
                return Content(HttpStatusCode.OK, rentalResponce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, rentalResponce.Item2);
            }
        }

        #endregion
    }
}
