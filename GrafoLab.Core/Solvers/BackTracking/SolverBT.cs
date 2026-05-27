using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Solvers.BackTracking;

public class SolverBT
{
    private readonly Grafo _grafo;
    private HashSet<int> _actual = [];
    private HashSet<int> _menor = [];
    private List<HashSet<int>> _soluciones = [];

    public SolverBT(Grafo grafo)
    {
        _grafo = grafo;
    }

    public HashSet<int> Resolver()
    {
        _actual = [];
        _menor = [];
        for (int i = 0; i < _grafo.Tamano(); i++)
            _menor.Add(i);

        _soluciones = [];

        GenerarDesde(0);

        return _menor;
    }

    public List<HashSet<int>> GetSoluciones() => _soluciones;

    private void GenerarDesde(int vertice)
    {
        if (vertice == _grafo.Tamano())
        {
            if (_actual.Count < _menor.Count && Auxiliares.EsConjuntoDominante(_grafo, _actual))
            {
                _soluciones.Add([.. _actual]);
                _menor = [.. _actual];
            }
            return;
        }

        if (Auxiliares.EsHoja(_grafo, vertice))
        {
            _actual.Remove(vertice);
            GenerarDesde(vertice + 1);
            return;
        }

        _actual.Add(vertice);
        GenerarDesde(vertice + 1);

        _actual.Remove(vertice);
        GenerarDesde(vertice + 1);
    }
}
