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
        try
        {
            Database connection = new Database();
            MySqlConnection conn = connection.getCon();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
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
        new ListItemTemplate(typeof(DashboardViewModel)),
        new ListItemTemplate(typeof(InventoryViewModel)),
        new ListItemTemplate(typeof(AppointmentViewModel)),
        new ListItemTemplate(typeof(POSViewModel)),
        new ListItemTemplate(typeof(SalesReportViewModel)),
        new ListItemTemplate(typeof(HistoryViewModel))
    };
}


public class ListItemTemplate
{
    public ListItemTemplate(Type type)
    {
        ModelType = type;
        Label = type.Name.Replace("ViewModel","");
    }
    public string Label { get; } 
    public Type ModelType { get; }
}