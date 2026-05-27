using System.Windows;

namespace GrafoLab.Wpf.Views;

public partial class VentanaPesoArista : Window
{
    public double? Peso { get; private set; }
    public bool EsDirigida { get; private set; }

    public VentanaPesoArista()
    {
        InitializeComponent();
    }

    private void BtnAceptar_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPeso.Text))
        {
            if (!double.TryParse(txtPeso.Text, out var peso))
            {
                MessageBox.Show("El peso debe ser un numero valido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPeso.Focus();
                return;
            }
            Peso = peso;
        }

        EsDirigida = chkDirigida.IsChecked == true;
        DialogResult = true;
    }
}
