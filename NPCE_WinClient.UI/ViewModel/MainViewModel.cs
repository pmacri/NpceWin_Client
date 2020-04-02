using Autofac.Features.Indexed;
using FriendOrganizer.UI.View.Services;
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
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogservice;
        
        private IDetailViewModel _selectedDetailViewModel;

        public INavigationViewModel NavigationViewModel { get;}

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set { 
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel(INavigationViewModel navigationViewModel,
                            IIndex<string, IDetailViewModel> detailViewModelCreator,
                            IEventAggregator eventAggregator,
                            IMessageDialogService messageDialogservice)
        {

            _detailViewModelCreator = detailViewModelCreator;

            _eventAggregator = eventAggregator;

            _messageDialogservice = messageDialogservice;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailEvent);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);
            _eventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Subscribe(AfterDetailClosed);

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            NavigationViewModel = navigationViewModel;

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailExecute);
        }

      

        public ICommand CreateNewDetailCommand { get; set; }
        public DelegateCommand<Type> OpenSingleDetailViewCommand { get; }

        public async Task LoadAllAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        // Viene riusato sia per visualizzare un'anagrafica esistente sia per crearne una nuova.
        // In questo secondo caso viene passato null come parametro
        private async void OnOpenDetailEvent(OpenDetailViewEventargs args)
        {

           var detailViewModel=  DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id &&
           vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            // Prima di aggiornare il ViewModel corrente Verificare se il view model corrente HasChanges
            if (SelectedDetailViewModel!=null && SelectedDetailViewModel.HasChanges)
            {
               var result = await _messageDialogservice.ShowOkCancelDialogAsync("You've made changes. Navigate away ?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            SelectedDetailViewModel = detailViewModel;
            
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args )
        {
            // The corrisponding view will be hidden
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string viewModelname)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == id &&
                      vm.GetType().Name == viewModelname);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private int nextNewItemId;
        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailEvent(new OpenDetailViewEventargs
            {
                Id = nextNewItemId--,
                ViewModelName = viewModelType.Name
            });
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void OnOpenSingleDetailExecute(Type viewModelType)
        {
            OnOpenDetailEvent(new OpenDetailViewEventargs
            {
                Id = -1,
                ViewModelName = viewModelType.Name
            });
        }
    }
}