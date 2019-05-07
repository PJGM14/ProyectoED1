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
            //Manda en una variable temporal a la vista la palabra reservada en lenguaje propio a editar
            ViewBag.PalabraEditar = Data.Instancia.EditorTexto.PalabrasReservadas[id];
            return View();
        }

        // POST: PalabrasReservadas/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                //Verifica si la palabra nueva existe o no en lenguaje original o en el lenguaje propio
                if ((!Data.Instancia.EditorTexto.PalabrasReservadas.ContainsValue(collection["NuevaPalabra"].ToUpper())) && (!Data.Instancia.EditorTexto.PalabrasReservadas.ContainsKey(collection["NuevaPalabra"].ToUpper())))
                {
                    Data.Instancia.EditorTexto.PalabrasReservadas[id] = collection["NuevaPalabra"].ToUpper();
                    Data.Instancia.ArchivoReservadas.EscribirArchivoPalabrasReservadas(Data.Instancia.PathDirectorio);
                    return RedirectToAction("TablaPalabras");
                }
                else
                {
                    return RedirectToAction("Edit");
                }
                
            }
            catch
            {
                return View();
            }
        }

        //Para regresar a las palabras reservadas originales
        public ActionResult RegresarDefault()
        {
            Data.Instancia.ArchivoReservadas.PalabrasReservadasDefault();
            Data.Instancia.ArchivoReservadas.EscribirArchivoPalabrasReservadas(Data.Instancia.PathDirectorio);
            return RedirectToAction("TablaPalabras");
        }
    }
}