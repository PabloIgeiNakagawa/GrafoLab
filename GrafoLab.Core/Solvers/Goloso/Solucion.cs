using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Solvers.Goloso;

public class Solucion
{
    private readonly List<Vertice> _vertices = [];

    public int GetCantidadVertices() => _vertices.Count;

    public List<Vertice> GetVertices() => [.. _vertices];

    public int CantidadDeVerticesAlcanzados()
    {
        int ret = 0;
        foreach (var vertice in _vertices)
            ret += vertice.CantidadDeVecinos();
        return ret;
    }

    public void Agregar(Vertice v) => _vertices.Add(v);

    public bool Dominantes(HashSet<int> verVecinos)
    {
        foreach (var vertice in _vertices)
            if (verVecinos.Contains(vertice.NumVertice))
                return true;
        return false;
    }

    public override string ToString()
    {
        var ret = new HashSet<int>();
        foreach (var vertice in _vertices)
            ret.Add(vertice.NumVertice);
        return $"[{string.Join(", ", ret)}]";
    }
}
