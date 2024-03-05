using Avalonia.Collections;
using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

            var Supp = new Supplier();
            Suppliers = new ObservableCollection<Supplier>(Supp.GetAllSupplier());

            var ther = new Therapist();
            Therapist = new ObservableCollection<Therapist>(ther.GetAllTherapist());

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


        public ObservableCollection<Therapist> Therapist { get; }
        private Therapist _selectedTherapist;
        public Therapist SelectedTherapist
        {
            get => _selectedTherapist;
            set
            {
                _selectedTherapist = value;
                OnPropertyChanged(nameof(SelectedTherapist));
            }
        }

        public ObservableCollection<Supplier> Suppliers { get; }
        private Supplier _selectedsupplier;
        public Supplier SelectedSupplier
        {
            get => _selectedsupplier;
            set
            {
                _selectedsupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }

        }


        private Inventory _selectedProduct;
        public Inventory SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct)); 
            }
        }

        private DateTimeOffset inputDate = DateTimeOffset.Now;
        public DateTimeOffset InputDate
        {
            get { return inputDate; }
            set
            {
                inputDate = value;
                OnPropertyChanged(nameof(InputDate));
            }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }




        public void AddStock()
        {
            try
            {
                Inventory inv = new Inventory();
                inv.StockIn(SelectedSupplier.Id, InputDate.Date, SelectedTherapist.Id);
                inv.AddStockInProduct(SelectedProduct.invID, Quantity, Price);

                SelectedSupplier = null;
                InputDate = DateTime.Now;
                SelectedTherapist = null;
                SelectedProduct = null;
                Quantity = 0;
                Price = 0;

                allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
                Inventory = allInventory;
                _errorText = "Added Item";

            }
            catch (Exception ex) {

                // Display error
                ErrorText = ex.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorText)));
            }
        }


    }
}
