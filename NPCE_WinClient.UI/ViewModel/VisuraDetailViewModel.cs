using NPCE_WinClient.Model;
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
    public class VisuraDetailViewModel : DetailViewModelBase, IServizioDetailViewModel
    {
        private readonly IVisureRepository _visureRepository;

        public VisuraWrapper Visura { get; private set; }

        public VisuraDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IVisureRepository visureRepository) : base(eventAggregator, messageDialogService)
        {
            _visureRepository = visureRepository;
        }

        public override async Task LoadAsync(int id)
        {
            Visura visura = (id > 0)
                 ? await _visureRepository.GetByIdAsync(id)
                 : CreateNewVisura();
            Id = id;

            InitializaVisura(visura);

        }

        private void InitializaVisura(Visura visura)
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
            if (Visura.Id == 0)
            {
                Visura.DestinatarioNominativo = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = "Nuova Visura";
        }

        private Visura CreateNewVisura()
        {
            Visura visura = new Visura();
            _visureRepository.Add(visura);

            return visura;
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }
    }
}
