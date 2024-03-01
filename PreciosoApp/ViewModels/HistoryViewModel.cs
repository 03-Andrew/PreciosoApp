using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ObservableCollection<ProductSold> PSold { get; set; } // Read-only property

        public HistoryViewModel()
        {
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(
                new ProductSoldTransactions().GetPTransactions());
            pTransactions = allPTransactions;
            PSold = new ObservableCollection<ProductSold>(new ProductSold().GetProductsSold());
        }


        private ProductSoldTransactions _selectedRow;
        public ProductSoldTransactions SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                if (_selectedRow != null)
                {
                    // Assuming ProductSold has a property related to ProductSoldTransactions (e.g., Transaction)
                    PSold = new ObservableCollection<ProductSold>(
                        PSold.Where(p => p.TransactionId == _selectedRow.Id)); // Filter PSold based on transaction ID
                }
                else
                {
                    // Reset PSold to all products if no row is selected
                    PSold = new ObservableCollection<ProductSold>(new ProductSold().GetProductsSold());
                }
                OnPropertyChanged(nameof(PSold)); // Notify UI of changes
            }
        }


    }
}
