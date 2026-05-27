using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Solvers.Goloso;

public class ComparadorPorCantidadDeVecinos : IComparer<Vertice>
{
    public int Compare(Vertice? uno, Vertice? otro)
    {
        if (uno is null || otro is null) return 0;
        return -uno.CantidadDeVecinos() + otro.CantidadDeVecinos();
    }
}
