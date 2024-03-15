using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using PreciosoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciosoApp.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private ObservableCollection<Account> accounts;
        public ICommand LoginCommand { get; }


        private LoginWindowView loginWindow; // Reference to the login window

        public LoginWindowViewModel(LoginWindowView loginWindow)
        {
            this.loginWindow = loginWindow; // Assign the reference
            var acc = new Account();
            Accounts = new ObservableCollection<Account>(acc.GetAccounts());
            LoginCommand = new RelayCommand(Login);
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ObservableCollection<Account> Accounts
        {
            get { return accounts; }
            set
            {
                accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }


        private void Login()
        {
            bool isValidLogin = CheckLogin(Username, Password);
            var window = new DialogWindow();
            if (isValidLogin)
            {
                if (IsCashierAccount(Username, Password))
                {
                    window.DialogText = "Login successful as Cashier";
                    window.Show();
                    // Set the MainWindowViewModel as the DataContext for MainWindowView
                    var mainWindowView = new MainWindow();
                    var mainWindowViewModel = new MainWindowViewModel("1", mainWindowView);

                    mainWindowViewModel.username = Username;

                    mainWindowView.DataContext = mainWindowViewModel;
                    mainWindowViewModel.username = Username;
                    mainWindowView.Show();
                    loginWindow.Close();
                }
                else if (IsAdminAccount(Username, Password))
                {
                    window.DialogText = "Login successful as Admin";
                    window.Show();
                    // Set the MainWindowViewModel as the DataContext for MainWindowView
                    var mainWindowView = new MainWindow();
                    var mainWindowViewModel = new MainWindowViewModel("2", mainWindowView);

                    mainWindowViewModel.username = Username;

                    mainWindowView.DataContext = mainWindowViewModel;
                    mainWindowViewModel.username = Username;
                    mainWindowView.Show();
                    loginWindow.Close();
                }
            }
            else
            {
                window.DialogText = "Invalid username or password";
                window.Show();
            }
        }

        private bool IsCashierAccount(string username, string password)
        {
            var acc = new Account();
            int type = acc.GetAccountType(username, password);
            return type == 1; // Assuming 1 represents cashier account type
        }

        private bool IsAdminAccount(string username, string password)
        {
            var acc = new Account();
            int type = acc.GetAccountType(username, password);
            return type == 2; // Assuming 2 represents admin account type
        }

        private bool CheckLogin(string username, string password)
        {
            var acc = new Account();
            return acc.CheckLogin(username, password);
        }
    }
}
