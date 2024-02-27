using PreciosoApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PreciosoApp.ViewModels
{
    public class AppointmentViewModel : ViewModelBase
    {
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

        public AppointmentViewModel()
        {
            var client = new Client();
            allClients = new ObservableCollection<Client>(client.GetAllClients());
            Client = allClients;
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
    }
}
