using MyMuzic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMuzic.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult Index()
        {
            using (MaaPlayerEntities db = new MaaPlayerEntities())
            {
                return View(db.UserAccounts.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (MaaPlayerEntities db = new MaaPlayerEntities())
                {
                    account.UserID = Guid.NewGuid();
                    db.UserAccounts.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered.";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            try
            {
                using (MaaPlayerEntities db = new MaaPlayerEntities())
                {

                    var usr = (from userlist in db.UserAccounts
                               where userlist.UserName == user.UserName && userlist.Password == user.Password
                               select new
                               {
                                   userlist.UserID,
                                   userlist.UserName
                               }).ToList();
                    //var usr = db.UserAccounts.Single(u => u.UserName == user.UserName && u.Password == user.Password);
                    if (usr != null)
                    {
                        Session["UserID"] = usr.FirstOrDefault().UserID.ToString();
                        Session["UserName"] = usr.FirstOrDefault().UserName.ToString();
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Pasword is Wong.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Username or Pasword is Wong.");
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}