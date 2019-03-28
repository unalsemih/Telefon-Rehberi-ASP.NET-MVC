using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telefon_Rehberi_MVC.DB;

namespace Telefon_Rehberi_MVC.Models.Account
{
    public class ChangePasswordModel
    {
        public DB.Admin admin { get; set; }
        public string password { get; set; }
        public string currentPassword { get; set; }

    }
}