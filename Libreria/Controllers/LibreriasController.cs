using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;

namespace Libreria.Controllers
{
    public class LibreriasController : Controller
    {
        private LibreroContext db = new LibreroContext();

        // GET: Librerias
        public ActionResult Index()
        {
            return View(db.Librerias.ToList());
        }

        // GET: Librerias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librerias librerias = db.Librerias.Find(id);
            if (librerias == null)
            {
                return HttpNotFound();
            }
            return View(librerias);
        }

        // GET: Librerias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Librerias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id")] Librerias librerias)
        {
            if (ModelState.IsValid)
            {
                db.Librerias.Add(librerias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(librerias);
        }

        // GET: Librerias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librerias librerias = db.Librerias.Find(id);
            if (librerias == null)
            {
                return HttpNotFound();
            }
            return View(librerias);
        }

        // POST: Librerias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] Librerias librerias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(librerias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(librerias);
        }

        // GET: Librerias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Librerias librerias = db.Librerias.Find(id);
            if (librerias == null)
            {
                return HttpNotFound();
            }
            return View(librerias);
        }

        // POST: Librerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Librerias librerias = db.Librerias.Find(id);
            db.Librerias.Remove(librerias);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
