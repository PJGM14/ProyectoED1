﻿using System;
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

        public List<string> TablasMostrar = new List<string>();

        public List<Registro> ContenidoMostrar;

        public List<string> PropiedadesMostrar;

        public int contadorMostrar = 0;

        //CONSTRUCTOR
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

            ContenidoMostrar = new List<Registro>();
            PropiedadesMostrar = new List<string>();
        }

        //*********************************************************************************************************************************************

        public void Read(string code)
        {
            Dictionary<string, string> crearTablaValores = new Dictionary<string, string>();
            List<string> insertarEnTablaValores = new List<string>();
            List<string> seleccionarDeTabla = new List<string>();
            List<string> borrarDeTabla = new List<string>();
            string borrarTabla;

            code = code.ToUpper();
            code = code.Trim();

            //SE REEMPLAZA TODO EL CODIGO PARA QUE QUEDE EN UNA SOLA LINEA
            code = code.Replace("=", " = ");
            code = code.Replace(",", ", ");
            code = code.Replace("\n", " ");
            code = code.Replace("(", " ( ");
            code = code.Replace(")", " ) ");
            code = code.Replace("\r", "");
            code = code.Replace("  ", " ");
            code = code.Replace("VARCHAR ( 100 ) ", "VARCHAR(100)");

            try
            {
                //SI CONTIENE UN GO O EL EQUIVALENTE EN OTRO IDIOMA
                if (code.Contains("GO") || code.Contains(PalabrasReservadas["GO"]))
                {
                    //SEPARA EL CODIGO POR INSTRUCCIONES
                    var codeInstructions = code.Split(new string[] { "GO" }, StringSplitOptions.None);

                    if (codeInstructions[codeInstructions.Length - 1].Trim().Equals(""))
                    {
                        for (int i = 0; i < codeInstructions.Length - 1; i++)
                        {
                            //SEPARA CADA INSRUCCION EN PALABRAS
                            codeInstructions[i] = codeInstructions[i].Trim();
                            var instructionWords = codeInstructions[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            int longitud = instructionWords.Length;

                            //***********************************SE VALIDA A QUE INSTRUCCION PERTENECE************************************
                            //CREATE TABLE
                            if ((instructionWords[0] + " " + instructionWords[1]).Equals("CREATE TABLE") || (instructionWords[0] + " " + instructionWords[1]).Equals(PalabrasReservadas["CREATE TABLE"]))
                            {
                                crearTablaValores.Clear();
                                CreateTable(ref crearTablaValores, ref instructionWords, longitud);

                                EjecucionCorrecta = CrearTabla(crearTablaValores);

                            }

                            //INSERT INTO
                            if ((instructionWords[0] + " " + instructionWords[1]).Equals("INSERT INTO") || (instructionWords[0] + " " + instructionWords[1]).Equals(PalabrasReservadas["INSERT INTO"]))
                            {
                                insertarEnTablaValores.Clear();
                                InsertInto(ref insertarEnTablaValores, ref instructionWords, longitud);

                                EjecucionCorrecta = InsertarEn(insertarEnTablaValores);
                            }

                            //SELECT FROM
                            if (instructionWords[0].Equals("SELECT") || instructionWords[0].Equals(PalabrasReservadas["SELECT"]))
                            {
                                seleccionarDeTabla.Clear();
                                SelectFrom(ref seleccionarDeTabla, ref instructionWords, longitud);
                                Data.Instancia.EditorTexto.contadorMostrar = 0;
                                SeleccionarDe(seleccionarDeTabla, ref PropiedadesMostrar);
                            }

                            //DELETE FROM
                            if (instructionWords[0].Equals("DELETE") || instructionWords[0].Equals(PalabrasReservadas["DELETE"]))
                            {
                                borrarTabla = "";
                                if (instructionWords[1].Equals("FROM") || instructionWords[1].Equals(PalabrasReservadas["FROM"]))
                                {
                                    DeleteFrom(ref borrarDeTabla, ref instructionWords, longitud);
                                    EliminarDe(borrarDeTabla);
                                }

                            }

                            //DROP TABLE
                            if ((instructionWords[0] + " " + instructionWords[1]).Equals("DROP TABLE") || (instructionWords[0] + " " + instructionWords[1]).Equals(PalabrasReservadas["DROP TABLE"]))
                            {
                                borrarTabla = "";
                                borrarTabla = instructionWords[2];
                                Drop(borrarTabla);
                            }
                        }
                    }
                    else
                    {
                        //LE FALTA EL ULTIMO GO
                    }


                }
                else
                {
                    //ERROR PORQUE NO HAY GO's

                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }



        }

        //******************METODO PARA ANALIZAR LAS INSTRUCCIONES PARA CREAR TABLAS*******************************************
        public void CreateTable(ref Dictionary<string, string> crearTablaValores, ref string[] instructionWords, int longitud)
        {
            if (instructionWords[longitud - 1].Equals(")"))
            {
                int cantVar = 0;
                int cantInt = 0;
                int cantDate = 0;

                crearTablaValores.Add(instructionWords[2].ToString(), "NOMBRE");

                if (instructionWords[3].Equals("("))
                {
                    if (instructionWords[4].Equals("ID"))
                    {
                        if ((instructionWords[5] + instructionWords[6] + instructionWords[7]).Equals("INTPRIMARYKEY,"))
                        {
                            crearTablaValores.Add("ID", "INT PRIMARY KEY");

                            var posicion = 8;
                            while (instructionWords[posicion] != ")")
                            {
                                if (posicion == (longitud - 3))
                                {
                                    if (instructionWords[posicion + 1].Equals("INT"))
                                    {
                                        if (cantInt <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "INT");
                                            cantInt++;
                                        }
                                    }
                                    if (instructionWords[posicion + 1].Equals("VARCHAR(100)"))
                                    {
                                        if (cantVar <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "VARCHAR(100)");
                                            cantVar++;
                                        }
                                    }
                                    if (instructionWords[posicion + 1].Equals("DATETIME"))
                                    {
                                        if (cantDate <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "DATETIME");
                                            cantDate++;
                                        }
                                    }
                                    posicion = longitud - 1;
                                }
                                else
                                {
                                    if (instructionWords[posicion + 1].Equals("INT,"))
                                    {
                                        if (cantInt <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "INT");
                                            cantInt++;
                                        }
                                    }
                                    if (instructionWords[posicion + 1].Equals("VARCHAR(100),"))
                                    {
                                        if (cantVar <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "VARCHAR(100)");
                                            cantVar++;
                                        }
                                    }
                                    if (instructionWords[posicion + 1].Equals("DATETIME,"))
                                    {
                                        if (cantDate <= 3)
                                        {
                                            crearTablaValores.Add(instructionWords[posicion], "DATETIME");
                                            cantDate++;
                                        }
                                    }
                                    posicion += 2;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ESTA MAL ESCRITO
                    }
                }
                else
                {
                    //ESTA MAL ESCRITO LE FALTA EL PARENTESIS DE APERTURA
                }
            }
            else
            {
                //ESTA MAL ESCRITO LE FALTA EL PARENTESIS DE CIERRE
            }
        }

        //*****************METODO PARA ANALIZAR LAS INSTRUCCIONES PARA INSERTAR EN TABLAS**************************************
        public void InsertInto(ref List<string> insertarEnTablaValores, ref string[] instructionWords, int longitud)
        {
            if (instructionWords[longitud - 1].Equals(")"))
            {
                insertarEnTablaValores.Add(instructionWords[2].ToString());

                if (instructionWords[3].Equals("("))
                {
                    if (instructionWords[4].Equals("ID,"))
                    {
                        insertarEnTablaValores.Add("ID");

                        int posicion = 5;

                        while (instructionWords[posicion] != ")")
                        {
                            string[] sinComa = instructionWords[posicion].Split(',');
                            insertarEnTablaValores.Add(sinComa[0]);
                            posicion++;
                        }

                        posicion++;

                        if (instructionWords[posicion].Equals("VALUES") || instructionWords[posicion].Equals(PalabrasReservadas["VALUES"]))
                        {
                            posicion++;

                            if (instructionWords[posicion].Equals("("))
                            {
                                posicion++;

                                while (instructionWords[posicion] != ")")
                                {
                                    string[] sinComa = instructionWords[posicion].Split(',');

                                    if (sinComa[0].Contains("'"))
                                    {
                                        string[] sinComillas = sinComa[0].Split('\'');

                                        if (sinComillas.Length == 3)
                                        {
                                            insertarEnTablaValores.Add(sinComillas[1]);
                                        }
                                        else
                                        {
                                            //FALTA UNA COMILLA
                                        }
                                    }
                                    else
                                    {
                                        insertarEnTablaValores.Add(sinComa[0]);
                                    }

                                    posicion++;
                                }
                            }
                            else
                            {
                                //FALTA PARENTESIS DE APERTURA
                            }
                        }
                        else
                        {
                            //FALTA LA PALABRA RESERVADA PARA INSERTAR VALORES
                        }
                    }
                    else
                    {
                        //FALTAN CAMPOS PARA INSERTAR LA INFORMACION
                    }
                }
                else
                {
                    //FALTA PARENTESIS DE APERTURA
                }
            }
            else
            {
                //FALTA PARENTESIS DE CIERRE
            }
        }

        //*****************METODO PARA ANALIZAR LAS INSTRUCCIONES PARA SELECCIONAR INFORMACION*********************************
        public void SelectFrom(ref List<string> seleccionarDeTabla, ref string[] instructionWords, int longitud)
        {
            int posicion = 1;
            bool contieneFrom = false;
            bool contieneWhere = false;

            for (int i = 0; i < longitud; i++)
            {
                if (instructionWords[i].Contains("FROM") || instructionWords[i].Contains(PalabrasReservadas["FROM"]))
                {
                    contieneFrom = true;
                }
            }
            if (contieneFrom)
            {
                while (instructionWords[posicion] != "FROM" || instructionWords[posicion] != PalabrasReservadas["FROM"])
                {
                    if (instructionWords[posicion].Contains(","))
                    {
                        string[] palabraSinComa = instructionWords[posicion].Split(',');
                        seleccionarDeTabla.Add(palabraSinComa[0]);
                        posicion++;
                    }
                    else
                    {
                        seleccionarDeTabla.Add(instructionWords[posicion]);
                        posicion++;
                    }
                }
                seleccionarDeTabla.Add(instructionWords[++posicion]);

                if (posicion < longitud)
                {
                    for (int i = ++posicion; i < longitud; i++)
                    {
                        if (instructionWords[i].Contains("WHERE") || instructionWords[i].Contains(PalabrasReservadas["WHERE"]))
                        {
                            contieneWhere = true;
                        }
                    }
                    if (contieneWhere)
                    {
                        seleccionarDeTabla.Add(instructionWords[longitud - 1]);
                    }
                }
            }
            else
            {
                //NO CONTIENE LA PALABRA FROM, POR LO TANTO NO ESTA BIEN ESCRITO
            }

        }

        //*****************METODO PARA ANALIZAR LAS INSTRUCCIONES PARA BORRAR INFORMACION**************************************
        public void DeleteFrom(ref List<string> borrarDeTabla, ref string[] instructionWords, int longitud)
        {
            bool contieneWhere = false;
            borrarDeTabla.Add(instructionWords[2]);

            for (int i = 0; i < instructionWords.Length; i++)
            {
                if (instructionWords[i].Equals("WHERE") || instructionWords[i].Equals(PalabrasReservadas["WHERE"]))
                {
                    contieneWhere = true;
                }
            }
            if (contieneWhere)
            {
                borrarDeTabla.Add(instructionWords[longitud - 1]);
            }
        }

        /// <summary>
        /// .******************************************************************************************************************************************
        /// </summary>
        //Método que obtiene la información de cada tabla que está en la carpeta de ~MicroSQL/Tablas
        public void ObtenerTablas()
        {
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();
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
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();
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
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();
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
                        else if (DateTime.TryParse(listaDatos[i], out DateTime fecha))
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
                            else if (cadena2 == "")
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

                    var NuevoRegistro = new Registro(Identificador, entero1, entero2, entero3, cadena1, cadena2, cadena3, Fecha1, Fecha2, Fecha3);

                    var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());
                    Data.Instancia.TreeResgitro.Agregar(NuevoRegistro.Identificador.ToString().Trim('x'), NuevoRegistro, "");
                    Data.Instancia.TreeResgitro.Cerrar();
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

        public bool SeleccionarDe(List<string> contenidoSeleccionar, ref List<string> PropiedadMostrar)
        {
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();


            var listaDatos = new List<Registro>();
            var con = contenidoSeleccionar.Count;
            var nombreTabla = "";
            var path = "";

            if (ContenidoTablas.ContainsKey(contenidoSeleccionar[con - 1]))
            {
                nombreTabla = contenidoSeleccionar[con - 1];
                contenidoSeleccionar.RemoveAt(con - 1); //SE ELIMINA EL NOMBRE DE LA TABLA DE LA LISTA

                if (contenidoSeleccionar[0] == "*")
                {
                    path = "";
                    path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                    foreach (var item in Data.Instancia.TreeResgitro.RecorrerPreOrden())
                    {
                        var fabricar = new FabricaRegistro();
                        var registro = fabricar.FabricarObtenido(item);

                        listaDatos.Add(registro);
                    }
                    ContenidoMostrar = listaDatos;

                    foreach (var prop in ContenidoTablas[nombreTabla].Keys)
                    {
                        PropiedadMostrar.Add(prop);
                    }

                    TablasMostrar.Add(nombreTabla);
                    Data.Instancia.TreeResgitro.Cerrar();
                }
                else
                {
                    path = "";
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
                    ContenidoMostrar = listaDatos;
                    TablasMostrar.Add(nombreTabla);
                    Data.Instancia.TreeResgitro.Cerrar();
                }
            }
            else if (Int32.TryParse(contenidoSeleccionar[con - 1], out int result) && ContenidoTablas.ContainsKey(contenidoSeleccionar[con - 2]))
            {
                var llave = result.ToString();

                contenidoSeleccionar.RemoveAt(con - 1);
                con = contenidoSeleccionar.Count;

                nombreTabla = contenidoSeleccionar[con - 1];
                contenidoSeleccionar.RemoveAt(con - 1);

                //CONTENIDO PUEDE HABER *
                if (contenidoSeleccionar[0] == "*")
                {
                    path = "";
                    path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                    var objeto = Data.Instancia.TreeResgitro.Obtener(llave);

                    var fabricarObj = new FabricaRegistro();
                    var registro = fabricarObj.FabricarObtenido(objeto.ToString());

                    listaDatos.Add(registro);
                    ContenidoMostrar = listaDatos;

                    foreach (var prop in ContenidoTablas[nombreTabla].Keys)
                    {
                        PropiedadMostrar.Add(prop);
                    }

                    TablasMostrar.Add(nombreTabla);
                    Data.Instancia.TreeResgitro.Cerrar();
                }
                else
                {
                    path = "";
                    path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                    var objeto = Data.Instancia.TreeResgitro.Obtener(llave);

                    var fabricarObj = new FabricaRegistro();
                    var registro = fabricarObj.FabricarObtenido(objeto.ToString());

                    listaDatos.Add(registro);


                    foreach (var prop in contenidoSeleccionar)
                    {
                        PropiedadesMostrar.Add(prop);

                    }

                    ContenidoMostrar = listaDatos;
                    TablasMostrar.Add(nombreTabla);
                    Data.Instancia.TreeResgitro.Cerrar();
                }
            }
            else
            {
                Data.Instancia.TreeResgitro.Cerrar();
                //TABLA NO EXISTE
            }

            return EjecucionCorrecta;
        }

        public bool EliminarDe(List<string> contenidoEliminar)
        {
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();
            var cont = contenidoEliminar.Count;
            var nombreTabla = contenidoEliminar[0];

            var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";

            if (File.Exists(path))
            {
                //DELETE COMPLETO
                if (cont == 1)
                {
                    File.Delete(path);
                    Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());
                    Data.Instancia.TreeResgitro.Cerrar();
                }

                if (cont == 2) //DELETE CON ID
                {
                    if (Int32.TryParse(contenidoEliminar[cont - 1], out int idResult))
                    {
                        var registro = new List<Registro>();

                        Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path, new FabricaRegistro());

                        foreach (var r in Data.Instancia.TreeResgitro.RecorrerPreOrden())
                        {
                            var fabricaNueva = new FabricaRegistro();
                            var AuxRegistro = fabricaNueva.FabricarObtenido(r);

                            registro.Add(AuxRegistro);
                        }
                        Data.Instancia.TreeResgitro.Cerrar();

                        Data.Instancia.TreeResgitro = new ArbolB<Registro>(4, path + "E", new FabricaRegistro());

                        foreach (var item in registro)
                        {
                            if (item.Identificador == idResult)
                            {
                                //VER AQUI QUE PUTAS
                                EjecucionCorrecta = false;
                            }
                            else
                            {
                                Data.Instancia.TreeResgitro.Agregar(item.Identificador.ToString().Trim('x'), item, "");
                            }
                        }

                        if (EjecucionCorrecta)
                        {
                            //FUE ENCONTRADO
                            File.Delete(path);
                            File.Move(path + "E", path);
                        }
                        else
                        {
                            File.Delete(path + "E");
                        }
                    }
                }
            }

            return EjecucionCorrecta;
        }


        public bool Drop(string nombreTabla)
        {
            TablasMostrar.Clear();

            ContenidoMostrar.Clear();

            PropiedadesMostrar.Clear();
            var path = Data.Instancia.PathDirectorio + "\\ArbolesB\\" + nombreTabla + ".arbolB";

            if (File.Exists(path))
            {
                Data.Instancia.TreeResgitro.Cerrar();
                File.Delete(path);

                var pathTabla = Data.Instancia.PathDirectorio + "\\Tablas\\" + nombreTabla + ".tabla";

                File.Delete(pathTabla);
                ObtenerTablas();
            }
            else
            {
                EjecucionCorrecta = false;
            }

            return EjecucionCorrecta;
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