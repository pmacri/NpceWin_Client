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
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        
        private IAnagraficaLookupDataService _serviceLookupDataService;

        private IEventAggregator _eventAggregator;
        public NavigationViewModel(IAnagraficaLookupDataService serviceLookupDataService, IEventAggregator eventAggregator)
        {
            _serviceLookupDataService = serviceLookupDataService;
            _eventAggregator = eventAggregator;
            Anagrafiche = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterAnagraficaSavedEvent>().Subscribe(AfterAnagraficaSaved);
        }

        private void AfterAnagraficaSaved(AfterAnagraficaSavedEventArgs obj)
        {
            var lookupItem = Anagrafiche.First(l => l.Id == obj.Id);

            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookups = await _serviceLookupDataService.GetAnagraficaLookupAsync();

            Anagrafiche.Clear();

            foreach (var lookup in lookups)
            {
                Anagrafiche.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember));
            }
        }
        public ObservableCollection<NavigationItemViewModel> Anagrafiche { get; set; }

        private NavigationItemViewModel _selectedAnagrafica;
        public NavigationItemViewModel SelectedAnagrafica
        {
            get { return _selectedAnagrafica; }
            set { 
                _selectedAnagrafica = value;
                OnPropertyChanged();
                if(_selectedAnagrafica != null)
                {
                    _eventAggregator.GetEvent<OpenDetailAnagraficaViewEvent>().Publish(_selectedAnagrafica.Id);
                }
            }
        }

    }
}
