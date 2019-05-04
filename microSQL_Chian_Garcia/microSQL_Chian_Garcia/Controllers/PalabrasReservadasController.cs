using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Controllers
{
    public class PalabrasReservadasController : Controller
    {
        //Muestra el diccionario de palabras reservadas
        public ActionResult TablaPalabras()
        {
            if (Data.Instancia.Ingreso != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Editor");
            }
        }

        // GET: PalabrasReservadas/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.PalabraEditar = Data.Instancia.EditorTexto.PalabrasReservadas[id];
            return View();
        }

        // POST: PalabrasReservadas/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                Data.Instancia.EditorTexto.PalabrasReservadas[id] = collection["NuevaPalabra"];
                Data.Instancia.ArchivoReservadas.EscribirArchivoPalabrasReservadas(Data.Instancia.PathDirectorio);

                return RedirectToAction("TablaPalabras");
            }
            catch
            {
                return View();
            }
        }
    }
}
