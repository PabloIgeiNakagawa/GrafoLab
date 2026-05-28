using System.Windows;
using System.Windows.Controls;

namespace GrafoLab.Wpf.Views;

public partial class VentanaAyuda : Window
{
    public VentanaAyuda()
    {
        InitializeComponent();
        navList.SelectedIndex = 0;
    }

    private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (navList.SelectedItem is ListBoxItem item && item.Tag is string tag)
        {
            var target = FindName(tag) as FrameworkElement;
            target?.BringIntoView();
        }
    }
}
