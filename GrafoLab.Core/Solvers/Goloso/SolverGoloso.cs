using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Solvers.Goloso;

public class SolverGoloso
{
    private readonly Grafo _grafo;
    private readonly IComparer<Vertice> _comparador;

    public SolverGoloso(Grafo grafo, IComparer<Vertice> comparador)
    {
        _grafo = grafo;
        _comparador = comparador;
    }

    public Solucion Resolver()
    {
        var ret = new Solucion();

        foreach (var vertice in ObjetosOrdenados())
        {
            if (ret.CantidadDeVerticesAlcanzados() + vertice.CantidadDeVecinos() <= _grafo.Tamano()
                && !ret.Dominantes(vertice.Vecinos))
            {
                ret.Agregar(vertice);
            }
        }

        return ret;
    }

    public List<Vertice> ObjetosOrdenados()
    {
        var ret = _grafo.Vertices.ToList();
        ret.Sort(_comparador);
        return ret;
    }
}
