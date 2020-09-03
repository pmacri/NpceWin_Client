using FriendOrganizer.UI.View.Services;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Event;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class VisuraDetailViewModel : DetailViewModelBase, IServizioDetailViewModel
    {
        private readonly IVisureRepository _visureRepository;
        private readonly IStatoServizioRepository _statoServizioRepository;
        private ObservableCollection<VisureCodiceDocumento> allCodiciDocumento;

        public VisuraWrapper Visura { get; private set; }

        public VisuraDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IVisureRepository visureRepository,
            IStatoServizioRepository statoServizioRepository) : base(eventAggregator, messageDialogService)
        {
            _visureRepository = visureRepository;
            _statoServizioRepository = statoServizioRepository;
            InvioCommand = new DelegateCommand(OnInvioExecute, OnInvioCanExecute);
        }



        private bool OnInvioCanExecute()
        {
            return true;
        }

        private void OnInvioExecute()
        {
            throw new NotImplementedException();
        }

        public override async Task LoadAsync(int id)
        {
            Visura visura = (id > 0)
                 ? await _visureRepository.GetByIdAsync(id)
                 : CreateNewVisura();
            Id = id;

            TipiDocumento = new ObservableCollection<VisureTipoDocumento>(await _visureRepository.GetAllTipiDocumentoAsync());

            FormatiDocumento = new ObservableCollection<VisureFormatoDocumento>(await _visureRepository.GetAllFormatiDocumentoAsync());

            allCodiciDocumento = new ObservableCollection<VisureCodiceDocumento>(await _visureRepository.GetAllCodiciDocumentoAsync());

            TipiRecapito = new ObservableCollection<VisureTipoRecapito>(await _visureRepository.GetAllTipiRecapitoAsync());

            InitializeVisura(visura);

            SetTitle(id);

        }

        private void InitializeVisura(Visura visura)
        {
            Visura = new VisuraWrapper(visura);
            Visura.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _visureRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Visura.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            //if (Visura.Id == 0)
            //{
            //    Visura.DestinatarioNominativo = "";
            //}

        }

        private void SetTitle(int id)
        {
            if (id<=0)
            {
                Title = "Nuova Visura";
            }
            else
            {
                Title = $"Edit Visura {id.ToString()}";
            }
        }

        private Visura CreateNewVisura()
        {
            Visura visura = new Visura
            {
                RichiedenteCognome = "ROSSI",
                RichiedenteNome = "MARIO",
                RichiedenteCap = "00144",
                RichiedenteIndirizzo = "VIALE EUROPA 175",
                RichiedenteLocalita = "ROMA ",
                RichiedentePrefissoTelefono = "06",
                RichiedenteTelefono = "282741",

                DestinatarioCap = "00144",
                DestinatarioEmail = "casarol7@posteitaliane.it",
                DestinatarioLocalita = "ROMA",
                DestinatarioNominativo = "BIANCHI GIULIO",

                DocumentoIntestatarioCCIAA = "RM",
                DocumentoIntestatarioNREA = "1064345",

                DocumentoIntestatarioCognome = "Verdi",
                DocumentoIntestatarioNome = "Emilio",
                DocumentoIntestatarioCodiceFiscale = "MCRPQL64T08F537U"

            };

            _visureRepository.Add(visura);

            return visura;
        }

        protected override async void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to cancel the visura ?",
                                                                  "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _visureRepository.Remove(Visura.Model);
            await _visureRepository.SaveAsync();
            RaiseDetailDeletedEvent(Visura.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return true;
        }
        protected override async void OnSaveExecute()
        {
            var statoCreated = _statoServizioRepository.GetByDescription("Salvato");
            Visura.StatoServizioId = statoCreated.Id;
            Visura.IdRichiesta = null;
            await _visureRepository.SaveAsync();
            Id = Visura.Id;
            HasChanges = _visureRepository.HasChanges();
            EventAggregator.GetEvent<VisuraSavedEvent>().Publish(new VisuraSavedEventArgs { Visura = Visura.Model });
        }
        public ObservableCollection<VisureTipoDocumento> TipiDocumento { get; set; }
        public ObservableCollection<VisureFormatoDocumento> FormatiDocumento { get; set; }

        ObservableCollection<VisureCodiceDocumento> codiciDocumento;
        public ObservableCollection<VisureCodiceDocumento> CodiciDocumento
        {
            get
            {
                return codiciDocumento;
            }

            set
            {
                codiciDocumento = value;
                OnPropertyChanged("CodiciDocumento");
            }
        }
        public ObservableCollection<VisureTipoRecapito> TipiRecapito { get; set; }
        public ICommand InvioCommand { get; set; }
        public ICommand TipoDocumentoChanged { get; set; }

        private string tipoDocumentoSelected;
        public string TipoDocumentoSelected
        {
            get { return tipoDocumentoSelected; }
            set
            {
                tipoDocumentoSelected = value;
                Visura.VisureTipoDocumentoId = tipoDocumentoSelected;
                CodiciDocumento = new ObservableCollection<VisureCodiceDocumento>(allCodiciDocumento.Where(c => c.VisureTipoDocumentoId == tipoDocumentoSelected));
            }
        }
    }
}
