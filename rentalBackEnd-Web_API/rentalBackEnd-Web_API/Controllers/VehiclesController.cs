using rentalBackEnd_Web_API.Models;
using System.Collections.Generic;
using System.Web.Http;
using rentalBackEnd_Web_API.ViewModels;
using rentalBackEnd_Web_API.EntityModel;
using System.Threading.Tasks;
using rentalBackEnd_Web_API.StoreClasses;
using System.Net;
using System;
using Newtonsoft.Json;

namespace rentalBackEnd_Web_API.Controllers


{
    [RoutePrefix("api/Vehicles")]
    public class VehiclesController : ApiController
    {
        Rentals_DB_BusinessEntities dBase = new Rentals_DB_BusinessEntities();
        VehicleStore store = new VehicleStore();

        #region Route("GetAll")
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            IEnumerable<PublicVehicleViewModel> responce = await store.GetAllVehicles();

            return Ok(responce);
        }



        #endregion

        #region Route("GetByID/{id}")

        [AllowAnonymous]
        [Route("GetByID/{id}")]
        public async Task<IHttpActionResult> GetByID(int id)
        {
            var responce = await store.GetVehicleByID(id);
            return Ok(responce);
        }


        [AllowAnonymous]
        [Route("GetWithStateByID/{id}")]
        public async Task<IHttpActionResult> GetWithStateByID(int id)
        {
            var responce = await store.GetVehicleWithStateByID(id);
            return Ok(responce);
        }


        #endregion

        #region Route("UpdateVehicleById/{id}")

