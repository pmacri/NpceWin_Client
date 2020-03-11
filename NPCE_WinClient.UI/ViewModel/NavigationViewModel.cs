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
            _eventAggregator.GetEvent<AfterAnagraficaDeletedEvent>().Subscribe(AfterAnagraficaDeleted);
        }

        
        private void AfterAnagraficaSaved(AfterAnagraficaSavedEventArgs obj)
        {
            var lookupItem = Anagrafiche.SingleOrDefault(l => l.Id == obj.Id);
            if (lookupItem==null)
            {
                lookupItem = new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator);
                Anagrafiche.Add(lookupItem);
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookups = await _serviceLookupDataService.GetAnagraficaLookupAsync();

            Anagrafiche.Clear();

            foreach (var lookup in lookups)
            {
                Anagrafiche.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember,_eventAggregator));
            }
        }

        private void AfterAnagraficaDeleted(long anagraficaId)
        {
            var anagrafica = Anagrafiche.SingleOrDefault(a => a.Id == anagraficaId);
            if (anagrafica != null)
            {
                Anagrafiche.Remove(anagrafica);
            }
        }

        public ObservableCollection<NavigationItemViewModel> Anagrafiche { get; set; }

        

    }
}
