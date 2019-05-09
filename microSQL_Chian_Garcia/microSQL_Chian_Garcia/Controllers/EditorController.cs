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
            //Data.Instancia.AlumnoTree = new ArbolB<EjemploAlumno>(5, Data.Instancia.PathDirectorio + "\\ArbolesB\\Alumnos.txt", new FabricaEjemploAlumno());

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

                //diccionario de prueba para CREAR TABLA
                var dicPruebaTablas = new Dictionary<string,string>();
                dicPruebaTablas.Add("MOUSE","NOMBRE");
                dicPruebaTablas.Add("ID", "INT PRIMARY KEY");
                dicPruebaTablas.Add("MARCA", "VARCHAR[100]");
                dicPruebaTablas.Add("COLOR", "VARCHAR[100]");
                Data.Instancia.EditorTexto.CrearTabla(dicPruebaTablas);

                //PRUEBA DE METER DATOS EN UNA TABLA
                Data.Instancia.EditorTexto.MandarDato();
                
                //PRUEBA PARA OBTENER UN OBJETO DE LA TABLA CON CIERTO ID
                var obj = Data.Instancia.EditorTexto.ObtenerDato();
                Data.Instancia.TreeResgitro.Cerrar(); //DESPUES DE OBTENER SIEMPRE MANDAR A CERRAR EL ARCHVIO
                var fabricar = new FabricaRegistro();
                var registro = fabricar.FabricarObtenido(obj.ToString());

                //LISTA PARA PROBAR LA INSERCION DE UN OBJETO YA DE ESTE PROYECTO
                var listaInsert = new List<string>();
                listaInsert.Add("MOUSE");
                listaInsert.Add("ID");
                listaInsert.Add("MARCA");
                listaInsert.Add("COLOR");
                listaInsert.Add("3333");
                listaInsert.Add("FERRARI");
                listaInsert.Add("ROJO");
                Data.Instancia.EditorTexto.InsertarEn(listaInsert);
                Data.Instancia.TreeResgitro.Cerrar();

                //SE PRUEBA EL SELECT CON UN OBJETO DE ESTE RPROYECTO
                var listaSelect = new List<string>();

                var listaPropiedades = new List<string>();
                listaSelect.Add("ID");
                listaSelect.Add("MARCA");
                listaSelect.Add("MOUSE");

                var RegistrosSelect = Data.Instancia.EditorTexto.SeleccionarDe(listaSelect, ref listaPropiedades);

                return RedirectToAction("Index"); //Cambiar esto
            }
            catch(Exception e)
            {
                var nom = e.Message;
                return RedirectToAction("Index"); //Cambiar esto
            }
        }
    }
}
