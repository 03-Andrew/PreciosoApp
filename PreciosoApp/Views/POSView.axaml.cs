using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PreciosoApp.ViewModels;

namespace PreciosoApp.Views;

public partial class POSView : UserControl
{
    public POSView()
    {
        InitializeComponent();
        var listbox = this.FindControl<ListBox>("OrderListBox");
        listbox.SelectionChanged += ListBox_SelectionChanged;
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(DataContext is POSViewModel vm)
        vm.SelectedListItem = null;
    }
}