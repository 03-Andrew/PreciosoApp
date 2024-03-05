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
        private ObservableCollection<ProductSoldTransactions> allPTransactions;
        private ObservableCollection<ProductSoldTransactions> pTransactions;
        private ObservableCollection<ProductSold> pSold;
        private ObservableCollection<ProductSold> allPSold;

        public ObservableCollection<ProductSoldTransactions> PTransactions
        {
            get { return pTransactions; }
            set
            {
                pTransactions = value;
                OnPropertyChanged(nameof(PTransactions));
            }
        }

        public ObservableCollection<ProductSold> PSold
        {
            get { return pSold; }
            set
            {
                pSold = value;
                OnPropertyChanged(nameof(PSold));
            }
        }

        public HistoryViewModel()
        {
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(
                new ProductSoldTransactions().GetPTransactions());
            pTransactions = allPTransactions;
            allPSold = new ObservableCollection<ProductSold>((new ProductSold().GetProductsSold()));
            PSold = allPSold; // Initialize PSold with all products sold
        }

        private ProductSoldTransactions _selectedRow;
        public ProductSoldTransactions SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                
                FilterPSold(); // Filter PSold based on the selected row
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private void FilterPSold()
        {
            if (SelectedRow != null)
            {
                PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId == SelectedRow.Id));
            }
        }
    }
}
