using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estructuras.NoLinearStructures.Trees.Arbol_B;
using microSQL_Chian_Garcia.Instancia;
using microSQL_Chian_Garcia.Models;

namespace microSQL_Chian_Garcia.Controllers
{
    public class EjemploAlumnoController : Controller
    {
        // GET: EjemploAlumno
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }

        // GET: EjemploAlumno/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToAction("Create");
        }

        // GET: EjemploAlumno/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EjemploAlumno/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                var Alumno = new EjemploAlumno(collection["Nombre"],long.Parse(collection["NumTelefono"]));


                //Data.Instancia.AlumnoTree.Agregar(Alumno.Nombre.Trim('x'),Alumno,"");
                //Data.Instancia.AlumnoTree.Cerrar();

                return RedirectToAction("Index","Editor");
            }
            catch
            {
                return View();
            }
        }

        // GET: EjemploAlumno/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Create");
        }

        // POST: EjemploAlumno/Edit/5
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
                return RedirectToAction("Create");
            }
        }

        // GET: EjemploAlumno/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Create");
        }

        // POST: EjemploAlumno/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }
    }
}
