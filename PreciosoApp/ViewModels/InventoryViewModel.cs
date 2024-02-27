using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Inventory> allInventory;
        private string searchProd;

        public ObservableCollection<Inventory> Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public string SearchProd
        {
            get { return searchProd; }
            set
            {
                searchProd = value;
                OnPropertyChanged(nameof(SearchProd));
                FilterInventory();
            }
        }

        public InventoryViewModel()
        {
            var inv = new Inventory();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
        }

        private void FilterInventory()
        {
            if (string.IsNullOrWhiteSpace(SearchProd))
            {
                Inventory = allInventory;
            }
            else
            {
                string searchProdLower = SearchProd.ToLower().Trim();
                Inventory = new ObservableCollection<Inventory>(allInventory.Where(i => i.prodName.ToLower().Contains(searchProdLower)));
            }
        }
    }
}
