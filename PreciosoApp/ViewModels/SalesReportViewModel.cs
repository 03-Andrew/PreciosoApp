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

        public ICommand FilterCommand { get; }
        public SalesReportViewModel() 
        {
            _dailyGross = new ObservableCollection<DailyGross>(new DailyGross().GetDailyGross());
            DailyGross = _dailyGross;
            FilterCommand = new RelayCommand(FilterByDate);
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

    }
}
