using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
//https://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files

namespace Libreria.Controllers
{
    public class PersonasController : Controller
    {
        private LibreroContext db = new LibreroContext();
        

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
        public async Task<ActionResult> Create(Persona persona, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //string fileName = file.FileName;
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Fotos"), fileName);
                        file.SaveAs(path);
                        byte[] img = GetImageAsByteArray(path.ToString());
                        //byte[] img = GetImageAsByteArray(fileName);
                        string resultado = await UploadUserPictureApiCommand("http://libros.westeurope.cloudapp.azure.com/nuevapersona", "[]", img);
                        //await UploadUserPictureApiCommand("http://libros.westeurope.cloudapp.azure.com/reconpersona", "[]", img);
                        if (resultado.Equals("-1"))
                        {
                            ViewBag.Message = "Seleccione una foto mas clara";
                            return View();
                        }
                        else
                        {
                            db.personas.Add(persona);
                            db.SaveChanges();
                            FotoPersona fotoPersona = new FotoPersona();
                            fotoPersona.idPersona = Int32.Parse(persona.Id);
                            fotoPersona.faceId = resultado;
                            db.fotoPersonas.Add(fotoPersona);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        
                    }
                    else
                    {
                        ViewBag.Message = "Seleccione Foto";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.ToString();
                    return View();

                }
            }
            else
            {
                ViewBag.Message = "Modelo Invalido";
                return View();
            }
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

       
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        

        public async Task<string> UploadUserPictureApiCommand(string api, string json, byte[] picture)
        {
            using (var httpClient = new HttpClient())
            {

                MultipartFormDataContent form = new MultipartFormDataContent();

                form.Add(new StringContent(json), "payload");
                form.Add(new ByteArrayContent(picture, 0, picture.Count()), "upload", "user_picture.jpg");
                HttpResponseMessage response = await httpClient.PostAsync(api, form);
                response.EnsureSuccessStatusCode();
                Task<string> responseBody = response.Content.ReadAsStringAsync();

                if (response.StatusCode.ToString() != "OK")
                {
                    return "ERROR: " + response.StatusCode.ToString();
                }
                else
                {
                    return responseBody.Result.ToString();
                }
            }
        }

    }


}
