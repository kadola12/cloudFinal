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
            if(ModelState.IsValid)
            {
            //  string path = file.FileName;
            //  Console.WriteLine(file.FileName);
            //if (file != null){
            //{

            //Console.WriteLine("hola"+file.ToString());

            //return View(persona);
            Console.WriteLine("Inicio");
            //ViewBag.Message = System.IO.Path.GetFullPath(file.FileName).Length;
                //return View();
                try
                    {
                        string fileName = System.IO.Path.GetFullPath(file.FileName);
                        if (fileName.Length > 0)
                        {

                            //ViewBag.Message="Lo logramos";
                            //return View();
                            identificarPersona(fileName);
                            //db.personas.Add(persona);
                            //db.SaveChanges();
                            
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            
                            ViewBag.Message=file.FileName;
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.ToString();
                        return View();

                    }
                //var fileName = Path.GetFileName(file.FileName);
                //Guarda el archivo
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                //file.SaveAs(path);

                //}
                //Console.WriteLine(Request.Files.Count);
                //return View("Index");
            }
            else
            {
                ViewBag.Message = System.IO.Path.GetFullPath(file.FileName);
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

        

        public async void identificarPersona(string imageFilePath)
        {
            var client = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ServiceKey);

            string uriBase = "https://eastus.api.cognitive.microsoft.com/face/v1.0/identify?";
            //HttpResponseMessage response;

            // The list of Face attributes to return.
            string requestParameters = "returnFaceId=true";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                //Console.WriteLine("\nResponse:\n");
                //Console.WriteLine(JsonPrettyPrint(contentString));
                //Console.WriteLine("\nPress Enter to exit...");
            }
        }


        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }

    }

}
