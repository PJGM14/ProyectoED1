using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras.NoLinearStructures.Trees.Arbol_B
{
    public class ArbolB<T> : ArbolBusqueda<string, T> where T : ITextoTamañoFijo
    {
        #region Atributos 
        // Tamaño total del encabezado 
        private const int _tamañoEncabezadoBinario = 5 * Utilidades.EnteroYEnterBinarioTamaño;

        // Atributos en el encabezado del archivo 
        private int _raiz;
        private int _ultimaPosicionLibre;

        // Otras variables para acceso al archivo 
        private FileStream _archivo = null;
        private string _archivoNombre = "";
        private IFabricaTextoTamañoFijo<T> _fabrica = null;

        // El grado del árbol, este es asignado en el momento de la creación 
        // del árbol y no puede cambiarse posteriormente 
        public int Orden { get; private set; }
        public int Altura { get; private set; }

        public List<string> datos = new List<string>();
        #endregion

        public ArbolB(int orden, string nombreArchivo, IFabricaTextoTamañoFijo<T> fabrica)
        {
            // Se guardan los parámetros recibidos 
            _archivoNombre = nombreArchivo;
            _fabrica = fabrica;

            //FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.AllAccess, );
            //f2.AddPathList(FileIOPermissionAccess.Read, Application.StartupPath + "\\BLrpi.lfc");


            // Se abre la conexión al archivo 
            _archivo = new FileStream(_archivoNombre, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            // Se obtienen los valores del encabezado del archivo 
            _raiz = Utilidades.LeerEntero(_archivo, 0);
            _ultimaPosicionLibre = Utilidades.LeerEntero(_archivo, 1);
            Tamaño = Utilidades.LeerEntero(_archivo, 2);
            Orden = Utilidades.LeerEntero(_archivo, 3);
            Altura = Utilidades.LeerEntero(_archivo, 4);
            // Se corrigen los valores del encabezado cuando el archivos no existe previamente 
            if (_ultimaPosicionLibre == Utilidades.ApuntadorVacio)
            {
                _ultimaPosicionLibre = 0;
            }
            if (Tamaño == Utilidades.ApuntadorVacio)
            {
                Tamaño = 0;
            }
            if (Orden == Utilidades.ApuntadorVacio)
            {
                Orden = orden;
            }
            if (Altura == Utilidades.ApuntadorVacio)
            {
                Altura = 1;
            }
            if (_raiz == Utilidades.ApuntadorVacio)
            {
                // Se crea la cabeza del árbol vacía 
                // para evitar futurs errores 
                NodoB<T> nodoCabeza = new NodoB<T>(Orden, _ultimaPosicionLibre, Utilidades.ApuntadorVacio, _fabrica);
                _ultimaPosicionLibre++;
                _raiz = nodoCabeza.Posicion;
                nodoCabeza.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
            }
            // Si el archivo existe solamente se actualizan los encabezados, sino 
            // se crea y luego se almacenan los valores iniciales 
            GuardarEncabezado();
        }

        private void GuardarEncabezado()
        {
            // Se escribe a disco 
            Utilidades.EscribirEntero(_archivo, 0, _raiz);
            Utilidades.EscribirEntero(_archivo, 1, _ultimaPosicionLibre);
            Utilidades.EscribirEntero(_archivo, 2, Tamaño);
            Utilidades.EscribirEntero(_archivo, 3, Orden);
            Utilidades.EscribirEntero(_archivo, 4, Altura);
            _archivo.Flush();
        }

        private void AgregarRecursivo(int posicionNodoActual, string llave, T dato)
        {
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _fabrica);

            if (nodoActual.PosicionExactaEnNodo(llave) != -1)
            {
                throw new InvalidOperationException("La llave indicada ya está contenida en el árbol.");
            }
            if (nodoActual.EsHoja)
            {
                // Se debe insertar en este nodo, por lo que se hace la llamada 
                // al método encargado de insertar y ajustar el árbol si es necesario 
                Subir(nodoActual, llave, dato, Utilidades.ApuntadorVacio);
                GuardarEncabezado();
            }
            else
            {
                // Se hace una llamada recursiva, bajando en el subarbol 
                // correspondiente según la posición aproximada de la llave 
                AgregarRecursivo(nodoActual.Hijos[nodoActual.PosicionAproximadaEnNodo(llave)], llave, dato);
            }
        }

        private void Subir(NodoB<T> nodoActual, string llave, T dato, int hijoDerecho)
        {
            // Si el nodo no está lleno, se agrega la información 
            // al nodo y el método termina 
            if (!nodoActual.Lleno)
            {
                nodoActual.AgregarDato(llave, dato, hijoDerecho);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                return;
            }
            // Creo un nuevo nodo hermano 
            NodoB<T> nuevoHermano = new NodoB<T>(Orden, _ultimaPosicionLibre, nodoActual.Padre, _fabrica);
            _ultimaPosicionLibre++;

            // Datos a subir al padre luego de la separación 
            string llavePorSubir = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            T datoPorSubir = _fabrica.FabricarNulo();

            // Se llama al método que hace la separación  
            nodoActual.SepararNodo(llave, dato, hijoDerecho, nuevoHermano, ref llavePorSubir, ref datoPorSubir);

            // Actualizar el apuntador en todos los hijos 
            NodoB<T> nodoHijo = null;
            for (int i = 0; i < nuevoHermano.Hijos.Count; i++)
            {
                if (nuevoHermano.Hijos[i] != Utilidades.ApuntadorVacio)
                {
                    // Se carga el hijo para modificar su apuntador al padre 
                    nodoHijo = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nuevoHermano.Hijos[i], _fabrica);
                    nodoHijo.Padre = nuevoHermano.Posicion;
                    nodoHijo.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                }
                else
                {
                    break;
                }
            }

            // Evaluo el caso del Padre 
            if (nodoActual.Padre == Utilidades.ApuntadorVacio) // Es la raiz 
            {
                // Creo un nuevo nodo Raiz 
                NodoB<T> nuevaRaiz = new NodoB<T>(Orden, _ultimaPosicionLibre, Utilidades.ApuntadorVacio, _fabrica);
                _ultimaPosicionLibre++;
                Altura++;

                // Agrego la información 
                nuevaRaiz.Hijos[0] = nodoActual.Posicion;
                nuevaRaiz.AgregarDato(llavePorSubir, datoPorSubir, nuevoHermano.Posicion);

                // Actualizo los apuntadores al padre 
                nodoActual.Padre = nuevaRaiz.Posicion;
                nuevoHermano.Padre = nuevaRaiz.Posicion;
                _raiz = nuevaRaiz.Posicion;

                // Guardo los cambios 
                nuevaRaiz.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
            }
            else // No es la raiz 
            {
                nodoActual.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);
                nuevoHermano.GuardarNodoEnDisco(_archivo, _tamañoEncabezadoBinario);

                // Cargar el nodo Padre 
                NodoB<T> nodoPadre = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, nodoActual.Padre, _fabrica);
                Subir(nodoPadre, llavePorSubir, datoPorSubir, nuevoHermano.Posicion);
            }
        }

        private NodoB<T> ObtenerRecursivo(int posicionNodoActual, string llave, out int posicion)
        {
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionNodoActual, _fabrica);
            posicion = nodoActual.PosicionExactaEnNodo(llave);
            if (posicion != -1)
            {
                return nodoActual;
            }
            else
            {
                if (nodoActual.EsHoja)
                {
                    return null;
                }
                else
                {
                    int posicionAproximada = nodoActual.PosicionAproximadaEnNodo(llave);
                    return ObtenerRecursivo(nodoActual.Hijos[posicionAproximada], llave, out posicion);
                }
            }
        }

        public override void Agregar(string llave, T dato, string llaveAux)
        {
            try
            {
                if (llave == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                {
                    throw new ArgumentOutOfRangeException("llave");
                }

                llave = llave + llaveAux;
                AgregarRecursivo(_raiz, llave, dato);
                Tamaño++;
            }
            catch (Exception)
            {

            }
        }

        public override T Obtener(string llave)
        {
            int posicion = -1;
            NodoB<T> nodoObtenido = ObtenerRecursivo(_raiz, llave, out posicion);
            if (nodoObtenido == null)
            {
                throw new InvalidOperationException("La llave indicada no está en el árbol.");
            }
            else
            {
                return nodoObtenido.Datos[posicion];
            }
        }

        public override bool Contiene(string llave)
        {
            int posicion = -1;
            NodoB<T> nodoObtenido = ObtenerRecursivo(_raiz, llave, out posicion);
            if (nodoObtenido == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void EscribirNodo(NodoB<T> nodoActual, StringBuilder texto)
        {
            for (int i = 0; i < nodoActual.Llaves.Count; i++)
            {
                if (nodoActual.Llaves[i] != "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
                {
                    texto.AppendLine(nodoActual.Llaves[i].ToString());
                    texto.AppendLine(nodoActual.Datos[i].ToString());
                    texto.AppendLine("---------------");
                }
                else
                {
                    break;
                }
            }
        }

        public override List<string> RecorrerPreOrden()
        {
            StringBuilder texto = new StringBuilder();
            RecorrerPreOrdenRecursivo(_raiz, texto);
            return datos;
        }

        private void RecorrerPreOrdenRecursivo(int posicionActual, StringBuilder texto)
        {
            if (posicionActual == Utilidades.ApuntadorVacio)
            {
                return;
            }
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionActual, _fabrica);

            EscribirNodo(nodoActual, texto);
            for (int i = 0; i < nodoActual.Hijos.Count; i++)
            {
                RecorrerPreOrdenRecursivo(nodoActual.Hijos[i], texto);
            }

            for (int i = 0; i < nodoActual.CantidadDatos; i++)
            {
                datos.Add(nodoActual.Datos[i].ToFixedSizeString());
            }
        }

        public override string RecorrerInOrden()
        {
            StringBuilder texto = new StringBuilder();
            RecorrerInOrdenRecursivo(_raiz, texto);
            return texto.ToString();
        }

        private void RecorrerInOrdenRecursivo(int posicionActual, StringBuilder texto)
        {
            if (posicionActual == Utilidades.ApuntadorVacio)
            {
                return;
            }
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionActual, _fabrica);
            for (int i = 0; i < nodoActual.Hijos.Count; i++)
            {
                RecorrerInOrdenRecursivo(nodoActual.Hijos[i], texto);
                if ((i < nodoActual.Llaves.Count) && (nodoActual.Llaves[i] != ""))
                {
                    texto.AppendLine(nodoActual.Llaves[i].ToString());
                    texto.AppendLine(nodoActual.Datos[i].ToString());
                    texto.AppendLine("---------------");
                }
            }
        }

        public override string RecorrerPostOrden()
        {
            StringBuilder texto = new StringBuilder();
            RecorrerPostOrdenRecursivo(_raiz, texto);
            return texto.ToString();
        }

        private void RecorrerPostOrdenRecursivo(int posicionActual, StringBuilder texto)
        {
            if (posicionActual == Utilidades.ApuntadorVacio)
            {
                return;
            }
            NodoB<T> nodoActual = NodoB<T>.LeerNodoDesdeDisco(_archivo, _tamañoEncabezadoBinario, Orden, posicionActual, _fabrica);
            for (int i = 0; i < nodoActual.Hijos.Count; i++)
            {
                RecorrerPreOrdenRecursivo(nodoActual.Hijos[i], texto);
            }
            EscribirNodo(nodoActual, texto);
        }

        public override int ObtenerAltura()
        {
            return Altura;
        }
        public override void Eliminar(string llave)
        {
            throw new NotImplementedException();
        }

        //public override Bitmap Dibujar()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Cerrar()
        {
            _archivo.Close();
        }


        public T Search(Delegate comparer, string llave)
        {
            return (T)comparer.DynamicInvoke(this, llave);
        }
    }
}