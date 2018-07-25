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
        public async Task<ActionResult> Create([Bind(Include = "IdLibro,FechaSalida,FechaDevolucion")] PersonaLibro personaLibro, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                /*db.personaLibros.Add(personaLibro);
                db.SaveChanges();
                return RedirectToAction("Index");*/
                try
                {
                    //string fileName = file.FileName;
                    if (file.ContentLength > 0)
                    {

                        

                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Fotos"), fileName);

                        //var path = "~/Fotos/"+fileName;
                        file.SaveAs(path);
                        byte[] img = GetImageAsByteArray(path.ToString());
                        //byte[] img = GetImageAsByteArray(Path.GetTempPath()+fileName);
                        string resultado = await UploadUserPictureApiCommand("http://libros.westeurope.cloudapp.azure.com/reconpersona", "[]", img);
                        if (resultado.Equals("-1"))
                        {
                            ViewBag.Message = "Ingrese Primero a la persona";
                            return View();
                        }
                        else if (resultado.Equals("-2"))
                        {
                            ViewBag.Message = "Seleccione una foto mas clara";
                            return View();
                        }
                        else
                        {
                            var query = from contact in db.fotoPersonas where contact.faceId == resultado select contact;


                            List<FotoPersona> lista = query.ToList<FotoPersona>();     
                              

                            //var fotoPersona = db.fotoPersonas.SqlQuery(query).ToList();

                            string idPersona = lista[0].idPersona.ToString();
                            personaLibro.IdPersona = idPersona;

                            db.personaLibros.Add(personaLibro);
                            db.SaveChanges();
                            
                            return RedirectToAction("Index");
                            /*ViewBag.Message = personaLibro.FechaSalida;
                            return View();*/
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
                ViewBag.Message = "Datos Incorrectos";
                return View();
            }
            
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
