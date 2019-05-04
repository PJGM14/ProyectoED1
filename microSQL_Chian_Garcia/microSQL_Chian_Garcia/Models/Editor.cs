﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.IO;

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

        public Editor()
        {
            PalabrasReservadas = new Dictionary<string, string>();
            TextoPantalla = "";
        }

        public bool ModificarDiccionario(string rutaArchivo, ref string errorLinea)
        {
            var edicionCorrecta = false;

            var csvData = File.ReadAllText(rutaArchivo);

            var conFila = 0;

            foreach (var fila in csvData.Split('\n'))
            {
                var _fila = fila.Trim();

                if (!string.IsNullOrEmpty(_fila))
                {
                    conFila++;

                    var matrizDatos = _fila.Split(',');

                    if (matrizDatos.Length > 2)
                    {
                        errorLinea = conFila.ToString();
                        edicionCorrecta = false;
                    }
                    else
                    {
                        PalabrasReservadas.Add(matrizDatos[0],matrizDatos[1]);
                        edicionCorrecta = true;
                    }
                }
            }
            return edicionCorrecta;
        }
    }
}