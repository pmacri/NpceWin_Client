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
    public class MainViewModel: ViewModelBase
    {

        public ObservableCollection<Service> Services { get; set; }
        public INavigationViewModel NavigationViewModel { get;}
        public IServiceDetailViewModel ServiceDetailViewModel { get; }
        public IMittenteDetailViewModel MittenteDetailViewModel { get; private set; }

        public MainViewModel(INavigationViewModel navigationViewModel, 
                            IServiceDetailViewModel serviceDetailViewModel,
                            IMittenteDetailViewModel mittenteDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            ServiceDetailViewModel = serviceDetailViewModel;
            MittenteDetailViewModel = mittenteDetailViewModel;
        }

        public async Task LoadAllAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

    }
}
