using System;
using System.Collections.Generic;
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PalabrasReservadas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
