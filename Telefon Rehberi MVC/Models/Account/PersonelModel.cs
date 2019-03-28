using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi_MVC.DB;
namespace Telefon_Rehberi_MVC.Models.Account
{
    public class PersonelModel
    {
        public Calisanlar calisan { get; set; }
        public string departmanAdi;
        public string yoneticiAdSoyad;
        public List<SelectListItem> departmanList { get; set; }
        public List<SelectListItem> yoneticiList { get; set; }
        public int yoneticiID;
        public int departmanID;
    }
}