        [HttpPatch]
        [Route("UpdateVehicleById")]
        public async Task<IHttpActionResult> UpdateVehicleById([FromBody] UpdateVehicleViewModel model)
        {
            Tuple<Status, string, PublicVehicleViewModel> responce = await store.UpdateVehicle(model);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK,responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, responce.Item2);
            }
        }

        [HttpPatch]
        [Route("UpdateVehicleStatus")]
        public async Task<IHttpActionResult> UpdateVehicleStatus([FromBody] VehicleStateModel state)
        {
            Tuple<Status, string> responce = await store.UpdateVehicleState(state);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, responce.Item2);
            }
        }



        #endregion

        #region Route("GetByQuery/{_query?}")

        [HttpPost]
        [AllowAnonymous]
        [Route("GetByQuery/{_query?}")]
        public async Task<IHttpActionResult> GetByQuery(SearchQueryModel viewModel, string _query = "")
        {
                if ((_query == null && store.EmptyViewModelCheck(viewModel._queryVehicle)) && (viewModel._dates == null))
                {
                    return Ok(store.GetAllVehicles());
                }
                else
                {
                    IEnumerable<PublicVehicleViewModel> model = await store.GetByQuery(viewModel, _query);
                    return Ok(model);
                }
        }

        #endregion

        #region Route("GetAllAvailableRentable")
        [Route("GetAllAvailableRentable")]
        public IEnumerable<string> GetAllAvailableRentable()
        {
            return store.GetAll_Available_Rentable_Vehicles();
        }

        #endregion

        #region [Route("GetSearchFieldInit")]
        [AllowAnonymous]
        [Route("GetSearchFieldInit")]
        public async Task<IHttpActionResult> GetSearchFieldInit()
        {
            SortedList<string, List<string>> returnList = await store.GetAllRentableVehicle_Makes_Models();
            return Ok(returnList);
        }
        #endregion

        #region [Route("GetMakes")]
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMakes")]
        public async Task<IHttpActionResult> GetMakes()
        {
            List<string> returnList = await store.GetAllVehicle_Makes();
            if (returnList.Count == 0)
            {
                return null;
            }
            else
            {
                returnList.Sort();
                return Ok(returnList);
            }
        }
        #endregion

        #region [Route("GetModels")]
        [HttpGet]
        [AllowAnonymous]
        [Route("GetModels")]
        public async Task<IHttpActionResult> GetModels(string make)
        {
            List<string> returnList = await store.GetModels(make);
            returnList.Sort();
            return Ok(returnList);
        }
        #endregion

        #region [Route("GetVehicleTypesForMake")]
        [HttpGet]
        [AllowAnonymous]
        [Route("GetVehicleTypesForMake")]
        public async Task<IHttpActionResult> GetVehicleTypesForMake(string make)
        {
            Tuple<Status, List<VehicleTypes>, string> returnList = await store.GetTypesForMake(make);

            if (returnList.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, returnList.Item2);
            }
            else
            {
                return Content(HttpStatusCode.OK, new string[] { "Error", returnList.Item3 });
            }

        }
        #endregion

        #region [Route("AddVehicleType")]
        [HttpPost]
        [Route("AddVehicleType")]
        public async Task<IHttpActionResult> AddVehicleType(VehicleTypes model)
        {

            if (model.Daily_Rate > 0 && model.Penalty_Rate > 0 && model.Year > 2000 && model.Gear_Type != "")
            {
                Tuple<Status, string> responce = await store.AddVehicleType(model);

                if (responce.Item1 == Status.Success)
                {
                    return Content(HttpStatusCode.OK, responce.Item2);
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, responce.Item2);
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Please Fill All Of The Details");
            }


        }
        #endregion

        #region [Route("AddVehicle")]
        [HttpPost]
        [Route("AddVehicle")]
        public async Task<IHttpActionResult> AddVehicle(PublicVehicleViewModel model)
        {

            if (model.Mileage != 0 && model.Vehicle_ID != 0 )
            {
                Tuple<Status, string> responce = await store.AddVehicle(model);

                if (responce.Item1 == Status.Success)
                {
                    return Content(HttpStatusCode.OK, responce.Item2);
                }
                else
                {
                    return Content(HttpStatusCode.NotAcceptable, responce.Item2);
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Please Fill Valid Details");
            }


        }
        #endregion

        #region [Route("AddMake")]
        [HttpGet]
        [Route("AddMake")]
        public async Task<IHttpActionResult> AddMake(string make)
        {
            Tuple<Status, string> responce = await store.AddMake(make);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, responce.Item2);
            }
        }
        #endregion

        #region [Route("TryDeleteMake")]
        [HttpDelete]
        [Route("TryDeleteMake")]
        public async Task<IHttpActionResult> TryDeleteMake(string make)
        {
            Tuple<Status, string> responce = await store.TryDeleteMake(make);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, responce.Item2);
            }
        }
        #endregion

        #region [Route("TryDeleteModel")]
        [HttpDelete]
        [Route("TryDeleteModel")]
        public async Task<IHttpActionResult> TryDeleteModel(string make, string model)
        {
            Tuple<Status, string> responce = await store.TryDeleteModel(make, model);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, responce.Item2);
            }
        }
        #endregion

        #region [Route("TryDeleteType")]
        [HttpDelete]
        [Route("TryDeleteType")]
        public async Task<IHttpActionResult> TryDeleteType(string type)
        {

            PublicVehicleViewModel model = JsonConvert.DeserializeObject<PublicVehicleViewModel>(type);

            Tuple<Status, string> responce = await store.TryDeleteType(model);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, responce.Item2);
            }

        }
        #endregion

        #region [Route("AddModel")]
        [HttpGet]
        [Route("AddModel")]
        public async Task<IHttpActionResult> AddModel(string make, string model)
        {
            Tuple<Status, string> responce = await store.AddModel(make, model);

            if (responce.Item1 == Status.Success)
            {
                return Content(HttpStatusCode.OK, responce.Item2);
            }
            else
            {
                return Content(HttpStatusCode.NotAcceptable, responce.Item2);
            }
        }
        #endregion

        #region ModelConverters

        private VehicleTypes ConvertDBModelToModel(Vehicle car)
        {
            return new VehicleTypes
            {
                Make = car.Vehicle_Types.Vehicle_Makes.Make_Name,
                Model = car.Vehicle_Types.Vehicle_Models.Model_Name,
                Year = car.Vehicle_Types.Year,
                Gear_Type = car.Vehicle_Types.Gear_Type,
                Daily_Rate = car.Vehicle_Types.Daily_Rate,
                Penalty_Rate = car.Vehicle_Types.Penalty_Rate
            };
        }

        #endregion


    }
}
