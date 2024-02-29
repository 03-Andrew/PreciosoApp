using CommunityToolkit.Mvvm.ComponentModel;
using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciosoApp.ViewModels
{
    public class TherapistViewModel : ViewModelBase
    {
        private ObservableCollection<Therapist> therapists;
        private ObservableCollection<Therapist> allTherapist;
        private string searchText;
        public ObservableCollection<Therapist> Therapist
        {
            get { return therapists; }
            set { 
                therapists = value; 
                OnPropertyChanged(nameof(Therapist));
            }
        }

        public TherapistViewModel()
        {
            var therapistInstance = new Therapist();
            allTherapist = new ObservableCollection<Therapist>(therapistInstance.GetAllTherapist());
            Therapist = allTherapist;
        }


        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterClients();
            }
        }

        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Therapist = allTherapist;
            }
            else
            {
                string searchTextLower = SearchText.ToLower().Trim();
                Therapist = new ObservableCollection<Therapist>(allTherapist.Where(t => t.Name.ToLower().Contains(searchTextLower)));
            }
        }
    }
}
