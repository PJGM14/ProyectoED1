using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Estructuras.NoLinearStructures.Trees.Arbol_B;

namespace microSQL_Chian_Garcia.Models
{
    public class FabricaEjemploAlumno : IFabricaTextoTamañoFijo<EjemploAlumno>
    {
        public EjemploAlumno Fabricar(string textoTamañoFijo)
        {
            EjemploAlumno EjAlumno = new EjemploAlumno();
            var datos = textoTamañoFijo.Split('-');

            EjAlumno.Nombre = datos[0].Trim();
            EjAlumno.NumTelefono = long.Parse(datos[1].Trim());

            return EjAlumno;
        }

        public EjemploAlumno FabricarNulo()
        {
            return new EjemploAlumno();
        }
    }
}