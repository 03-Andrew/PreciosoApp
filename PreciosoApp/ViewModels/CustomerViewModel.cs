using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        public ICommand AddClientCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand UpdateServStatus { get; }

        public CustomerViewModel()
        {
            var client = new Client();
            allClients = new ObservableCollection<Client>(client.GetAllClients());
            Client = allClients;

            var genders = new TypesQueries();
            Genders = new ObservableCollection<Gender>(genders.GetGenders());
            AddClientCommand = new RelayCommand(AddClient);

            FilterCommand = new RelayCommand(FilterClients);
            ServiceStatus = new ObservableCollection<ServiceStatus>(new ServiceStatus().GetServicesStatus());

            UpdateServStatus = new RelayCommand(UpdateStatus);
        }

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

        private ObservableCollection<DailyReport> _prodTransactions;
        public ObservableCollection<DailyReport> ProdTransactions
        {
            get { return _prodTransactions; }
            set
            {
                _prodTransactions = value;
                OnPropertyChanged(nameof(ProdTransactions));
            }
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
            if (string.IsNullOrWhiteSpace(SearchText) && _startDate == DateTime.Today.AddYears(-100) && _endDate == DateTime.MinValue)
            {
                Client = allClients;
                return;
            }

            var filteredClients = allClients.AsQueryable();


            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchTextLower = SearchText.ToLower().Trim();
                filteredClients = filteredClients.Where(c => c.Name.ToLower().Contains(searchTextLower));
            }

            if (_startDate != DateTime.MinValue)
            {
                filteredClients = filteredClients.Where(c => c.DOB >= _startDate);
            }

            if (_endDate != DateTime.MinValue)
            {
                filteredClients = filteredClients.Where(c => c.DOB <= _endDate);
            }

            Client = new ObservableCollection<Client>(filteredClients.ToList());
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
                var tran = new DailyReport();
                ProdTransactions = new ObservableCollection<DailyReport>(
                     tran.GetDailyReports().Where(tr =>
                         tr.Client.Contains(SelectedClient.Name) &&
                         tr.Type.Contains("product")
                     )
                 );
                ServicePromoUsed = new ObservableCollection<ServicePromoHistory>(
                    new ServicePromoHistory().GetServicePromoHistory()
                    .Where(tr => tr.Client.Contains(SelectedClient.Name))
                );
            }
            else
            {
                ProdTransactions = _prodTransactions;
                ServicePromoUsed = _servicePromoUsed;
                Note = _note;
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
            catch (Exception ex)
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

        private DateTime _startDate = DateTime.Today.AddYears(-100);
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                FilterClients();
            }
        }
        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                FilterClients();
            }
        }

        private ObservableCollection<ServicePromoHistory> _servicePromoUsed;
        public ObservableCollection<ServicePromoHistory> ServicePromoUsed
        {
            get { return _servicePromoUsed; }
            set
            {
                _servicePromoUsed = value;
                OnPropertyChanged(nameof(ServicePromoUsed));
            }
        }

        private ServicePromoHistory _selectedPromoService;
        public ServicePromoHistory SelectedPromoService
        {
            get { return _selectedPromoService; }
            set
            {
                _selectedPromoService = value;
                OnPropertyChanged(nameof(SelectedPromoService));
                setNotes();
            }
        }

        public void setNotes()
        {
            if (SelectedPromoService != null)
            {
                Note = SelectedPromoService.Notes;
            }
            else
            {
                Note = _note;
            }
        }

        private string _note = "";
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        public ObservableCollection<ServiceStatus> ServiceStatus { get; }

        public void UpdateStatus()
        {
            if (SelectedPromoService == null)
            {
                return;
            }
            try
            {
                if (SelectedPromoService.Type == "promo")
                {
                    int newStatus = SelectedPromoService.Status == "in progress" ? 2 : 1;
                    int transactionID = SelectedPromoService.TransactionID;

                    new ServiceStatus().UpdateStatusPromo(transactionID, SelectedPromoService.Availed, newStatus);
                }
                else
                {
                    int newStatus = SelectedPromoService.Status == "in progress" ? 2 : 1;
                    int serviceId = GetServiceId(SelectedPromoService.Availed);
                    int transactionId = SelectedPromoService.TransactionID;

                    new ServiceStatus().UpdateStatusAppointment(transactionId, serviceId, newStatus);
                }
            }
            catch (Exception ex)
            {
                var window = new DialogWindow();
                window.DialogText = "Error: " + ex;
                window.Show();
            }

            filteredTransaction();
        }


        public int GetStatusId(string status)
        {
            foreach (var item in ServiceStatus)
            {
                if (item.Status == status) return item.StatusId;
            }
            return -1;
        }

        public int GetServiceId(string service)
        {
            foreach(var item in new Services().GetServices())
            {
                if (item.servName == service) return item.servID;
            }
            return -1;
        }

        public int GetPromoId(string promo)
        {
            foreach(var item in new Promos().GetPromos())
            {
                if(item.promoName == promo) return item.promoID;
            }
            return -1;
        }
    }
}
