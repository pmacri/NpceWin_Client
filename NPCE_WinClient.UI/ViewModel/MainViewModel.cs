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
    public class MainViewModel
    {

        INpceDataService _npceDataService;
        public ObservableCollection<Service> Services { get; set; }

        public MainViewModel(INpceDataService npceDataService)
        {
            _npceDataService = npceDataService;
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
