using GrafoLab.Core.Modelos;
using GrafoLab.Core.Solvers.BackTracking;

namespace GrafoLab.Tests.Solvers.BackTracking;

public class AuxiliaresTest
{
    [Fact]
    public void CDGrafoVacioTest()
    {
        var grafoVacio = new Grafo(0);
        var conjVacio = new HashSet<int>();

        Assert.True(Auxiliares.EsConjuntoDominante(grafoVacio, conjVacio));
    }

    [Fact]
    public void CDUnSoloVerticeTest()
    {
        var grafoUnVertice = new Grafo(1);
        var conj = new HashSet<int> { 0 };

        Assert.True(Auxiliares.EsConjuntoDominante(grafoUnVertice, conj));
    }

    [Fact]
    public void CDGrafoCompletoTest()
    {
        var grafoCompleto = Completo();

        for (int i = 0; i < grafoCompleto.Tamano(); i++)
        {
            var conj = new HashSet<int> { i };
            Assert.True(Auxiliares.EsConjuntoDominante(grafoCompleto, conj));
        }
    }

    [Fact]
    public void CDConjuntoDominanteTest()
    {
        var grafo = EjemploTP();
        var conj = new HashSet<int> { 0, 3 };

        Assert.True(Auxiliares.EsConjuntoDominante(grafo, conj));
    }

    [Fact]
    public void CDNoConjuntoDominanteTest()
    {
        var grafo = EjemploTP();
        var conj = new HashSet<int> { 0, 5 };

        Assert.False(Auxiliares.EsConjuntoDominante(grafo, conj));
    }

    [Fact]
    public void EHGrafoVacio()
    {
        var grafoVacio = new Grafo(0);
        Assert.Throws<ArgumentOutOfRangeException>(() => Auxiliares.EsHoja(grafoVacio, 0));
    }

    [Fact]
    public void EHUnSoloVerticeTest()
    {
        var grafoUnVertice = new Grafo(1);
        Assert.False(Auxiliares.EsHoja(grafoUnVertice, 0));
    }

    [Fact]
    public void EHGrafoCompletoTest()
    {
        var grafoCompleto = Completo();

        for (int i = 0; i < grafoCompleto.Tamano(); i++)
            Assert.False(Auxiliares.EsHoja(grafoCompleto, i));
    }

    [Fact]
    public void EHhojaTest()
    {
        var grafo = EjemploTP();
        Assert.True(Auxiliares.EsHoja(grafo, 5));
    }

    [Fact]
    public void EHnoHojaTest()
    {
        var grafo = EjemploTP();
        Assert.False(Auxiliares.EsHoja(grafo, 0));
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
}
