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
        private ObservableCollection<DailyGross> _dailyGross;
        private ObservableCollection<DailyGross> allDailyGross;
        public ObservableCollection<DailyGross> DailyGross
        {
            get { return _dailyGross; } 
            set
            {
                _dailyGross = value;
                OnPropertyChanged(nameof(DailyGross));
            }
        }

        public void loadDailyReport()
        {
            _dailyReport = new ObservableCollection<DailyReport>(new DailyReport().GetDailyReports());
            DailyReport = _dailyReport;
 
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

        public ICommand FilterCommand { get; }
        public SalesReportViewModel() 
        {
            //_dailyGross = new ObservableCollection<DailyGross>(new DailyGross().GetDailyGross());
            //DailyGross = _dailyGross;
            //FilterCommand = new RelayCommand(FilterByDate);

            loadDailyReport();
            FilterGrid = new RelayCommand(FilterBySelectedDate);
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

        private DateTime _selectedDate = DateTime.Now;
        public  DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                FilterBySelectedDate();
            }
        }

        private void FilterBySelectedDate()
        {
            var filteredByDate = _dailyReport.AsQueryable();

            filteredByDate = filteredByDate.Where(c => c.DateTime.Date == _selectedDate.Date);

            DailyReport = new ObservableCollection<DailyReport>(filteredByDate.ToList());
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

        public ICommand FilterGrid { get; }
    }
}
