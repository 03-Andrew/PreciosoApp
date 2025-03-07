﻿using PreciosoApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

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

        private ObservableCollection<DailyReport> allT;
        private ObservableCollection<DailyReport> _allTs;
        public ObservableCollection<DailyReport> AllTransactions
        {
            get { return _allTs; }
            set
            {
                _allTs = value;
                OnPropertyChanged(nameof(AllTransactions));
            }
        }

        private ObservableCollection<ServicePromoTransactions> servicesPromosTransactions;
        private ObservableCollection<ServicePromoTransactions> _servicesPromosTransactions;
        public ObservableCollection<ServicePromoTransactions> ServicesPromosTransactions
        {
            get { return _servicesPromosTransactions; }
            set
            {
                _servicesPromosTransactions = value;
                OnPropertyChanged(nameof(ServicesPromosTransactions));
            }
        }

        private ObservableCollection<ServicePromoUsed> allSPUsed;
        private ObservableCollection<ServicePromoUsed> _servicesPromosUsed;
        public ObservableCollection<ServicePromoUsed> ServicesPromosUsed
        {
            get { return _servicesPromosUsed; }
            set
            {
                _servicesPromosUsed = value;
                OnPropertyChanged(nameof(ServicesPromosUsed));
            }
        }

        public HistoryViewModel()
        {
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(new ProductSoldTransactions().GetPTransactions());
            pTransactions = allPTransactions;
            allPSold = new ObservableCollection<ProductSold>(new ProductSold().GetProductsSold());

            allT = new ObservableCollection<DailyReport>(new DailyReport().GetDailyReports());
            AllTransactions = allT;

            servicesPromosTransactions = new ObservableCollection<ServicePromoTransactions>(new ServicePromoTransactions().GetTransactions());
            ServicesPromosTransactions = servicesPromosTransactions;
            allSPUsed = new ObservableCollection<ServicePromoUsed>(new ServicePromoUsed().GetServicePromoUsed());

            FilterCommand = new RelayCommand(FilterRowsByDate);


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

        private ServicePromoTransactions _selectedRow1;
        public ServicePromoTransactions SelectedRow1
        {
            get => _selectedRow1;
            set
            {
                _selectedRow1 = value;
                OnPropertyChanged(nameof(SelectedRow1));
                FilterServicesUsed();

            }
        }

        private ServicePromoTransactions _selectedRowSP;
        public ServicePromoTransactions SelectedRowSP
        {
            get => _selectedRowSP;
            set
            {
                _selectedRowSP = value;
                OnPropertyChanged(nameof(SelectedRowSP));
                FilterPromoServiceUsed();
            }
        }

        private AllTransactions _selectedRowT;
        public AllTransactions SelectedRowT
        {
            get => _selectedRowT;
            set
            {
                _selectedRowT = value;
                OnPropertyChanged(nameof(SelectedRowT));
                FilterTransactions();

            }
        }

        private void FilterTransactions()
        {
            if (SelectedRowT != null)
            {
                PSold = new ObservableCollection<ProductSold>(allPSold.Where(ps => ps.TransactionId == SelectedRowT.ID));
                ServicesUsed = new ObservableCollection<ServicesUsed>(allSUsed.Where(su => su.TransactionId == SelectedRowT.ID));
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

        private void FilterPromoServiceUsed()
        {
            if (SelectedRowSP != null)
            {
                ServicesPromosUsed = new ObservableCollection<ServicePromoUsed>(allSPUsed.Where(sp => sp.ID == SelectedRowSP.ID));
            }

        }

        private DateTime _startDate = DateTime.Today.AddYears(-1);
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));

            }
        }
        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));

            }
        }

        public void FilterRowsByDate()
        {

            var allRows = new ProductSoldTransactions().GetPTransactions().AsQueryable(); //allPTransactions.AsQueryable();
            var allRows2 = new ServicePromoTransactions().GetTransactions().AsQueryable();
            var allRows3 = new DailyReport().GetDailyReports().AsQueryable();
            if (_startDate != DateTime.MinValue)
            {

                allRows = allRows.Where(c => c.Date_Time >= _startDate);
                allRows2 = allRows2.Where(c => c.Date >= _startDate);
                allRows3 = allRows3.Where(c => c.DateTime >= _startDate);

            }

            if (_endDate != DateTime.MinValue)
            {
                allRows = allRows.Where(c => c.Date_Time <= _endDate);
                allRows2 = allRows2.Where(c => c.Date <= _endDate);
                allRows3 = allRows3.Where(c => c.DateTime <= _endDate);
            }

            PTransactions = new ObservableCollection<ProductSoldTransactions>(allRows.ToList());
            ServicesPromosTransactions = new ObservableCollection<ServicePromoTransactions>(allRows2.ToList());
            AllTransactions = new ObservableCollection<DailyReport>(allRows3.ToList());
        }

        public ICommand FilterCommand { get; }

    }
}



/*
 * 
 * 
            //allSUsed = new ObservableCollection<ServicesUsed>(new ServicesUsed().GetServicesUsed());

            //_serviceTransactions = new ObservableCollection<Service_Transaction>(new Service_Transaction().GetService_Transactions());
            //ServiceTransactions = _serviceTransactions;
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