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
    public class EstantesController : Controller
    {
        private LibreroContext db = new LibreroContext();

        // GET: Estantes
        public ActionResult Index()
        {
            return View(db.estantes.ToList());
        }

        // GET: Estantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estante estante = db.estantes.Find(id);
            if (estante == null)
            {
                return HttpNotFound();
            }
            return View(estante);
        }

        // GET: Estantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Genero")] Estante estante)
        {
            if (ModelState.IsValid)
            {
                db.estantes.Add(estante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estante);
        }

        // GET: Estantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estante estante = db.estantes.Find(id);
            if (estante == null)
            {
                return HttpNotFound();
            }
            return View(estante);
        }

        // POST: Estantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Genero")] Estante estante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estante);
        }

        // GET: Estantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estante estante = db.estantes.Find(id);
            if (estante == null)
            {
                return HttpNotFound();
            }
            return View(estante);
        }

        // POST: Estantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estante estante = db.estantes.Find(id);
            db.estantes.Remove(estante);
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
