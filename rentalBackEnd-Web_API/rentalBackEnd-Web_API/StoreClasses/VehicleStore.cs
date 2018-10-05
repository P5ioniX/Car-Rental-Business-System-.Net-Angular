using Newtonsoft.Json;
using rentalBackEnd_Web_API.EntityModel;
using rentalBackEnd_Web_API.Models;
using rentalBackEnd_Web_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace rentalBackEnd_Web_API.StoreClasses
{
    public enum Status { Success, Failure };
    public class VehicleStore
    {
        Rentals_DB_BusinessEntities dBase = new Rentals_DB_BusinessEntities();

        public async Task<IEnumerable<PublicVehicleViewModel>> GetAllVehicles()
        {

            List<PublicVehicleViewModel> carList = new List<PublicVehicleViewModel>();

            await Task.Run(() =>
            {
                foreach (var car in dBase.Vehicles)
                {
                    carList.Add(ConvertDBModelToViewModel(car));
                }
            });
            return carList;
        }

        internal async Task<PublicVehicleViewModel> GetVehicleByID(int id)
        {
            PublicVehicleViewModel car = new PublicVehicleViewModel();

            await Task.Run(() =>
            {
                var vehicle = ConvertDBModelToViewModel(dBase.Vehicles.FirstOrDefault(x => x.Vehicle_ID == id));
                car = vehicle;
            });

            if (car != null)
            {
                return car;
            }
            else
            {
                return null;
            }
        }

        internal async Task<Tuple<PublicVehicleViewModel, VehicleStateModel>> GetVehicleWithStateByID(int id)
        {
            PublicVehicleViewModel car = new PublicVehicleViewModel();
            Vehicle vhcle = new Vehicle();
            await Task.Run(() =>
            {
                vhcle = dBase.Vehicles.FirstOrDefault(x => x.Vehicle_ID == id);
                car = ConvertDBModelToViewModel(vhcle);
            });

            if (car != null)
            {
                return new Tuple<PublicVehicleViewModel, VehicleStateModel>(car, new VehicleStateModel() { Available = vhcle.Available, Rentable = vhcle.Rentable });
            }
            else
            {
                return null;
            }
        }

        internal async Task<Vehicle_Types> GetVehicleType(int vehicleID)
        {
            Vehicle_Types car = new Vehicle_Types();

            await Task.Run(() =>
            {
                var vehicleFound = dBase.Vehicles.FirstOrDefault(vehicle => vehicle.Vehicle_ID == vehicleID);
                if (vehicleFound != null)
                {
                    car = dBase.Vehicle_Types.FirstOrDefault(vehicle => vehicle.Vehicle_Makes.Make_Name == vehicleFound.Vehicle_Types.Vehicle_Makes.Make_Name && vehicle.Vehicle_Models.Model_Name == vehicleFound.Vehicle_Types.Vehicle_Models.Model_Name);
                }
            });

            return car;
        }




        //Finish Vehicle Update
        internal async Task<Tuple<Status, string, PublicVehicleViewModel>> UpdateVehicle(UpdateVehicleViewModel model)
        {
            Vehicle car = await dBase.Vehicles.FindAsync(model.Vehicle_ID);

            car.Mileage = model.Mileage;

            try
            {
                dBase.Vehicles.AddOrUpdate(car);
                dBase.SaveChanges();
                return new Tuple<Status, string, PublicVehicleViewModel>(Status.Success, $"Vehicle Number {model.Vehicle_ID} Updated", ConvertDBModelToViewModel(car));
            }
            catch (Exception err)
            {
                var error = err;
                return new Tuple<Status, string, PublicVehicleViewModel>(Status.Failure, $"Could Not Update Vehicle Number {model.Vehicle_ID}", ConvertDBModelToViewModel(car));
            }

        }

        internal async Task<Tuple<Status, string>> UpdateVehicleState(VehicleStateModel state)
        {
            Vehicle car = await dBase.Vehicles.FindAsync(state.Vehicle_ID);


            if (state != null)
            {
                car.Available = state.Available;
                car.Rentable = state.Rentable;
            }


            try
            {
                dBase.Vehicles.AddOrUpdate(car);
                dBase.SaveChanges();
                return new Tuple<Status, string>(Status.Success, $"Vehicle Number {state.Vehicle_ID} Updated");
            }
            catch (Exception err)
            {
                var error = err;
                return new Tuple<Status, string>(Status.Failure, $"Could Not Update Vehicle Number {state.Vehicle_ID}");
            }
        }

        public async Task<IEnumerable<PublicVehicleViewModel>> GetByQuery(SearchQueryModel _queryModel, string _query = "")
        {
            List<PublicVehicleViewModel> carList = new List<PublicVehicleViewModel>();
            List<PublicVehicleViewModel> carDateList = new List<PublicVehicleViewModel>();

            await Task.Run(() =>
                {
                    foreach (Vehicle car in dBase.Vehicles)
                    {
                        if (PublicSearchParameters(_query, car, _queryModel._queryVehicle))
                        {
                            carList.Add(ConvertDBModelToViewModel(car));
                        }
                    }
                });

            if (_queryModel._dates != null)
            {
                await Task.Run(() =>
                {
                    foreach (PublicVehicleViewModel item in carList)
                    {
                        if (checkRentals(item, _queryModel._dates))
                        {
                            carDateList.Add(item);
                        }
                    }
                });
                return carDateList;
            }

            return carList;
        }

        private bool checkRentals(PublicVehicleViewModel vehicleModel, RentalModel _dates)
        {
            IQueryable<Rental> lst = dBase.Rentals.Where(id => id.Vehicle_ID == vehicleModel.Vehicle_ID);

            if (lst != null)
            {
                foreach (Rental item in lst)
                {
                    if (_dates.Start_Date < item.Start_Date && _dates.Return_Date < item.Start_Date && item.Returned_Date != null)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        internal async Task<List<string>> GetModels(string make)
        {
            List<string> models = new List<string>();

            await Task.Run(() =>
            {
                int makeId = dBase.Vehicle_Makes.FirstOrDefault(carMake => carMake.Make_Name == make).Make_ID;

                foreach (var item in dBase.Vehicle_Models)
                {
                    if (item.Make_ID == makeId)
                    {
                        models.Add(item.Model_Name);
                    }
                }

            });

            return models;
        }

        internal async Task<Tuple<Status, List<VehicleTypes>, string>> GetTypesForMake(string make)
        {
            List<VehicleTypes> returnList = new List<VehicleTypes>();
            int id = dBase.Vehicle_Makes.FirstOrDefault(brand => brand.Make_Name == make).Make_ID;
            await Task.Run(() =>
            {



                foreach (Vehicle_Types type in dBase.Vehicle_Types)
                {
                    if (type.Make_ID == id)
                    {
                        returnList.Add(new VehicleTypes()
                        {
                            Make = type.Vehicle_Makes.Make_Name,
                            Model = type.Vehicle_Models.Model_Name,
                            Year = type.Year,
                            Gear_Type = type.Gear_Type,
                            Daily_Rate = type.Daily_Rate,
                            Penalty_Rate = type.Penalty_Rate
                        });
                    }
                }
            });


            if (returnList.Count > 0)
            {
                return new Tuple<Status, List<VehicleTypes>, string>(Status.Success, returnList, $"There Are {returnList.Count} Items In The List");
            }
            else
            {
                return new Tuple<Status, List<VehicleTypes>, string>(Status.Failure, returnList, "There Are No Types For That Make");
            }
        }

        internal async Task<Tuple<Status, string>> AddMake(string make)
        {
            Vehicle_Makes result = dBase.Vehicle_Makes.FirstOrDefault(name => name.Make_Name.ToLower() == make.ToLower());

            if (result == null)
            {
                Vehicle_Makes added = dBase.Vehicle_Makes.Add(new Vehicle_Makes() { Make_Name = make });
                if (added != null)
                {
                    dBase.Vehicle_Makes.OrderBy(x => x.Make_Name);
                    await dBase.SaveChangesAsync();
                    return new Tuple<Status, string>(Status.Success, $" {make} Successfully Added");
                }
                else
                {
                    return new Tuple<Status, string>(Status.Failure, "Something Went Wrong");
                }
            }
            else
            {
                return new Tuple<Status, string>(Status.Failure, $" {make} Already Exists");
            }
        }

        internal async Task<Tuple<Status, string>> AddVehicle(PublicVehicleViewModel model)
        {
            int makeId = dBase.Vehicle_Makes.FirstOrDefault(make => make.Make_Name == model.Make).Make_ID;

            Vehicle added = dBase.Vehicles.Add(new Vehicle()
            {
                Vehicle_ID = model.Vehicle_ID,
                Available = true,
                Rentable = true,
                Mileage = model.Mileage,
                Vehicle_Type_ID = dBase.Vehicle_Types.FirstOrDefault(carModel => carModel.Make_ID == makeId).Vehicle_Type_ID,
                //Create FrontEnd User Selection & Change Default Branch insertion
                Branch_ID = 1
            });

            if (added != null)
            {

                await dBase.SaveChangesAsync();

                return new Tuple<Status, string>(Status.Success, $"{model.Make} Successfully Added !");
            }
            else
            {
                return new Tuple<Status, string>(Status.Failure, "Something Went Wrong !!!");
            }

        }

        internal async Task<Tuple<Status, string>> AddModel(string make, string model)
        {
            int makeID = dBase.Vehicle_Makes.FirstOrDefault(brand => brand.Make_Name == make).Make_ID;

            var carModels = from carModel in dBase.Vehicle_Models
                            where carModel.Make_ID == makeID
                            select carModel;

            if (carModels.Any())
            {
                foreach (Vehicle_Models vehicleModel in carModels)
                {
                    if (vehicleModel.Model_Name.ToLower() == model.ToLower())
                    {
                        return new Tuple<Status, string>(Status.Failure, $"The {model} Model Already Exists");
                    }
                }

                return await TryAddModelToDB(model, makeID, make);

            }
            else
            {
                return await TryAddModelToDB(model, makeID, make);
            }
        }



        private async Task<Tuple<Status, string>> TryAddModelToDB(string model, int makeID, string make)
        {
            Vehicle_Models added = dBase.Vehicle_Models.Add(new Vehicle_Models() { Model_Name = model, Make_ID = makeID });
            if (added != null)
            {

                await dBase.SaveChangesAsync();
                return new Tuple<Status, string>(Status.Success, $" {model} Successfully Added To The {make}'s");
            }
            else
            {
                return new Tuple<Status, string>(Status.Failure, "Something Went Wrong");
            }
        }


        internal async Task<Tuple<Status, string>> TryDeleteType(PublicVehicleViewModel model)
        {

            Vehicle_Types chosenType = dBase.Vehicle_Types.FirstOrDefault(type =>
            (
            type.Vehicle_Models.Model_Name == model.Model
            && type.Vehicle_Makes.Make_Name == model.Make
            && type.Year == model.Year
            && type.Gear_Type == model.Gear_Type
            ));

            int typeId = chosenType.Vehicle_Type_ID;

            bool vehicles = false,
                 rentals = false;

            if (dBase.Vehicles.FirstOrDefault(carModel => carModel.Vehicle_Types.Vehicle_Type_ID == typeId) != null)
            {
                vehicles = true;

            }
            if (dBase.Rentals.FirstOrDefault(carModel => carModel.Vehicle.Vehicle_Type_ID == typeId) != null)
            {
                rentals = true;
            }

            if (vehicles || rentals)
            {
                string error = concatErrorString(vehicles, rentals);
                return new Tuple<Status, string>(Status.Failure, error);
            }
            else
            {
                dBase.Vehicle_Types.Remove(chosenType);
                await dBase.SaveChangesAsync();
                return new Tuple<Status, string>(Status.Success, model + " Deleted");
            }
        }



        internal async Task<Tuple<Status, string>> TryDeleteModel(string make, string model)
        {
            Vehicle_Models chosenModel = dBase.Vehicle_Models.FirstOrDefault(id => id.Model_Name == model);
            int modelId = chosenModel.Model_ID;

            bool types = false,
                 vehicles = false,
                 rentals = false;

            if (dBase.Vehicle_Types.FirstOrDefault(carModel => carModel.Make_ID == modelId) != null)
            {
                types = true;

            }
            if (dBase.Vehicles.FirstOrDefault(carModel => carModel.Vehicle_Types.Make_ID == modelId) != null)
            {
                vehicles = true;

            }
            if (dBase.Rentals.FirstOrDefault(carModel => carModel.Vehicle.Vehicle_Types.Make_ID == modelId) != null)
            {
                rentals = true;
            }

            if (types || vehicles || rentals)
            {
                string error = concatErrorString(types, vehicles, rentals);
                return new Tuple<Status, string>(Status.Failure, error);
            }
            else
            {
                dBase.Vehicle_Models.Remove(chosenModel);
                await dBase.SaveChangesAsync();
                return new Tuple<Status, string>(Status.Success, model + " Deleted");
            }
        }



        internal async Task<Tuple<Status, string>> TryDeleteMake(string make)
        {
            Vehicle_Makes chosenMake = dBase.Vehicle_Makes.FirstOrDefault(id => id.Make_Name == make);
            int makeId = chosenMake.Make_ID;

            bool models = false,
                 types = false,
                 vehicles = false,
                 rentals = false;



            if (dBase.Vehicle_Models.FirstOrDefault(model => model.Make_ID == makeId) != null)
            {
                models = true;

            }
            if (dBase.Vehicle_Types.FirstOrDefault(model => model.Make_ID == makeId) != null)
            {
                types = true;

            }
            if (dBase.Vehicles.FirstOrDefault(model => model.Vehicle_Types.Make_ID == makeId) != null)
            {
                vehicles = true;

            }
            if (dBase.Rentals.FirstOrDefault(model => model.Vehicle.Vehicle_Types.Make_ID == makeId) != null)
            {
                rentals = true;
            }

            if (models || types || vehicles || rentals)
            {
                string error = concatErrorString(models, types, vehicles, rentals);
                return new Tuple<Status, string>(Status.Failure, error);
            }
            else
            {
                dBase.Vehicle_Makes.Remove(chosenMake);
                await dBase.SaveChangesAsync();
                return new Tuple<Status, string>(Status.Success, make + " Deleted");
            }

        }



        internal async Task<Tuple<Status, string>> AddVehicleType(VehicleTypes model)
        {

            Vehicle_Types existingType = dBase.Vehicle_Types.FirstOrDefault(car =>
            (
            car.Vehicle_Makes.Make_Name.ToLower() == model.Make.ToLower()
            && car.Vehicle_Models.Model_Name.ToLower() == model.Model.ToLower()
            && car.Gear_Type == model.Gear_Type
            && car.Year == model.Year
            ));

            if (existingType == null)
            {
                int makeId = dBase.Vehicle_Makes.FirstOrDefault(make => make.Make_Name == model.Make).Make_ID;
                int modelId = dBase.Vehicle_Models.FirstOrDefault(carModel => carModel.Model_Name == model.Model).Model_ID;

                Vehicle_Types addedType = dBase.Vehicle_Types.Add(new Vehicle_Types()
                {
                    Daily_Rate = model.Daily_Rate,
                    Penalty_Rate = model.Penalty_Rate,
                    Gear_Type = model.Gear_Type,
                    Year = model.Year,
                    Make_ID = makeId,
                    Model_ID = modelId
                });

                await dBase.SaveChangesAsync();

                if (addedType != null)
                {
                    return new Tuple<Status, string>(Status.Success, addedType.Vehicle_Makes.Make_Name + " " + addedType.Vehicle_Models.Model_Name + " Successfully Added To The Models Inventory");
                }
                else
                {
                    return new Tuple<Status, string>(Status.Failure, "Something Went Wrong While Trying To Add The Type");
                }
            }
            else
            {
                return new Tuple<Status, string>(Status.Failure, "Type Already Exists");
            }

        }

        internal async Task<List<string>> GetAllVehicle_Makes()
        {
            List<string> makes = new List<string>();

            await Task.Run(() =>
            {

                foreach (var item in dBase.Vehicle_Makes)
                {
                    makes.Add(item.Make_Name);
                }

            });

            return makes;
        }

        public List<string> GetAll_Available_Rentable_Vehicles()
        {

            List<string> carList = new List<string>();
            foreach (var car in dBase.Vehicles)
            {
                if (AvailableAndRentable(car))
                {
                    carList.Add(JsonConvert.SerializeObject(ConvertDBModelToViewModel(car), Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
                }
            }

            return carList;
        }

        public async Task<SortedList<string, List<string>>> GetAllRentableVehicle_Makes_Models()
        {

            SortedList<string, List<string>> infoList = await makeInfoListFromRentableCars(dBase);

            return infoList;
        }

        private async Task<SortedList<string, List<string>>> makeInfoListFromRentableCars(Rentals_DB_BusinessEntities dBase)
        {

            SortedList<string, List<string>> populatedList = new SortedList<string, List<string>>();

            await Task.Run(() =>
            {
                foreach (var car in dBase.Vehicles)
                {

                    if (Rentable(car))
                    {
                        string make = car.Vehicle_Types.Vehicle_Makes.Make_Name;
                        string model = car.Vehicle_Types.Vehicle_Models.Model_Name;
                        List<string> models;

                        if (!populatedList.ContainsKey(make))
                        {
                            models = new List<string>();
                            models.Add(model);
                            populatedList.Add(make, models);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, List<string>> item in populatedList)
                            {

                                if (item.Key.Equals(make) && !item.Value.Contains(model))
                                {
                                    item.Value.Add(model);
                                }
                            }
                        }


                    }
                }
            });



            return populatedList;
        }




        #region Helper Methods

        #region Concatenate Error String Method Overloads


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicles"></param>
        /// <param name="rentals"></param>
        /// <returns>A string with an error stating what has to be deleted prior to deleting the entry the user chose</returns>
        private string concatErrorString(bool vehicles, bool rentals)
        {
            string errString = "Please Delete All Related";

            if (vehicles && rentals)
            {
                errString += " Vehicles  & Rentals First";
            }
            else if (vehicles)
            {
                errString += " Vehicles First";
            }

            return errString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="vehicles"></param>
        /// <param name="rentals"></param>
        /// <returns>A string with an error stating what has to be deleted prior to deleting the entry the user chose</returns>
        private string concatErrorString(bool types, bool vehicles, bool rentals)
        {
            string errString = "Please Delete All Related";

            if (types && vehicles && rentals)
            {
                errString += " Types , Vehicles  & Rentals First";
            }
            else if (types && vehicles)
            {
                errString += " Types & Vehicles First";
            }
            else if (types)
            {
                errString += " Types First";
            }

            return errString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="types"></param>
        /// <param name="vehicles"></param>
        /// <param name="rentals"></param>
        /// <returns>A string with an error stating what has to be deleted prior to deleting the entry the user chose</returns>
        private string concatErrorString(bool models, bool types, bool vehicles, bool rentals)
        {
            string errString = "Please Delete All Related";

            if (models && types && vehicles && rentals)
            {
                errString += " Models , Types , Vehicles  & Rentals First";
            }
            else if (models && types && vehicles)
            {
                errString += " Models , Types & Vehicles First";
            }
            else if (models && types)
            {
                errString += " Models & Types First";
            }
            else if (models)
            {
                errString += " Models First";
            }

            return errString;
        }

        #endregion

        private PublicVehicleViewModel ConvertDBModelToViewModel(Vehicle car)
        {
            if (car != null)
            {
                return new PublicVehicleViewModel
                {
                    Vehicle_ID = car.Vehicle_ID,
                    Make = car.Vehicle_Types.Vehicle_Makes.Make_Name,
                    Model = car.Vehicle_Types.Vehicle_Models.Model_Name,
                    Year = car.Vehicle_Types.Year,
                    Mileage = car.Mileage,
                    Gear_Type = car.Vehicle_Types.Gear_Type,
                    Daily_Rate = car.Vehicle_Types.Daily_Rate,
                    Penalty_Rate = car.Vehicle_Types.Penalty_Rate,
                    Available = car.Available
                };
            }
            else
            {
                return null;
            }

        }

        #region Vehicle Search Methods

        /// <summary>
        /// compare the search text to set entity
        /// </summary>
        /// <param name="srchText"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        private bool PublicSearchByText(string srchText, Vehicle car)
        {
            bool
            condition1 = AvailableAndRentable(car),
            condition2 = TextMatchesMakeOrModel(srchText, car.Vehicle_Types.Vehicle_Makes.Make_Name, car.Vehicle_Types.Vehicle_Models.Model_Name);


            if (condition1 && condition2)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Compare The Search Conditions And Text To Set Entity
        /// </summary>
        /// <param name="srchText"></param>
        /// <param name="car"></param>
        /// <param name="model"></param>
        /// <returns>boolean</returns>
        private bool PublicSearchParameters(string srchText, Vehicle car, PublicVehicleViewModel model)
        {
            if (srchText == null)
            {
                srchText = "";
            }
            //bool condition1 = AvailableAndRentable(car);
            //if (!condition1) { return false; }

            bool condition2 = GearMatches(car.Vehicle_Types.Gear_Type, model.Gear_Type);
            if (!condition2) { return false; }

            bool condition3 = YearMatches(car.Vehicle_Types.Year, model.Year);
            if (!condition3) { return false; }

            bool condition4 = MakeMatches(car.Vehicle_Types.Vehicle_Makes.Make_Name, model.Make);
            if (!condition4) { return false; }

            bool condition5 = ModelMatches(car.Vehicle_Types.Vehicle_Models.Model_Name, model.Model);
            if (!condition5) { return false; }

            bool condition6 = TextMatchesMakeOrModel(srchText, car.Vehicle_Types.Vehicle_Makes.Make_Name, car.Vehicle_Types.Vehicle_Models.Model_Name);
            if (!condition6) { return false; }

            if (/*condition1 &&*/ condition2 && condition3 && condition4 && condition5 && condition6)
            {
                return true;
            }

            return false;

        }
        #endregion

        #region Vehicle Search Conditions

        public bool EmptyViewModelCheck(PublicVehicleViewModel queryVehicle)
        {
            if ((queryVehicle.Gear_Type == null || queryVehicle.Gear_Type == "") && queryVehicle.Year == 0 && queryVehicle.Make == "" && queryVehicle.Model == "")
            {
                return true;
            }
            return false;
        }

        private bool Rentable(Vehicle car)
        {
            if (car.Rentable)
            {
                return true;
            }
            return false;
        }
        private bool AvailableAndRentable(Vehicle car)
        {
            if (car.Rentable && car.Available)
            {
                return true;
            }
            return false;
        }
        private bool GearMatches(string carGear, string modelGear)
        {
            if (carGear == modelGear || (modelGear == "" || modelGear == null))
            {
                return true;
            }
            return false;
        }
        private bool YearMatches(int carYear, int modelYear)
        {
            if (carYear == modelYear || (modelYear == 0))
            {
                return true;
            }
            return false;
        }
        private bool MakeMatches(string carMake, string modelMake)
        {
            if (carMake == modelMake || (modelMake == "" || modelMake == null))
            {
                return true;
            }
            return false;
        }
        private bool ModelMatches(string carModel, string modelModel)
        {
            if (carModel == modelModel || (modelModel == "" || modelModel == null))
            {
                return true;
            }
            return false;
        }
        private bool TextMatchesMakeOrModel(string srchText, string carMake, string carModel)
        {
            if (carMake.ToLower().Contains(srchText.ToLower()) || carModel.ToLower().Contains(srchText.ToLower()) || (srchText == null || srchText == ""))
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion


    }
}