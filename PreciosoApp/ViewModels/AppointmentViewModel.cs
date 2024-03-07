﻿using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class AppointmentViewModel : ViewModelBase
    {
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));

            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _middleInitial;
        public string MiddleInitial
        {
            get { return _middleInitial; }
            set
            {
                _middleInitial = value;
                OnPropertyChanged(nameof(MiddleInitial));
            }
        }

        private string _contactInfo;
        public string ContactInfo
        {
            get { return _contactInfo; }
            set
            {
                _contactInfo = value;
                OnPropertyChanged(nameof(ContactInfo));
            }
        }

        private DateTimeOffset _dob = DateTimeOffset.Now;
        public DateTimeOffset DOB
        {
            get { return _dob; }
            set
            {
                _dob = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        private Gender _selectedGender;

        public Gender SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
            }
        }

        public int GetSelectedGenderId()
        {
            if (_selectedGender != null)
            {
                return _selectedGender.Id;
            }
            else
            {
                return -1;
            }
        }

        public ObservableCollection<Gender> Genders { get; }

        private ObservableCollection<Client> clients;
        private ObservableCollection<Client> allClients;
        public ObservableCollection<Client> Client
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        private ObservableCollection<ProductSoldTransactions> _transactions;
        private ObservableCollection<ProductSoldTransactions> allTransactions;
        public ObservableCollection<ProductSoldTransactions> Transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));   
            }
        }


        public ICommand AddClientCommand { get; }

        public AppointmentViewModel()
        {
            var client = new Client();
            allClients = new ObservableCollection<Client>(client.GetAllClients());
            Client = allClients;

            var genders = new TypesQueries();
            Genders = new ObservableCollection<Gender>(genders.GetGenders());
            AddClientCommand = new RelayCommand(AddClient);

         

        }

        private string searchText;
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



        private void AddClient()
        {
            var client = new Client();
            // Retrieve values from bound properties
            string lastName = LastName;
            string firstName = FirstName;
            string middleInitial = MiddleInitial;
            string contactInfo = ContactInfo;
            DateTime dob = DOB.Date;
            int gender = GetSelectedGenderId();

            // Call method to add client to database
            client.addClient(lastName, firstName, dob, contactInfo, gender);

            allClients = new ObservableCollection<Client>(client.GetAllClients());
            FilterClients();

            // Optionally, clear input fields after adding client
            LastName = "";
            FirstName = "";
            MiddleInitial = "";
            ContactInfo = "";
            DOB = DateTimeOffset.Now;
            SelectedGender = null;
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
                filteredTransaction();
            }
        }

       

        private void filteredTransaction()
        {
            if (_selectedClient != null)
            {
                var tran = new ProductSoldTransactions();
                Transactions = new ObservableCollection<ProductSoldTransactions>(tran.GetPTransactions().Where(tr => tr.ClientName.Contains(SelectedClient.Name)));
            }
            else
            {
                Transactions = _transactions;
            }
        } 

        public void updateCustomer()
        {
            try
            {
                var client = new Client();
                int id = SelectedClient.Id;
                string name = SelectedClient.Name;
                DateTime dob = SelectedClient.DOB;
                string contact = SelectedClient.ContactInfo;
                int gender = SelectedGender?.Id ?? GetGenderId(SelectedClient.Gender);


                client.updateClient(id, name, dob, contact, gender);

                Client = new ObservableCollection<Client>(client.GetAllClients());

                OnPropertyChanged(nameof(SelectedClient));

                SelectedGender = null;
            } 
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            

        }

        public int GetGenderId(string gender)
        {
            foreach (var item in Genders)
            {
                if (item.GenderType == gender) return item.Id;
            }
            return -1;
        }

        
    }
}
