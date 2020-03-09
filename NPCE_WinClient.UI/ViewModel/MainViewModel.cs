using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {

        public ObservableCollection<Service> Services { get; set; }

        private Func<IAnagraficaDetailViewModel> _anagraficaDetailViewModelCreator;

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


        private IEventAggregator _eventAggregator;

        public MainViewModel(INavigationViewModel navigationViewModel, 
                            IServiceDetailViewModel serviceDetailViewModel,
                            Func<IAnagraficaDetailViewModel> anagraficaDetailViewModelCreator, 
                            IEventAggregator eventAggregator)
        {
            ServiceDetailViewModel = serviceDetailViewModel;
            _eventAggregator = eventAggregator;

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
            AnagraficaDetailViewModel = _anagraficaDetailViewModelCreator();
            await AnagraficaDetailViewModel.LoadById(AnagraficaId);
        }


    }
}