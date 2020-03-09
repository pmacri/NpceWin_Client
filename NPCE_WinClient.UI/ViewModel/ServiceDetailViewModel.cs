using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class ServiceDetailViewModel : ViewModelBase, IServiceDetailViewModel
    {
        public ServiceDetailViewModel(INpceRepository npceRepository, IEventAggregator eventAggregator)
        {
            _dataService = npceRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenDetailServiceViewEvent>()
                .Subscribe(OnOpenServiceDetailEvent);
        }

        private async void OnOpenServiceDetailEvent(long serviceId)
        {
            await LoadServiceById(serviceId);
        }

        private Service _service;
        private INpceRepository _dataService;
        private IEventAggregator _eventAggregator;

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
