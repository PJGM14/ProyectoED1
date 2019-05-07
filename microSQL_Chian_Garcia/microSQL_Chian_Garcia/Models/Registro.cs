using System;
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

        //15 caracteres
        public DateTime Tiempo1 { get; set; }
        public DateTime Tiempo2 { get; set; }
        public DateTime Tiempo3 { get; set; }

        //355 de datos + 19 separadores
        private const string FormatoConst = "";

        public int FixedSizeText
        {
            get { return 364; }
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

            Tiempo1 = Convert.ToDateTime("00/00/00");
            Tiempo2 = Convert.ToDateTime("00/00/00");
            Tiempo3 = Convert.ToDateTime("00/00/00");
        }

        public Registro(int identificador, int entero1, int entero2, int entero3, string cadena1, string cadena2, string cadena3, DateTime tiempo1, DateTime tiempo2, DateTime tiempo3)
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
            sb.Append('¬');
            sb.Append(Entero1.ToString().PadLeft(10, '0'));
            sb.Append('¬');
            sb.Append(Entero2.ToString().PadLeft(10, '0'));
            sb.Append('¬');
            sb.Append(Entero3.ToString().PadLeft(10, '0'));
            sb.Append('¬');

            sb.Append(Cadena1.PadLeft(100, '$'));
            sb.Append('¬');
            sb.Append(Cadena2.PadLeft(100, '$'));
            sb.Append('¬');
            sb.Append(Cadena3.PadLeft(100, '$'));
            sb.Append('¬');

            sb.Append(Tiempo1.ToString("d").PadLeft(15, '0'));
            sb.Append('¬');
            sb.Append(Tiempo2.ToString("d").PadLeft(15, '0'));
            sb.Append('¬');
            sb.Append(Tiempo3.ToString("d").PadLeft(15, '0'));

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToFixedSizeString();
        }
    }
}