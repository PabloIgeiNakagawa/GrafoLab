using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GrafoLab.Wpf.ViewModels;

public class VerticeVM : INotifyPropertyChanged
{
    private double _x;
    private double _y;
    private int _numVertice;
    private bool _isSelected;

    public double X
    {
        get => _x;
        set { _x = value; OnPropertyChanged(); }
    }

    public double Y
    {
        get => _y;
        set { _y = value; OnPropertyChanged(); }
    }

    public int NumVertice
    {
        get => _numVertice;
        set { _numVertice = value; OnPropertyChanged(); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
