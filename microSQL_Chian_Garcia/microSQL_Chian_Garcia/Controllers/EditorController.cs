using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Estructuras.NoLinearStructures.Trees.Arbol_B;
using microSQL_Chian_Garcia.Instancia;
using microSQL_Chian_Garcia.Models;

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
                var path = Server.MapPath("~\\MicroSQL"); //Se obtiene la direccion del equipo y de la carpeta del proyecto, luego se le agrega la carpeta que tendrá que buscar
                Data.Instancia.PathDirectorio = path;
                Data.Instancia.ArchivoReservadas.VerificarArchivoPalabrasReservadas(path); //Manda a verificar si existe un archivo inicial para leer
                Data.Instancia.EditorTexto.ObtenerTablas();
            }

            //Para prueba
            Data.Instancia.AlumnoTree = new ArbolB<EjemploAlumno>(5, Data.Instancia.PathDirectorio + "\\ArbolesB\\Alumnos.txt", new FabricaEjemploAlumno());

            return View();
        }

        // POST: Editor/Create
        [HttpPost]
        public ActionResult Ejecutar(FormCollection collection)
        {
            try
            {
                Data.Instancia.EditorTexto.TextoPantalla = collection["TextoPantalla"];

                //AQUI MANDAR A LLAMAR ALGUN MÉTODO QUE DEVUELVA ALGÚN BOOL PARA VER A QUE VISTA SE MANDA

                //EL MÉTODO DEBERÍA DE ESTAR EN ALGÚN MODELO QUE HAGA LAS VERIFICACIONES, O BIEN EN EL MODELO EDITOR QUE NO POSEE NINGÚN MÉTODO


                return RedirectToAction("Index"); //Cambiar esto
            }
            catch
            {
                return RedirectToAction("Index"); //Cambiar esto
            }
        }
    }
}
