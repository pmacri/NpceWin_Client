using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class ServiceDetailViewModel : ViewModelBase, IServiceDetailViewModel
    {
        public ServiceDetailViewModel(INpceDataService dataService)
        {
            _dataService = dataService;
        }

        private Service _service;
        private INpceDataService _dataService;

        public Service Service
        {
            get { return _service; }
            set
            {
                _service = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadServiceById(long id)
        {
            Service = await _dataService.GetServiceByIdAsync(id);
        }

    }
}
