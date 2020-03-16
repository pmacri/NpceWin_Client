using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Data.Lookups;
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
        private IAnagraficaLookupDataService _anagraficaLookupDataService;
        private readonly IAmbienteLookupDataService _ambienteLookupDataService;
        private IDocumentoLookupDataService _documentoLookupDataService;
        private IEventAggregator _eventAggregator;
        public NavigationViewModel(
            IEventAggregator eventAggregator, 
            IDocumentoLookupDataService documentoLookupDataService,
            IAnagraficaLookupDataService anagraficaLookupDataService,
            IAmbienteLookupDataService ambienteLookupDataService)
        {
            _ambienteLookupDataService = ambienteLookupDataService;
            _anagraficaLookupDataService = anagraficaLookupDataService;
            _documentoLookupDataService = documentoLookupDataService;
            _eventAggregator = eventAggregator;
            Anagrafiche = new ObservableCollection<NavigationItemViewModel>();
            Documenti = new ObservableCollection<NavigationItemViewModel>();
            Ambienti = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }


        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(AnagraficaDetailViewModel):
                    AfterDetailSaved(Anagrafiche, obj);
                    break;
                case nameof(DocumentoDetailViewModel):
                    AfterDetailSaved(Documenti, obj);
                    break;
                case nameof(AmbienteDetailViewModel):
                    AfterDetailSaved(Ambienti, obj);
                    break;
            }            
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, 
            AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                lookupItem = new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator,
                    args.ViewModelName);
                items.Add(lookupItem);
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            };
        }

        public async Task LoadAsync()
        {
            var lookups = await _anagraficaLookupDataService.GetAnagraficaLookupAsync();

            Anagrafiche.Clear();

            foreach (var lookup in lookups)
            {
                Anagrafiche.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember,
                    _eventAggregator, nameof(AnagraficaDetailViewModel)));
            }

            lookups = await _documentoLookupDataService.GetDocumentoLookupAsync();

            Documenti.Clear();

            foreach (var lookup in lookups)
            {
                Documenti.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember,
                    _eventAggregator, nameof(DocumentoDetailViewModel)));
            }

            lookups = await _ambienteLookupDataService.GetAmbienteLookupAsync();

            Ambienti.Clear();

            foreach (var lookup in lookups)
            {
                Ambienti.Add(new NavigationItemViewModel(lookup.Id, lookup.DisplayMember,
                    _eventAggregator, nameof(AmbienteDetailViewModel)));
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(AnagraficaDetailViewModel):
                    AfterDetailDeleted(Anagrafiche, args);
                    break;

                case nameof(DocumentoDetailViewModel):
                    AfterDetailDeleted(Documenti, args);
                    break;

                case nameof(AmbienteDetailViewModel):
                    AfterDetailDeleted(Ambienti, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, 
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(a => a.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            };
        }

        public ObservableCollection<NavigationItemViewModel> Anagrafiche { get; set; }

        public ObservableCollection<NavigationItemViewModel> Documenti { get; set; }

        public ObservableCollection<NavigationItemViewModel> Ambienti { get; set; }



    }
}
