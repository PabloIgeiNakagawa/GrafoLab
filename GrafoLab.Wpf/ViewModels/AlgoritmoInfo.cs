namespace GrafoLab.Wpf.ViewModels;

public enum TipoAlgoritmo
{
    ConjuntoDominanteGoloso,
    ConjuntoDominanteBackTracking
}

public class AlgoritmoInfo
{
    public TipoAlgoritmo Tipo { get; set; }
    public string Nombre { get; set; } = "";
}
