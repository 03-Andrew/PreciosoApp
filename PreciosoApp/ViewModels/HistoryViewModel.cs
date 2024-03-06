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

        private ObservableCollection<ProductSold> _pSold;
        public ObservableCollection<ProductSold> PSold
        {
            get { return _pSold; }
            set
            {
                _pSold = value;
                OnPropertyChanged(nameof(PSold));
            }
        }


        public HistoryViewModel()
        {
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(
                new ProductSoldTransactions().GetPTransactions());
            pTransactions = allPTransactions;
            allPSold = new ObservableCollection<ProductSold>((new ProductSold().GetProductsSold()));
            
        }

        private ProductSoldTransactions _selectedRow;
        public ProductSoldTransactions SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
                FilterPSold();
            }
        }

        private void FilterPSold()
        {
            if (SelectedRow != null)
            {
                PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId == SelectedRow.Id)); ;
            }
        }

        /*
        private string searchText; 
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterSold();
            }
        }

        private void FilterSold()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                PSold = pSold;
            } else
            {
                PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId.ToString().Equals(SearchText)));
            }
        }
        *
        */
    }
}
