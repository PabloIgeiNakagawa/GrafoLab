using GrafoLab.Core.Modelos;

namespace GrafoLab.Tests.Modelos;

public class GrafoTest
{
    [Fact]
    public void VerticeNegativoTest()
    {
        var grafo = new Grafo(4);
        Assert.Throws<ArgumentException>(() => grafo.AgregarArista(-1, 3));
    }

    [Fact]
    public void VerticeExcedidoTest()
    {
        var grafo = new Grafo(4);
        Assert.Throws<ArgumentException>(() => grafo.AgregarArista(2, 4));
    }

    [Fact]
    public void SinLoopsTest()
    {
        var grafo = new Grafo(4);
        Assert.Throws<ArgumentException>(() => grafo.AgregarArista(2, 2));
    }

    [Fact]
    public void AristaExistenteTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 3);
        Assert.True(grafo.ExisteArista(2, 3));
    }

    [Fact]
    public void AristaOpuestaTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 3);
        Assert.True(grafo.ExisteArista(3, 2));
    }

    [Fact]
    public void AristaInexistenteTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 3);
        Assert.False(grafo.ExisteArista(1, 4));
    }

    [Fact]
    public void AgregarAristaDosVecesTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 3);
        grafo.AgregarArista(2, 3);
        Assert.True(grafo.ExisteArista(2, 3));
    }

    [Fact]
    public void EliminarAristaExistenteTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 4);
        grafo.EliminarArista(2, 4);
        Assert.False(grafo.ExisteArista(2, 4));
    }

    [Fact]
    public void EliminarAristaInexistenteTest()
    {
        var grafo = new Grafo(5);
        grafo.EliminarArista(2, 4);
        Assert.False(grafo.ExisteArista(2, 4));
    }

    [Fact]
    public void EliminarAristaDosVecesTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 4);
        grafo.EliminarArista(2, 4);
        grafo.EliminarArista(2, 4);
        Assert.False(grafo.ExisteArista(2, 4));
    }

    [Fact]
    public void AristaConPesoTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(2, 3, 4.5);
        var info = grafo.GetArista(2, 3);
        Assert.NotNull(info);
        Assert.Equal(4.5, info!.Peso);
    }

    [Fact]
    public void AristaDirigidaAsimetricaTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(0, 1, dirigida: true);
        Assert.True(grafo.ExisteArista(0, 1));
        Assert.False(grafo.ExisteArista(1, 0));
    }

    [Fact]
    public void AristaNoDirigidaSimetricaTest()
    {
        var grafo = new Grafo(5);
        grafo.AgregarArista(0, 1, dirigida: false);
        Assert.True(grafo.ExisteArista(0, 1));
        Assert.True(grafo.ExisteArista(1, 0));
    }
}
