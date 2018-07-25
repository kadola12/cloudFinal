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
    public class PersonaLibroesController : Controller
    {
        private LibreroContext db = new LibreroContext();

        // GET: PersonaLibroes
        public ActionResult Index()
        {
            return View(db.personaLibros.ToList());
        }

        // GET: PersonaLibroes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaLibro personaLibro = db.personaLibros.Find(id);
            if (personaLibro == null)
            {
                return HttpNotFound();
            }
            return View(personaLibro);
        }

        // GET: PersonaLibroes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonaLibroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPersona,IdLibro,FechaSalida,FechaDevolucion")] PersonaLibro personaLibro)
        {
            if (ModelState.IsValid)
            {
                db.personaLibros.Add(personaLibro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personaLibro);
        }

        // GET: PersonaLibroes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaLibro personaLibro = db.personaLibros.Find(id);
            if (personaLibro == null)
            {
                return HttpNotFound();
            }
            return View(personaLibro);
        }

        // POST: PersonaLibroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPersona,IdLibro,FechaSalida,FechaDevolucion")] PersonaLibro personaLibro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaLibro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personaLibro);
        }

        // GET: PersonaLibroes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaLibro personaLibro = db.personaLibros.Find(id);
            if (personaLibro == null)
            {
                return HttpNotFound();
            }
            return View(personaLibro);
        }

        // POST: PersonaLibroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PersonaLibro personaLibro = db.personaLibros.Find(id);
            db.personaLibros.Remove(personaLibro);
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
