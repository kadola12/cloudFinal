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
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.IO;
using System.Windows.Input;
//https://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files

namespace Libreria.Controllers
{
    public class PersonasController : Controller
    {
        private LibreroContext db = new LibreroContext();
        private static string ServiceKey = ConfigurationManager.AppSettings["FaceServiceKey"];

        private readonly IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(ServiceKey),new System.Net.Http.DelegatingHandler[] { });

        IList<DetectedFace> faceList;   // The list of detected faces.
        String[] faceDescriptions;      // The list of descriptions for the detected faces.
        double resizeFactor;            // The resize factor for the displayed image.



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
        public ActionResult Create(Persona persona, HttpPostedFileBase file)
        {
            //if (ModelState.IsValid)
            //{
                //string path = file.FileName;
                //Console.WriteLine(file.FileName);
                //if (file != null)
                //{

                //Console.WriteLine("hola"+file.ToString());

                //return View(persona);
                try
                {
                    if (file != null)
                    {
                        string fileName= System.IO.Path.GetFullPath(file.FileName);
                        identificarPersona(fileName);
                        //db.personas.Add(persona);
                        //db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(persona);
                    }
                }
                catch
                {
                    ViewBag.Message = "No se pudo cargar la imagen";
                    return View();
                    
                }
                    //var fileName = Path.GetFileName(file.FileName);
                    //Guarda el archivo
                    //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    //file.SaveAs(path);
                    
                //}
                //Console.WriteLine(Request.Files.Count);
                //return View("Index");
            /*}
            else
            {
                return View(persona);
            }*/
            
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

        

        public void identificarPersona(string file)
        {
            var client = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString();

            // Request headers
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ServiceKey);

            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify?";
            //HttpResponseMessage response;

            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =new FaceAttributeType[]
            {
                FaceAttributeType.Gender, FaceAttributeType.Age,
                FaceAttributeType.Smile, FaceAttributeType.Emotion,
                FaceAttributeType.Glasses, FaceAttributeType.Hair
            };

            // Call the Face API.
            try
            {   
                using (Stream imageFileStream = System.IO.File.OpenRead(file))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    //IList<DetectedFace> faceList =faceClient.Face.DetectWithStreamAsync(imageFileStream, true, false, faceAttributes);
                    //return faceList;
                }
            }
            // Catch and display Face API errors.
            catch (APIErrorException f)
            {
                //MessageBox.Show(f.Message);
                //return new List<DetectedFace>();
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error");
                //return new List<DetectedFace>();
            }

        }

        

    }

}
