using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;
//https://code.msdn.microsoft.com/Face-API-Using-ASPNet-MVC-40259a76#content
//https://docs.microsoft.com/en-gb/azure/cognitive-services/face/quickstarts/csharp
//https://southcentralus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395237

namespace Libreria.Controllers
{
    public class FotoPersonasController : Controller
    {
        private LibreroContext db = new LibreroContext();

        // GET: FotoPersonas
        public ActionResult Index()
        {
            return View(db.fotoPersonas.ToList());
        }

        // GET: FotoPersonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FotoPersona fotoPersona = db.fotoPersonas.Find(id);
            if (fotoPersona == null)
            {
                return HttpNotFound();
            }
            return View(fotoPersona);
        }

        // GET: FotoPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FotoPersonas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idPersona,faceId")] FotoPersona fotoPersona)
        {
            if (ModelState.IsValid)
            {
                db.fotoPersonas.Add(fotoPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fotoPersona);
        }

        // GET: FotoPersonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FotoPersona fotoPersona = db.fotoPersonas.Find(id);
            if (fotoPersona == null)
            {
                return HttpNotFound();
            }
            return View(fotoPersona);
        }

        // POST: FotoPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idPersona,faceId")] FotoPersona fotoPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fotoPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fotoPersona);
        }

        // GET: FotoPersonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FotoPersona fotoPersona = db.fotoPersonas.Find(id);
            if (fotoPersona == null)
            {
                return HttpNotFound();
            }
            return View(fotoPersona);
        }

        // POST: FotoPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FotoPersona fotoPersona = db.fotoPersonas.Find(id);
            db.fotoPersonas.Remove(fotoPersona);
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
