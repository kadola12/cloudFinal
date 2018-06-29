using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;
using System.Windows;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Drawing;
using System.Diagnostics;


namespace Libreria.Controllers
{
    public class PersonasController : Controller
    {
        private LibreroContext db = new LibreroContext();
        private static string ServiceKey = ConfigurationManager.AppSettings["FaceServiceKey"];
        // GET: Personas
        public ActionResult Index()
        {
            return View(db.personas.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Estudiante,Genero,Edad,Correo")] Persona persona, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //Console.WriteLine(file.FileName);
                //if (file != null)
                //{
                    
                    //HttpPostedFileBase file = Request.Files[0];
                    //Console.WriteLine("hola"+file);
                    //if (file.ContentLength > 0)
                    //{
                        identificarPersona(file);
                        db.personas.Add(persona);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    //}
                    //var fileName = Path.GetFileName(file.FileName);
                    //Guarda el archivo
                    //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    //file.SaveAs(path);
                    
                //}
                //Console.WriteLine(Request.Files.Count);
                //return View(persona);
            }

            return View(persona);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Estudiante,Genero,Edad,Correo")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Persona persona = db.personas.Find(id);
            db.personas.Remove(persona);
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

        

        public void identificarPersona(HttpPostedFileBase file)
        {
            var client = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ServiceKey);

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify?" + file;

            //HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
                //response = client.PostAsync(uri, content);
            }

        }

    }

}
