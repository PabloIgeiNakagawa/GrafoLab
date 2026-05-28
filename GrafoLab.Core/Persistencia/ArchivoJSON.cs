using System.Text.Json;
using GrafoLab.Core.Modelos;

namespace GrafoLab.Core.Persistencia;

public class ArchivoJSON
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = false,
        WriteIndented = true
    };

    public string GenerarJSONBasico(List<Grafo> grafos)
    {
        return JsonSerializer.Serialize(grafos, _options);
    }

    public void GuardarJSON(string jsonParaGuardar, string archivoDestino)
    {
        File.WriteAllText(archivoDestino, jsonParaGuardar);
    }

    public static void GuardarGrafo(Grafo grafo, string archivoDestino)
    {
        var json = JsonSerializer.Serialize(grafo, _options);
        File.WriteAllText(archivoDestino, json);
    }

    public static Grafo? LeerGrafo(string archivo)
    {
        try
        {
            var json = File.ReadAllText(archivo);
            return JsonSerializer.Deserialize<Grafo>(json, _options);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return null;
        }
    }

    public static List<Grafo>? LeerHistorialGrafos(string archivo)
    {
        try
        {
            if (!File.Exists(archivo))
                return [];

            var json = File.ReadAllText(archivo);
            return JsonSerializer.Deserialize<List<Grafo>>(json, _options);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return [];
        }
    }
}
