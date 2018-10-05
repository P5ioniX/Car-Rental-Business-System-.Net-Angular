using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rentalBackEnd_Web_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace rentalBackEnd_Web_API.StoreClasses
{
    public class AccountStore
    {
        UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(new ApplicationDbContext());

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> TryAddUserToDataBase(AccountModel model)
        {
            IdentityResult createdUserResult = new IdentityResult();

            await Task.Run(async ()=> {

                UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    fullName = model.fullName,
                    Email = model.eMail,
                    dateOfBirth = model.dateOfBirth,
                    profilePicture = model.profilePicture,
                    sex = model.sex,
                    Id = model.Id.ToString()
                };

                createdUserResult = await TryCreateUser(user, manager, store , model);

                if (createdUserResult.Succeeded)
                {
                    manager.AddToRole(user.Id, "User");
                }
            });

            return createdUserResult;
        }


        /// <summary>
        /// Check if user exists & Create A User In the users database -> return an Identity Result
        /// </summary>
        /// <param name="user"></param>
        /// <param name="manager"></param>
        /// <param name="store"></param>
        /// <returns>an IdentityResult</returns>
        private async Task<IdentityResult> TryCreateUser(ApplicationUser user, UserManager<ApplicationUser> manager, UserStore<ApplicationUser> store , AccountModel setUser)
        {
            IdentityResult returnResult = new IdentityResult();

            await Task.Run(()=> {
                try
                {
                    ApplicationUser userToCheck = manager.FindById(setUser.Id.ToString());

                    if (userToCheck != null) //Check if the same ID exists (PK Field...)
                    {
                        returnResult = IdentityResult.Failed("You are already registered...");
                    }
                    else
                    {
                        manager.Create(user, setUser.passWord); //Create A User In the users database
                        IdentityResult role = manager.AddToRole(user.Id.ToString(), "User"); //Create Add The User To A User Role
                        returnResult = role;
                    }
                }
                catch (Exception err)
                {
                    string error = err.Message;

                    returnResult = IdentityResult.Failed("Something went WRONG !!!");
                }
            });

            return returnResult;

        }

        internal async Task<AccountModel> UpdateUserById(int id, AccountModel model)
        {
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
            ApplicationUser usr = await manager.FindByIdAsync(id.ToString());

            if (usr!=null)
            {
                store.AutoSaveChanges = true;
                usr.UserName = model.UserName;
                usr.fullName = model.fullName;
                usr.Email = model.eMail;
                usr.dateOfBirth = model.dateOfBirth;
                usr.sex = model.sex;
                await store.UpdateAsync(usr);
                return model;
            }

            return null;
        }

        internal async Task<string> DeleteUserById(int id)
        {
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
            ApplicationUser usr = await manager.FindByIdAsync(id.ToString());

            if (usr != null)
            {
                await store.DeleteAsync(usr);
                return usr.UserName;
            }

            return null;
        }

        private ApplicationUser ConvertModelToApplicationUser(AccountModel model)
        {
            return new ApplicationUser()
            {
                UserName = model.UserName,
                fullName = model.fullName,
                dateOfBirth = model.dateOfBirth,
                sex = model.sex,
                Email = model.eMail,
                profilePicture = model.profilePicture
            };
        }

        internal async Task<AccountModel> GetUserById(int id)
        {
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
            ApplicationUser usr = await manager.FindByIdAsync(id.ToString());

            if (usr != null)
            {
                usr.PasswordHash = null;
                AccountModel model = convertApplicationUserToModel(usr);
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Convert ApplicationUser To an AccountModel Object
        /// </summary>
        /// <param name="usr"></param>
        /// <returns>AccountModel</returns>
        private AccountModel convertApplicationUserToModel(ApplicationUser usr)
        {
            return new AccountModel()
            {
                fullName = usr.fullName,
                UserName = usr.UserName,
                dateOfBirth = usr.dateOfBirth,
                eMail = usr.Email,
                Role = usr.Roles.ToString(),
                sex = usr.sex,
                Id = int.Parse(usr.Id),
                profilePicture = usr.profilePicture
            };
        }
    }
}