using CommunityToolkit.Mvvm.Input;
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
        private string searchText;
        public ObservableCollection<Client> Client
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        public ICommand AddClientCommand { get; }

        public AppointmentViewModel()
        {
            var client = new Client();
            allClients = new ObservableCollection<Client>(client.GetAllClients());
            var genders = new TypesQueries();
            Genders = new ObservableCollection<Gender>(genders.GetGenders());
            Client = allClients;
            AddClientCommand = new RelayCommand(AddClient);
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



        private void AddClient()
        {
            var client = new ClientQueries();
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
            }
        }


    }
}
