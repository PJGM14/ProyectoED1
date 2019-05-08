using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.IO;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Models
{
    public class Editor
    {
        //Diccionario de palabras reservadas que puede ser modificado en cualquier
        //momento de la ejecución del programa
        public Dictionary<string,string> PalabrasReservadas { get; set; }

        //Propiedad que va guardar todo el texto que se escriba en pantalla (instrucciones de ejecución)
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "No hay instrucciones para ser ejecutadas")]
        public string TextoPantalla { get; set; }

        //Esta lista contiene los tipos de datos que son permitidos en el programa
        public List<string> TipoDato { get; set; }

        //Diccionario que tiene el contenido de las tablas
        //               <nombreTabla,(nombreColumna,TipoDato)>
        public Dictionary<string,Dictionary<string,string>> ContenidoTablas { get; set; }

        public Editor()
        {
            PalabrasReservadas = new Dictionary<string, string>();
            TextoPantalla = "";

            TipoDato = new List<string>();

            TipoDato.Add("INT");
            TipoDato.Add("PRIMARY");
            TipoDato.Add("KEY");
            TipoDato.Add("VARCHAR[100]");
            TipoDato.Add("DATETIME");

            ContenidoTablas = new Dictionary<string, Dictionary<string, string>>();
        }

        //Método que obtiene la información de cada tabla que está en la carpeta de ~MicroSQL/Tablas
        public void ObtenerTablas()         
        {
            //Limpia el diccionario que contiene la info de las tablas
            ContenidoTablas.Clear();

            //Lista que contiene las direcciones de cada archivo tabla para poder leer los archivos que están en la carpeta
            List<string> direccionesArchivo = new List<string>();

            //Obtiene todos los archivos de la carpeta
            var ubicacion = Directory.GetFiles(Data.Instancia.PathDirectorio + "\\Tablas");

            foreach (var path in ubicacion)
            {
                direccionesArchivo.Add(path);
            }

            if (direccionesArchivo.Count != 0)
            {
                foreach (var nombre in direccionesArchivo)
                {
                    var texto = File.ReadAllText(nombre);

                    if (!texto.IsNullOrWhiteSpace())
                    {
                        var lineas = File.ReadAllLines(nombre);

                        var listaColumnas = new Dictionary<string, string>();

                        var nombreTabla = Path.GetFileName(nombre).Split('.')[0];

                        ContenidoTablas.Add(nombreTabla,listaColumnas);

                        foreach (var linea in lineas)
                        {
                            var campoLinea = linea.Split(',');

                            ContenidoTablas[nombreTabla].Add(campoLinea[0],campoLinea[1]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

    }
}