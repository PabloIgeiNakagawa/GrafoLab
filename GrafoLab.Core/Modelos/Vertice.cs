using System.Text.Json.Serialization;

namespace GrafoLab.Core.Modelos;

public class Vertice
{
    [JsonPropertyName("_numVertice")]
    public int NumVertice { get; set; }

    [JsonPropertyName("_vecinos")]
    public HashSet<int> Vecinos { get; private set; }

    [JsonPropertyName("_x")]
    public double? X { get; set; }

    [JsonPropertyName("_y")]
    public double? Y { get; set; }

    public Vertice()
    {
        Vecinos = new HashSet<int>();
    }

    public Vertice(int numVertice)
    {
        NumVertice = numVertice;
        Vecinos = new HashSet<int>();
    }

    public int CantidadDeVecinos() => Vecinos.Count;

    internal void Vecindad(HashSet<int> vecinos)
    {
        Vecinos = vecinos;
    }

    public override string ToString() =>
        $"Vertice {NumVertice}, Vecinos: [{string.Join(", ", Vecinos)}]";
}
