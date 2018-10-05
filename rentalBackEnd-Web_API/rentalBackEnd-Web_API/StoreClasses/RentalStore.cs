using rentalBackEnd_Web_API.EntityModel;
using rentalBackEnd_Web_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace rentalBackEnd_Web_API.StoreClasses
{
    public class RentalStore
    {
        Rentals_DB_BusinessEntities DBase = new Rentals_DB_BusinessEntities();


        /// <summary>
        /// //Check If Vehicle Is Available & Set It To Unavailable Or Return A Message That Vehicle Is Unavailable (Async Method)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="model"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<Rental> rentVehicle(int userID, RentalModel model, Vehicle vehicle)
        {

            //Check If The Rental Exists In The DataBase
            var existingRental = DBase.Rentals.FirstOrDefault(rental => rental.Vehicle_ID == model.Vehicle_ID && rental.User_ID == userID && rental.Returned_Date == null);


            //If Rental Does Not Exist Create It & Set Vehicle Availability To False
            if (existingRental == null)
            {
                vehicle.Available = false;

                if (model.Start_Date <= DateTime.Now)
                {
                    vehicle.Rentable = false;
                }

                var result = DBase.Rentals.Add(new Rental { User_ID = userID, Start_Date = model.Start_Date, Return_Date = model.Return_Date, Vehicle_ID = model.Vehicle_ID });

                if (result != null)
                {
                    await DBase.SaveChangesAsync();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        internal async Task<RentalModel> getRentalByVehicleNumber(int vehicleID)
        {
            RentalModel model = new RentalModel();
            await Task.Run(() =>
            {
                Rental rental = new Rental();
                rental = DBase.Rentals.FirstOrDefault(rent => rent.Vehicle_ID == vehicleID);

                try
                {
                    if (rental != null)
                    {
                        model = ConverRentalToModel(rental);
                    }
                    else
                    {
                        model = null;
                    }
                }
                catch (Exception a)
                {

                    throw a;
                }


            });
            if (model != null)
            {
                return model;
            }
            else
            {
                return null;
            }

        }

        internal async Task<RentalModel> getOpenByVehicleNumber(int vehicleID)
        {
            RentalModel model = new RentalModel();
            await Task.Run(() =>
            {
                Rental rental = new Rental();
                rental = DBase.Rentals.FirstOrDefault(rent => rent.Vehicle_ID == vehicleID && rent.Returned_Date == null);

                try
                {
                    if (rental != null)
                    {
                        model = ConverRentalToModel(rental);
                    }
                    else
                    {
                        model = null;
                    }
                }
                catch (Exception a)
                {

                    throw a;
                }


            });
            if (model != null)
            {
                return model;
            }
            else
            {
                return null;
            }

        }

        public Tuple<string, Vehicle> getVehicle(RentalModel model)
        {

            //Get Vehicle
            var carInDB = DBase.Vehicles.FirstOrDefault(car => car.Vehicle_ID == model.Vehicle_ID);

            if (carInDB != null && carInDB.Available)
            {
                return new Tuple<string, Vehicle>("Vehicle Available", carInDB);
            }
            else
            {
                Rental rentalFromDB = DBase.Rentals.FirstOrDefault(x => x.Vehicle_ID == carInDB.Vehicle_ID);

                if (rentalFromDB != null)
                {
                    return new Tuple<string, Vehicle>($"Vehicle Is Taken Until {rentalFromDB.Return_Date.Date}", carInDB);
                }
                else //When A Vehicle Is Not Related To A Rental & Is Not Available .
                {
                    return new Tuple<string, Vehicle>("Vehicle Is Not Available - In Company Use", carInDB);
                }
            }

        }

        internal async Task<Tuple<Status, string>> CancelRental(int id)
        {
            Rental rentalEntity = new Rental();
            string message = "Something Wen't Wrong";
            Status returnStatus = Status.Failure;


            rentalEntity = DBase.Rentals.FirstOrDefault(rent => rent.Vehicle_ID == id);

            if (rentalEntity != null)
            {
                if (rentalEntity.Start_Date > DateTime.Now)
                {
                    rentalEntity.Returned_Date = DateTime.Now;
                    await DBase.SaveChangesAsync();
                    message = "Booking Canceled";
                    returnStatus = Status.Success;
                }
            }
            else
            {
                message = "No Rental Found";
            }
            await Task.Run(() =>
            {
            });

            return new Tuple<Status, string>(returnStatus, message);
        }

        internal async Task<Tuple<Status, string>> ReturnRental(int id, int mileage)
        {
            Rental rentalEntity = new Rental();
            Vehicle car = new Vehicle();
            string message;
            Status returnStatus;


            await Task.Run(async () =>
            {
                car = await DBase.Vehicles.FindAsync(id);

                if (car.Mileage > mileage)
                {
                    message = $"Can't Set Mileage Lower Than Before {car.Mileage}";
                    returnStatus = Status.Failure;
                }
                else
                {
                    car.Mileage = mileage;
                }
            });

            try
            {
                rentalEntity = DBase.Rentals.FirstOrDefault(rental => rental.Vehicle_ID == id);
                rentalEntity.Returned_Date = DateTime.Now;
                car.Available = true;
                car.Rentable = true;
                await DBase.SaveChangesAsync();
                returnStatus = Status.Success;
                message = $"Vehicle Number {id} Returned";
            }
            catch (Exception error)
            {
                string err = error.Message;
                returnStatus = Status.Failure;
                message = "SomeThing Wen't Wrong";
            }

            return new Tuple<Status, string>(returnStatus, message);
        }

        internal async Task<Tuple<Status, List<RentalModel>, string>> GetUsersRentals(int userId)
        {
            List<RentalModel> returnList = new List<RentalModel>();
            IEnumerable<Rental> usersRentals = DBase.Rentals.Where(rent => rent.User_ID == userId);
            if (usersRentals != null)
            {
                foreach (Rental rental in usersRentals)
                {
                    RentalModel model = new RentalModel();
                    model = ConverRentalToModel(rental);
                    if (rental.Returned_Date != null)
                    {
                        model.Total_Price = CalculateTotalSum(await DBase.Vehicle_Types.FindAsync(rental.Vehicle.Vehicle_Type_ID), model);
                    }
                    else
                    {
                        model.Total_Price = CalculateCurrentTotalSum(await DBase.Vehicle_Types.FindAsync(rental.Vehicle.Vehicle_Type_ID), model);// model.Vehicle_ID
                    }
                    returnList.Add(model);
                }
                return new Tuple<Status, List<RentalModel>, string>(Status.Success, returnList, "Rental List");
            }
            else
            {
                return new Tuple<Status, List<RentalModel>, string>(Status.Success, null, "No Rentals Found For This User");
            }
        }

        internal async Task<Tuple<Status, string>> DeleteRental(int rentalId)
        {
            Rental rnt = new Rental();
            rnt = DBase.Rentals.FirstOrDefault(rent => rent.Rental_ID == rentalId);
            if (rnt != null)
            {
                try
                {
                    DBase.Rentals.Remove(rnt);
                    await DBase.SaveChangesAsync();
                    return new Tuple<Status, string>(Status.Success, $"Rental Number {rnt.Rental_ID} Deleted");
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    return new Tuple<Status, string>(Status.Failure, $"Couldn't Delete Rental Number {rnt.Rental_ID}");
                }

            }

            return null;
        }

        internal async Task<List<RentalModel>> getAll(int userID)
        {
            List<RentalModel> rentals = new List<RentalModel>();
            await Task.Run(() =>
            {


                foreach (Rental value in DBase.Rentals)
                {
                    if (value.User_ID == userID)
                    {
                        rentals.Add(ConverRentalToModel(value));
                    }
                }

            });

            if (rentals.Count > 0)
            {
                return rentals;
            }
            else
            {
                return null;
            }

        }

        private RentalModel ConverRentalToModel(Rental value)
        {
            return new RentalModel() { Rental_ID = value.Rental_ID, User_ID = value.User_ID, Vehicle_ID = value.Vehicle_ID, Start_Date = value.Start_Date, Return_Date = value.Return_Date, Returned_Date = value.Returned_Date };
        }

        internal int? CalculateCurrentTotalSum(Vehicle_Types vehicle, RentalModel rental)
        {
            var car = vehicle;
            int total;

            int daysInRental = (int)rental.Return_Date.Subtract(rental.Start_Date).TotalDays;
            int daysRented = (int)DateTime.Now.Subtract(rental.Start_Date).TotalDays;


            if (daysRented > 0)
            {
                if (daysRented > daysInRental)
                {
                    total = ((daysRented - daysInRental) * (car.Penalty_Rate + car.Daily_Rate)) + (daysInRental * car.Daily_Rate);
                }
                else
                {
                    total = daysRented * car.Daily_Rate;
                }

            }
            else
            {
                total = 0;
            }

            return total;
        }

        internal int? CalculateTotalSum(Vehicle_Types vehicle, RentalModel rental)
        {
            var car = vehicle;
            int total;

            double daysRented = rental.Returned_Date.Value.Subtract(rental.Start_Date).TotalDays;

            double daysIntended = rental.Return_Date.Subtract(rental.Start_Date).TotalDays;


            if (daysRented > 0)
            {
                int days = (int)daysIntended - (int)daysRented;
                if ((int)daysIntended >= (int)daysRented)
                {
                    total = (int)daysRented * car.Daily_Rate;
                }
                else
                {
                    total = (int)(((daysRented - daysIntended) * (car.Penalty_Rate + car.Daily_Rate)) + (daysRented * car.Daily_Rate));
                }
            }
            else
            {
                total = 0;
            }

            return total;
        }


    }
}