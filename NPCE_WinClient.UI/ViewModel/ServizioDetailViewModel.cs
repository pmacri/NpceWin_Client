using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class ServizioDetailViewModel : DetailViewModelBase, IServizioDetailViewModel
    {
        private readonly IServizioRepository _servizioRepository;
        private IEnumerable<Anagrafica> _allAnagrafiche;

        public ServizioDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IServizioRepository servizioRepository): base(eventAggregator, messageDialogService)
        {
            _servizioRepository = servizioRepository;
            Mittenti = new ObservableCollection<Anagrafica>();
            DestinatariAdded = new ObservableCollection<Anagrafica>();
            DestinatariAvailable= new ObservableCollection<Anagrafica>();

            AddDestinatarioCommand = new DelegateCommand(OnAddDestinatrioExecute, OnAddDestinatarioCanExecute);
            RemoveDestinatarioCommand = new DelegateCommand(OnRemoveDestinatrioExecute, OnRemoveDestinatarioCanExecute);
        }

        private void OnAddDestinatrioExecute()
        {
            var destinatarioToAdd = SelectedDestinatarioAvailable;

            Servizio.Model.Anagrafiche.Add(destinatarioToAdd);
            DestinatariAvailable.Remove(destinatarioToAdd);
            DestinatariAdded.Add(destinatarioToAdd);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddDestinatarioCanExecute()
        {
            return SelectedDestinatarioAvailable != null;
        }

        private bool OnRemoveDestinatarioCanExecute()
        {
            return SelectedDestinatarioAdded != null;
        }

        private void OnRemoveDestinatrioExecute()
        {
            var destinatarioToRemove = SelectedDestinatarioAdded;

            Servizio.Model.Anagrafiche.Remove(destinatarioToRemove);
            DestinatariAdded.Remove(destinatarioToRemove);
            DestinatariAvailable.Add(destinatarioToRemove);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public override async Task LoadAsync(int? id)
        {
            Servizio servizio = (id.HasValue)
                 ? await _servizioRepository.GetByIdAsync(id.Value)
                 : CreateNewServizio();
            Id = servizio.Id;
            InitializeServizio(servizio);
            _allAnagrafiche = _servizioRepository.GetAll();            
            SetupControls();           
        }

        private void SetupControls()
        {
            var serviceDestinatariIds = Servizio.Model.Anagrafiche.Where(a=> a.IsMittente==false).Select(a => a.Id).ToList();
            var addedDestinatari = _allAnagrafiche.Where(a => serviceDestinatariIds.Contains(a.Id)).ToList();
            var availableDestinatari = _allAnagrafiche.Except(addedDestinatari).ToList();

            Mittenti.Clear();
            foreach (var mittente in _allAnagrafiche)
            {
                Mittenti.Add(mittente);
            }

            DestinatariAdded.Clear();

            DestinatariAvailable.Clear();

            foreach (var addedDest in addedDestinatari)
            {
                DestinatariAdded.Add(addedDest);
            }

            foreach (var destAvailable in availableDestinatari)
            {
                DestinatariAvailable.Add(destAvailable);
            }
        }

        private void InitializeServizio(Servizio servizio)
        {
            Servizio = new ServizioWrapper(servizio);
            Servizio.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _servizioRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Servizio.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Servizio.Id == 0)
            {
                Servizio.FronteRetro= false;
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = "Title";
        }

        private Servizio CreateNewServizio()
        {
            var servizio = new Servizio();
            _servizioRepository.Add(servizio);
            return servizio;
        }

        protected override async void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog($"Do you really want to cancel the service ?",
                                                                  "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _servizioRepository.Remove(Servizio.Model);
            await _servizioRepository.SaveAsync();
            RaiseDetailDeletedEvent(Servizio.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        public ServizioWrapper Servizio { get; set; }
        public Anagrafica Mittente { get; set; }
        public ObservableCollection<Anagrafica> Mittenti { get; set; }

        public ObservableCollection<Anagrafica> DestinatariAdded { get; set; }

        private Anagrafica _selectedDestinatarioAdded;
        public Anagrafica SelectedDestinatarioAdded {
            get
            {
                return _selectedDestinatarioAdded;
            }

            set
            {
                _selectedDestinatarioAdded = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveDestinatarioCommand).RaiseCanExecuteChanged();
            }
        }

        private Anagrafica _selectedDestinatarioAvailable;
        public Anagrafica SelectedDestinatarioAvailable
        {
            get
            {
                return _selectedDestinatarioAvailable;
            }

            set
            {
                _selectedDestinatarioAvailable = value;
                OnPropertyChanged();
                ((DelegateCommand)AddDestinatarioCommand).RaiseCanExecuteChanged();
            }
        }


        public ObservableCollection<Anagrafica> DestinatariAvailable { get; set; }

        public ICommand AddDestinatarioCommand { get; set; }

        public ICommand RemoveDestinatarioCommand { get; set; }
    }
}
