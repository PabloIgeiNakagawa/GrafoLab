using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GrafoLab.Wpf.ViewModels;

namespace GrafoLab.Wpf;

public partial class MainWindow : Window
{
    private readonly EditorGrafoViewModel _vm;
    private bool _isDragging;
    private VerticeVM? _dragVertice;
    private Point _dragStart;
    private double _dragVerticeStartX;
    private double _dragVerticeStartY;
    private bool _isPanning;
    private Point _panStartViewport;
    private Point _panScrollStart;
    private double _currentZoom = 1.0;
    private const double ZoomMin = 0.1;
    private const double ZoomMax = 5.0;

    public MainWindow()
    {
        _vm = new EditorGrafoViewModel();
        DataContext = _vm;
        InitializeComponent();

        Loaded += (_, _) =>
        {
            scrollViewer.ScrollToHorizontalOffset(2000 * _currentZoom - scrollViewer.ViewportWidth / 2);
            scrollViewer.ScrollToVerticalOffset(2000 * _currentZoom - scrollViewer.ViewportHeight / 2);
        };
    }

    private void ModoRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.Tag is string tag)
        {
            if (Enum.TryParse<ModoEditor>(tag, out var modo))
                _vm.ModoActual = modo;
        }
    }

    private void EditorGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _isPanning = true;
            _panStartViewport = e.GetPosition(scrollViewer);
            _panScrollStart = new Point(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset);
            editorGrid.CaptureMouse();
            Cursor = Cursors.Hand;
            e.Handled = true;
            return;
        }

        if (_vm.ModoActual == ModoEditor.AgregarVertice)
        {
            var pos = e.GetPosition(editorGrid);
            _vm.AgregarVertice(Math.Max(0, pos.X - 18), Math.Max(0, pos.Y - 18));
            e.Handled = true;
        }
    }

    private void Vertice_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right) return;
        if (sender is not FrameworkElement fe) return;
        var vertice = fe.DataContext as VerticeVM;
        if (vertice == null) return;

        switch (_vm.ModoActual)
        {
            case ModoEditor.AgregarArista:
                _vm.IniciarCreacionArista(vertice);
                e.Handled = true;
                break;

            case ModoEditor.Mover:
                _isDragging = true;
                _dragVertice = vertice;
                _dragStart = e.GetPosition(editorGrid);
                _dragVerticeStartX = vertice.X;
                _dragVerticeStartY = vertice.Y;
                fe.CaptureMouse();
                e.Handled = true;
                break;

            case ModoEditor.Eliminar:
                _vm.EliminarVertice(vertice);
                e.Handled = true;
                break;
        }
    }

    private void Vertice_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || _dragVertice == null) return;

        var pos = e.GetPosition(editorGrid);
        double dx = pos.X - _dragStart.X;
        double dy = pos.Y - _dragStart.Y;

        _vm.MoverVertice(_dragVertice,
            Math.Max(0, _dragVerticeStartX + dx),
            Math.Max(0, _dragVerticeStartY + dy));
        e.Handled = true;
    }

    private void Vertice_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDragging && sender is FrameworkElement fe)
        {
            _isDragging = false;
            _dragVertice = null;
            fe.ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    private void EditorGrid_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isPanning)
        {
            var p = e.GetPosition(scrollViewer);
            double dx = p.X - _panStartViewport.X;
            double dy = p.Y - _panStartViewport.Y;
            scrollViewer.ScrollToHorizontalOffset(_panScrollStart.X - dx);
            scrollViewer.ScrollToVerticalOffset(_panScrollStart.Y - dy);
            e.Handled = true;
            return;
        }

        if (_vm.ModoActual != ModoEditor.AgregarArista || _vm.OrigenArista == null)
        {
            tempLine.Visibility = Visibility.Collapsed;
            return;
        }

        var pos = e.GetPosition(editorGrid);
        tempLine.X1 = _vm.OrigenArista.X + 18;
        tempLine.Y1 = _vm.OrigenArista.Y + 18;
        tempLine.X2 = pos.X;
        tempLine.Y2 = pos.Y;
        tempLine.Visibility = Visibility.Visible;
    }

    private void EditorGrid_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_isPanning)
        {
            _isPanning = false;
            Cursor = null;
            editorGrid.ReleaseMouseCapture();
            e.Handled = true;
        }
        tempLine.Visibility = Visibility.Collapsed;
    }

    private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var sv = (ScrollViewer)sender;
        var pos = e.GetPosition(editorGrid);

        double factor = e.Delta > 0 ? 1.15 : 1.0 / 1.15;
        double newZoom = Math.Clamp(_currentZoom * factor, ZoomMin, ZoomMax);
        if (Math.Abs(newZoom - _currentZoom) < 0.001) return;

        double oldZoom = _currentZoom;
        double offsetX = sv.HorizontalOffset;
        double offsetY = sv.VerticalOffset;

        double vpX = pos.X * oldZoom - offsetX;
        double vpY = pos.Y * oldZoom - offsetY;

        _currentZoom = newZoom;
        zoomTransform.ScaleX = _currentZoom;
        zoomTransform.ScaleY = _currentZoom;

        sv.ScrollToHorizontalOffset(pos.X * _currentZoom - vpX);
        sv.ScrollToVerticalOffset(pos.Y * _currentZoom - vpY);

        lblZoomStatus.Text = $"Zoom: {(_currentZoom * 100):F0}%";
        e.Handled = true;
    }

    private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
    {
        _vm.EjecutarAlgoritmo();
    }

    private void BtnCerrarBanner_Click(object sender, RoutedEventArgs e)
    {
        _vm.CerrarBanner();
    }

}
