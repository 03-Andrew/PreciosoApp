using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.ViewModels
{
    public class POSViewModel:ViewModelBase
    {
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Inventory> allInventory;
        private ObservableCollection<string> categories;
        private List<string> prodNames;
        private string selectedCategory;

        public POSViewModel()
        {
            var inv = new Inventory();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
            LoadProductNames();

            Categories = new ObservableCollection<string>
            {
            "Services",
            "Products",
            };
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

        public List<string> ProdNames
        {
            get { return prodNames; }
            set
            {
                prodNames = value;
                OnPropertyChanged(nameof(ProdNames));
            }
        }

        public ObservableCollection<string> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public string SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                LoadSelectedCategory();
            }
        }

        private void LoadProductNames()
        {
            var inv = new Inventory();
            List<string> productNames = inv.GetProductName();
            ProdNames = new List<string>(productNames);
        }

        private void LoadServiceNames()
        {
            var serv = new Services();
            List<string> servicesNames = serv.GetServicesName();
            ProdNames = new List<string>(servicesNames);

        }

        private void LoadSelectedCategory()
        {
            if (SelectedCategory == "Services")
            {
                LoadServiceNames();
            }
            else if (SelectedCategory == "Products")
            {
                LoadProductNames();
            }
        }
    }
}
