using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.IO;
using Estructuras.NoLinearStructures.Trees.Arbol_B;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Models
{
    public class Editor
    {
        //Diccionario de palabras reservadas que puede ser modificado en cualquier
        //momento de la ejecución del programa
        public Dictionary<string, string> PalabrasReservadas { get; set; }

        //Propiedad que va guardar todo el texto que se escriba en pantalla (instrucciones de ejecución)
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "No hay instrucciones para ser ejecutadas")]
        public string TextoPantalla { get; set; }

        //Esta lista contiene los tipos de datos que son permitidos en el programa
        public List<string> TipoDato { get; set; }

        //Diccionario que tiene el contenido de las tablas
        //               <nombreTabla,(nombreColumna,TipoDato)>
        public Dictionary<string, Dictionary<string, string>> ContenidoTablas { get; set; }

        public bool EjecucionCorrecta { get; set; }

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

            EjecucionCorrecta = false;
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

                        ContenidoTablas.Add(nombreTabla, listaColumnas);

                        foreach (var linea in lineas)
                        {
                            var campoLinea = linea.Split(',');

                            ContenidoTablas[nombreTabla].Add(campoLinea[0], campoLinea[1]);
                        }
                    }
                }
            }
        }

        public bool CrearTabla(Dictionary<string, string> contenidoCrearTabla)
        {
            var nombre = "";
            var detener = false;

            var dicContenido = new Dictionary<string, string>();

            foreach (var item in contenidoCrearTabla)
            {
                if (item.Value == "NOMBRE")
                {
                    nombre = item.Key;

                    if (File.Exists(Data.Instancia.PathDirectorio + "\\Tablas\\" + nombre + ".tabla"))
                    {
                        EjecucionCorrecta = false;
                        detener = true;
                        break;
                    }
                }
                else
                {
                    dicContenido.Add(item.Key, item.Value);

                    using (StreamWriter file = new StreamWriter(Data.Instancia.PathDirectorio + "\\Tablas\\" + nombre + ".tabla", true))
                    {
                        file.WriteLine(item.Key + "," + item.Value);
                    }
                }
            }

            if (detener == false)
            {
                Data.Instancia.EditorTexto.ContenidoTablas.Add(nombre, dicContenido);

                var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombre + ".arbolB";
                Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());
                Data.Instancia.TreeResgitro.Cerrar();

                EjecucionCorrecta = true;
                ObtenerTablas();
            }

            return EjecucionCorrecta;
        }


        public bool InsertarEn(List<string> listaDatos)
        {
            var nombreTabla = listaDatos[0];

            if (!File.Exists(Data.Instancia.PathDirectorio + "Tablas\\" + listaDatos[0] + ".tabla"))
            {
                listaDatos.RemoveAt(0);

                var listaNombresColumnas = new List<string>();
                var listaTipoDato = new List<string>();

                foreach (var item in ContenidoTablas)
                {
                    foreach (var contenido in item.Value)
                    {
                        listaNombresColumnas.Add(contenido.Key);
                        listaTipoDato.Add(contenido.Value);
                    }
                }

                int Identificador = 0;
                int entero1 = 0;
                int entero2 = 0;
                int entero3 = 0;

                string cadena1 = "";
                string cadena2 = "";
                string cadena3 = "";

                string Fecha1 = "";
                string Fecha2 = "";
                string Fecha3 = "";

                var cantidadColumnas = listaDatos.Count / 2;

                if (listaDatos.Count % 2 == 0)
                {
                    for (int i = 0; i < cantidadColumnas; i++)
                    {
                        if (listaDatos[i] == listaNombresColumnas[i])
                        {

                        }
                        else
                        {
                            throw new Exception("No ha ingresado las columnas en el orden que fue creada la tabla " + nombreTabla);
                        }
                    }

                    for (int i = cantidadColumnas; i < listaDatos.Count; i++)
                    {

                        if (Int32.TryParse(listaDatos[i], out int result))
                        {
                            if (Identificador == 0)
                            {
                                Identificador = result;
                            }
                            else if (entero1 == 0)
                            {
                                entero1 = result;
                            }
                            else if (entero2 == 0)
                            {
                                entero2 = result;
                            }
                            else
                            {
                                entero3 = result;
                            }
                        }
                        else if (DateTime.TryParse(listaDatos[i],out DateTime fecha))
                        {
                            if (Fecha1 == "")
                            {
                                Fecha1 = listaDatos[i];
                            }
                            else if (Fecha2 == "")
                            {
                                Fecha2 = listaDatos[i];
                            }
                            else
                            {
                                Fecha3 = listaDatos[i];
                            }
                        }
                        else
                        {
                            if (cadena1 == "")
                            {
                                cadena1 = listaDatos[i];
                            }
                            else if( cadena2 == "")
                            {
                                cadena2 = listaDatos[i];
                            }
                            else
                            {
                                cadena3 = listaDatos[i];
                            }
                        }
                    }
                    EjecucionCorrecta = true;

                    var NuevoRegistro = new Registro(Identificador,entero1,entero2,entero3,cadena1,cadena2,cadena3,Fecha1,Fecha2,Fecha3);

                    var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4,path,new FabricaRegistro());
                    Data.Instancia.TreeResgitro.Agregar(NuevoRegistro.Identificador.ToString().Trim('x'), NuevoRegistro,"");
                }
                else
                {
                    throw new Exception("Ha ingresado más valores que columnas o más columnas que datos");
                }
            }
            else
            {
                EjecucionCorrecta = false;
            }

            return EjecucionCorrecta;
        }

        public List<Registro> SeleccionarDe(List<string> contenidoSeleccionar, ref List<string> PropiedadMostrar)
        {
            var listaDatos = new List<Registro>();
            var con = contenidoSeleccionar.Count;
            var nombreTabla = "";
            var path = "";

            if (ContenidoTablas.ContainsKey(contenidoSeleccionar[con-1]))
            {
                nombreTabla = contenidoSeleccionar[con - 1];
                contenidoSeleccionar.RemoveAt(con-1); //SE ELIMINA EL NOMBRE DE LA TABLA DE LA LISTA

                if (contenidoSeleccionar[0] == "*")
                {
                    path = Data.Instancia.PathDirectorio + "\\ArbolesB\\"+ nombreTabla+".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4,path,new FabricaRegistro());

                    foreach (var item in Data.Instancia.TreeResgitro.RecorrerPreOrden())
                    {
                        var fabricar = new FabricaRegistro();
                        var registro = fabricar.FabricarObtenido(item);

                        listaDatos.Add(registro);
                    }
                    PropiedadMostrar.Add("");
                    Data.Instancia.TreeResgitro.Cerrar();
                }
                else
                {
                    path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                    foreach (var item in Data.Instancia.TreeResgitro.RecorrerPreOrden())
                    {
                        var fabricar = new FabricaRegistro();
                        var registro = fabricar.FabricarObtenido(item);

                        listaDatos.Add(registro);
                    }

                    foreach (var paraMostrar in contenidoSeleccionar)
                    {
                        PropiedadMostrar.Add(paraMostrar);
                    }

                    Data.Instancia.TreeResgitro.Cerrar();
                }
            }
            else if(Int32.TryParse(contenidoSeleccionar[con-1],out int result) && ContenidoTablas.ContainsKey(contenidoSeleccionar[con-2]))
            {
                var llave = result.ToString();

                contenidoSeleccionar.RemoveAt(con - 1);
                con = contenidoSeleccionar.Count;

                nombreTabla = contenidoSeleccionar[con - 1];

                path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                listaDatos.Add(Data.Instancia.TreeResgitro.Obtener(llave));
                Data.Instancia.TreeResgitro.Cerrar();
            }
            else
            {
                Data.Instancia.TreeResgitro.Cerrar();
                //TABLA NO EXISTE
            }

            return listaDatos;
        }


        //DE EJEMPLOOOOO---------------------------------------------------------------------
        public void MandarDato()
        {
            //OBJ
            var prueba = new Registro(12, 40, 0, 0, "", "Hola", "Que", "15/02/2019", "", "");

            var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\MOUSE.arbolB";

            Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());
            Data.Instancia.TreeResgitro.Agregar(prueba.Identificador.ToString().Trim('x'), prueba, "");

            var prueba2 = new Registro(13, 43434, 34344, 3434, "Com", "pu", "tadora", "25/02/2909", "", "");
            Data.Instancia.TreeResgitro.Agregar(prueba2.Identificador.ToString().Trim('x'), prueba2, "");
            Data.Instancia.TreeResgitro.Cerrar();
        }

        public object ObtenerDato()
        {
            var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\MOUSE.arbolB";

            Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());
            return Data.Instancia.TreeResgitro.Obtener("12");
        }
    }
}