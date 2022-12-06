using ProjetM2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ProjetM2.Models;
using System.IO;

namespace ProjetM2.Controllers
{
    public class FilmController : Controller
    {
        private ProjetM2Entities1 db = new ProjetM2Entities1();
        // GET: Film
        public ActionResult Index()
        {
            return View(db.FILM.ToList());
        }

        // GET: Film/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Film/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Film/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude ="IDOBJET")] FILM FilmToCreate)
        {
            if(!ModelState.IsValid)
                return View();
            try
            {
              //  var f = HttpContext.Request.Files[0];
              //  string path = Server.MapPath("~/uploadfile/photo");
               // if(!Directory.Exists(path))
               //     Directory.CreateDirectory(path);
               // string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
               // f.SaveAs(path + fileName);
              //  FilmToCreate.PHOTO = fileName;
             
                db.FILM.Add(FilmToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Film/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var FilmToEdit = (from m in db.FILM where m.IDOBGET == id select m).First();

            if(FilmToEdit == null)
            {
                return HttpNotFound();
            }
            return View(FilmToEdit);
        }

        // POST: Film/Edit/5
        [HttpPost]
        public ActionResult Edit(FILM filmToEdit)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                // TODO: Add update logic here
                db.Entry(filmToEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Film/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            FILM course = db.FILM.Find(id);
            if(course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Film/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                FILM course = db.FILM.Find(id); 
                db.FILM.Remove(course);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //search by filmname categorie ou director
        public ActionResult Search(string keyword)
        {
            
            var work = from t in db.FILM select t;
           
            //work = work.Where(t => t.NAMEFILM.Contains(keyword));
            work = work.Where(t => t.CATEGORIE.Contains(keyword));
            //work = work.Where(t => t.DIRECTOR.Contains(keyword));

            return View(work);

        }
    }
}
