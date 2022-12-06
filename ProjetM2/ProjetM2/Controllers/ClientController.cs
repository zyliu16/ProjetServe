using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetM2.Models;

namespace ProjetM2.Controllers
{
    public class ClientController : Controller
    {
        // Login Client
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(CLIENT c)
        {
            using(ProjetM2Entities1 db = new ProjetM2Entities1())
            {
                CLIENT cs = db.CLIENT.Where(x => x.USERNAME == c.USERNAME && x.MDP == c.MDP).FirstOrDefault();
                if (cs != null)
                {
                    Session["CLIENT"] = cs.IDOBGET;
                    return Redirect("/Louer/Index");
                }
                else
                {
                    ViewBag.err = "USER NAME OR PASSWORD FAILE";
                    return View();
                }
            }
        }

        // login admin
        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(ADMIN a)
        {
            using (ProjetM2Entities1 db = new ProjetM2Entities1())
            {
                ADMIN ad = db.ADMIN.Where(x => x.NOM == a.NOM && x.MDP == a.MDP).FirstOrDefault();
                if (ad != null)
                {
                    Session["ADMIN"] = ad;
                    return Redirect("/Film/Index");
                }
                else
                {
                    ViewBag.err = "USER NAME OR PASSWORD FAILE";
                    return View();
                }
            }
        }

        // signup client
        [HttpGet]
        public ActionResult SignUP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUP(CLIENT c)
        {
            using (ProjetM2Entities1 db = new ProjetM2Entities1())
            {
                db.CLIENT.Add(c);
                db.SaveChanges();
                return Redirect("/Client/Login");
            }
        }
    }
}