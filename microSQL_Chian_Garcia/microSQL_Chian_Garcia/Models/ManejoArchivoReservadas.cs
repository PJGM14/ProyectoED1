using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using microSQL_Chian_Garcia.Instancia;

namespace microSQL_Chian_Garcia.Models
{
    public class ManejoArchivoReservadas
    {
        public bool ExisteArchivoInicial { get; set; } //Esta propiedad dice si existe un archivo inicial para cargar (sirve para la vista)
        public bool ErrorEnArchivo { get; set; } //Propiedad para saber si el archivo encontrado fue leído correctamente o no

        //Método que asigna al diccionario las palabras reservadas por default y crea el archivo
        public void PalabrasReservadasDefault(string pathDirectorio)
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
                ExisteArchivoInicial = true;
            }
            else //Se crea el directorio con las carpetas que servirán posteriormente y se manda a llamar al método de PalabrasReservadasDefault
            {
                ExisteArchivoInicial = false;
                Directory.CreateDirectory(pathDirectorio);
                Directory.CreateDirectory(pathDirectorio + "\\ArbolesB");
                Directory.CreateDirectory(pathDirectorio+ "\\Tablas");
                
                PalabrasReservadasDefault(pathDirectorio);
                ErrorEnArchivo = false;
            }
        }
    }
}