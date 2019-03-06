using ShareBook.BusinessLayer;
using ShareBook.Entities;
using ShareBook.Entities.ValueObjects;
using ShareBook.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private SharebookUserManager sharebookUserManager = new SharebookUserManager();
        public ActionResult LoginPage()
        {
            if (CurrentSession.shareBookUserAdmin != null)
            {
                CurrentSession.RemoveSession("LoginAdmin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(AdminViewModel adm)
        {
            var loginAdmin = sharebookUserManager.Find(x => x.Email == adm.Email && x.Password == adm.Password && x.isAdmin);

            if (loginAdmin != null)
            {
                CurrentSession.Set<ShareBookUser>("LoginAdmin",loginAdmin);
                return RedirectToAction("Index", "AdminHome");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    ModelState.AddModelError("username", "Kullanıcı Adı ya da Şifre Hatalı");
                }
                return View(adm);

            }
        }
    }
}