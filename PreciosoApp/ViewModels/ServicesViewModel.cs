using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class ServicesViewModel : ViewModelBase
    {
        private string selectedTab;

        // List Services and Promos Related Variables
        private ObservableCollection<Services> services;
        private ObservableCollection<Services> allServices;
        private ObservableCollection<Promos> promos;
        private ObservableCollection<Promos> allPromos;
        private string searchServ;
        private string searchPromo;
        private List<string> servNames;

        // Add Promos and Add Services Related Variables
            // Add Services
        public ObservableCollection<CommissionRate> commissionRates;
        public ObservableCollection<ServiceType> serviceTypes;
        public CommissionRate selectedRate;
        public ServiceType selectedServiceType;
            // Add Promos
        public ObservableCollection<CommissionRate> promoCommissionRates;
        public ObservableCollection<PromoServicesItems> serviceItems;
        private ObservableCollection<Services> promoServices;
        private ObservableCollection<Services> promoAllServices;
        public CommissionRate selectedPromoRate;
        public Services selectedPromoServices;
        private string promoName;
        private float promoPrice;
        private string serviceName;
        private float servicePrice;
        private string searchPromoServicesText;
        private int selectedPromoServicesQty;

        public ICommand AddServicesCommand { get; }
        public ICommand AddPromosCommand { get; }
        public ICommand AddPromoServicesCommand { get; }


        public ServicesViewModel()
        {
            AddServicesCommand = new RelayCommand(AddServices);
            AddPromosCommand = new RelayCommand(AddPromos);
            AddPromoServicesCommand = new RelayCommand(AddToPromoDataGrid);
            var serv = new Services();
            var prmo = new Promos();
            var cRate = new CommissionRate();
            var sType = new ServiceType();
            allServices = new ObservableCollection<Services>(serv.GetServices());
            Services = allServices;
            allPromos = new ObservableCollection<Promos>(prmo.GetPromos());
            promoAllServices = new ObservableCollection<Services>(serv.GetServices());
            Promos = allPromos;
            PromoServices = promoAllServices;
            CommissionRates = new ObservableCollection<CommissionRate>(cRate.GetComissionRate());
            PromoCommissionRates = new ObservableCollection<CommissionRate>(cRate.GetComissionRate());
            ServiceTypes = new ObservableCollection<ServiceType>(sType.GetServiceType());
            ServiceItems = new ObservableCollection<PromoServicesItems>();


            selectedTab = "Services";
            LoadServiceNames();
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

        public ObservableCollection<Services> PromoServices
        {
            get { return promoServices; }
            set
            {
                promoServices = value;
                OnPropertyChanged(nameof(PromoServices));
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

        public ObservableCollection<CommissionRate> CommissionRates
        {
            get { return commissionRates; }
            set
            {
                commissionRates = value;
                OnPropertyChanged(nameof(CommissionRates));
            }
        }

        public ObservableCollection<CommissionRate> PromoCommissionRates
        {
            get { return promoCommissionRates; }
            set
            {
                promoCommissionRates = value;
                OnPropertyChanged(nameof(PromoCommissionRates));
            }
        }

        public ObservableCollection<ServiceType> ServiceTypes
        {
            get { return serviceTypes; }
            set
            {
                serviceTypes = value;
                OnPropertyChanged(nameof(ServiceTypes));
            }
        }

        public ObservableCollection<PromoServicesItems> ServiceItems
        {
            get { return serviceItems; }
            set
            {
                serviceItems = value;
                OnPropertyChanged(nameof(ServiceItems));
            }
        }

        public CommissionRate SelectedRate
        {
            get { return selectedRate; }
            set
            {
                selectedRate = value;
                OnPropertyChanged(nameof(SelectedRate));
            }
        }

        public CommissionRate SelectedPromoRate
        {
            get { return selectedPromoRate; }
            set
            {
                selectedPromoRate = value;
                OnPropertyChanged(nameof(SelectedPromoRate));
                System.Diagnostics.Debug.WriteLine(GetSelectedPromoRateID());

            }
        }

        public ServiceType SelectedServiceType
        {
            get { return selectedServiceType; }
            set
            {
                selectedServiceType = value;
                OnPropertyChanged(nameof(SelectedServiceType));
            }
        }

        public Services SelectedPromosServices
        {
            get { return selectedPromoServices; }
            set
            {
                selectedPromoServices = value;
                OnPropertyChanged(nameof(SelectedPromosServices));
            }
        }

        public string ServiceName
        {
            get { return serviceName; }
            set
            {
                serviceName = value;
                OnPropertyChanged(nameof(ServiceName));
            }
        }

        public float ServicePrice
        {
            get { return servicePrice; }
            set
            {
                servicePrice = value;
                OnPropertyChanged(nameof(ServicePrice));
            }
        }

        public string PromoName
        {
            get { return promoName; }
            set
            {
                promoName = value;
                OnPropertyChanged(nameof(PromoName));
            }
        }

        public float PromoPrice
        {
            get { return promoPrice; }
            set
            {
                promoPrice = value;
                OnPropertyChanged(nameof(PromoPrice));
            }
        }

        public string SearchPromoServicesText
        {
            get { return searchPromoServicesText; }
            set
            {
                searchPromoServicesText = value;
                OnPropertyChanged(nameof(SearchPromoServicesText));
                FilterSelectedPromoServices();
            }
        }

        public int SelectedPromoServicesQty
        {
            get { return selectedPromoServicesQty; }
            set
            {
                selectedPromoServicesQty = value;
                OnPropertyChanged(nameof(SelectedPromoServicesQty));
            }
        }

        public string SearchServ
        {
            get { return searchServ; }
            set
            {
                searchServ = value;
                OnPropertyChanged(nameof(SearchServ));
                selectedTab = "Services";
                FilterInventory();
            }
        }

        public string SearchPromo
        {
            get { return searchPromo; }
            set
            {
                searchPromo = value;
                OnPropertyChanged(nameof(SearchPromo));
                selectedTab = "Promos";
                FilterInventory();
            }
        }

        public List<string> ServNames
        {
            get { return servNames; }
            set
            {
                servNames = value;
                OnPropertyChanged(nameof(ServNames));
            }
        }

        private void LoadServiceNames()
        {
            var serv = new Services();
            List<string> servicesNames = serv.GetServicesName();

            ServNames = new List<string>(servicesNames);
        }

        public int GetCommissionRateID(float rate)
        {
            foreach (var item in CommissionRates)
            {
                if (item.rateValue == rate) return item.id;
            }
            return -1;
        }

        public int GetServiceTypeID(string type)
        {
            foreach (var item in ServiceTypes)
            {
                if (item.serviceType == type) return item.id;
            }
            return -1;
        }

        public int GetSelectedRateID()
        {
            if (selectedRate != null)
            {
                return selectedRate.id;
            }
            else
            {
                return -1;
            }
        }

        public int GetSelectedPromoRateID()
        {
            if (selectedPromoRate != null)
            {
                return selectedPromoRate.id;
            }
            else
            {
                return -1;
            }
        }

        public int GetSelectedServiceTypeID()
        {
            if (selectedServiceType != null)
            {
                return selectedServiceType.id;
            }
            else
            {
                return -1;
            }
        }

        public int GetSelectedPromoServicesID()
        {
            if (SelectedPromosServices != null)
            {
                return SelectedPromosServices.servID;
            }
            else
            {
                return -1;
            }
        }


        private void FilterInventory()
        {
            if (string.IsNullOrWhiteSpace(SearchServ))
            {
                if (selectedTab == "Services")
                {
                    Services = allServices;
                }
                else if (selectedTab == "Promos")
                {
                    Promos = allPromos;
                }
            }
            else
            {
                if (selectedTab == "Services")
                {
                    string searchServLower = SearchServ.ToLower().Trim();
                    Services = new ObservableCollection<Services>(allServices.Where(i => i.servName.ToLower().Contains(searchServLower)));
                }
                else if (selectedTab == "Promos")
                {
                    string searchServLower = SearchPromo.ToLower().Trim();
                    Promos = new ObservableCollection<Promos>(allPromos.Where(i => i.promoName.ToLower().Contains(searchServLower)));
                }
            }
        }

        private void FilterSelectedPromoServices()
        {
            if (string.IsNullOrWhiteSpace(SearchPromoServicesText))
            {
                PromoServices = promoAllServices;
            }
            else
            {
                string searchTextLower = SearchPromoServicesText.ToLower().Trim();
                PromoServices = new ObservableCollection<Services>(promoAllServices.Where(c => c.servName.ToLower().Contains(searchTextLower)));
            }
        }

        private void AddServices()
        {
            var serv = new Services();
            // Retrieve values from bound properties
            string servName = ServiceName;
            float servPrice = ServicePrice;
            int servRate = GetSelectedRateID();
            int servType = GetSelectedServiceTypeID();

            // Call method to add client to database
            serv.addClient(servName, servPrice, servRate, servType);

            allServices = new ObservableCollection<Services>(serv.GetServices());
            selectedTab = "Services";
            FilterInventory();

            // Optionally, clear input fields after adding client
            ServiceName = "";
            ServicePrice = 0;
            SelectedRate = null;
            SelectedServiceType = null;
        }

        private void AddToPromoDataGrid()
        {
            var window = new DialogWindow();

            if (SelectedPromosServices == null || SelectedPromoServicesQty == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                string serviceName = SelectedPromosServices.servName;
                int serviceID = GetSelectedPromoServicesID();
                int qty = selectedPromoServicesQty;

                var existingOrderItem = ServiceItems.FirstOrDefault(item => item.SelectedServiceName == serviceName);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity++;
                }
                else
                {
                    ServiceItems.Add(new PromoServicesItems
                    {
                        ServiceID = serviceID,
                        SelectedServiceName = serviceName,
                        Quantity = qty,
                    });
                }
            }
        }

        private void AddPromos()
        {
            var promo = new Promos();
            var promoServ = new PromoTransaction();
            var window = new DialogWindow();

            if (PromoName == null || PromoPrice == null || PromoCommissionRates == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                int promoID = promo.InsertPromos(PromoName, PromoPrice, GetSelectedPromoRateID());

                for (int i = 0; i < ServiceItems.Count; i++)
                {
                    var item = ServiceItems[i];
                    if (item != null)
                    {
                        promoServ.InsertPromoServices(promoID, item.serviceID, item.quantity);
                    }
                }

                PromoName = "";
                PromoPrice = 0;
                SelectedPromoRate = null;
                ServiceItems.Clear();
            }
        }
    }

    public class PromoServicesItems : ViewModelBase
    {
        public int serviceID;
        public string selectedServiceName;
        public int quantity;

        public int ServiceID
        {
            get { return serviceID; }
            set
            {
                serviceID = value;
                OnPropertyChanged(nameof(ServiceID));
            }
        }

        public string SelectedServiceName
        {
            get { return selectedServiceName; }
            set
            {
                selectedServiceName = value;
                OnPropertyChanged(nameof(SelectedServiceName));
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
