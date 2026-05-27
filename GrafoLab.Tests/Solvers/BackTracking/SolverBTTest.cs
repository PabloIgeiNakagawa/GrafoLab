using GrafoLab.Core.Modelos;
using GrafoLab.Core.Solvers.BackTracking;

namespace GrafoLab.Tests.Solvers.BackTracking;

public class SolverBTTest
{
    [Fact]
    public void AisladoTest()
    {
        var grafoAislado = Aislado();
        var solver = new SolverBT(grafoAislado);
        var obtenido = solver.Resolver();

        Assert.Equal(grafoAislado.Tamano(), obtenido.Count);
    }

    [Fact]
    public void CompletoTest()
    {
        var solver = new SolverBT(Completo());
        var obtenido = solver.Resolver();

        Assert.Single(obtenido);
    }

    [Fact]
    public void BackTrackingCDMTest()
    {
        var solver = new SolverBT(EjemploTP());
        var obtenido = solver.Resolver();

        Assert.Equal(2, obtenido.Count);
    }

    [Fact]
    public void TrianguloConAntenaTest()
    {
        var solver = new SolverBT(TrianguloConAntena());
        var obtenido = solver.Resolver();

        var esperado = new HashSet<int> { 1 };
        Assert.Equal(esperado, obtenido);
    }

    private static Grafo Aislado()
    {
        return new Grafo(5);
    }

    private static Grafo Completo()
    {
        var grafo = new Grafo(4);
        grafo.AgregarArista(0, 1);
        grafo.AgregarArista(0, 2);
        grafo.AgregarArista(0, 3);
        grafo.AgregarArista(1, 2);
        grafo.AgregarArista(1, 3);
        grafo.AgregarArista(2, 3);
        return grafo;
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

    private static Grafo TrianguloConAntena()
    {
        var grafo = new Grafo(4);
        grafo.AgregarArista(0, 1);
        grafo.AgregarArista(0, 2);
        grafo.AgregarArista(1, 2);
        grafo.AgregarArista(3, 1);
        return grafo;
    }
}
