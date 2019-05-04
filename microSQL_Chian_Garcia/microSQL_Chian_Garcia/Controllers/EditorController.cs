using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index()
        {
            Data.Instancia.Ingreso++;
            if (Data.Instancia.Ingreso == 1)
            {
                var path = Server.MapPath("~\\MicroSQL");
                Data.Instancia.ArchivoReservadas.VerificarArchivoPalabrasReservadas(path);
            }
            
            return View();
        }

        // POST: Editor/Create
        [HttpPost]
        public ActionResult Ejecutar(FormCollection collection)
        {
            try
            {
                var texto = collection["TextoPantalla"];

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
