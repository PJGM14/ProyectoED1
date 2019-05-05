using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using Estructuras.NoLinearStructures.Trees.Arbol_B;

namespace microSQL_Chian_Garcia.Models
{
    public class EjemploAlumno : ITextoTamañoFijo
    {
        [Display(Name = "Nombre de alumno")]
        [Required(ErrorMessage = "No hay instrucciones para ser ejecutadas")]
        public string Nombre { get; set; }

        [Display(Name = "Número de Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public long NumTelefono { get; set; }

        private const string FormatoConst = "xxxxxxxxxxxxxxxxxxxx-00000000000000000000"; //41 caracteres

        public int FixedSizeText
        {
            get { return 41; }
        }

        public EjemploAlumno()
        {
            Nombre = "";
            NumTelefono = 0;
        }

        public EjemploAlumno( string nombre, long numTel)
        {
            Nombre = nombre;
            NumTelefono = numTel;
        }

        public string ToFixedSizeString()
        {
            var sb = new StringBuilder();
            sb.Append(Nombre.PadLeft(20, 'x'));
            sb.Append('-');
            sb.Append(NumTelefono.ToString().PadLeft(20, '0'));

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToFixedSizeString();
        }
    }
}