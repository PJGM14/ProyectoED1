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
        public bool ExisteArchivoInicial { get; set; }
        public bool ErrorEnArchivo { get; set; }

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

        public void VerificarArchivoPalabrasReservadas(string pathDirectorio)
        {
            if (Directory.Exists(pathDirectorio))
            {
                ExisteArchivoInicial = true;
            }
            else
            {
                ExisteArchivoInicial = false;
                Directory.CreateDirectory(pathDirectorio);
                Directory.CreateDirectory(pathDirectorio + "\\ArbolesB");
                Directory.CreateDirectory(pathDirectorio+ "\\Tablas");
                
                PalabrasReservadasDefault(pathDirectorio);
            }
        }
    }
}