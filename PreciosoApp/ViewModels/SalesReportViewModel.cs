using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PreciosoApp.Models;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class SalesReportViewModel : ViewModelBase
    {
        public SalesReportViewModel()
        {
            loadDailyReport();
            loadDailySales();
            FilterGrid = new RelayCommand(FilterBySelectedDate);
            FilterCommand = new RelayCommand(FilterByDate);
        }

        public ICommand FilterGrid { get; }
        public ICommand FilterCommand { get; }

        public void loadDailyReport()
        {
            FilterBySelectedDate();
            
        }
        private double _totalProdSalesD { get; set; }
        public double TotalProdSalesD
        {
            get { return _totalProdSalesD; }
            set
            {
                _totalProdSalesD = value;
                OnPropertyChanged(nameof(TotalProdSalesD));
            }
        }
        private double _totalServPromoSalesD { get; set; }
        public double TotalServPromoSalesD
        {
            get { return _totalServPromoSalesD; }
            set
            {
                _totalServPromoSalesD = value;
                OnPropertyChanged(nameof(TotalServPromoSalesD));
            }
        }
        private double _totalSalesD { get; set; }
        public double TotalSalesD
        {
            get { return _totalSalesD; }
            set
            {
                _totalSalesD = value;
                OnPropertyChanged(nameof(TotalSalesD));
            }
        }

        public void loadDailySales()
        {
            FilterByDate();
        }

        private ObservableCollection<DailyReport> _dailyReport;
        public ObservableCollection<DailyReport> DailyReport
        {
            get { return _dailyReport; }
            set
            {
                _dailyReport = value;
                OnPropertyChanged(nameof(DailyReport));
            }
        }

        private ObservableCollection<Commissions> _comm;
        public ObservableCollection<Commissions> Comm
        {
            get { return _comm; }
            set
            {
                _comm = value;
                OnPropertyChanged(nameof(Comm));
            }
        }


        public string _dateStr = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        public string DateStr
        {
            get => _dateStr;
            set
            {
                _dateStr= value;
                OnPropertyChanged(nameof(DateStr));
            }
        }
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }

        private void FilterBySelectedDate()
        {
            var record = new DailyReport();
            DailyReport = new ObservableCollection<DailyReport>(record.GetDailyReports().Where(r => r.DateTime.Date == SelectedDate.Date));
            DateStr = SelectedDate.ToString("dddd, MMMM dd, yyyy");

            var comtbl = new Commissions();
            Comm = new ObservableCollection<Commissions>(comtbl.GetCommissions().Where(c => c.Date == DateOnly.FromDateTime(SelectedDate)));

            var filteredByDate = new DailyGross().GetDailyGross().Where(r => r.Date == SelectedDate.Date);
            TotalProdSalesD = filteredByDate.Sum(d => d.ProdSales);
            TotalServPromoSalesD = filteredByDate.Sum(d => d.ServPromoSales);
            TotalSalesD = filteredByDate.Sum(d => d.TotalSales);

            SalesData = new ObservableCollection<List<object>>
            {
                new List<object> {"Product", TotalProdSalesD},
                new List<object> {"Service/Promo", TotalServPromoSalesD},
                new List<object> {"Total", TotalSalesD}
            };

        }

        private ObservableCollection<List<object>> _salesData;
        public ObservableCollection<List<object>> SalesData
        {
            get { return _salesData; }
            set
            {
                _salesData = value;
                OnPropertyChanged(nameof(SalesData));
            }
        }




        private ObservableCollection<DailyGross> _dailyGross;
        public ObservableCollection<DailyGross> DailyGross
        {
            get { return _dailyGross; }
            set
            {
                _dailyGross = value;
                OnPropertyChanged(nameof(DailyGross));
            }
        }

        private void FilterByDate()
        {

            var filteredByDate = new DailyGross().GetDailyGross().AsQueryable();

            if (_startDate != DateTime.MinValue)
            {
                filteredByDate = filteredByDate.Where(c => c.Date >= _startDate);
            }

            if (_endDate != DateTime.MinValue)
            {
                filteredByDate = filteredByDate.Where(c => c.Date <= _endDate);
            }

            DailyGross = new ObservableCollection<DailyGross>(filteredByDate.ToList());
            TotalProdSales = filteredByDate.Sum(d => d.ProdSales);
            TotalServPromoSales = filteredByDate.Sum(d => d.ServPromoSales);
            TotalSales = filteredByDate.Sum(d => d.TotalSales);

        }

        private double _totalProdSales {  get; set; }
        public double TotalProdSales
        {
            get { return _totalProdSales; }
            set
            {
                _totalProdSales = value;
                OnPropertyChanged(nameof(TotalProdSales));
            }
        }
        private double _totalServPromoSales { get; set; }
        public double TotalServPromoSales
        {
            get { return _totalServPromoSales; }
            set
            {
                _totalServPromoSales = value;
                OnPropertyChanged(nameof(TotalServPromoSales));
            }
        }
        private double _totalSales { get; set; }
        public double TotalSales
        {
            get { return _totalSales; }
            set
            {
                _totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
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

        
    }
}


//_dailyGross = new ObservableCollection<DailyGross>(new DailyGross().GetDailyGross());
//DailyGross = _dailyGross;
//FilterCommand = new RelayCommand(FilterByDate);

