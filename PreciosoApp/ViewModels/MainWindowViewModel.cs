using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows.Input;

namespace PreciosoApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ICommand moveToCheckout { get; }
    public ICommand logoutCommand { get; }
    public MainWindow mainwindow { get; }

    public ObservableCollection<OrderItem> orderItems;
    public string username { get; set; }
    public string UserType { get; set; }

    public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>();

    public MainWindowViewModel()
    {
        moveToCheckout = new RelayCommand(MoveToCheckoutWindow);
        logoutCommand = new RelayCommand(Logout);
    }

    public MainWindowViewModel(String usertype, MainWindow mainWindow)
    {
        mainwindow = mainWindow;

        UserType = usertype;
        moveToCheckout = new RelayCommand(MoveToCheckoutWindow);
        logoutCommand = new RelayCommand(Logout);

        if (UserType == "1")
        {
            Items1();
        }
        else if (UserType == "2")
        {
            Items2();
        }
    }



    public string Greeting => setString();
    public string Title => "Precioso Spa";
    public string setString()
    {
        Database db = new Database();
        return "Hello, "+username;

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

    private void Items1()
    {
        Items.Clear(); // Clear existing items

        Items.Add(new ListItemTemplate(typeof(POSViewModel), "cart_regular"));
    }

    private void Items2()
    {
        Items.Clear(); // Clear existing items

        Items.Add(new ListItemTemplate(typeof(DashboardViewModel), "desktop_regular"));
        Items.Add(new ListItemTemplate(typeof(InventoryViewModel), "toolbox_regular"));
        Items.Add(new ListItemTemplate(typeof(AppointmentViewModel), "person_clock_regular"));
        Items.Add(new ListItemTemplate(typeof(POSViewModel), "cart_regular"));
        Items.Add(new ListItemTemplate(typeof(ServicesViewModel), "calendar_star_regular"));
        Items.Add(new ListItemTemplate(typeof(SalesReportViewModel), "data_bar_vertical_regular"));
        Items.Add(new ListItemTemplate(typeof(HistoryViewModel), "book_regular"));
        Items.Add(new ListItemTemplate(typeof(TherapistViewModel), "video_person_regular"));
    }

    /*
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
    */

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
        var window = new DialogWindow();
        bool hasNegativeQuantity = OrderItems.Any(item => item.Quantity < 0);

        if (hasNegativeQuantity)
        {
            window.DialogText = "An order has a negative quantity! Please change the value to a valid number!";
            window.Show();
        }
        else
        {
            var checkoutViewModel = new CheckoutViewModel(OrderItems, this);
            CurrentPage = checkoutViewModel;
        }
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

    public void Logout()
    {
        var loginWindowView = new LoginWindowView();
        var loginWindowViewModel = new LoginWindowViewModel(loginWindowView);
        loginWindowView.DataContext = loginWindowViewModel;
        loginWindowView.Show();
        mainwindow.Close();
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