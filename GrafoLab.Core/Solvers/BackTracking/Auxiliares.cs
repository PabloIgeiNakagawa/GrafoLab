using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Solvers.BackTracking;

internal static class Auxiliares
{
    internal static bool EsConjuntoDominante(Grafo grafo, HashSet<int> conjunto)
    {
        var verticesAlcanzados = new HashSet<int>();
        foreach (int i in conjunto)
        {
            verticesAlcanzados.Add(i);
            verticesAlcanzados.UnionWith(grafo.GetVertice(i).Vecinos);
        }

        return verticesAlcanzados.Count == grafo.Tamano();
    }

    internal static bool EsHoja(Grafo grafo, int vertice)
    {
        return grafo.GetVertice(vertice).CantidadDeVecinos() == 1;
    }
}
