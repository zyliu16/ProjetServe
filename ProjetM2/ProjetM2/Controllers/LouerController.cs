using ProjetM2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using System.Runtime.Remoting.Contexts;
using System.Runtime.InteropServices;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Html2pdf;
using Org.BouncyCastle.Asn1.Cms;

namespace ProjetM2.Controllers
{
    
    public class LouerController : Controller
    {
        private ProjetM2Entities1 db = new ProjetM2Entities1();
        // GET: Louer
        public ActionResult Index()
        {
            return View(db.FILM.OrderByDescending(FILM => FILM.NBPRET).Take(10));
        }
        public ActionResult Index2()
        {
            return View(db.FILM.ToList());
        }


        // GET: Louer
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var FilmToLouer = (from m in db.FILM where m.IDOBGET == id select m).First();
            

            if (FilmToLouer == null)
            {
                return HttpNotFound();
            }
            return View(FilmToLouer);
        }

        // POST: Louer
        [HttpPost]
        public ActionResult Edit(FILM f) 
        { 
            if (!ModelState.IsValid)
                    return View();
            try
             {
                string client = Session["CLIENT"].ToString();
                int clientID;
                Int32.TryParse(client, out clientID);
                CLIENT cl = db.CLIENT.Find(clientID);
                FILM fl = db.FILM.Find(f.IDOBGET);
                if (cl.TIMERENDU!=null  && fl.LIST > 0)
                {
                    cl.LISTLOUEE = null;
                    cl.TIMERENDU = null;
                    fl.LIST--;
                    cl.LISTLOUEE = fl.NAMEFILM;
                    cl.TIMELOUEE = DateTime.Now;
                    db.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
                  {
                        return View();
                  }
        }
        public ActionResult Search(string keyword, bool? done , bool?done1 , bool?done2)
        {
           
            var work = from t in db.FILM select t;

            if (done.Value)
            {
                work = work.Where(t => t.NAMEFILM.Contains(keyword));
            }

            if (done1.Value)
            {
                work = work.Where(t => t.DIRECTOR.Contains(keyword));
            }

            if (done2.Value)
            {
                work = work.Where(t => t.CATEGORIE.Contains(keyword));
            }



            //System.Diagnostics.Debug.WriteLine(work.GetType());//afficher le resultat dans le console

            return View(work);

        }

        public ActionResult Rendre()
        {
            string client = Session["CLIENT"].ToString();
            int clientID;
            Int32.TryParse(client, out clientID);
            CLIENT cl = db.CLIENT.Find(clientID);
           
            if(cl.LISTLOUEE == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string filmLouee = cl.LISTLOUEE;
               
                FILM fl = db.FILM
                            .Where(b => b.NAMEFILM == filmLouee)
                            .FirstOrDefault();
                
                fl.LIST += 1;
                cl.TIMERENDU = DateTime.Now;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Facture()
        {
            string client = Session["CLIENT"].ToString();
            int clientID;
            Int32.TryParse(client, out clientID);
            CLIENT cl = db.CLIENT.Find(clientID);

            if (cl.LISTLOUEE == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(db.CLIENT.Where(p => p.IDOBGET == clientID).ToList());
            }
        }

    }
}
