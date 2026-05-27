using GrafoLab.Core.Modelos;
using GrafoLab.Core.Solvers.Goloso;

namespace GrafoLab.Tests.Solvers.Goloso;

public class SolverGolosoTest
{
    [Fact]
    public void ResolverPorVecinosTest()
    {
        var solver = new SolverGoloso(EjemploTP(), new ComparadorPorCantidadDeVecinos());
        var solucion = solver.Resolver();

        Assert.Equal(2, solucion.GetCantidadVertices());
        Assert.Equal(6, solucion.CantidadDeVerticesAlcanzados());
    }

    private static Grafo EjemploTP()
    {
        var g = new Grafo(6);
        g.AgregarArista(0, 1);
        g.AgregarArista(0, 4);
        g.AgregarArista(1, 4);
        g.AgregarArista(1, 2);
        g.AgregarArista(4, 3);
        g.AgregarArista(3, 2);
        g.AgregarArista(3, 5);
        return g;
    }
}
