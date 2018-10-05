using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rentalBackEnd_Web_API.Models
{
    public class AccountModel
    {
        public int      Id             { get; set; }
        public string   fullName       { get; set; }
        public string   eMail          { get; set; }
        public string   UserName       { get; set; }
        public string   passWord       { get; set; }
        public DateTime dateOfBirth    { get; set; }
        public string   sex            { get; set; }
        public byte[]   profilePicture { get; set; }

        public string   Role           { get; set; }
        public string   LoggedInTime   { get; set; }

    }
}