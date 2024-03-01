using CommunityToolkit.Mvvm.ComponentModel;
using PreciosoApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PreciosoApp.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public ObservableCollection<OrderItem> orderItems;
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Inventory> allInventory;
        private List<string> clientNames;
        private List<string> thrpstNames;
        public string Notes { get; set; } = string.Empty;

        public CheckoutViewModel(ObservableCollection<OrderItem> orderItems)
        {
            OrderItems = orderItems;
            var inv = new Inventory();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
            LoadClientNames();
            LoadTherapistNames();
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

        private void CheckoutButton()
        {
            for (int i = 0; i < OrderItems.Count; i++) 
            {
                var item = OrderItems[i];
                if (item != null)
                {
                    
                }
            }
        }

    }
}
