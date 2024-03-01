using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.ViewModels
{
    public class HistoryViewModel: ViewModelBase
    {
        private ObservableCollection<ProductSoldTransactions> pTransaction;
        private ObservableCollection<ProductSoldTransactions> allPTransactions;
        private string searchProd;
        private List<string> prodNames;
        public ObservableCollection<ProductSoldTransactions> PTransactions
        {
            get { return pTransaction; }
            set
            {
                pTransaction = value;
                OnPropertyChanged(nameof(PTransactions));
            }
        }
        public HistoryViewModel()
        {
            var pTran = new ProductSoldTransactions();
            allPTransactions = new ObservableCollection<ProductSoldTransactions>(pTran.GetPTransactions());
            PTransactions = allPTransactions;

        }
    }
}
