using ProjetM2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Infrastructure;

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
                db.FILM.Add(FilmToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Photo(int id)
        {
            var Img = (from m in db.FILM where m.IDOBGET == id select m).First();
           // ViewData.Model = Img;
            return View(Img);
        }

        [HttpPost]
        public ActionResult Photo(FILM Img,HttpPostedFileBase files)
        {

            System.Diagnostics.Debug.WriteLine(Img.IDOBGET);

            string fileName = files.FileName;
            string extension = Path.GetExtension(fileName).ToLower();
            string dir = "../Images/";
            
            if (!Directory.Exists(Server.MapPath(dir)))
                   Directory.CreateDirectory(Server.MapPath(dir));
            
            if (extension.Equals(".jpg")|| extension.Equals(".png"))
            {
                
                string saveUrl = dir + fileName;
                files.SaveAs(Server.MapPath(saveUrl));
               

                //img.PHOTO= saveUrl;
                var img = new FILM {IDOBGET=16 , PHOTO = saveUrl};
                db.FILM.Attach(img);
                db.Entry(img).Property(x=>x.PHOTO).IsModified=true;
                db.SaveChanges();

            }
            
            return RedirectToAction("Index");
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
            System.Diagnostics.Debug.WriteLine(filmToEdit.IDOBGET);
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
        public ActionResult Search(string keyword, bool? done, bool? done1, bool? done2)
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
    }
}
