using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GrafoLab.Core.Modelos;
using GrafoLab.Core.Solvers.BackTracking;
using GrafoLab.Core.Solvers.Goloso;
using GrafoLab.Wpf.Views;

namespace GrafoLab.Wpf.ViewModels;

public enum ModoEditor
{
    Seleccionar,
    AgregarVertice,
    AgregarArista,
    Mover,
    Eliminar,
    EliminarArista
}

public class EditorGrafoViewModel : INotifyPropertyChanged
{
    private ModoEditor _modoActual = ModoEditor.Seleccionar;
    private VerticeVM? _origenArista;
    private VerticeVM? _verticeSeleccionado;
    private AristaVM? _aristaSeleccionada;
    private AlgoritmoInfo? _algoritmoSeleccionado;
    private string _resultado = "";
    private bool _ejecutando;
    private bool _mostrarBanner;

    public ObservableCollection<VerticeVM> VerticesVisibles { get; } = [];
    public ObservableCollection<AristaVM> AristasVisibles { get; } = [];
    public ObservableCollection<AlgoritmoInfo> AlgoritmosDisponibles { get; } = [];

    public EditorGrafoViewModel()
    {
        AlgoritmosDisponibles =
        [
            new() { Tipo = TipoAlgoritmo.ConjuntoDominanteGoloso, Nombre = "Conjunto Dominante Mínimo - Goloso" },
            new() { Tipo = TipoAlgoritmo.ConjuntoDominanteBackTracking, Nombre = "Conjunto Dominante Mínimo - BackTracking" }
        ];
        AlgoritmoSeleccionado = AlgoritmosDisponibles[0];
    }

