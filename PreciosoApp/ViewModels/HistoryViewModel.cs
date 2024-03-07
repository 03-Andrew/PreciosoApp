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
        

        public ObservableCollection<ProductSoldTransactions> PTransactions
        {
            get { return pTransactions; }
            set
            {
                pTransactions = value;
                OnPropertyChanged(nameof(PTransactions));
            }
        }

        private ObservableCollection<ProductSold> allPSold;
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

        private ObservableCollection<Service_Transaction> _serviceTransactions; 
        public ObservableCollection<Service_Transaction> ServiceTransactions
        {
            get { return _serviceTransactions; }
            set
            {
                _serviceTransactions = value;
                OnPropertyChanged(nameof(ServiceTransactions));
            }
        }

        private ObservableCollection<ServicesUsed> allSUsed;
        private ObservableCollection<ServicesUsed> _servicesUsed;
        public ObservableCollection<ServicesUsed> ServicesUsed
        {
            get { return _servicesUsed; }
            set
            {
                _servicesUsed = value;
                OnPropertyChanged(nameof(ServicesUsed));
            }
        }


        public HistoryViewModel()
        {
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(
                new ProductSoldTransactions().GetPTransactions());
            pTransactions = allPTransactions;
            allPSold = new ObservableCollection<ProductSold>(new ProductSold().GetProductsSold());
            allSUsed = new ObservableCollection<ServicesUsed>(new ServicesUsed().GetServicesUsed());
            _serviceTransactions = new ObservableCollection<Service_Transaction>(new Service_Transaction().GetService_Transactions());
            ServiceTransactions = _serviceTransactions;


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

        private Service_Transaction _selectedRow1;
        public Service_Transaction SelectedRow1
        {
            get => _selectedRow1;
            set
            {
                _selectedRow1 = value;
                OnPropertyChanged(nameof(SelectedRow1));
                FilterServicesUsed();

            }
        }

        private void FilterPSold()
        {
            if (SelectedRow != null)
            {
                PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId == SelectedRow.Id)); ;
            }
        }

        private void FilterServicesUsed()
        {
            if (SelectedRow1 != null)
            {
                ServicesUsed = new ObservableCollection<ServicesUsed>(allSUsed.Where(su => su.TransactionId == SelectedRow1.ID));
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
