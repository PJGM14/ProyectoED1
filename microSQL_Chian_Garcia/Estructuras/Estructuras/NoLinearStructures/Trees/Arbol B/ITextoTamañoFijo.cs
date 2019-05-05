using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras.NoLinearStructures.Trees.Arbol_B
{
    public interface ITextoTamañoFijo
    {
        int FixedSizeText { get; }
        string ToFixedSizeString();
    }
}