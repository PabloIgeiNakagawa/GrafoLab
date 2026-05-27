using GrafoLab.Core.Modelos;

namespace GrafoLab.Tests.Modelos;

public class VecinosTest
{
    [Fact]
    public void TodosAisladosTest()
    {
        var grafo = new Grafo(5);
        Assert.Empty(grafo.Vecinos(2));
    }

    [Fact]
    public void VerticeUniversalTest()
    {
        var grafo = new Grafo(4);
        grafo.AgregarArista(1, 0);
        grafo.AgregarArista(1, 2);
        grafo.AgregarArista(1, 3);

        var esperado = new HashSet<int> { 0, 2, 3 };
        Assert.Equal(esperado, grafo.Vecinos(1));
    }

    [Fact]
    public void VerticeNormalTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(1, 3);
        grafo.AgregarArista(2, 3);
        grafo.AgregarArista(2, 4);

        var esperado = new HashSet<int> { 1, 2 };
        Assert.Equal(esperado, grafo.Vecinos(3));
    }

    [Fact]
    public void VerticeNormalConVecinosTest()
    {
        var grafo = DiamanteConVerticeAislado();

        int[] esperado = [0, 3, 4];
        SetsIguales(esperado, grafo.Vecinos(2));
    }

    [Fact]
    public void VecinosVaciosTest()
    {
        var grafo = DiamanteConVerticeAislado();

        int[] esperado = [];
        SetsIguales(esperado, grafo.Vecinos(1));
    }

    [Fact]
    public void UnSoloVecinoTest()
    {
        var grafo = DiamanteConVerticeAislado();
        grafo.EliminarArista(0, 3);

        int[] esperado = [2];
        SetsIguales(esperado, grafo.Vecinos(0));
    }

    private static void SetsIguales(int[] esperado, HashSet<int> obtenido)
    {
        foreach (var elemento in esperado)
            Assert.Contains(elemento, obtenido);
    }

    private static Grafo DiamanteConVerticeAislado()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(0, 2);
        grafo.AgregarArista(0, 3);
        grafo.AgregarArista(2, 3);
        grafo.AgregarArista(2, 4);
        grafo.AgregarArista(3, 4);
        return grafo;
    }
}
