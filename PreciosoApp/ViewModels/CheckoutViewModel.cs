﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public ObservableCollection<OrderItem> orderItems;
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Inventory> allInventory;
        private ObservableCollection<Client> clients;
        private ObservableCollection<Client> allClients;
        private MainWindowViewModel mainWindow;
        private Client selectedClientData;
        private List<string> clientNames;
        private List<string> thrpstNames;
        private List<string> mopNames;
        private string searchText;
        private string selectedClient;
        private string selectedTherapist;
        private string selectedMOP;
        private DateTimeOffset? selectedDateTime;
        private int selectedClientID;
        private int selectedTherapistID;
        private int selectedMOPID;
        public ICommand checkOut { get; }

        public string Notes { get; set; } = string.Empty;

        public CheckoutViewModel(ObservableCollection<OrderItem> orderItems , MainWindowViewModel mwVM)
        {
            mainWindow = mwVM;

            OrderItems = orderItems;
            var inv = new Inventory();
            var client = new Client();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            allClients = new ObservableCollection<Client>(client.GetAllClients());
            Inventory = allInventory;
            Client = allClients;
            SelectedDateTime = DateTimeOffset.Now.LocalDateTime;
            System.Diagnostics.Debug.WriteLine(selectedDateTime);
            checkOut = new RelayCommand(CheckoutButton);

            LoadClientNames();
            LoadTherapistNames();
            LoadMOPNames();
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

        public ObservableCollection<Inventory> Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public ObservableCollection<Client> Client
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        public Client SelectedClientData
        {
            get => selectedClientData;
            set
            {
                selectedClientData = value;
                if(selectedClientData != null)
                {
                    var client = new Client();
                    selectedClient = selectedClientData.Name;
                    selectedClientID = client.GetClientID(selectedClient).FirstOrDefault();
                }
                else
                {
                    selectedClient = "";
                }
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        public List<string> ClientNames
        {
            get { return clientNames; }
            set
            {
                clientNames = value;
                OnPropertyChanged(nameof(ClientNames));
            }
        }

        public List<string> TherapistNames
        {
            get { return thrpstNames; }
            set
            {
                thrpstNames = value;
                OnPropertyChanged(nameof(TherapistNames));
            }
        }

        public List<string> MOPNames
        {
            get { return mopNames; }
            set
            {
                mopNames = value;
                OnPropertyChanged(nameof(MOPNames));
            }
        }

        public int SelectedClientID
        {
            get { return selectedClientID; }
            set
            {
                selectedClientID = value;
                OnPropertyChanged(nameof(SelectedClientID));
            }
        }


        public string SelectedClient
        {
            get { return selectedClient; }
            set
            {
                var client = new Client();
                selectedClient = value;
                selectedClientID = client.GetClientID(selectedClient).FirstOrDefault();
                OnPropertyChanged(nameof(selectedClient));
            }
        }

        public int SelectedTherapistID
        {
            get { return selectedTherapistID; }
            set
            {
                selectedTherapistID = value;
                OnPropertyChanged(nameof(selectedTherapistID));
            }
        }

        public string SelectedTherapist
        {
            get { return selectedTherapist; }
            set
            {
                var therapist = new Therapist();
                selectedTherapist = value;
                selectedTherapistID = therapist.GetTherapistID(selectedTherapist).FirstOrDefault();
                OnPropertyChanged(nameof(SelectedTherapist));
            }
        }

        public DateTimeOffset? SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }

        public int SelectedMOPID
        {
            get { return selectedMOPID; }
            set
            {
                selectedMOPID = value;
                OnPropertyChanged(nameof(SelectedMOPID));
            }
        }

        public string SelectedMOP
        {
            get { return selectedMOP; }
            set
            {
                var mop = new ModeOfPayment();
                selectedMOP = value;
                selectedMOPID = mop.GetMOPID(selectedMOP).FirstOrDefault();
                OnPropertyChanged(nameof(SelectedMOP));
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterClients();
            }
        }

        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Client = allClients;
            }
            else
            {
                string searchTextLower = SearchText.ToLower().Trim();
                Client = new ObservableCollection<Client>(allClients.Where(c => c.Name.ToLower().Contains(searchTextLower)));
            }
        }

        private void LoadClientNames()
        {
            var client = new Client();
            List<string> clientNames = client.GetClientName();

            ClientNames = new List<string>(clientNames);
        }

        private void LoadTherapistNames()
        {
            var therapist = new Therapist();
            List<string> therapistNames = therapist.GetTherapistName();

            TherapistNames = new List<string>(therapistNames);
        }

        private void LoadMOPNames()
        {
            var mop = new ModeOfPayment();
            List<string> mopNames = mop.GetMOPName();

            MOPNames = new List<string>(mopNames);
        }

        private void CheckoutButton()
        {
            var trnsc = new Transactions();
            var pSold = new ProductSold();
            var sUsed = new ServicesUsed();
            var pTrnsc = new PromoTransaction();
            var window = new DialogWindow();

            if (selectedDateTime == null || selectedClientID == null || selectedTherapistID == null || selectedMOP == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                int trnscID = trnsc.InsertTransaction(SelectedDateTime, selectedClientID, selectedTherapistID, selectedMOPID, Notes);

                for (int i = 0; i < OrderItems.Count; i++)
                {
                    var item = OrderItems[i];
                    if (item != null)
                    {
                        switch (item.ItemType)
                        {
                            case "Product":
                                pSold.InsertProductSold(trnscID, item.ItemID, item.Quantity);
                                break;
                            case "Service":
                                sUsed.insertServiceUsed(trnscID, item.ItemID, item.Quantity);
                                break;
                            case "Promo":
                                pTrnsc.insertPromoTransaction(trnscID, item.ItemID, item.Quantity);
                                break;

                        }
                    }
                }
                window.Show();

                mainWindow.CurrentPage = new POSViewModel(mainWindow);
                System.Diagnostics.Debug.WriteLine("Success");
            }
        }
    }
}
