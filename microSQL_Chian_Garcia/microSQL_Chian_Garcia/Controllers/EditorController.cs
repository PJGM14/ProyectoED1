using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Controllers
{
    public class EditorController : Controller
    {
        //Es el primero que se carga
        public ActionResult Index()
        {
            Data.Instancia.Ingreso++; //Sirve solamente para la vista para saber si muestra ventana emergente de carga exitosa
            if (Data.Instancia.Ingreso == 1)
            {
                var path = Server.MapPath("~\\MicroSQL"); //Se obtiene la direccion del equipo y de la carpeta del proyecto, luego se le agrega la carpeta
                Data.Instancia.ArchivoReservadas.VerificarArchivoPalabrasReservadas(path); //Manda a verificar si existe un archivo inicial para leer
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
