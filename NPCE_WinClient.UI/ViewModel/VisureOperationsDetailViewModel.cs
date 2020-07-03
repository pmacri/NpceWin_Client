using FriendOrganizer.UI.View.Services;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Event;
using NPCE_WinClient.UI.Npce;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPCE_WinClient.UI.ViewModel
{
    public class VisureOperationsDetailViewModel : DetailViewModelBase, IDetailViewModel
    {
        private readonly IVisureRepository visureRepository;
        private readonly IAmbienteRepository ambienteRepository;
        private readonly IStatoServizioRepository statoServizioRepository;
        private int idStatoServizioSalvato;

        public VisureOperationsDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IVisureRepository visureRepository,
            IStatoServizioRepository statoServizioRepository,
            IAmbienteRepository ambienteRepository) : base(eventAggregator, messageDialogService)
        {
            this.visureRepository = visureRepository;
            this.ambienteRepository = ambienteRepository;
            this.statoServizioRepository = statoServizioRepository;

            idStatoServizioSalvato = statoServizioRepository.GetByDescription("Salvato").Id;

            Ambienti = new ObservableCollection<AmbienteWrapper>();

            Visure = new ObservableCollection<VisuraWrapper>();

            InvioCommand = new DelegateCommand(OnInvioExecute, OnInvioCanExecute);

            ConfermaCommand = new DelegateCommand(OnConfermaExecute, OnConfermaCanExecute);

            CopyIdRichiestaCommand = new DelegateCommand<VisuraWrapper>(OnCopyIdRichiestaExecute);

            EditVisuraCommand = new DelegateCommand<VisuraWrapper>(OnEditVisuraExecute);
        }

        private void OnEditVisuraExecute(VisuraWrapper visura)
        {
            EventAggregator.GetEvent<EditVisuraEvent>().Publish(new EditVisuraEventArgs { IdVisura = visura.Id });
        }

        private void OnCopyIdRichiestaExecute(VisuraWrapper visura)
        {
            Clipboard.SetText(visura.IdRichiesta?? string.Empty);
        }

        private bool OnConfermaCanExecute()
        {
            statoInviato = statoServizioRepository.GetByDescription("Inviato");
            return ( Visura != null && Visura.StatoServizioId == statoInviato.Id);
        }

        private async void OnConfermaExecute()
        {
            NpceOperationResult result = null;
            string message;

            if (Ambiente.IsPil)
            {
                var confermaPil = new ConfermaVisuraPil(Visura.Model, Ambiente.Model);

                result = confermaPil.Execute();
                
            }
            else
            {
                var vol = new Vol(_ambiente.Model, _visura.Model, null);

                result = vol.Conferma();
            }

           

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\nDescription: {result.Errors[0].Description}";
            }

            await MessageDialogService.ShowOkCancelDialogAsync(message, "Info");

            if (result.Success)
            {

                var newState = statoServizioRepository.GetByDescription("Confermato");

                Visura.Model.StatoServizioId = newState.Id;


                OnSaveExecute();
            } 
        }

        private async void OnInvioExecute()
        {
            //Visura.VisureTipoRecapito = new VisureTipoRecapito { Id = "DL", Descrizione = "Download" };
            NpceOperationResult result=null;
            string message;

            if (Ambiente.IsPil)
            {
                var operation = new InvioVisuraPIL(Visura.Model, Ambiente.Model);

                result = operation.Execute(AutoConferma, ControllaPrezzo);
            }
            else
            {
                var vol = new Vol(_ambiente.Model, _visura.Model, null);

                result = vol.Invio();
            }

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\nDescription: {result.Errors[0].Description}";
            }

            await MessageDialogService.ShowOkCancelDialogAsync(message, "Info");

            if (result.Success)
            {
                Visura.IdRichiesta = result.IdRichiesta;

                statoCreated = AutoConferma ? statoServizioRepository.GetByDescription("Confermato") : statoServizioRepository.GetByDescription("Inviato");

                Visura.Model.StatoServizioId = statoInviato.Id;

                Visura.AmbienteId = Ambiente.Id;
                Visura.IdOrdine = result.IdOrdine;

                OnSaveExecute();
            }
        }

        private bool OnInvioCanExecute()
        {
            return Visura != null && Visura.StatoServizioId == idStatoServizioSalvato && Ambiente != null;
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override async void OnSaveExecute()
        {
            await visureRepository.SaveAsync();
            await LoadAsync(-1);
        }

        public override async Task LoadAsync(int id)
        {
            if (Ambiente == null)
            {
                var ambienti = await ambienteRepository.GetAllAsync();

                foreach (var wrapper in Ambienti)
                {
                    wrapper.PropertyChanged -= Wrapper_PropertyChanged;
                }

                Ambienti.Clear();

                foreach (var ambiente in ambienti)
                {
                    var wrapper = new AmbienteWrapper(ambiente);
                    wrapper.PropertyChanged += Wrapper_PropertyChanged;
                    Ambienti.Add(wrapper);
                }
            }

            Id = id;

            var visure = await visureRepository.GetAllAsync();

            InitializeVisure(visure);
        }

        private void InitializeVisure(IEnumerable<Visura> visure)
        {
            Title = "Visure Operations";

            foreach (var wrapper in Visure)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Visure.Clear();

            foreach (var visura in visure)
            {
                var wrapper = new VisuraWrapper(visura);

                wrapper.PropertyChanged += Wrapper_PropertyChanged;

                Visure.Add(wrapper);
            }

        }

        public void UpdateVisure(Visura visura)
        {
            var item = Visure.Where(v => v.Id == visura.Id).FirstOrDefault();

            if (item== null)
            {
                var wrapper = new VisuraWrapper(visura);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                Visure.Add(wrapper);
            }
            else
            {
                item.PropertyChanged -= Wrapper_PropertyChanged;
                Visure.Remove(item);
                var newWrapper = new VisuraWrapper(visura);
                newWrapper.PropertyChanged += Wrapper_PropertyChanged;
                Visure.Add(newWrapper); 
            }
           // OnPropertyChanged("Visure");
        }

        public void DeleteVisura(int visuraId)
        {
            var item = Visure.Where(v => v.Id == visuraId).FirstOrDefault();

            if (item != null)
            {
                Visure.Remove(item);
            }
            
            //OnPropertyChanged("Visure");
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = ambienteRepository.HasChanges();
            }
        }

        public ObservableCollection<AmbienteWrapper> Ambienti { get; set; }


        AmbienteWrapper _ambiente;
        public AmbienteWrapper Ambiente
        {
            get { return _ambiente; }
            set
            {
                _ambiente = value;
                OnPropertyChanged();
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
            }
        }

        VisuraWrapper _visura;
        private StatoServizio statoCreated;
        private StatoServizio statoInviato;

        public VisuraWrapper Visura { get
            {
                return _visura;
            }
            set {
                _visura = value;
                OnPropertyChanged();
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ConfermaCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<VisuraWrapper> Visure { get; set; }

        public DelegateCommand InvioCommand { get; set; }

        public DelegateCommand<VisuraWrapper> CopyIdRichiestaCommand { get; set; }

        public DelegateCommand<VisuraWrapper> EditVisuraCommand { get; set; }        

        public DelegateCommand ConfermaCommand { get; set; }

        public bool AutoConferma { get; set; }

        public bool ControllaPrezzo { get; set; }
    }
}
