using System.Text.Json.Serialization;

namespace GrafoLab.Core.Modelos;

public class AristaInfo
{
    [JsonPropertyName("_peso")]
    public double? Peso { get; set; }

    [JsonPropertyName("_dirigida")]
    public bool EsDirigida { get; set; }
}
