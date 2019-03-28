using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi_MVC.DB;
namespace Telefon_Rehberi_MVC.Controllers
{
    public class BaseController : Controller
    {
        protected TelefonRehberiDBEntity context { get; private set; }
        public static string logonUserName {get; set;}
        public BaseController()
        {
            context = new TelefonRehberiDBEntity();
        }
    }
}