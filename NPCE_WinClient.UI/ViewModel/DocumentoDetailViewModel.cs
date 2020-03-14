using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class DocumentoDetailViewModel : DetailViewModelBase, IDocumentoDetailViewModel
    {
        private IDocumentoRepository _documentoRepository;
        private IMessageDialogService _messageDialogService;
        private DocumentoWrapper _documento;


        public DocumentoDetailViewModel(IDocumentoRepository documentoRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService) : base(eventAggregator)
        {
            _documentoRepository = documentoRepository;
            _messageDialogService = messageDialogService;

        }

        public DocumentoWrapper Documento
        {
            get { return _documento; }
            set {
                _documento = value;
                OnPropertyChanged();
            }
        }


        public override async Task LoadAsync(int? id)
        {
            var documento = (id.HasValue)
                ? await _documentoRepository.GetByIdAsync(id.Value)
                : CreateNewDocumento();
            Documento = new DocumentoWrapper(documento);
            Documento.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _documentoRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Documento.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Documento.Id == 0)
            {
                Documento.Tag = "";
            }
        }

        private Documento CreateNewDocumento()
        {
            var documento = new Documento();
            _documentoRepository.Add(documento);
            return documento;
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return (_documento != null) && (!_documento.HasErrors) && (HasChanges);
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }
    }

}
