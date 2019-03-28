using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telefon_Rehberi_MVC.DB;
namespace Telefon_Rehberi_MVC.Models
{
    public class DepartmanListModel
    {
   
        public Departmanlar departman { get; set; }
        public int PersonelSayisi { get; set; }
    }
}