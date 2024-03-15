using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows.Input;

namespace PreciosoApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ICommand moveToCheckout { get; }
    public ObservableCollection<OrderItem> orderItems;

    public string Greeting => setString();
    public string setString()
    {
        Database db = new Database();
        return db.TestConnection() + " " + DateTime.Now;

    }

    [ObservableProperty]
    public ViewModelBase _currentPage = new DashboardViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        var instance = Activator.CreateInstance(value.ModelType);
        if (instance == null) return;
        var propertyInfo = instance.GetType().GetProperty("mainWindow");

        if (value.ModelType == typeof(POSViewModel))
        {
            System.Diagnostics.Debug.WriteLine("Success");
            instance = Activator.CreateInstance(value.ModelType, this);
        }

        CurrentPage = (ViewModelBase)instance;
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(DashboardViewModel), "desktop_regular"),
        new ListItemTemplate(typeof(InventoryViewModel), "toolbox_regular"),
        new ListItemTemplate(typeof(AppointmentViewModel), "person_clock_regular"),
        new ListItemTemplate(typeof(POSViewModel), "cart_regular"),
        new ListItemTemplate(typeof(ServicesViewModel), "calendar_star_regular"),
        new ListItemTemplate(typeof(SalesReportViewModel), "data_bar_vertical_regular"),
        new ListItemTemplate(typeof(HistoryViewModel), "book_regular"),
        new ListItemTemplate(typeof(TherapistViewModel), "video_person_regular")
    };

    public MainWindowViewModel()
    {
        moveToCheckout = new RelayCommand(MoveToCheckoutWindow);
    }

    public ObservableCollection<OrderItem> OrderItems
    {
        get { return orderItems; }
        set
        {
            orderItems = value;
            OnPropertyChanged(nameof(OrderItems));
        }
    }

    public void MoveToCheckoutWindow()
    {
        var checkoutViewModel = new CheckoutViewModel(OrderItems, this);
        CurrentPage = checkoutViewModel;
    }

    public void MoveToPOSView()
    {
        var posViewModel = new POSViewModel(this, OrderItems);
        CurrentPage = posViewModel;
    }

    public void dialogWindowButton()
    {
        var posViewModel = new POSViewModel();
        CurrentPage = posViewModel;
    }
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