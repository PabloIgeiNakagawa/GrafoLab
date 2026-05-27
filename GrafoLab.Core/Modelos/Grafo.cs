using System.Text.Json.Serialization;

namespace GrafoLab.Core.Modelos;

public class Grafo
{
    [JsonPropertyName("_matrizAdyacencia")]
    public AristaInfo?[][] MatrizAdyacencia { get; set; } = [];

    [JsonPropertyName("_vertices")]
    public List<Vertice> Vertices { get; set; } = [];

    public Grafo() { }

    public Grafo(int vertices)
    {
        MatrizAdyacencia = new AristaInfo?[vertices][];
        for (int i = 0; i < vertices; i++)
            MatrizAdyacencia[i] = new AristaInfo?[vertices];

        Vertices = new List<Vertice>();
        for (int i = 0; i < vertices; i++)
            Vertices.Add(new Vertice(i));
    }

    public int Tamano() => MatrizAdyacencia.Length;

    public Vertice GetVertice(int i) => Vertices[i];

    public void AgregarArista(int i, int j, double? peso = null, bool dirigida = false)
    {
        VerificarVertice(i);
        VerificarVertice(j);
        VerificarDistintos(i, j);

        var info = new AristaInfo { Peso = peso, EsDirigida = dirigida };
        MatrizAdyacencia[i][j] = info;
        if (!dirigida)
            MatrizAdyacencia[j][i] = info;
        else
            MatrizAdyacencia[j][i] = null;

        Vertices[i].Vecindad(Vecinos(i));
        Vertices[j].Vecindad(Vecinos(j));
    }

    public void EliminarArista(int i, int j)
    {
        VerificarVertice(i);
        VerificarVertice(j);
        VerificarDistintos(i, j);

        MatrizAdyacencia[i][j] = null;
        MatrizAdyacencia[j][i] = null;

        Vertices[i].Vecindad(Vecinos(i));
        Vertices[j].Vecindad(Vecinos(j));
    }

    internal HashSet<int> Vecinos(int i)
    {
        VerificarVertice(i);
        var ret = new HashSet<int>();

        for (int j = 0; j < Tamano(); j++)
            if (i != j && MatrizAdyacencia[i][j] != null)
                ret.Add(j);

        return ret;
    }

    public bool ExisteArista(int i, int j)
    {
        VerificarVertice(i);
        VerificarVertice(j);
        VerificarDistintos(i, j);

        return MatrizAdyacencia[i][j] != null;
    }

    public AristaInfo? GetArista(int i, int j)
    {
        VerificarVertice(i);
        VerificarVertice(j);
        VerificarDistintos(i, j);

        return MatrizAdyacencia[i][j];
    }

    private void VerificarVertice(int i)
    {
        if (i < 0)
            throw new ArgumentException($"El vertice no puede ser negativo: {i}");
        if (i >= MatrizAdyacencia.Length)
            throw new ArgumentException($"Los vertices deben estar entre 0 y |V|-1: {i}");
    }

    private static void VerificarDistintos(int i, int j)
    {
        if (i == j)
            throw new ArgumentException($"No se permiten loops: ({i}, {j})");
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("========= Grafo ===========");
        foreach (var vertice in Vertices)
            sb.AppendLine(vertice.ToString());
        sb.AppendLine("===========================");
        return sb.ToString();
    }
}
