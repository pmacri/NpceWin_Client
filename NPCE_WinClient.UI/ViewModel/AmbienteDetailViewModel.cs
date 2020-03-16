﻿using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class AmbienteDetailViewModel : DetailViewModelBase, IAmbienteDetailViewModel 
    {
        private readonly IAmbienteRepository _ambienteRepository;
        private readonly IMessageDialogService _messageDialogService;

        public AmbienteDetailViewModel(IAmbienteRepository ambienteRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService): base(eventAggregator)
        {
            _ambienteRepository = ambienteRepository;
            _messageDialogService = messageDialogService;
        }
        public override async Task LoadAsync(int? id)
        {
            var ambiente = (id.HasValue)
               ? await _ambienteRepository.GetByIdAsync(id.Value)
               : CreateNewAmbiente();

            Ambiente = new AmbienteWrapper(ambiente);

            Ambiente.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _ambienteRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Ambiente.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Ambiente.Id == 0)
            {
                Ambiente.Description = "";
            }
        }

        private AmbienteWrapper _ambiente;

        public AmbienteWrapper Ambiente
        {
            get { return _ambiente; }
            set {
                _ambiente = value;
                OnPropertyChanged();
            }
        }


        private Ambiente CreateNewAmbiente()
        {
            var ambiente = new Ambiente();
            _ambienteRepository.Add(ambiente);
            return ambiente;
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOKCancelDialog($"Do you really want to cancel the ambiente {Ambiente.Description}",
                                                                  "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _ambienteRepository.Remove(Ambiente.Model);
            await _ambienteRepository.SaveAsync();
            RaiseDetailDeletedEvent(Ambiente.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return (_ambiente != null) && (!_ambiente.HasErrors) && (HasChanges);
        }

        protected override async void OnSaveExecute()
        {
            await _ambienteRepository.SaveAsync();
            HasChanges = _ambienteRepository.HasChanges();
            RaiseDetailSavedEvent(Ambiente.Id, $"{Ambiente.Description}");
        }
    }
}
