using Avalonia.Collections;
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
        private List<string> prodNames;
        public ObservableCollection<Inventory> Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }
        public InventoryViewModel()
        {
            var inv = new Inventory();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
            LoadProductNames();

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

        public List<string> ProdNames
        {
            get { return prodNames; }
            set
            {
                prodNames = value;
                OnPropertyChanged(nameof(ProdNames));
            }
        }

        private void LoadProductNames()
        {
            var inv = new Inventory();
            List<string> productNames = inv.GetProductName();

            ProdNames = new List<string>(productNames);
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
