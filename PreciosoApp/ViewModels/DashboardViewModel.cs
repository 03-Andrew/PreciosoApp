using Avalonia.Data;
using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            AllTransactions = new ObservableCollection<AllTransactions>(new AllTransactions().GetTransactions());
            nums = new ObservableCollection<string>() { "3", "6", "9" };
        }

        private ObservableCollection<AllTransactions> _allTransactions;
        public ObservableCollection<AllTransactions> AllTransactions
        {
            get { return _allTransactions; }
            set
            {
                _allTransactions = value;
                OnPropertyChanged(nameof(AllTransactions));
                UpdateRecordCount();
            }
        }


        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateEnableState();
            }
        }


        private int _numberOfPages = 10;
        public int NumberOfPages
        {
            get { return _numberOfPages; }
            set
            { 
                _numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
                UpdateEnableState();
            }
        }

        private string _selectedRecord = "3";
        public string SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                UpdateRecordCount();
            }
        }
        public ObservableCollection<string> nums { get; private set; }
        
        public void UpdateRecordCount()
        {
            NumberOfPages = AllTransactions.Count / Convert.ToInt32(SelectedRecord);
            NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;
            CurrentPage = 1;
        }

        private bool _isFirstEnabled;
        public bool IsFirstEnabled
        {
            get { return _isFirstEnabled; }
            set
            {
                _isFirstEnabled = value;
                OnPropertyChanged(nameof(IsFirstEnabled));

            }
        }

        private bool _isPreviousEnabled;
        public bool IsPreviousEnabled
        {
            get { return _isPreviousEnabled; }
            set
            {
                _isPreviousEnabled = value;
                OnPropertyChanged(nameof(IsPreviousEnabled));

            }
        }

        private bool _isNextEnabled;
        public bool IsNextEnabled
        {
            get { return _isNextEnabled; }
            set
            {
                _isNextEnabled = value;
                OnPropertyChanged(nameof(IsNextEnabled));

            }
        }

        private bool _isLastEnabled;
        public bool IsLastEnabled
        {
            get { return _isLastEnabled; }
            set
            {
                _isLastEnabled = value;
                OnPropertyChanged(nameof(IsLastEnabled));
            }
        }

        private void UpdateEnableState()
        {
            IsFirstEnabled = CurrentPage > 1;
            IsPreviousEnabled = CurrentPage > 1;
            IsNextEnabled = CurrentPage < NumberOfPages;
            IsLastEnabled = CurrentPage < NumberOfPages;

        }
    }
}
