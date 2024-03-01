using Avalonia.Controls;
using Avalonia.Threading;
using PreciosoApp.Models;
using ReactiveUI;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Views;

namespace PreciosoApp.ViewModels
{
    public class POSViewModel: ViewModelBase
    {
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Services> services;
        private ObservableCollection<Inventory> allInventory;
        private ObservableCollection<string> categories;
        private ObservableCollection<OrderItem> orderItems;
        private List<string> prodNames;
        private OrderItem selectedOrderItem;
        private ViewModelBase currentViewModel;
        private string selectedCategory;
        private string selectedListItem;

        public MainWindowViewModel mainWindow { get; set; }
        public ICommand removeItem { get; }

        public POSViewModel()
        {
            removeItem = new RelayCommand(RemoveSelected);
            var inv = new Inventory();
            var serv = new Services();
            services = new ObservableCollection<Services>(serv.GetServices());
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            OrderItems = new ObservableCollection<OrderItem>();
            Inventory = allInventory;
            LoadProductNames();

            Categories = new ObservableCollection<string>
            {
            "Services",
            "Products",
            };
        }

        public POSViewModel(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow;

            removeItem = new RelayCommand(RemoveSelected);
            var inv = new Inventory();
            var serv = new Services();
            services = new ObservableCollection<Services>(serv.GetServices());
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            OrderItems = new ObservableCollection<OrderItem>();
            mainWindow.OrderItems = new ObservableCollection<OrderItem>();
            Inventory = allInventory;
            LoadProductNames();

            Categories = new ObservableCollection<string>
            {
            "Services",
            "Products",
            };
        }

        public ViewModelBase CurrentViewModel
        {

            get { return currentViewModel; }
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
}
        }

        public ObservableCollection<Inventory> Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public ObservableCollection<Services> Services
        {
            get { return services; }
            set
            {
                services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

        public ObservableCollection<string> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
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

        public List<string> ProdNames
        {
            get { return prodNames; }
            set
            {
                prodNames = value;
                OnPropertyChanged(nameof(ProdNames));
            }
        }

        public string SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                LoadSelectedCategory();
            }
        }

        public string SelectedListItem
        {
            get { return selectedListItem; }
            set
            {
                if (selectedListItem == value)
                    return;
                selectedListItem = value;
                System.Diagnostics.Debug.WriteLine(selectedListItem);


                var selectedProduct = Inventory.FirstOrDefault(item => item.prodName == selectedListItem);
                if (selectedProduct == null)
                {
                    var selectedService = Services.FirstOrDefault(item => item.servName == selectedListItem);
                    UpdateDataGrid(selectedService);
                }
                else
                {
                    UpdateDataGrid(selectedProduct);
                }

                OnPropertyChanged(nameof(SelectedListItem));
            }
        }

        public OrderItem SelectedOrderItem
        {
            get { return selectedOrderItem; }
            set
            {
                selectedOrderItem = value;
                System.Diagnostics.Debug.WriteLine(selectedOrderItem);

                OnPropertyChanged(nameof(SelectedOrderItem));
            }
        }

        private void LoadProductNames()
        {
            var inv = new Inventory();
            List<string> productNames = inv.GetProductName();
            ProdNames = new List<string>(productNames);
        }

        private void LoadServiceNames()
        {
            var serv = new Services();
            List<string> servicesNames = serv.GetServicesName();
            ProdNames = new List<string>(servicesNames);

        }

        private void LoadSelectedCategory()
        {
            if (SelectedCategory == "Services")
            {
                LoadServiceNames();
            }
            else if (SelectedCategory == "Products")
            {
                LoadProductNames();
            }
        }

        private void UpdateDataGrid(object selectedItem)
        {
            if (selectedItem != null)
            {
                string itemName = "";

                if (selectedItem is Inventory product)
                {
                    itemName = product.prodName;
                } else if (selectedItem is Services service)
                {
                    itemName = service.servName;
                }

                var existingOrderItem = mainWindow.OrderItems.FirstOrDefault(item => item.ItemName == itemName);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity++;
                }
                else
                {
                    mainWindow.OrderItems.Add(new OrderItem
                    {
                        ItemID = (selectedItem is Inventory products) ? products.invID : ((selectedItem is Services service) ? service.servID : 0),
                        ItemName = itemName,
                        ItemPrice = (selectedItem is Inventory productss) ? productss.prodCost : ((selectedItem is Services services) ? services.servCost : 0),
                        Quantity = 1
                    });
                }
                this.OrderItems = mainWindow.OrderItems;
            }
        }

        private void RemoveSelected()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var selectedItem = SelectedOrderItem; 
                mainWindow.OrderItems.Remove(selectedItem);
            });
            this.OrderItems = mainWindow.OrderItems;
        }

        
    }

    public class OrderItem : ViewModelBase
    {
        private int itemID;
        private string itemName;
        private float itemPrice;
        private int quantity;

        public int ItemID
        {
            get { return itemID; }
            set
            {
                itemID = value;
                OnPropertyChanged(nameof(ItemID));
            }
        }

        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }

        public float ItemPrice
        {
            get { return itemPrice; }
            set
            {
                itemPrice = value;
                OnPropertyChanged(nameof(ItemPrice));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }
}