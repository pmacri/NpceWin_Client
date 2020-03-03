using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private IServiceLookupDataService _serviceLookupDataService;

        public NavigationViewModel(IServiceLookupDataService serviceLookupDataService)
        {
            _serviceLookupDataService = serviceLookupDataService;
            Services = new ObservableCollection<LookupItem>();
        }

        public async Task LoadServicesAsync()
        {
            var lookups = await _serviceLookupDataService.GetServiceLookupAsync();

            Services.Clear();

            foreach (var lookup in lookups)
            {
                Services.Add(lookup);
            }
        }

        public ObservableCollection<LookupItem> Services { get; set; }

    }
}
