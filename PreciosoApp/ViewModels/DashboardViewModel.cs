using Avalonia.Collections;
using Avalonia.Data;
using PreciosoApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ICommand FirstCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand LastCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataGridCollectionView _transactionCollection;
        public DataGridCollectionView TransactionCollection
        {
            get { return _transactionCollection; }
            set
            {
                _transactionCollection = value;
                OnPropertyChanged(nameof(TransactionCollection));
            }
        }



        private ObservableCollection<AllTransactions> _transactionList;
        public ObservableCollection<AllTransactions> TransactionList
        {
            get { return _transactionList; }
            set
            {
                _transactionList = value;
                OnPropertyChanged(nameof(AllTransactions));

            }
        }


        public DashboardViewModel()
        {
            TransactionCollection = new DataGridCollectionView(ListOfTransactions);
            nums = new ObservableCollection<int>() { 3, 6, 9 };


            NextCommand = new Command((s) => true, NextPage);
            FirstCommand = new Command((s) => true, FirstPage);
            LastCommand = new Command((s) => true, LastPage);
            PreviousCommand = new Command((s) => true, PreviousPage);
            Load();
        }

        public void Load()
        {
            foreach(var item in new AllTransactions().GetTransactions())
            {
                ListOfTransactions.Add(item);
            }
            UpdateCollection(ListOfTransactions.Take(SelectedRecord));
            UpdateRecordCount();
             
        }
        private void UpdateCollection(IEnumerable<AllTransactions> enumerable)
        {
            TransactionList.Clear();
            foreach (var item in enumerable)
            {
                TransactionList.Add(item);
            }
        }

        public List<AllTransactions> ListOfTransactions = new List<AllTransactions>();

        int RecordStartFrom = 0;

        private void PreviousPage(object obj)
        {
            CurrentPage--;
            RecordStartFrom = ListOfTransactions.Count - SelectedRecord * (NumberOfPages - (CurrentPage - 1));
            var recorsToShow = ListOfTransactions.Skip(RecordStartFrom).Take(SelectedRecord);
            UpdateCollection(recorsToShow);
            UpdateEnableState();
        }

        private void LastPage(object obj)
        {
            var recordsToskip = SelectedRecord * (NumberOfPages - 1);
            UpdateCollection(ListOfTransactions.Skip(recordsToskip));
            CurrentPage = NumberOfPages;
            UpdateEnableState();
        }

        private void FirstPage(object obj)
        {
            UpdateCollection(ListOfTransactions.Take(SelectedRecord));
            CurrentPage = 1;
            UpdateEnableState();
        }

        private void NextPage(object obj)
        {
            RecordStartFrom = CurrentPage * SelectedRecord;
            var recordsToShow = ListOfTransactions.Skip(RecordStartFrom).Take(SelectedRecord);
            UpdateCollection(recordsToShow);
            CurrentPage++;
            UpdateEnableState();
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


        private int _numberOfPages = 5;
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

        private int _selectedRecord = 3;
        public int SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged(nameof(SelectedRecord));
                UpdateRecordCount();
            }
        }
        public ObservableCollection<int> nums { get; private set; }
        
        public void UpdateRecordCount()
        {
            NumberOfPages = (int)Math.Ceiling((double)ListOfTransactions.Count / SelectedRecord);
            NumberOfPages = NumberOfPages == 0 ? 1 : NumberOfPages;
            UpdateCollection(ListOfTransactions.Take(SelectedRecord));
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


    public class Command : ICommand
    {
        public Command(Func<object, bool> methodCanExecute, Action<object> methodExecute)
        {
            MethodCanExecute = methodCanExecute;
            MethodExecute = methodExecute;
        }
        Action<object> MethodExecute;
        Func<object, bool> MethodCanExecute;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return MethodExecute != null && MethodCanExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            MethodExecute(parameter);
        }
    }
}
