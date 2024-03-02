    using PreciosoApp.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    namespace PreciosoApp.ViewModels
    {
        public class HistoryViewModel : ViewModelBase
        {
            private readonly ObservableCollection<ProductSoldTransactions> allPTransactions;
            private ObservableCollection<ProductSoldTransactions> pTransactions;
            private string searchProd;
            private List<string> prodNames;

            public ObservableCollection<ProductSoldTransactions> PTransactions
            {
                get { return pTransactions; }
                set
                {
                    pTransactions = value;
                    OnPropertyChanged(nameof(PTransactions));
                }
            }

            private ObservableCollection<ProductSold> pSold;
            private ObservableCollection<ProductSold> allPSold;

            public ObservableCollection<ProductSold> PSold
            {
                get { return new ObservableCollection<ProductSold>(allPSold); }
                set
                {
                    allPSold = new ObservableCollection<ProductSold>(value);
                    OnPropertyChanged(nameof(PSold));
                }
            }


        public HistoryViewModel()
            {
                allPTransactions = new ObservableCollection<ProductSoldTransactions>(
                    new ProductSoldTransactions().GetPTransactions());
                pTransactions = allPTransactions;
                PSold = new ObservableCollection<ProductSold>(new ProductSold().GetProductsSold()); // Initialize with all products sold
            }

            private ProductSoldTransactions _selectedRow;
            public ProductSoldTransactions SelectedRow
            {
                get => _selectedRow;
                set
                {
                    _selectedRow = value;
                    OnPropertyChanged(nameof(SelectedRow)); // Notify UI of changes
                    FilterPSold(); // Filter PSold based on the selected row
                }
            }

            private void FilterPSold()
            {
                if (SelectedRow != null)
                {
                    string selectedRowIdAsString = SelectedRow.Id.ToString(); // Convert ID to string
                    PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId.ToString().Contains(selectedRowIdAsString)));
               

                 }
           }

        }
    }
