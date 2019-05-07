using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using microSQL_Chian_Garcia.Instancia;
using Microsoft.Ajax.Utilities;

namespace microSQL_Chian_Garcia.Models
{
    public class ManejoArchivoReservadas
    {
        public bool ExisteArchivoInicial { get; set; } //Esta propiedad dice si existe un archivo inicial para cargar (sirve para la vista)
        public bool ErrorEnArchivo { get; set; } //Propiedad para saber si el archivo encontrado fue leído correctamente o no
       
        //El diccionario de palabras reservadas toma valores predeterminados
        public void PalabrasReservadasDefault()
        {
            Data.Instancia.EditorTexto.PalabrasReservadas.Clear();

            Data.Instancia.EditorTexto.PalabrasReservadas.Add("SELECT", "SELECT");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("FROM", "FROM");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("DELETE", "DELETE");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("WHERE", "WHERE");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("CREATE TABLE", "CREATE TABLE");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("DROP TABLE", "DROP TABLE");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("INSERT INTO", "INSERT INTO");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("VALUES", "VALUES");
            Data.Instancia.EditorTexto.PalabrasReservadas.Add("GO", "GO");
        }

        //Método que escribe el archivo con las palabras reservadas que tiene el diccionario
        public void EscribirArchivoPalabrasReservadas(string pathDirectorio)
        {
            using (StreamWriter file = new StreamWriter(pathDirectorio + "\\MicroSQL.ini",false))
            {
                foreach (var ItemDicc in Data.Instancia.EditorTexto.PalabrasReservadas)
                {
                    file.WriteLine(ItemDicc.Key +","+ ItemDicc.Value);
                }
            }
        }

        //Método para verificar si al iniciar el programa existe o no archivo MicroSQL.ini
        public void VerificarArchivoPalabrasReservadas(string pathDirectorio)
        {
            //Si existe el directorio se busca el archvio en el directorio
            if (Directory.Exists(pathDirectorio))
            {
                if (File.Exists(pathDirectorio+"\\MicroSQL.ini"))
                {
                    ExisteArchivoInicial = true;

                    var texto = File.ReadAllText(pathDirectorio + "\\MicroSQL.ini");

                    if (!texto.IsNullOrWhiteSpace())
                    {
                        var lineas = File.ReadAllLines(pathDirectorio + "\\MicroSQL.ini");

                        PalabrasReservadasDefault();

                        foreach (var lineaCompleta in lineas)
                        {
                            if (!lineaCompleta.IsNullOrWhiteSpace())
                            {
                                var campoLinea = lineaCompleta.Split(',');

                                if (campoLinea.Length == 2)
                                {
                                    if (Data.Instancia.EditorTexto.PalabrasReservadas.ContainsKey(campoLinea[0]))
                                    {
                                        Data.Instancia.EditorTexto.PalabrasReservadas[campoLinea[0]] = campoLinea[1].ToUpper();
                                    }
                                    else
                                    {
                                        ErrorEnArchivo = true;
                                        PalabrasReservadasDefault();
                                        EscribirArchivoPalabrasReservadas(pathDirectorio);
                                        break;
                                    }
                                }
                                else
                                {
                                    ErrorEnArchivo = true;
                                    PalabrasReservadasDefault();
                                    EscribirArchivoPalabrasReservadas(pathDirectorio);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ExisteArchivoInicial = false;
                        PalabrasReservadasDefault();
                        EscribirArchivoPalabrasReservadas(pathDirectorio);
                    }
                }
                else
                {
                    ExisteArchivoInicial = false;
                    PalabrasReservadasDefault();
                    EscribirArchivoPalabrasReservadas(pathDirectorio);
                }

            }
            else //Se crea el directorio con las carpetas que servirán posteriormente y se manda a llamar al método de PalabrasReservadasDefault
            {
                ExisteArchivoInicial = false;
                Directory.CreateDirectory(pathDirectorio);
                Directory.CreateDirectory(pathDirectorio + "\\ArbolesB");
                Directory.CreateDirectory(pathDirectorio+ "\\Tablas");
                
                PalabrasReservadasDefault();
                EscribirArchivoPalabrasReservadas(pathDirectorio);
                ErrorEnArchivo = false;
            }
        }
    }
}