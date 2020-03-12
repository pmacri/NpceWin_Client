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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }


        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(AnagraficaDetailViewModel):
                    var lookupItem = Anagrafiche.SingleOrDefault(l => l.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        lookupItem = new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator,
                            nameof(AnagraficaDetailViewModel));
                        Anagrafiche.Add(lookupItem);
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    };
                    break;
            }

            
        }

        public async Task LoadAsync()
        {
            var lookups = await _serviceLookupDataService.GetAnagraficaLookupAsync();

            Anagrafiche.Clear();

            foreach (var lookup in lookups)
            {
                Anagrafiche.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember,
                    _eventAggregator, nameof(AnagraficaDetailViewModel)));
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(AnagraficaDetailViewModel):
                    var anagrafica = Anagrafiche.SingleOrDefault(a => a.Id == args.Id);
                    if (anagrafica != null)
                    {
                        Anagrafiche.Remove(anagrafica);
                    };
                    break;
            }
        }

        public ObservableCollection<NavigationItemViewModel> Anagrafiche { get; set; }



    }
}
