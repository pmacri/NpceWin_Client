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
    public class PagineBollettinoDetailViewModel : DetailViewModelBase, IPagineBollettinoDetailViewModel
    {
        private IServizioRepository _servizioRepository;

        public PagineBollettinoDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IServizioRepository servizioRepository) : base(eventAggregator, messageDialogService)
        {
            _servizioRepository = servizioRepository;
            NuovaPaginaCommand = new DelegateCommand(OnNuovaPaginaExecute);
            NuovoBollettinoCommand = new DelegateCommand(OnNuovoBollettinoExecute, OnNuovoBollettinoCanExecute);
            SaveBollettiniCommand = new DelegateCommand(OnSaveBollettiniExecute);
            NumeroPaginaCorrente = 0;
        }

        private void OnSaveBollettiniExecute()
        {
            _servizioRepository.SaveAsync();
        }

        private bool OnNuovoBollettinoCanExecute()
        {
            return PaginaBollettinoSelected != null;
        }

        private void OnNuovoBollettinoExecute()
        {
            Bollettino = new Bollettino { IntestatoA = "A me asassassasasas" };
            PaginaBollettinoSelected.Bollettini.Add(Bollettino);
        }

        private void OnNuovaPaginaExecute()
        {
            Servizio.PagineBollettini.Add(new PaginaBollettino { Description = $"Pagina {++NumeroPaginaCorrente}" }); 
        }

        public override async Task LoadAsync(int id)
        {
            var servizio = await _servizioRepository.GetByIdAsync(id);
            Servizio = new ServizioWrapper(servizio);
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

        public ServizioWrapper Servizio { get; set; }

        public ICommand NuovaPaginaCommand { get; set; }
        public int NumeroPaginaCorrente { get; set; }

        PaginaBollettino _paginaBollettinoSelected;
        public PaginaBollettino PaginaBollettinoSelected {
            get
            {
                return _paginaBollettinoSelected;
            }            

            set
            {
                _paginaBollettinoSelected = value;
                //foreach (var bollettino in _paginaBollettinoSelected.Bollettini)
                //{
                //    BollettiniPerPagina.Add(bollettino);
                //}
                OnPropertyChanged("PaginaBollettinoSelected");
                ((DelegateCommand)NuovoBollettinoCommand).RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<Bollettino> BollettiniPerPagina{ get; set; }

        Bollettino _bollettino;
        public Bollettino Bollettino { 
            get {
                return _bollettino;
            } 
            set {
                _bollettino = value;
                OnPropertyChanged("Bollettino");
            } }
        public ICommand NuovoBollettinoCommand { get; set; }

        public ICommand SaveBollettiniCommand { get; set; }
    }
}
