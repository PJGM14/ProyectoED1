using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Estructuras.NoLinearStructures.Trees.Arbol_B;
using microSQL_Chian_Garcia.Models;

namespace microSQL_Chian_Garcia.Instancia
{
    public class Data
    {
        private static Data _instancia = null;

        public static Data Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Data();
                }

                return _instancia;
            }
        }

        public string PathDirectorio = "";
        public int Ingreso = 0;
        public ManejoArchivoReservadas ArchivoReservadas = new ManejoArchivoReservadas();
        public Editor EditorTexto = new Editor();

        //Para probar el arbol
        public ArbolB<Registro> TreeResgitro;
    }
}