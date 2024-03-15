using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private ObservableCollection<Inventory> inventory;
        private ObservableCollection<Inventory> critInventory;
        private ObservableCollection<Inventory> allInventory;
        private ObservableCollection<Supplier> allSupplier;
        public ObservableCollection<ProductType> prodTypes;
        public ObservableCollection<Therapist> Therapist { get; }
        public ObservableCollection<Supplier> suppliers;
        public ObservableCollection<Supplier> supplierData;
        private ProductType selectedProductType;
        private Supplier _selectedsupplier;
        private Therapist _selectedTherapist;
        private Inventory _selectedProduct;
        private Inventory selectedProductData;
        private Supplier selectedSupplierData;
        private DateTimeOffset inputDate = DateTimeOffset.Now;
        private string searchProd;
        private string searchSupp;
        private List<string> prodNames;
        private List<string> suppNames;

        //Multiple Stock In
        private string searchStockInProductsText;
        private int quantity;
        private double price;
        private ObservableCollection<Inventory> stockInProducts;
        private ObservableCollection<Inventory> stockInAllProducts;
        public Inventory selectedStockInProducts;
        public ObservableCollection<StockInProductItems> stockInItems;
        private StockInProductItems selectedStockInItems;




        // Add new service
        public string newProductName { get; set; }
        public float newProductPrice { get; set; }

        // Add new Supplier
        public string newSupplierName { get; set; }
        public string newSupplierNo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _errorText;

        public ICommand AddNewProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand AddNewSupplierCommand { get; }
        public ICommand UpdateSupplierCommand { get; }
        public ICommand AddToStockInGrid { get; }
        public ICommand RemoveFromStockInGrid { get; }

        public InventoryViewModel()
        {
            var inv = new Inventory();
            allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
            stockInAllProducts = new ObservableCollection<Inventory>(inv.GetInventory());
            Inventory = allInventory;
            CritInventory = allInventory;
            StockInProducts = stockInAllProducts;
            LoadProductNames();
            LoadSupplierNames();
            showCriticalStock();


            var Supp = new Supplier();
            allSupplier = new ObservableCollection<Supplier>(Supp.GetAllSupplier());
            Suppliers = allSupplier;
            SupplierData = allSupplier;

            var ther = new Therapist();
            Therapist = new ObservableCollection<Therapist>(ther.GetAllTherapist());

            var productTypes = new ProductType();
            ProdTypes = new ObservableCollection<ProductType>(productTypes.GetProductType());

            StockInItems = new ObservableCollection<StockInProductItems>();

            AddNewProductCommand = new RelayCommand(AddProduct);
            UpdateProductCommand = new RelayCommand(UpdateProduct);
            AddNewSupplierCommand = new RelayCommand(AddSupplier);
            UpdateSupplierCommand = new RelayCommand(UpdateSupplier);
            AddToStockInGrid = new RelayCommand(AddToStockProductDataGrid);
            RemoveFromStockInGrid = new RelayCommand(RemoveSelectedStockInProductGrid);

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

        public ObservableCollection<Inventory> CritInventory
        {
            get { return critInventory; }
            set
            {
                critInventory = value;
                OnPropertyChanged(nameof(CritInventory));
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(suppliers));
            }
        }

        public ObservableCollection<Supplier> SupplierData
        {
            get { return supplierData; }
            set
            {
                supplierData = value;
                OnPropertyChanged(nameof(supplierData));
            }
        }


        public ObservableCollection<ProductType> ProdTypes
        {
            get { return prodTypes; }
            set
            {
                prodTypes = value;
                OnPropertyChanged(nameof(ProdTypes));
            }
        }

        public ObservableCollection<Inventory> StockInProducts
        {
            get { return stockInProducts; }
            set
            {
                stockInProducts = value;
                OnPropertyChanged(nameof(StockInProducts));
            }
        }

        public ObservableCollection<StockInProductItems> StockInItems
        {
            get { return stockInItems; }
            set
            {
                stockInItems = value;
                OnPropertyChanged(nameof(StockInItems));
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

        public string SearchSupp
        {
            get { return searchSupp; }
            set
            {
                searchSupp = value;
                OnPropertyChanged(nameof(SearchSupp));
                FilterSupplier();
            }
        }

        public string SearchStockInProductsText
        {
            get { return searchStockInProductsText; }
            set
            {
                searchStockInProductsText = value;
                OnPropertyChanged(nameof(SearchStockInProductsText));
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

        public List<string> SuppNames
        {
            get { return suppNames; }
            set
            {
                suppNames = value;
                OnPropertyChanged(nameof(SuppNames));
            }
        }

        private void LoadProductNames()
        {
            var inv = new Inventory();
            List<string> productNames = inv.GetProductName();

            ProdNames = new List<string>(productNames);
        }

        private void LoadSupplierNames()
        {
            var supp = new Supplier();
            List<Supplier> suppliers = supp.GetAllSupplier(); 
            List<string> supplierNames = suppliers.Select(s => s.Name).ToList();

            SuppNames = supplierNames;
        }

        public Therapist SelectedTherapist
        {
            get => _selectedTherapist;
            set
            {
                _selectedTherapist = value;
                OnPropertyChanged(nameof(SelectedTherapist));
            }
        }
        
        public Supplier SelectedSupplier
        {
            get => _selectedsupplier;
            set
            {
                _selectedsupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }
        public Supplier SelectedSupplierData
        {
            get { return selectedSupplierData; }
            set
            {
                selectedSupplierData = value;
                OnPropertyChanged(nameof(SelectedSupplierData));
            }
        }

        public Inventory SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct)); 
            }
        }

        public Inventory SelectedProductData
        {
            get { return selectedProductData; }
            set
            {
                selectedProductData = value;
                OnPropertyChanged(nameof(SelectedProductData));
            }
        }

        public ProductType SelectedProductType
        {
            get { return selectedProductType; }
            set
            {
                selectedProductType = value;
                OnPropertyChanged(nameof(SelectedProductType));
            }
        }
        public Inventory SelectedStockInProducts
        {
            get { return selectedStockInProducts; }
            set
            {
                selectedStockInProducts = value;
                OnPropertyChanged(nameof(SelectedStockInProducts));
            }
        }

        public StockInProductItems SelectedStockInItems
        {
            get { return selectedStockInItems; }
            set
            {
                selectedStockInItems = value;
                OnPropertyChanged(nameof(SelectedStockInItems));
            }
        }

        public DateTimeOffset InputDate
        {
            get { return inputDate; }
            set
            {
                inputDate = value;
                OnPropertyChanged(nameof(InputDate));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
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

        private void FilterSupplier()
        {
            if (string.IsNullOrWhiteSpace(SearchSupp))
            {
                SupplierData = allSupplier;
            }
            else
            {
                string searchSuppLower = SearchSupp.ToLower().Trim();
                SupplierData = new ObservableCollection<Supplier>(allSupplier.Where(i => i.Name.ToLower().Contains(searchSuppLower)));
            }

            OnPropertyChanged(nameof(SupplierData));
        }

        private void showCriticalStock()
        {
            CritInventory = new ObservableCollection<Inventory>(allInventory.Where(i => i.prodStock <= i.prodCritLevel));
        }

        public int GetProductTypeID(string type)
        {
            foreach (var item in ProdTypes)
            {
                if (item.productType == type) return item.id;
            }
            return -1;
        }

        public int GetSelectedStockInProductID()
        {
            if (SelectedStockInProducts != null)
            {
                return SelectedStockInProducts.invID;
            }
            else
            {
                return -1;
            }
        }

        public void AddStock()
        {
            try
            {
                Inventory inv = new Inventory();
                int invID = inv.StockIn(SelectedSupplier.Id, InputDate.Date, SelectedTherapist.Id);
                for (int i = 0; i < StockInItems.Count; i++)
                {
                    var item = StockInItems[i];
                    if(item != null)
                    {
                        inv.AddStockInProduct(invID, item.ProductID, Quantity, Price);
                    }
                }

                SelectedSupplier = null;
                InputDate = DateTime.Now;
                SelectedTherapist = null;
                StockInItems.Clear();
                Quantity = 0;
                Price = 0;

                allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
                Inventory = allInventory;
                _errorText = "Added Item";
                showCriticalStock();
            }
            catch (Exception ex) {

                // Display error
                ErrorText = ex.Message;
            }
        }

        private void AddToStockProductDataGrid()
        {
            var window = new DialogWindow();

            if (SelectedStockInProducts == null || Quantity == null || Quantity == 0 || Price == null || Price == 0)
            {
                if (Quantity == 0 || Quantity == null)
                {
                    window.DialogText = "Quantity added is 0, please input a valid quantity number!";
                }else if(Price == null || Price == 0)
                {
                    window.DialogText = "Price added is 0, please input a valid price number!";
                }
                else
                {
                    window.DialogText = "You are missing some required fields! please fill them out!";
                }
                window.Show();
            }
            else
            {
                string serviceName = SelectedStockInProducts.prodName;
                int serviceID = GetSelectedStockInProductID();
                int qty = Quantity;
                double cost = Price;

                var existingOrderItem = StockInItems.FirstOrDefault(item => item.SelectedProductName == serviceName);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity++;
                }
                else
                {
                    StockInItems.Add(new StockInProductItems
                    {
                        ProductID = serviceID,
                        SelectedProductName = serviceName,
                        Quantity = qty,
                        Cost = cost,
                    });
                }
            }
        }

        private void RemoveSelectedStockInProductGrid()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var selectedItem = SelectedStockInItems;
                StockInItems.Remove(selectedItem);
            });
        }

        public void AddProduct()
        {
            try
            {
                var window = new DialogWindow();

                Inventory inv = new Inventory();
                if (newProductName == null || newProductPrice == null || SelectedProductType.id == null || newProductPrice == 0)
                {
                    if (newProductPrice == 0 || newProductPrice == null)
                    {
                        window.DialogText = "Quantity added is 0, please input a valid quantity number!";
                    }
                    else
                    {
                        window.DialogText = "You are missing some required fields! please fill them out!";
                    }
                    window.Show();
                }
                else
                {
                    inv.AddNewProduct(newProductName, newProductPrice, SelectedProductType.id);

                    newProductName = "";
                    newProductPrice = 0;
                    selectedProductType = null;

                    allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
                    Inventory = allInventory;
                    _errorText = "Added Item";
                }
                 
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }

        private void UpdateProduct()
        {
            Inventory inv = new Inventory();
            var window = new DialogWindow();
            int id = SelectedProductData.invID;
            string name = SelectedProductData.prodName;
            float price = SelectedProductData.prodCost;
            int typeID = SelectedProductType?.id ?? GetProductTypeID(SelectedProductData.prodType);
            
            if(id == null || name == null || price == null || typeID == null || price == 0)
            {
                if (price == 0 || price == null)
                {
                    window.DialogText = "Quantity added is 0, please input a valid quantity number!";
                }
                else
                {
                    window.DialogText = "You are missing some required fields! please fill them out!";
                }
                window.Show();
            }
            else
            {
                inv.UpdateProduct(name, price, typeID, id);

                allInventory = new ObservableCollection<Inventory>(inv.GetInventory());
                Inventory = allInventory;

                OnPropertyChanged(nameof(SelectedProductData));

                SelectedProductType = null;
            }
        }

        private void AddSupplier()
        {
            var window = new DialogWindow();
            Supplier supp = new Supplier();

            if (newSupplierName == null || newSupplierNo == null)
            {
                window.DialogText = "You are missing some required fields! please fill them out!";
                window.Show();
            }
            else
            {
                if (newSupplierNo.Length != 11)
                {
                    window.DialogText = "Supplier number must be exactly 11 characters long!";
                    window.Show();
                }
                else
                {
                    supp.addNewSuppler(newSupplierName, newSupplierNo);
                    newSupplierName = "";
                    newSupplierNo = "";
                    SupplierData = new ObservableCollection<Supplier>(supp.GetAllSupplier());
                    OnPropertyChanged(nameof(SupplierData));
                }

            } 
        }

        private void UpdateSupplier()
        {
            var window = new DialogWindow();
            Supplier supp = new Supplier();
            string name = SelectedSupplierData.Name;
            string contact = SelectedSupplierData.Contact;
            int id = SelectedSupplierData.Id;

            if (name == null || contact == null)
            {
                if (contact.Length != 11)
                {
                    window.DialogText = "Supplier number must be exactly 11 characters long!";
                }
                else
                {
                    window.DialogText = "You are missing some required fields! please fill them out!";
                }
                window.Show();
            }
            else
            {
                supp.updateSupplier(name, contact, id);

                SupplierData = new ObservableCollection<Supplier>(supp.GetAllSupplier());
                OnPropertyChanged(nameof(SupplierData));

                OnPropertyChanged(nameof(SelectedProductData));
            }
        }

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

    public class StockInProductItems : ViewModelBase
    {
        public int productID;
        public string selectedProductName;
        public int quantity;
        public double cost;

        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }

        public string SelectedProductName
        {
            get { return selectedProductName; }
            set
            {
                selectedProductName = value;
                OnPropertyChanged(nameof(SelectedProductName));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }
    }
}
