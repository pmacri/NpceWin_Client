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

        INpceDataService _npceDataService;
        public ObservableCollection<Service> Services { get; set; }

        private Service _selectedService;
        public Service SelectedService
        {
            get { return _selectedService; }
            set { 
                if (value != _selectedService)
                {
                    _selectedService = value;
                    OnPropertyChanged();
                }

            }
        }
        public MainViewModel(INpceDataService npceDataService)
        {
            _npceDataService = npceDataService;
            Services = new ObservableCollection<Service>();
        }

        public void LoadAll()
        {
            var services = _npceDataService.GetAll();
            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(service);
            }
        }

    }
}
