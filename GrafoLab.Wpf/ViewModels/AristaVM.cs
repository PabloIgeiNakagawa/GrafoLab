using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GrafoLab.Wpf.ViewModels;

public class AristaVM : INotifyPropertyChanged
{
    public VerticeVM Origen { get; }
    public VerticeVM Destino { get; }

    public double X1 => Origen.X + 18;
    public double Y1 => Origen.Y + 18;
    public double X2 => Destino.X + 18;
    public double Y2 => Destino.Y + 18;

    private double? _peso;
    private bool _esDirigida;

    public double? Peso
    {
        get => _peso;
        set
        {
            _peso = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TienePeso));
            OnPropertyChanged(nameof(PesoTexto));
        }
    }

    public bool EsDirigida
    {
        get => _esDirigida;
        set
        {
            _esDirigida = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MostrarFlecha));
            NotificarCambios();
        }
    }

    public bool TienePeso => Peso.HasValue;
    public bool MostrarFlecha => EsDirigida;
    public string PesoTexto => Peso?.ToString("0.##") ?? "";

    public double TextX => (X1 + X2) / 2;
    public double TextY => (Y1 + Y2) / 2 - 12;

    public double ArrowTipX { get; private set; }
    public double ArrowTipY { get; private set; }
    public double ArrowLeftX { get; private set; }
    public double ArrowLeftY { get; private set; }
    public double ArrowRightX { get; private set; }
    public double ArrowRightY { get; private set; }

    public AristaVM(VerticeVM origen, VerticeVM destino)
    {
        Origen = origen;
        Destino = destino;

        Origen.PropertyChanged += (_, _) => NotificarCambios();
        Destino.PropertyChanged += (_, _) => NotificarCambios();
    }

    private void NotificarCambios()
    {
        OnPropertyChanged(nameof(X1));
        OnPropertyChanged(nameof(Y1));
        OnPropertyChanged(nameof(X2));
        OnPropertyChanged(nameof(Y2));
        OnPropertyChanged(nameof(TextX));
        OnPropertyChanged(nameof(TextY));
        CalcularFlecha();
    }

    private void CalcularFlecha()
    {
        if (!EsDirigida)
        {
            ArrowTipX = ArrowTipY = ArrowLeftX = ArrowLeftY = ArrowRightX = ArrowRightY = 0;
            OnPropertyChanged(nameof(ArrowTipX));
            OnPropertyChanged(nameof(ArrowTipY));
            OnPropertyChanged(nameof(ArrowLeftX));
            OnPropertyChanged(nameof(ArrowLeftY));
            OnPropertyChanged(nameof(ArrowRightX));
            OnPropertyChanged(nameof(ArrowRightY));
            return;
        }

        double angle = Math.Atan2(Y2 - Y1, X2 - X1);
        double radius = 15;
        double len = 12;
        double spread = 0.5;

        double tipX = X2 - radius * Math.Cos(angle);
        double tipY = Y2 - radius * Math.Sin(angle);

        ArrowTipX = tipX;
        ArrowTipY = tipY;
        OnPropertyChanged(nameof(ArrowTipX));
        OnPropertyChanged(nameof(ArrowTipY));

        ArrowLeftX = tipX - len * Math.Cos(angle - spread);
        ArrowLeftY = tipY - len * Math.Sin(angle - spread);
        OnPropertyChanged(nameof(ArrowLeftX));
        OnPropertyChanged(nameof(ArrowLeftY));

        ArrowRightX = tipX - len * Math.Cos(angle + spread);
        ArrowRightY = tipY - len * Math.Sin(angle + spread);
        OnPropertyChanged(nameof(ArrowRightX));
        OnPropertyChanged(nameof(ArrowRightY));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
