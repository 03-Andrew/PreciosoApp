using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using MySql.Data.MySqlClient;
using PreciosoApp.Models;
using System;
using System.Collections.ObjectModel;

namespace PreciosoApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => setString();
    public string setString()
    {
        Database db = new Database();
        return db.TestConnection() + "Yes";

    }

    [ObservableProperty]
    private ViewModelBase _currentPage = new DashboardViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance == null) return;
        CurrentPage = (ViewModelBase)instance;
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(DashboardViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(InventoryViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(AppointmentViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(POSViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(SalesReportViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(HistoryViewModel), "desktop_regular")
    };


}


public class ListItemTemplate
{
    public ListItemTemplate(Type type, string icon)
    {
        ModelType = type;
        Label = type.Name.Replace("ViewModel","");

        Application.Current!.TryFindResource(icon, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }
    public string Label { get; } 
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
}