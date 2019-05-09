using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Estructuras.NoLinearStructures.Trees.Arbol_B;

namespace microSQL_Chian_Garcia.Models
{
    public class FabricaRegistro : IFabricaTextoTamañoFijo<Registro>
    {
        public Registro Fabricar(string textoTamañoFijo)
        {
            var registro = new Registro();
            var datos = textoTamañoFijo.Split('~');

            registro.Identificador = Int32.Parse(datos[0].Trim());
            registro.Entero1 = Int32.Parse(datos[1].Trim());
            registro.Entero2 = Int32.Parse(datos[2].Trim());
            registro.Entero3 = Int32.Parse(datos[3].Trim());

            registro.Cadena1 = datos[4].Trim();
            registro.Cadena2 = datos[5].Trim();
            registro.Cadena3 = datos[6].Trim();

            registro.Tiempo1 = datos[7].Trim(); ;
            registro.Tiempo1 = datos[8].Trim(); ;
            registro.Tiempo1 = datos[9].Trim(); ;

            return registro;
        }

        public Registro FabricarObtenido(string textoTamañoFijo)
        {
            var registro = new Registro();
            var datos = textoTamañoFijo.Split('~');

            registro.Identificador = Int32.Parse(datos[0].Trim());
            registro.Entero1 = Int32.Parse(datos[1].Trim());
            registro.Entero2 = Int32.Parse(datos[2].Trim());
            registro.Entero3 = Int32.Parse(datos[3].Trim());

            registro.Cadena1 = datos[4].Trim('$');
            registro.Cadena2 = datos[5].Trim('$');
            registro.Cadena3 = datos[6].Trim('$');

            registro.Tiempo1 = datos[7].Trim('$');
            registro.Tiempo1 = datos[8].Trim('$');
            registro.Tiempo1 = datos[9].Trim('$');

            return registro;
        }

        public Registro FabricarNulo()
        {
            return new Registro();
        }
    }
}