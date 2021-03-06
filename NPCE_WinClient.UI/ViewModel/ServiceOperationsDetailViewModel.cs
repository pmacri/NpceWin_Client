﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL;
using FriendOrganizer.UI.View.Services;
using MahApps.Metro.Controls.Dialogs;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Npce;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace NPCE_WinClient.UI.ViewModel
{
    public class ServiceOperationsDetailViewModel : DetailViewModelBase, IServiceOperationsDetailViewModel
    {
        private IAmbienteRepository _ambienteRepository;
        private IServizioRepository _servizioRepository;
        private IStatoServizioRepository _statoServizioRepository;
        private IMessageDialogService _messageDialogService;
        private IEnumerable<TipoServizio> _allTipi;
        private string tipoArchiviazioneSelected;


        public ServiceOperationsDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IAmbienteRepository ambienteRepository,
            IServizioRepository servizioRepository,
            IStatoServizioRepository statoServizioRepository) : base(eventAggregator, messageDialogService)
        {
            Title = "Service Operations";

            TipiServizio = new ObservableCollection<TipoServizio>();

            _ambienteRepository = ambienteRepository;
            _servizioRepository = servizioRepository;
            _statoServizioRepository = statoServizioRepository;
            _messageDialogService = messageDialogService;

            Ambienti = new ObservableCollection<AmbienteWrapper>();

            Servizi = new ObservableCollection<ServizioWrapper>();

            InvioCommand = new DelegateCommand(OnInvioExecute, OnInvioCanExecute);

            ConfermaCommand = new DelegateCommand(OnConfermaExecute, OnConfermaCanExecute);

            PreConfermaCommand = new DelegateCommand(OnPreConfermaExecute, OnPreConfermaCanExecute);

            TipiArchiviazione = new ObservableCollection<string>();
            TipiArchiviazione.Add("NESSUNA");
            TipiArchiviazione.Add("SEMPLICE");
            TipiArchiviazione.Add("STORICA");

        }

        private bool OnConfermaCanExecute()
        {
            return true;
        }

        private void OnConfermaExecute()
        {
            NpceOperationResult result = null;

            switch (TipoServizio.Descrizione)
            {
                //case "Posta1":
                //case "Posta4":
                //    {
                //        result = ConfermaLolExecute();
                //    }
                //    break;

                //case "Raccomandata":
                //    {
                //        result = ConfermaRolExecute();
                //    }
                //    break;
                case "Mol1":
                case "MOL4":
                case "COL1":
                case "COL4":
                    {
                        result =  ConfermaPostaEvo();
                    }
                    break;
            }
            string message;

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\n Description: {result.Errors[0].Description}";
            }

             _messageDialogService.ShowOkCancelDialogAsync(message, "Info");

            if (result.Success)
            {
                Servizio.IdRichiesta = result.IdRichiesta;
                Servizio.IdOrdine = result.IdOrdine;
                // TODO: AutoConferma
                var statoCreated = AutoConfirm ? _statoServizioRepository.GetByDescription("Confermato") : _statoServizioRepository.GetByDescription("PreConfermato");
                Servizio.Model.StatoServizioId = statoCreated.Id;
                OnSaveExecute();
            }
        }

        private NpceOperationResult ConfermaPostaEvo()
        {
            NpceOperationResult result;

            if (Ambiente.IsPil)
            {
                var postaEvoPil = new PostaEvoPil(_servizio.Model, Ambiente.Model);

                result =  postaEvoPil.Conferma();
            }
            else
            {
                var preConfermaOperation = new PreConfermaLol(Ambiente.Model, Servizio.Model, Servizio.IdRichiesta, Servizio.GuidUtente, AutoConfirm);

                result = preConfermaOperation.Execute();
            }

            return result;
        }

        private async void OnPreConfermaExecute()
        {

            NpceOperationResult result = null;

            switch (TipoServizio.Descrizione)
            {
                case "Posta1":
                case "Posta4":
                    {
                        result = PreConfermaLolExecute();
                    }
                    break;

                case "Raccomandata":
                    {
                        result = PreConfermaRolExecute();
                    }
                    break;
            }
            string message;

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\n Description: {result.Errors[0].Description}";
            }

            await _messageDialogService.ShowOkCancelDialogAsync(message, "Info");

            

            if (result.Success)
            {
                Servizio.IdRichiesta = result.IdRichiesta;
                Servizio.IdOrdine = result.IdOrdine;

                // TODO: AutoConferma
                var statoCreated = AutoConfirm ? _statoServizioRepository.GetByDescription("Confermato") : _statoServizioRepository.GetByDescription("PreConfermato");
                Servizio.Model.StatoServizioId = statoCreated.Id;
                OnSaveExecute();
            }
           
        }

        private NpceOperationResult PreConfermaRolExecute()
        {
            var preConfermaOperation = new PreConfermaRol(Ambiente.Model, Servizio.Model, Servizio.IdRichiesta, Servizio.GuidUtente, AutoConfirm);

            var result = preConfermaOperation.Execute();

            return result;
        }

        private NpceOperationResult PreConfermaLolExecute()
        {
            NpceOperationResult result;

            if (Ambiente.IsPil)
            {
                var letteraPil = new LetteraPil(_servizio.Model, Ambiente.Model);

                result = letteraPil.PreConferma(_servizio.IdRichiesta, AutoConfirm);
            }
            else
            {
                var preConfermaOperation = new PreConfermaLol(Ambiente.Model, Servizio.Model, Servizio.IdRichiesta, Servizio.GuidUtente, AutoConfirm);

                result = preConfermaOperation.Execute();
            }

            return result;
        }

        private bool OnPreConfermaCanExecute()
        {
            return (TipoServizio != null && Ambiente != null && Servizio != null && Servizio.StatoServizio.Descrizione == "Inviato");
        }

        private bool OnInvioCanExecute()
        {
            return (TipoServizio != null && Ambiente != null && Servizio != null && Servizio.StatoServizio.Descrizione=="Salvato");
        }

        private async void OnInvioExecute()
        {
            // Impostare sul servizio il suo TipoServizio
            _servizioRepository.UpdateTipoServizioAsync(Servizio.Id, TipoServizio.Id);

            NpceOperationResult result = null;

            switch (TipoServizio.Descrizione)
            {
                case "Posta1":
                case "Posta4":
                    {
                        result = await InvioLolExecute();
                    }
                    break;

                case "Raccomandata":
                    {
                        result = await InvioRolExecute();
                    }
                    break;
               
                case "Mol4":
                case "Mol1":
                    {
                        result = await InvioMolExecute();
                    }
                    break;

                case "Col4":
                case "Col1":
                    {
                        result = await InvioColExecute();
                    }
                    break;
            }

            string message;

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\n Description: {result.Errors[0].Description}";
            }

            await _messageDialogService.ShowOkCancelDialogAsync(message, "Info");

            if (result.Success)
                {
                Servizio.IdRichiesta = result.IdRichiesta;

                Servizio.GuidUtente = result.GuidUtente;

                Servizio.IdOrdine = result.IdOrdine;

                var statoCreated =Servizio.Autoconferma ? _statoServizioRepository.GetByDescription("Confermato") : _statoServizioRepository.GetByDescription("Inviato");

                Servizio.Model.StatoServizioId = statoCreated.Id;

                OnSaveExecute();

                TipoServizio.Id = Servizio.TipoServizio.Id;
            }

        }

        private async Task<NpceOperationResult> InvioColExecute()
        {
            var idServizio = Servizio.Id;

            var servizio = await _servizioRepository.GetByIdAsync(idServizio);

            var invioOperation = new InvioCol(Ambiente.Model, servizio, null);

            var result = invioOperation.Execute();

            return result;
        }

        private async Task<NpceOperationResult> InvioLolExecute()
        {
            NpceOperationResult result = null;

            if (Ambiente.IsPil)
            {
                ServizioPil servizioPil = new LetteraPil(Servizio.Model, Ambiente.Model);

                result = servizioPil.Invio();
            }
            else
            {
                var operation = new RecuperaIdRichiestaLol(Ambiente.Model);

                var idRichiesta = operation.Execute();

                var idServizio = Servizio.Id;

                var servizio = await _servizioRepository.GetByIdAsync(idServizio);

                var invioOperation = new InvioLol(Ambiente.Model, servizio, idRichiesta);

                result = invioOperation.Execute();
            }
            

            return result;
        }

        private async Task<NpceOperationResult> InvioRolExecute()
        {
            NpceOperationResult result = null;

            if (Ambiente.IsPil)
            {
                ServizioPil servizioPil = new RaccomandataPil(Servizio.Model, Ambiente.Model);

                result = servizioPil.Invio();
            }
            else
            {
                var operation = new RecuperaIdRichiestaRol(Ambiente.Model);

                var idRichiesta = operation.Execute();

                var idServizio = Servizio.Id;

                var servizio = await _servizioRepository.GetByIdAsync(idServizio);

                var invioOperation = new InvioRol(Ambiente.Model, servizio, idRichiesta);

                result = invioOperation.Execute();
            }
            
            return result;
        }

        private async Task<NpceOperationResult> InvioMolExecute()
        {

            NpceOperationResult result = null;

            var idServizio = Servizio.Id;

            var servizio = await _servizioRepository.GetByIdAsync(idServizio);

            if (Ambiente.IsPil)
            {
                ServizioPil servizioPil = new PostaEvoPil(Servizio.Model, Ambiente.Model);

                result = servizioPil.Invio();
            }
            else
            {
                InvioMol invioOperation = new InvioMol(Ambiente.Model, servizio, null);

                result = invioOperation.Execute();
            }
            
            return result;
        }

        public ICommand InvioCommand { get; set; }

        public ICommand ConfermaCommand { get; set; }

        public DelegateCommand PreConfermaCommand { get; }

        public async override Task LoadAsync(int id)
        {
            if (TipoServizio == null)
            {
                _allTipi = _servizioRepository.GetAllTipiServizio();
                TipiServizio.Clear();

                foreach (var tipo in _allTipi)
                {
                    TipiServizio.Add(tipo);
                }
            }
            if (Ambiente == null)
            {
                var ambienti = await _ambienteRepository.GetAllAsync();

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

            var servizi = await _servizioRepository.GetAllAsync();
            InitializeServizi(servizi);

            
        }

        private void InitializeServizi(IEnumerable<Servizio> servizi)
        {
            foreach (var wrapper in Servizi)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Servizi.Clear();

            foreach (var servizio in servizi)
            {
                var wrapper = new ServizioWrapper(servizio);
               
                wrapper.PropertyChanged += Wrapper_PropertyChanged;

                Servizi.Add(wrapper);
            }
            
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _ambienteRepository.HasChanges();
            }
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
            await _servizioRepository.SaveAsync();
            await LoadAsync(-1);
        }

        public ObservableCollection<AmbienteWrapper> Ambienti { get; set; }

        private AmbienteWrapper ambienteWrapper;

        private TipoServizio _tipoServizio;
        public AmbienteWrapper Ambiente
        {
            get { return ambienteWrapper; }
            set
            {
                ambienteWrapper = value;
                OnPropertyChanged();
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)PreConfermaCommand).RaiseCanExecuteChanged();
            }
        }
        public TipoServizio TipoServizio
        {
            get
            {
                return _tipoServizio;
            }
            set
            {
                _tipoServizio = value;
                if (_tipoServizio != null)
                {
                    HasChanges = _servizioRepository.HasChanges();
                    ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)PreConfermaCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<TipoServizio> TipiServizio
        {
            get;
            set;
        }
        public ObservableCollection<ServizioWrapper> Servizi { get; set; }

        ServizioWrapper _servizio;
        public ServizioWrapper Servizio
        {
            get { return _servizio; }

            set {
                _servizio = value;
                OnPropertyChanged("Servizio");
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)PreConfermaCommand).RaiseCanExecuteChanged();
            }
        }

        public bool AutoConfirm { get; set; }

        public ObservableCollection<string> TipiArchiviazione { get; set; }

        public string TipoArchiviazioneSelected
        {
            get { return tipoArchiviazioneSelected; }
            set
            {
                tipoArchiviazioneSelected = value;
                Servizio.TipoArchiviazione = tipoArchiviazioneSelected;
            }
        }

        private int anniArchiviazione;
        public int AnniArchiviazione
        {
            get { return anniArchiviazione; }
            set { 
                anniArchiviazione = value;
                Servizio.AnniArchiviazione = anniArchiviazione;            
            }
        }

    }
}
