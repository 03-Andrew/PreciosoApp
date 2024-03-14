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
        public void loadDailyReport()
        {
            FilterBySelectedDate(); 
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

        public ICommand FilterGrid { get; }
        public ICommand FilterCommand { get; }
        public SalesReportViewModel() 
        {
            loadDailyReport();
            FilterGrid = new RelayCommand(FilterBySelectedDate);
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
          
            var filteredByDate = _dailyGross.AsQueryable();


            if (_startDate != DateTime.MinValue)
            {
                filteredByDate = filteredByDate.Where(c => c.Date >= _startDate);
            }

            if (_endDate != DateTime.MinValue)
            {
                filteredByDate = filteredByDate.Where(c => c.Date <= _endDate);
            }

            DailyGross = new ObservableCollection<DailyGross>(filteredByDate.ToList());
        }

        private DateTime _startDate = DateTime.Today.AddYears(-15);
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                FilterByDate();
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
                FilterByDate();
            }
        }
        
    }
}


//_dailyGross = new ObservableCollection<DailyGross>(new DailyGross().GetDailyGross());
//DailyGross = _dailyGross;
//FilterCommand = new RelayCommand(FilterByDate);

