using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using rentalBackEnd_Web_API.Models;
using rentalBackEnd_Web_API.StoreClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace rentalBackEnd_Web_API.Controllers
{
    [RoutePrefix("api/User")]
    public class AccountController : ApiController
    {
        AccountStore store = new AccountStore();

        #region [Route("GetUserClaims")]
        [HttpGet]
        [Route("GetUserClaims")]
        public AccountModel GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identityClaims.Claims;
            AccountModel model = new AccountModel()
            {
                UserName = identityClaims.FindFirst("UserName").Value,
                eMail = identityClaims.FindFirst("E-Mail").Value,
                fullName = identityClaims.FindFirst("Full Name").Value,
                Role = identityClaims.FindFirst("Role").Value,
                LoggedInTime = identityClaims.FindFirst("LoggedIn").Value
            };

            return model;
        }

        #endregion

        #region [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IdentityResult> Register(AccountModel model)
        {
            try
            {
                return await store.TryAddUserToDataBase(model);

            }
            catch (Exception msg)
            {
                return IdentityResult.Failed();
            }

        }

        #endregion

        #region [Route("GetUserById")]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IHttpActionResult> GetUserById(int Id)
        {
            AccountModel usr = await store.GetUserById(Id);

            if (usr == null)
            {

                return Content(HttpStatusCode.NotFound, "User Not Found");
            }
            else
            {
                return Content(HttpStatusCode.OK, usr);
            }
        }

        #endregion

        #region [Route("UpdateUserById")]
        [HttpPatch]
        [Route("UpdateUserById")]
        public async Task<IHttpActionResult> UpdateUserById(int Id, [FromBody] AccountModel model)
        {
            AccountModel usr = await store.UpdateUserById(Id, model);

            if (usr == null)
            {

                return Content(HttpStatusCode.NotFound, "User Not Found");
            }
            else
            {
                return Content(HttpStatusCode.Created, usr);
            }
        }

        #endregion

        #region [Route("DeleteUserById")]
        [HttpDelete]
        [Route("DeleteUserById")]
        public async Task<IHttpActionResult> DeleteUserById(int Id)
        {
            string usr = await store.DeleteUserById(Id);

            if (usr == null)
            {
                return Content(HttpStatusCode.NotFound, "User Not Found");
            }
            else
            {
                return Content(HttpStatusCode.OK, usr + " Deleted");
            }
        }

        #endregion


    }
}
