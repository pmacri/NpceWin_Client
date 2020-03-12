using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
using NPCE_WinClient.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogservice;
        private Func<IAnagraficaDetailViewModel> _anagraficaDetailViewModelCreator;
        private IDetailViewModel _detailViewModel;

        public ObservableCollection<Service> Services { get; set; }
        public INavigationViewModel NavigationViewModel { get;}
        public IServiceDetailViewModel ServiceDetailViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set { 
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel(INavigationViewModel navigationViewModel, 
                            IServiceDetailViewModel serviceDetailViewModel,
                            Func<IAnagraficaDetailViewModel> anagraficaDetailViewModelCreator, 
                            IEventAggregator eventAggregator,
                            IMessageDialogService messageDialogservice)
        {
            ServiceDetailViewModel = serviceDetailViewModel;
            _eventAggregator = eventAggregator;

            _messageDialogservice = messageDialogservice;

            _eventAggregator.GetEvent<OpenDetailAnagraficaViewEvent>()
                .Subscribe(OnOpenAnagraficaDetailEvent);
            _eventAggregator.GetEvent<AfterAnagraficaDeletedEvent>()
                .Subscribe(AfterFriendDeleted);

            _anagraficaDetailViewModelCreator = anagraficaDetailViewModelCreator;
            NavigationViewModel = navigationViewModel;

            CreateNewAnagraficaCommand = new DelegateCommand(OnCreateNewAnagraficaExecute);
        }

        
        public ICommand CreateNewAnagraficaCommand { get; set; }
        public async Task LoadAllAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        // Viene riusato sia per visualizzare un'anagrafica esistente sia per crearne una nuova.
        // In questo secondo caso viene passato null come parametro
        private async void OnOpenAnagraficaDetailEvent(int? AnagraficaId)
        {
            //Verificare se il view model corrente HasChanges
            if (DetailViewModel!=null && DetailViewModel.HasChanges)
            {
               var result = _messageDialogservice.ShowOKCancelDialog("You've made changes. Navigate away ?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }

            }
            DetailViewModel = _anagraficaDetailViewModelCreator();
            await DetailViewModel.LoadAsync(AnagraficaId);
        }

        private void AfterFriendDeleted(long anagraficaId)
        {
            // The corrisponding view will be hidden
            DetailViewModel = null;
        }

        private void OnCreateNewAnagraficaExecute()
        {
            OnOpenAnagraficaDetailEvent(null);
        }



    }
}