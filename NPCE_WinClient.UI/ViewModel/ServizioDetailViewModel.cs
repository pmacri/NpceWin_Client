using FriendOrganizer.UI.View.Services;
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
        private readonly IStatoServizioRepository _statoServizioRepository;
        private readonly IServizioRepository _servizioRepository;
        private IEnumerable<Anagrafica> _allAnagrafiche;
        private IEnumerable<Documento> _allDocumenti;

        public ServizioDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IServizioRepository servizioRepository,
            IStatoServizioRepository statoServizioRepository) : base(eventAggregator, messageDialogService)
        {
            _statoServizioRepository = statoServizioRepository;
            _servizioRepository = servizioRepository;
            Mittenti = new ObservableCollection<Anagrafica>();
           
            DocumentiAdded = new ObservableCollection<Documento>();
            DocumentiAvailable = new ObservableCollection<Documento>();
            

            DestinatariAdded = new ObservableCollection<Anagrafica>();
            DestinatariAvailable = new ObservableCollection<Anagrafica>();

            DestinatariArAdded = new ObservableCollection<Anagrafica>();
            DestinatariArAvailable = new ObservableCollection<Anagrafica>();

            DocumentiAdded = new ObservableCollection<Documento>();
            DocumentiAvailable = new ObservableCollection<Documento>();

            AddDestinatarioCommand = new DelegateCommand(OnAddDestinatarioExecute, OnAddDestinatarioCanExecute);
            RemoveDestinatarioCommand = new DelegateCommand(OnRemoveDestinatarioExecute, OnRemoveDestinatarioCanExecute);

            AddDestinatarioArCommand = new DelegateCommand(OnAddDestinatarioArExecute, OnAddDestinatarioArCanExecute);
            RemoveDestinatarioARCommand = new DelegateCommand(OnRemoveDestinatarioArExecute, OnRemoveDestinatarioArCanExecute);

            AddDocumentoCommand = new DelegateCommand(OnAddDocumentoExecute, OnAddDocumentoCanExecute);
            RemoveDocumentoCommand = new DelegateCommand(OnRemoveDocumentExecute, OnRemoveDocumentCanExecute);
        }

        private bool OnRemoveDestinatarioArCanExecute()
        {
            return SelectedDestinatarioArAdded != null;
        }

        private void OnRemoveDestinatarioArExecute()
        {
            var destinatarioToRemove = SelectedDestinatarioArAdded;
            Servizio.Model.Anagrafiche.Remove(destinatarioToRemove);
            DestinatariAdded.Remove(destinatarioToRemove);
            DestinatariAvailable.Add(destinatarioToRemove);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddDestinatarioArCanExecute()
        {
            return _selectedDestinatarioArAvailable != null;
        }

        private void OnAddDestinatarioArExecute()
        {
            var destinatarioToAdd = SelectedDestinatarioARAvailable;
            destinatarioToAdd.IsDestinatarioAR = true;
            Servizio.Model.Anagrafiche.Add(destinatarioToAdd);
            DestinatariArAvailable.Remove(destinatarioToAdd);
            DestinatariArAdded.Add(destinatarioToAdd);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveDocumentCanExecute()
        {
            return SelectedDocumentoAdded != null;
        }

        private void OnRemoveDocumentExecute()
        {
            var documentoToRemove = SelectedDestinatarioAdded;

            Servizio.Model.Anagrafiche.Remove(documentoToRemove);
            DestinatariAdded.Remove(documentoToRemove);
            DestinatariAvailable.Add(documentoToRemove);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddDocumentoCanExecute()
        {
            return SelectedDocumentoAvailable != null;
        }

        private void OnAddDocumentoExecute()
        {
            var documentoToAdd = SelectedDocumentoAvailable;
            Servizio.Model.Documenti.Add(documentoToAdd);
            DocumentiAvailable.Remove(documentoToAdd);
            DocumentiAdded.Add(documentoToAdd);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public Documento SelectedDocumentoAvailable {
            get {
                return _selectDocumentoAvailable;
            }
            set
            {
                _selectDocumentoAvailable = value;
                OnPropertyChanged();
                ((DelegateCommand)AddDocumentoCommand).RaiseCanExecuteChanged();
            }
        }

        public Documento SelectedDocumentoAdded {
            get
            {
                return _selectDocumentoAdded;
            }
            set
            {
                _selectDocumentoAdded = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveDocumentoCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnAddDestinatarioExecute()
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

        private void OnRemoveDestinatarioExecute()
        {
            var destinatarioToRemove = SelectedDestinatarioAdded;

            Servizio.Model.Anagrafiche.Remove(destinatarioToRemove);
            DestinatariAdded.Remove(destinatarioToRemove);
            DestinatariAvailable.Add(destinatarioToRemove);
            HasChanges = _servizioRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public override async Task LoadAsync(int id)
        {
            Servizio servizio = (id > 0)
                 ? await _servizioRepository.GetByIdAsync(id)
                 : CreateNewServizio();
            Id = id;
            InitializeServizio(servizio);

            _allAnagrafiche = _servizioRepository.GetAllAnagrafiche();
            _allDocumenti = _servizioRepository.GetAllDocumenti();

            SetupControls();
        }

        private void SetupControls()
        {
            var serviceDestinatariIds = Servizio.Model.Anagrafiche.Where(a => a.IsMittente == false).Select(a => a.Id).ToList();
            var addedDestinatari = _allAnagrafiche.Where(a => serviceDestinatariIds.Contains(a.Id)).ToList();
            var availableDestinatari = _allAnagrafiche.Except(addedDestinatari).ToList();

            var serviceDestinatariArIds = Servizio.Model.Anagrafiche.Where(a => a.IsMittente == false && a.IsDestinatarioAR==true).Select(a => a.Id).ToList();
            var addedDestinatariAr = _allAnagrafiche.Where(a => serviceDestinatariArIds.Contains(a.Id)).ToList();
            var availableDestinatariAr = _allAnagrafiche.Except(addedDestinatariAr).ToList();

            var serviceDocumentiIds = Servizio.Model.Documenti.Select(a => a.Id).ToList();
            var addedDocumenti = _allDocumenti.Where(a => serviceDocumentiIds.Contains(a.Id)).ToList();
            var availableDocumenti = _allDocumenti.Except(addedDocumenti).ToList();

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

            DestinatariArAdded.Clear();

            DestinatariArAvailable.Clear();

            foreach (var addedDest in addedDestinatariAr)
            {
                DestinatariArAdded.Add(addedDest);
            }

            foreach (var destAvailable in availableDestinatariAr)
            {
                DestinatariArAvailable.Add(destAvailable);
            }



            // Documenti
            DocumentiAvailable.Clear();
            foreach (var doc in availableDocumenti)
            {
                DocumentiAvailable.Add(doc);
            }

            DocumentiAdded.Clear();
            foreach (var doc in addedDocumenti)
            {
                DocumentiAdded.Add(doc);
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
                Servizio.FronteRetro = false;
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = "Nuovo Servizio";
        }

        private Servizio CreateNewServizio()
        {
            var servizio = new Servizio();
            _servizioRepository.Add(servizio);
            return servizio;
        }

        protected override async void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to cancel the service ?",
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
            return true;
        }

        protected override async void OnSaveExecute()
        {
            var statoCreated = _statoServizioRepository.GetByDescription("Salvato");
            Servizio.StatoServizioId = statoCreated.Id;
            await _servizioRepository.SaveAsync();
            Id = Servizio.Id;
            HasChanges = _servizioRepository.HasChanges();
        }

        public ServizioWrapper Servizio { get; set; }

        Anagrafica _mittente;
        public Anagrafica Mittente
        {
            get
            {
                return _mittente;
            }
            set
            {
                _mittente = value;
                if (_mittente != null)
                {
                    _mittente.IsMittente = true;
                    var previousMittente = Servizio.Model.Anagrafiche.SingleOrDefault(m => m.IsMittente == true);
                    if (previousMittente != null)
                    {
                        Servizio.Model.Anagrafiche.Remove(previousMittente);
                    }
                    Servizio.Model.Anagrafiche.Add(_mittente);
                    HasChanges = _servizioRepository.HasChanges();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

            }
        }
       
        public ObservableCollection<Anagrafica> Mittenti { get; set; }
        public ObservableCollection<Documento> Documenti { get; set; }
        public ObservableCollection<Anagrafica> DestinatariAdded { get; set; }
        public ObservableCollection<Documento> DocumentiAdded { get; set; }
        public ObservableCollection<Documento> DocumentiAvailable { get; set; }

        private Anagrafica _selectedDestinatarioAdded;
        public Anagrafica SelectedDestinatarioAdded
        {
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


        private Anagrafica _selectedDestinatarioArAdded;
        public Anagrafica SelectedDestinatarioArAdded
        {
            get
            {
                return _selectedDestinatarioArAdded;
            }

            set
            {
                _selectedDestinatarioArAdded = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveDestinatarioCommand).RaiseCanExecuteChanged();
            }
        }

        private Anagrafica _selectedDestinatarioArAvailable;
        public Anagrafica SelectedDestinatarioARAvailable
        {
            get
            {
                return _selectedDestinatarioArAvailable;
            }

            set
            {
                _selectedDestinatarioArAvailable = value;
                OnPropertyChanged();
                ((DelegateCommand)AddDestinatarioArCommand).RaiseCanExecuteChanged();
            }
        }




        private Documento _selectDocumentoAvailable;

        private Documento _selectDocumentoAdded;

        public ObservableCollection<Anagrafica> DestinatariAvailable { get; set; }
        public ObservableCollection<Anagrafica> DestinatariArAdded { get; private set; }
        public ObservableCollection<Anagrafica> DestinatariArAvailable { get; }
        public ICommand AddDestinatarioCommand { get; set; }
        public ICommand RemoveDestinatarioCommand { get; set; }
        public ICommand AddDocumentoCommand { get; set; }
        public ICommand RemoveDocumentoCommand { get; set; }

        public ICommand AddDestinatarioArCommand { get; set; }
        public ICommand RemoveDestinatarioArCommand { get; set; }

        public ICommand RemoveDestinatarioARCommand { get; set; }
    }
}
