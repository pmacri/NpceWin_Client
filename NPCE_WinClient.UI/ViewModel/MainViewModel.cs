using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
using NPCE_WinClient.UI.View.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPCE_WinClient.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogservice;
        private Func<IAnagraficaDetailViewModel> _anagraficaDetailViewModelCreator;
        public ObservableCollection<Service> Services { get; set; }
        public INavigationViewModel NavigationViewModel { get;}
        public IServiceDetailViewModel ServiceDetailViewModel { get; }

        private IAnagraficaDetailViewModel _anagraficaDetailViewModel;
        public IAnagraficaDetailViewModel AnagraficaDetailViewModel
        {
            get { return _anagraficaDetailViewModel; }
            set { 
                _anagraficaDetailViewModel = value;
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

            _anagraficaDetailViewModelCreator = anagraficaDetailViewModelCreator;
            NavigationViewModel = navigationViewModel;
        }
        public async Task LoadAllAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenAnagraficaDetailEvent(long AnagraficaId)
        {
            //Verificare se il view model corrente HasChanges
            if (AnagraficaDetailViewModel!=null && AnagraficaDetailViewModel.HasChanges)
            {
               var result = _messageDialogservice.ShowOKCancelDialog("You've made changes. Navigate away ?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }

            }
            AnagraficaDetailViewModel = _anagraficaDetailViewModelCreator();
            await AnagraficaDetailViewModel.LoadById(AnagraficaId);
        }


    }
}