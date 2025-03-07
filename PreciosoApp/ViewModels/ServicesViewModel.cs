﻿using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Org.BouncyCastle.Tls;
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

        // Selected Data
            // Services
        private Services selectedService;
        public CommissionRate selectedRate;
        public ServiceType selectedServiceType;

        // Selected Promos
        private Promos selectedPromo;
        public CommissionRate selectedPromoRate;
        public Services selectedPromoServices;
        ObservableCollection<PromoServicesItems> selectedPromosServicesUpdate;


        // Add Promos and Add Services Related Variables
            // Add Services
        public ObservableCollection<CommissionRate> commissionRates;
        public ObservableCollection<ServiceType> serviceTypes;
        
            // Add Promos
        public ObservableCollection<CommissionRate> promoCommissionRates;
        public ObservableCollection<PromoServicesItems> serviceItems;
        private ObservableCollection<Services> promoServices;
        private ObservableCollection<Services> promoAllServices;
        private PromoServicesItems selectedServiceItem;
        private string promoName;
        private float promoPrice;
        private string serviceName;
        private float servicePrice;
        private string searchPromoServicesText;
        private int selectedPromoServicesQty;

        public ICommand AddServicesCommand { get; }
        public ICommand UpdateServicesCommand { get; }

        public ICommand AddPromosCommand { get; }
        public ICommand UpdatePromosCommand { get; }
        public ICommand AddPromoServicesCommand { get; }
        public ICommand RemovePromoServicesCommand { get; }


        public ServicesViewModel()
        {
            AddServicesCommand = new RelayCommand(AddServices);
            UpdateServicesCommand = new RelayCommand(UpdateService);
            AddPromosCommand = new RelayCommand(AddPromos);
            UpdatePromosCommand = new RelayCommand(UpdatePromos);
            AddPromoServicesCommand = new RelayCommand(AddToPromoDataGrid);
            RemovePromoServicesCommand = new RelayCommand(RemoveSelectedPromoDataGrid);

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

        public PromoServicesItems SelectedServiceItem
        {
            get { return selectedServiceItem; }
            set
            {
                selectedServiceItem = value;
                OnPropertyChanged(nameof(SelectedServiceItem));
            }
        }

        public CommissionRate SelectedRate
        {
            get { return selectedRate; }
            set
            {
                selectedRate = value;
                OnPropertyChanged(nameof(SelectedRate));
                System.Diagnostics.Debug.WriteLine(selectedRate);

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

        public ObservableCollection<PromoServicesItems> SelectedPromosServicesUpdate
        {
            get { return selectedPromosServicesUpdate; }
            set
            {
                selectedPromosServicesUpdate = value;
                OnPropertyChanged(nameof(SelectedPromosServicesUpdate));
            }
        }

        public ServiceType SelectedServiceType
        {
            get { return selectedServiceType; }
            set
            {
                selectedServiceType = value;
                OnPropertyChanged(nameof(SelectedServiceType));
                System.Diagnostics.Debug.WriteLine(selectedServiceType);
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

        public Services SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        public Promos SelectedPromo
        {
            get { return selectedPromo; }
            set
            {
                selectedPromo = value;
                OnPropertyChanged(nameof(SelectedPromo));
                LoadSelectedPromoServices();
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

        private void LoadSelectedPromoServices()
        {
            if (SelectedPromo != null)
            {
                var promoServ = new PromoTransaction();
                int promoID = SelectedPromo.promoID;
                List<PromoServicesItems> promoServicesList = promoServ.GetPromoServices(promoID)
                    .Select(item =>
                    {
                        var serviceName = GetServiceName(item.serviceID);
                        return new PromoServicesItems { ServiceID = item.serviceID, SelectedServiceName = serviceName, Quantity = item.quantity };
                    })
                    .ToList();

                ServiceItems = new ObservableCollection<PromoServicesItems>(promoServicesList);
            }
        }

        private string GetServiceName(int serviceID)
        {
            var service = new Services();
            string serviceDetails = service.GetServicesName(serviceID);

            return serviceDetails ?? "Unknown"; 
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
            var window = new DialogWindow();
            var serv = new Services();
            // Retrieve values from bound properties
            string servName = ServiceName;
            float servPrice = ServicePrice;
            int servRate = GetSelectedRateID();
            int servType = GetSelectedServiceTypeID();

            if(servName == null || servPrice == null || servRate == null || servType == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                if(servPrice <= 0)
                {
                    window.DialogText = "Price inputted is invalid, please input a valid price!";
                    window.Show();
                }
                else
                {
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
            }
        }

        private void UpdateService()
        {
            var window = new DialogWindow();
            Services serv = new Services();
            int id = SelectedService.servID;
            string name = SelectedService.servName;
            float price = SelectedService.servCost;
            int rateID = SelectedRate?.id ?? GetCommissionRateID(SelectedService.servComm);
            int typeID = SelectedServiceType?.id ?? GetServiceTypeID(SelectedService.servType);

            if (SelectedService == null || name == null || price == null || rateID == null || typeID == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                if(price <= 0)
                {
                    window.DialogText = "Price inputted is invalid, please input a valid price!";
                    window.Show();
                }
                else
                {
                    serv.UpdateServices(id, name, price, rateID, typeID);

                    Services = new ObservableCollection<Services>(serv.GetServices());

                    OnPropertyChanged(nameof(SelectedService));

                    SelectedRate = null;
                    SelectedServiceType = null;
                }
            }
        }

        private void AddToPromoDataGrid()
        {
            var window = new DialogWindow();

            if (SelectedPromosServices == null || SelectedPromoServicesQty == null || SelectedPromoServicesQty == 0)
            {
                if(SelectedPromoServicesQty == 0 || SelectedPromoServicesQty == null)
                {
                    window.DialogText = "Quantity added is 0, please input a valid quantity number!";
                }
                else
                {
                    window.DialogText = "You are missing some required fields! please fill them out!";
                }
                window.Show();
            }
            else
            {
                if (SelectedPromoServicesQty <= 0)
                {
                    window.DialogText = "Quantity inputted is invalid, please input a valid quantity!";
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
        }

        private void RemoveSelectedPromoDataGrid()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var selectedItem = SelectedServiceItem;
                ServiceItems.Remove(selectedItem);
            });
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

        private void UpdatePromos()
        {
            var window = new DialogWindow();
            Promos promo = new Promos();
            var promoServ = new PromoTransaction();
            int promoID = SelectedPromo.promoID;
            string name = SelectedPromo.promoName;
            float price = SelectedPromo.promoCost;
            int rateID = SelectedPromoRate?.id ?? GetCommissionRateID(SelectedPromo.promoRate);

            if(promoID == null || name == null || price == null || rateID == null || SelectedPromo == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                if(price <= 0)
                {
                    window.DialogText = "Price inputted is invalid, please input a valid price!";
                    window.Show();
                }
                else
                {
                    promo.UpdatePromos(promoID, name, price, rateID);

                    promoServ.ClearPromoServices(promoID);

                    for (int i = 0; i < ServiceItems.Count; i++)
                    {
                        var item = ServiceItems[i];
                        if (item != null)
                        {
                            promoServ.InsertPromoServices(promoID, item.serviceID, item.quantity);
                        }
                    }

                    LoadSelectedPromoServices();

                    PromoName = "";
                    PromoPrice = 0;
                    SelectedPromoRate = null;
                    ServiceItems.Clear();
                }
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
