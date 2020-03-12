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

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailEvent);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            _anagraficaDetailViewModelCreator = anagraficaDetailViewModelCreator;
            NavigationViewModel = navigationViewModel;

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
        }

        
        public ICommand CreateNewDetailCommand { get; set; }
        public async Task LoadAllAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        // Viene riusato sia per visualizzare un'anagrafica esistente sia per crearne una nuova.
        // In questo secondo caso viene passato null come parametro
        private async void OnOpenDetailEvent(OpenDetailViewEventargs args)
        {
            // Verificare se il view model corrente HasChanges
            if (DetailViewModel!=null && DetailViewModel.HasChanges)
            {
               var result = _messageDialogservice.ShowOKCancelDialog("You've made changes. Navigate away ?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            // Additional switches for others view models
            switch(args.ViewModelName)
            {
                case nameof(AnagraficaDetailViewModel) :
                    DetailViewModel = _anagraficaDetailViewModelCreator();
                    break;

            }
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args )
        {
            // The corrisponding view will be hidden
            DetailViewModel = null;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailEvent(new OpenDetailViewEventargs
            {
                ViewModelName = viewModelType.Name
            });
        }



    }
}