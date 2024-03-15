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
        private ObservableCollection<Promos> promos;
        private ObservableCollection<Inventory> allInventory;
        private ObservableCollection<string> categories;
        private ObservableCollection<OrderItem> orderItems;
        private List<string> prodNames;
        private List<string> allList;
        private OrderItem selectedOrderItem;
        private ViewModelBase currentViewModel;
        private string selectedCategory;
        private string selectedListItem;
        private string searchText;

        public MainWindowViewModel mainWindow { get; set; }
        public ICommand removeItem { get; }

        public POSViewModel()
        {
            removeItem = new RelayCommand(RemoveSelected);
            var inv = new Inventory();
            var serv = new Services();
            var promo = new Promos();
            promos = new ObservableCollection<Promos>(promo.GetPromos());
            services = new ObservableCollection<Services>(serv.GetServices());
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            OrderItems = new ObservableCollection<OrderItem>();
            Inventory = allInventory;
            LoadProductNames();

            Categories = new ObservableCollection<string>
            {
            "Services",
            "Products",
            "Promos",
            };
        }

        public POSViewModel(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow;

            removeItem = new RelayCommand(RemoveSelected);
            var inv = new Inventory();
            var serv = new Services();
            var promo = new Promos();
            promos = new ObservableCollection<Promos>(promo.GetPromos());
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
            "Promos",
            };
        }

        public POSViewModel(MainWindowViewModel mainWindow, ObservableCollection<OrderItem> orderItems)
        {
            this.mainWindow = mainWindow;
            OrderItems = orderItems;

            removeItem = new RelayCommand(RemoveSelected);
            var inv = new Inventory();
            var serv = new Services();
            var promo = new Promos();
            promos = new ObservableCollection<Promos>(promo.GetPromos());
            services = new ObservableCollection<Services>(serv.GetServices());
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
            LoadProductNames();

            Categories = new ObservableCollection<string>
            {
            "Services",
            "Products",
            "Promos",
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

        public ObservableCollection<Promos> Promos
        {
            get { return promos; }
            set
            {
                promos = value;
                OnPropertyChanged(nameof(Promos));
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


                var selectedProduct = Inventory.FirstOrDefault(item => item.prodName == selectedListItem);
                if (selectedProduct == null)
                {
                    var selectedService = Services.FirstOrDefault(item => item.servName == selectedListItem);
                    if (selectedService != null)
                    {
                        UpdateDataGrid(selectedService);
                    }
                    else
                    {
                        var selectedPromo = Promos.FirstOrDefault(item => item.promoName == selectedListItem);
                        if (selectedPromo != null)
                        {
                            UpdateDataGrid(selectedPromo);
                        }
                    }
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

                OnPropertyChanged(nameof(SelectedOrderItem));
            }
        }
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterList();
            }
        }

        private void FilterList()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ProdNames = allList;
            }
            else
            {
                string searchTextLower = SearchText.ToLower().Trim();
                ProdNames = new List<string>(allList.Where(c => c.ToLower().Contains(searchTextLower)));
            }
        }

        private void LoadProductNames()
        {
            var inv = new Inventory();
            List<string> productNames = inv.GetProductName();
            allList = new List<string>(productNames);
            ProdNames = new List<string>(productNames);
        }

        private void LoadServiceNames()
        {
            var serv = new Services();
            List<string> servicesNames = serv.GetServicesName();
            allList = new List<string>(servicesNames);
            ProdNames = new List<string>(servicesNames);
        }
        private void LoadPromoNames()
        {
            var promo = new Promos();
            List<string> promoNames = promo.GetPromoNames();
            allList = new List<string>(promoNames);
            ProdNames = new List<string>(promoNames);

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
            else if (SelectedCategory == "Promos")
            {
                LoadPromoNames();
            }
        }

        private void UpdateDataGrid(object selectedItem)
        {
            if (selectedItem != null)
            {
                string itemName = "";
                string itemType = "None";

                if (selectedItem is Inventory product)
                {
                    itemName = product.prodName;
                    itemType = "Product";
                } else if (selectedItem is Services service)
                {
                    itemName = service.servName;
                    itemType = "Service";
                } else if (selectedItem is Promos promo)
                {
                    itemName = promo.promoName;
                    itemType = "Promo";
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
                    ItemID = (selectedItem is Inventory products) ? products.invID :
                             (selectedItem is Services service) ? service.servID :
                             ((selectedItem is Promos promo) ? promo.promoID : 0),
                    ItemName = itemName,
                    ItemPrice = (selectedItem is Inventory productss) ? productss.prodCost :
                                (selectedItem is Services services) ? services.servCost :
                                ((selectedItem is Promos promos) ? promos.promoCost : 0),
                    Quantity = 1,
                    ItemType = itemType,
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
        private string itemType;

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

        public string ItemType
        {
            get { return itemType; }
            set
            {
                itemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }
    }
}