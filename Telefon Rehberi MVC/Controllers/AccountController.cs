using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi_MVC.Models.Account;

namespace Telefon_Rehberi_MVC.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Profile(int id)
        {
            PersonelModel personel = new PersonelModel();
            personel.calisan = context.Calisanlar.FirstOrDefault(x => x.calisanID == id);
            personel.departmanAdi = context.Departmanlar.FirstOrDefault(x => x.id == personel.calisan.departmanID).departmanAdi;
            DB.Calisanlar yonetici = context.Calisanlar.FirstOrDefault(y => y.calisanID == personel.calisan.yoneticiID);

            if (yonetici == null)
                personel.yoneticiAdSoyad = "Yönetici bilgisi yok!";
            else
                personel.yoneticiAdSoyad = yonetici.ad + " " + yonetici.soyad;
            return View(personel);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["LogonAdmin"] != null)
            {
                ChangePasswordModel model = new ChangePasswordModel();
         
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
       
            if(Session["LogonAdmin"] == null)
            {
                TempData["resultInfo"] = "Oturum süreniz dolmuştur. Lütfen Oturum Açıp Tekrar Deneyiniz!";
                return RedirectToAction("Login", "Action");
            }
            if (model.admin.Password == model.password)
            {
                var admin = context.Admin.FirstOrDefault(x => x.adminUserName == logonUserName);
                if (admin != null)
                {
                    if (admin.Password == model.currentPassword)//girilen mevcut şifre doğrumu
                    {
                        admin.Password = model.admin.Password;
                        try
                        {

                            context.Entry<DB.Admin>(admin).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            TempData["resultInfo"] = "Şifreniz Başarıyla Değiştirildi";

                        }
                        catch (Exception ex)
                        {
                            TempData["resultInfo"] = "Üzgünüz şifreniz değiştirilemedi!";

                        }
                        return View(new ChangePasswordModel());
                    }
                    else
                    {
                        TempData["resultInfo"] = "Mevcut Şifreniz Doğru Değil";
                        return View(new ChangePasswordModel());
                    }
                }
                else
                {
                    TempData["resultInfo"] = "Şifreniz Değiştirilemedi!";
                    return View(new ChangePasswordModel());
                }
            }
            else
            {
                TempData["resultInfo"] = "Şifreler Uyuşmamaktadır!";
                return View(new ChangePasswordModel());
            }
            
        }

        [HttpGet]
        public ActionResult ProfileCreate()
        {
            DB.Calisanlar model = new DB.Calisanlar();
            var departmanlar = context.Departmanlar.Select(x => new SelectListItem()
            {
                Text = x.departmanAdi,
                Value = x.id.ToString()
            }).ToList();
            ViewBag.departmanList = departmanlar;
            // model.departmanList = departmanlar;
            var yoneticiler = context.Calisanlar.Select(x => new SelectListItem()
            {
                Text = x.ad + " " + x.soyad,
                Value = x.calisanID.ToString()
            }).ToList();
            ViewBag.yoneticiList = yoneticiler;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileCreate(DB.Calisanlar personel)
        {

            System.Diagnostics.Debug.WriteLine("SomeText");
            System.Diagnostics.Debug.WriteLine("ad" + personel.ad);
            System.Diagnostics.Debug.WriteLine("soyad" + personel.soyad);
            System.Diagnostics.Debug.WriteLine("tel" + personel.telefon);
            System.Diagnostics.Debug.WriteLine("yonetici" + personel.yoneticiID);
            System.Diagnostics.Debug.WriteLine("departman" + personel.departmanID);



            context.Entry<DB.Calisanlar>(personel).State = System.Data.Entity.EntityState.Added;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Yeni Profil Kaydı Oluşturuldu.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = ex.Message;
                var departmanlar = context.Departmanlar.Select(x => new SelectListItem()
                {
                    Text = x.departmanAdi,
                    Value = x.id.ToString()
                }).ToList();
                ViewBag.departmanList = departmanlar;
                // model.departmanList = departmanlar;
                var yoneticiler = context.Calisanlar.Select(x => new SelectListItem()
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.calisanID.ToString()
                }).ToList();
                ViewBag.yoneticiList = yoneticiler;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult ProfileEdit(int id = 0)
        {
            var personel = new DB.Calisanlar();
            personel.calisanID = 0;
            DB.Calisanlar model = new DB.Calisanlar();
            if (id != 0)
            {
                personel = context.Calisanlar.FirstOrDefault(x => x.calisanID == id);
                model = personel;
            }
            System.Diagnostics.Debug.WriteLine("aaa--" + model.calisanID + "--");
            var departmanlar = context.Departmanlar.Select(x => new SelectListItem()
            {
                Text = x.departmanAdi,
                Value = x.id.ToString()
            }).ToList();
            ViewBag.departmanList = departmanlar;

            var yoneticiler = context.Calisanlar.Select(x => new SelectListItem()
            {
                Text = x.ad + " " + x.soyad,
                Value = x.calisanID.ToString()
            }).Where(x => x.Value != model.calisanID.ToString()).ToList();
            ViewBag.yoneticiList = yoneticiler;
          
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileEdit(DB.Calisanlar model)
        {
            context.Entry<DB.Calisanlar>(model).State = System.Data.Entity.EntityState.Modified;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Bilgiler Güncellendi!";
                return RedirectToAction("Profile", "Account", new { id = model.calisanID });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("aaa--" + "" + ex.Message);
                TempData["resultInfo"] ="Bilgiler Güncellenemedi!";
                var user = context.Calisanlar.FirstOrDefault(x => x.calisanID == model.calisanID);
                PersonelModel personel = new PersonelModel();
                personel.calisan = user;
                context.Entry<DB.Calisanlar>(model).State = System.Data.Entity.EntityState.Added;
                return RedirectToAction("ProfileEdit", "Account", new { id = model.calisanID });
            }
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                var admin = context.Admin.FirstOrDefault(x => x.adminUserName == model.admin.adminUserName && x.Password == model.admin.Password);
                if (admin != null)
                {
                    Session["LogonAdmin"] = admin;
                    logonUserName = admin.adminUserName;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Admin Bilgileri Yanlış!";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return View();


            }
        }

        public ActionResult Logout()
        {
            Session["LogonAdmin"] = null;
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Delete(int id)
        {
            var personel = context.Calisanlar.FirstOrDefault(x => x.calisanID == id);
            if(context.Calisanlar.Any(x=>x.yoneticiID == personel.calisanID))
            {
                TempData["resultInfo"] = "Bu personel yönetici olduğu için silinemez!";
                return RedirectToAction("Index", "Home");
            }
            context.Entry<DB.Calisanlar>(personel).State = System.Data.Entity.EntityState.Deleted;
            try
            {
                context.SaveChanges();
                TempData["resultInfo"] = "Personel silindi!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["resultInfo"] = "Kullanıcı bir sebepten dolayı silinemedi!";
                return RedirectToAction("Index", "Home");
            }

        }
    }
}