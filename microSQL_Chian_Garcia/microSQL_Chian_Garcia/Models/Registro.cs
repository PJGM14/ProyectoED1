﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Estructuras.NoLinearStructures.Trees.Arbol_B;

namespace microSQL_Chian_Garcia.Models
{
    public class Registro : ITextoTamañoFijo
    {
        //10 caracteres
        public int Identificador { get; set; }
        public int Entero1 { get; set; }
        public int Entero2 { get; set; }
        public int Entero3 { get; set; }

        //Como máximo 100 caracteres
        public string Cadena1 { get; set; }
        public string Cadena2 { get; set; }
        public string Cadena3 { get; set; }

        //25 caracteres
        public string Tiempo1 { get; set; }
        public string Tiempo2 { get; set; }
        public string Tiempo3 { get; set; }

        //415 de datos + 9 separadores
        private const string FormatoConst = "";

        public int FixedSizeText
        {
            get { return 424; }
        }

        public Registro()
        {
            Identificador = 0;
            Entero1 = 0;
            Entero2 = 0;
            Entero3 = 0;

            Cadena1 = "";
            Cadena2 = "";
            Cadena3 = "";

            Tiempo1 = "";
            Tiempo2 = "";
            Tiempo3 = "";
        }

        public Registro(int identificador, int entero1, int entero2, int entero3, string cadena1, string cadena2, string cadena3, string tiempo1, string tiempo2, string tiempo3)
        {
            Identificador = identificador;
            Entero1 = entero1;
            Entero2 = entero2;
            Entero3 = entero3;

            Cadena1 = cadena1;
            Cadena2 = cadena2;
            Cadena3 = cadena3;

            Tiempo1 = tiempo1;
            Tiempo2 = tiempo2;
            Tiempo3 = tiempo3;
        }

        public string ToFixedSizeString()
        {
            var sb = new StringBuilder();
            sb.Append(Identificador.ToString().PadLeft(10, '0'));
            sb.Append('~');
            sb.Append(Entero1.ToString().PadLeft(10, '0'));
            sb.Append('~');
            sb.Append(Entero2.ToString().PadLeft(10, '0'));
            sb.Append('~');
            sb.Append(Entero3.ToString().PadLeft(10, '0'));
            sb.Append('~');

            sb.Append(Cadena1.PadLeft(100, '$'));
            sb.Append('~');
            sb.Append(Cadena2.PadLeft(100, '$'));
            sb.Append('~');
            sb.Append(Cadena3.PadLeft(100, '$'));
            sb.Append('~');

            sb.Append(Tiempo1.PadLeft(25, '$'));
            sb.Append('~');
            sb.Append(Tiempo2.PadLeft(25, '$'));
            sb.Append('~');
            sb.Append(Tiempo3.PadLeft(25, '$'));

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToFixedSizeString();
        }
    }
}