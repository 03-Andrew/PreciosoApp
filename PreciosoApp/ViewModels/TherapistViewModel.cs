using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PreciosoApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ObservableCollection<TherapistTypes> TherapistTypes { get; }
        public ObservableCollection<Gender> Genders { get; }
        public ObservableCollection<TherapistStatus> TherapistStatuses { get; }

        public string NewTherapistName { get; set; }
        public DateTime NewTherapistDOB { get; set; }
        public string NewTherapistContactInfo { get; set; }
        public string NewTherapistSched { get; set; }

        private Therapist _selectedTherapist;
        public Therapist SelectedTherapist
        {
            get => _selectedTherapist;
            set
            {
                _selectedTherapist = value;
                OnPropertyChanged(nameof(SelectedTherapist));
            }
        }

        private Gender _selectedGender;
        public Gender SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
            }
        }

        private TherapistTypes _selectedTherapistType;
        public TherapistTypes SelectedTherapistType
        {
            get => _selectedTherapistType;
            set
            {
                _selectedTherapistType = value;
                OnPropertyChanged(nameof(SelectedTherapistType));
            }
        }

        private TherapistStatus _selectedTherapistStatus;
        public TherapistStatus SelectedTherapistStatus
        {
            get => _selectedTherapistStatus;
            set
            {
                _selectedTherapistStatus = value;
                OnPropertyChanged(nameof(SelectedTherapistStatus));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = SelectedTherapist.Name;
                OnPropertyChanged(nameof(Name));

            }
        }

        public ICommand UpdateCommand { get; private set; }
        public ICommand AddTherapistCommand { get; }

        public TherapistViewModel()
        {
            var therapistInstance = new Therapist();
            allTherapist = new ObservableCollection<Therapist>(therapistInstance.GetAllTherapist());
            Therapist = allTherapist;

            var therapistTypes = new TherapistTypes();
            var genderTypes = new Gender();
            var therapistStatus = new TherapistStatus();

            TherapistTypes = new ObservableCollection<TherapistTypes>(therapistTypes.GetTherapistTypes());
            Genders = new ObservableCollection<Gender>(genderTypes.GetGenders());
            TherapistStatuses = new ObservableCollection<TherapistStatus>(therapistStatus.GetTStatus());

            UpdateCommand = new RelayCommand(UpdateTherapist);
            AddTherapistCommand = new RelayCommand(AddTherapist);

            NewTherapistDOB = DateTime.Now;

        }

        private void AddTherapist()
        {

            try
            {
                Therapist tp = new Therapist();
                string name = NewTherapistName;
                DateTime dob = NewTherapistDOB;
                string contact = NewTherapistContactInfo;
                string sched = NewTherapistSched;
                int gender_id = SelectedGender.Id;
                int status_id = SelectedTherapistStatus.Id;
                int type_id = SelectedTherapistType.Id;

                // Add the therapist to the database
                tp.AddTherapist(name, dob.Date, contact, sched, gender_id, status_id, type_id);

                // Refresh the Therapist collection
                Therapist = new ObservableCollection<Therapist>(tp.GetAllTherapist());

                // Clear out the fields
                NewTherapistName = string.Empty;
                NewTherapistDOB = DateTime.Now;
                NewTherapistContactInfo = string.Empty;
                NewTherapistSched = string.Empty;
                SelectedGender = null;
                SelectedTherapistStatus = null;
                SelectedTherapistType = null;
            }
            catch (Exception ex)
            {
                // Display error message to the user



            }
        }



        private void UpdateTherapist()
        {
            Therapist tp = new Therapist();
            int id = SelectedTherapist.Id;
            string name = SelectedTherapist.Name;
            DateTime dob = SelectedTherapist.DOB;
            string contact = SelectedTherapist.ContactInfo;
            string sched = SelectedTherapist.Sched;
            int gender_id = SelectedGender?.Id ?? GetGenderId(SelectedTherapist.Gender);
            int status_id = SelectedTherapistStatus?.Id ?? GetStatusId(SelectedTherapist.Status);
            int type_id = SelectedTherapistType?.Id ?? GetTypeId(SelectedTherapist.Type);
  

            tp.UpdateTherapist(id, name, dob, contact, sched, gender_id, status_id, type_id);

            Therapist = new ObservableCollection<Therapist>(tp.GetAllTherapist());

            OnPropertyChanged(nameof(SelectedTherapist));

            SelectedGender = null;
            SelectedTherapistType = null;
            SelectedTherapistStatus = null;

        }

        public int GetGenderId(string gender)
        {
            foreach (var item in Genders)
            {
                if (item.GenderType == gender) return item.Id;
            }
            return -1;
        }

        public int GetStatusId(string status)
        {
            foreach (var item in TherapistStatuses)
            {
                if (item.StatusName == status)
                {
                    return item.Id;
                }
            }
            return -1;
        }
        public int GetTypeId(string type)
        {
            foreach (var item in TherapistTypes)
            {
                if (item.TypeName == type)
                {
                    return item.Id;
                }
            }
            return -1;
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
    