    public ModoEditor ModoActual
    {
        get => _modoActual;
        set
        {
            _modoActual = value;
            CancelarCreacionArista();
            LimpiarSeleccion();
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusText));
        }
    }

    public VerticeVM? VerticeSeleccionado
    {
        get => _verticeSeleccionado;
        set
        {
            _verticeSeleccionado = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(InfoTexto));
            OnPropertyChanged(nameof(InfoVisible));
        }
    }

    public AristaVM? AristaSeleccionada
    {
        get => _aristaSeleccionada;
        set
        {
            _aristaSeleccionada = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(InfoTexto));
            OnPropertyChanged(nameof(InfoVisible));
        }
    }

    public bool InfoVisible => VerticeSeleccionado != null || AristaSeleccionada != null;

    public string InfoTexto
    {
        get
        {
            if (VerticeSeleccionado != null)
            {
                var v = VerticeSeleccionado;
                var vecinos = AristasVisibles
                    .Where(a => a.Origen == v || a.Destino == v)
                    .Select(a => a.Origen == v ? a.Destino.NumVertice : a.Origen.NumVertice)
                    .OrderBy(n => n);
                var grado = vecinos.Count();
                var vecinosStr = grado > 0 ? string.Join(", ", vecinos) : "ninguno";
                return $"Vértice {v.NumVertice} — Vecinos: [{vecinosStr}] — Grado: {grado} — Pos: ({v.X:F0}, {v.Y:F0})";
            }
            if (AristaSeleccionada != null)
            {
                var a = AristaSeleccionada;
                var dir = a.EsDirigida ? "Dirigida" : "No dirigida";
                var peso = a.TienePeso ? $" — Peso: {a.Peso:0.##}" : "";
                return $"Arista {a.Origen.NumVertice} → {a.Destino.NumVertice} — {dir}{peso}";
            }
            return "";
        }
    }

    public int CantidadVertices => VerticesVisibles.Count;
    public int CantidadAristas => AristasVisibles.Count;

    public AlgoritmoInfo? AlgoritmoSeleccionado
    {
        get => _algoritmoSeleccionado;
        set { _algoritmoSeleccionado = value; OnPropertyChanged(); }
    }

    public string Resultado
    {
        get => _resultado;
        set { _resultado = value; OnPropertyChanged(); }
    }

    public bool Ejecutando
    {
        get => _ejecutando;
        set { _ejecutando = value; OnPropertyChanged(); }
    }

    public bool MostrarBanner
    {
        get => _mostrarBanner;
        set { _mostrarBanner = value; OnPropertyChanged(); }
    }

    public string AlgoritmoNombre => AlgoritmoSeleccionado?.Nombre ?? "";

    public string StatusText
    {
        get
        {
            var modo = _modoActual switch
            {
                ModoEditor.Seleccionar => "Seleccionar",
                ModoEditor.AgregarVertice => "Haga clic en el lienzo para agregar un vertice",
                ModoEditor.AgregarArista => _origenArista != null
                    ? "Seleccione el vertice destino"
                    : "Seleccione el vertice origen",
                ModoEditor.Mover => "Arrastre un vertice para moverlo",
                ModoEditor.Eliminar => "Haga clic en un vertice para eliminarlo",
                ModoEditor.EliminarArista => "Haga clic sobre una arista para eliminarla",
                _ => ""
            };
            return $"Modo: {modo}";
        }
    }

    public VerticeVM? OrigenArista
    {
        get => _origenArista;
        set
        {
            _origenArista = value;
            OnPropertyChanged(nameof(StatusText));
        }
    }

    public void AgregarVertice(double x, double y)
    {
        if (ModoActual != ModoEditor.AgregarVertice) return;

        var vm = new VerticeVM { X = x, Y = y, NumVertice = VerticesVisibles.Count };
        VerticesVisibles.Add(vm);
        OnPropertyChanged(nameof(CantidadVertices));
    }

    public void SeleccionarVertice(VerticeVM vertice)
    {
        if (ModoActual != ModoEditor.Seleccionar) return;
        VerticeSeleccionado = vertice;
        AristaSeleccionada = null;
    }

    public void SeleccionarArista(AristaVM arista)
    {
        if (ModoActual != ModoEditor.Seleccionar) return;
        AristaSeleccionada = arista;
        VerticeSeleccionado = null;
    }

    public void LimpiarSeleccion()
    {
        VerticeSeleccionado = null;
        AristaSeleccionada = null;
    }

    public void IniciarCreacionArista(VerticeVM vertice)
    {
        if (ModoActual != ModoEditor.AgregarArista) return;

        if (OrigenArista == null)
        {
            OrigenArista = vertice;
            vertice.IsSelected = true;
        }
        else
        {
            if (vertice != OrigenArista && !AristaExiste(OrigenArista, vertice))
            {
                var dialog = new VentanaPesoArista();
                dialog.Owner = Application.Current.MainWindow;

                if (dialog.ShowDialog() == true)
                {
                    var arista = new AristaVM(OrigenArista, vertice)
                    {
                        Peso = dialog.Peso,
                        EsDirigida = dialog.EsDirigida
                    };
                    AristasVisibles.Add(arista);
                    OnPropertyChanged(nameof(CantidadAristas));
                }
            }

            OrigenArista.IsSelected = false;
            OrigenArista = null;
        }
    }

    public void EliminarArista(AristaVM arista)
    {
        if (ModoActual != ModoEditor.EliminarArista) return;

        var result = MessageBox.Show(
            "¿Está seguro de que desea eliminar esta arista?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            AristasVisibles.Remove(arista);
            OnPropertyChanged(nameof(CantidadAristas));
        }
    }

    public void MoverVertice(VerticeVM vertice, double x, double y)
    {
        vertice.X = x;
        vertice.Y = y;
    }

    public void EliminarVertice(VerticeVM vertice)
    {
        if (ModoActual != ModoEditor.Eliminar) return;

        var result = MessageBox.Show(
            "¿Está seguro de que desea eliminar este vértice y todas sus aristas?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        var aristasAEliminar = AristasVisibles
            .Where(a => a.Origen == vertice || a.Destino == vertice)
            .ToList();

        foreach (var arista in aristasAEliminar)
            AristasVisibles.Remove(arista);

        VerticesVisibles.Remove(vertice);
        RenumerarVertices();
        OnPropertyChanged(nameof(CantidadVertices));
        OnPropertyChanged(nameof(CantidadAristas));
    }


    public void CerrarBanner()
    {
        MostrarBanner = false;
    }

    public async void EjecutarAlgoritmo()
    {
        if (AlgoritmoSeleccionado == null) return;

        var grafo = ConstruirGrafo();

        if (grafo == null)
        {
            Resultado = "No hay grafos en el editor para ejecutar algoritmos.";
            Ejecutando = false;
            return;
        }
        MostrarBanner = true;
        OnPropertyChanged(nameof(AlgoritmoNombre));
        Ejecutando = true;
        Resultado = "Ejecutando...";

        try
        {
            switch (AlgoritmoSeleccionado.Tipo)
            {
                case TipoAlgoritmo.ConjuntoDominanteGoloso:
                    var resultadoG = await Task.Run(() =>
                        new SolverGoloso(grafo, new ComparadorPorCantidadDeVecinos()).Resolver());
                    Resultado = resultadoG.ToString();
                    break;

                case TipoAlgoritmo.ConjuntoDominanteBackTracking:
                    var resultadoBT = await Task.Run(() => new SolverBT(grafo).Resolver());
                    Resultado = $"[{string.Join(", ", resultadoBT)}]";
                    break;
            }
        }
        catch
        {
            Resultado = "Error";
        }
        finally
        {
            Ejecutando = false;
        }
    }

    private void CancelarCreacionArista()
    {
        if (OrigenArista != null)
        {
            OrigenArista.IsSelected = false;
            OrigenArista = null;
        }
    }

    private bool AristaExiste(VerticeVM a, VerticeVM b)
    {
        return AristasVisibles.Any(ar =>
            (ar.Origen == a && ar.Destino == b) ||
            (ar.Origen == b && ar.Destino == a));
    }

    private void RenumerarVertices()
    {
        for (int i = 0; i < VerticesVisibles.Count; i++)
            VerticesVisibles[i].NumVertice = i;
    }

    public Grafo? ConstruirGrafo()
    {
        if (VerticesVisibles.Count == 0) return null;

        RenumerarVertices();
        int n = VerticesVisibles.Count;
        var grafo = new Grafo(n);

        for (int i = 0; i < n; i++)
        {
            grafo.GetVertice(i).X = VerticesVisibles[i].X;
            grafo.GetVertice(i).Y = VerticesVisibles[i].Y;
        }

        foreach (var arista in AristasVisibles)
            grafo.AgregarArista(arista.Origen.NumVertice, arista.Destino.NumVertice, arista.Peso, arista.EsDirigida);

        return grafo;
    }

    public void CargarDesdeGrafo(Grafo grafo)
    {
        LimpiarSeleccion();
        VerticesVisibles.Clear();
        AristasVisibles.Clear();

        int n = grafo.Tamano();
        const double centro = 2000;
        const double radio = 150;

        for (int i = 0; i < n; i++)
        {
            double angulo = 2.0 * Math.PI * i / n;
            var v = grafo.GetVertice(i);
            VerticesVisibles.Add(new VerticeVM
            {
                NumVertice = i,
                X = v.X ?? centro + radio * Math.Cos(angulo),
                Y = v.Y ?? centro + radio * Math.Sin(angulo)
            });
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                bool existeIJ = grafo.ExisteArista(i, j);
                bool existeJI = grafo.ExisteArista(j, i);

                if (existeIJ && existeJI)
                {
                    var info = grafo.GetArista(i, j);
                    AristasVisibles.Add(new AristaVM(VerticesVisibles[i], VerticesVisibles[j])
                    {
                        Peso = info?.Peso,
                        EsDirigida = false
                    });
                }
                else if (existeIJ)
                {
                    var info = grafo.GetArista(i, j);
                    AristasVisibles.Add(new AristaVM(VerticesVisibles[i], VerticesVisibles[j])
                    {
                        Peso = info?.Peso,
                        EsDirigida = true
                    });
                }
                else if (existeJI)
                {
                    var info = grafo.GetArista(j, i);
                    AristasVisibles.Add(new AristaVM(VerticesVisibles[j], VerticesVisibles[i])
                    {
                        Peso = info?.Peso,
                        EsDirigida = true
                    });
                }
            }
        }

        OnPropertyChanged(nameof(CantidadVertices));
        OnPropertyChanged(nameof(CantidadAristas));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
