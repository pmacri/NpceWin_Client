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
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class DocumentoDetailViewModel : DetailViewModelBase, IDocumentoDetailViewModel
    {
        private IDocumentoRepository _documentoRepository;
        private IFileService _fileService;
        private DocumentoWrapper _documento;


        public DocumentoDetailViewModel(IDocumentoRepository documentoRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IFileService fileService) : base(eventAggregator, messageDialogService)
        {
            _documentoRepository = documentoRepository;
            _fileService = fileService;
            SelectDocumentCommand = new DelegateCommand(OnSelectDocumentExecute);

        }

        private void OnSelectDocumentExecute()
        {
            var selectFileResult = _fileService.SelectFile();
            Documento.Content = selectFileResult.Content;
            Documento.FileName = selectFileResult.FileName;
            Documento.Size = selectFileResult.FileSize;
        }

        public DocumentoWrapper Documento
        {
            get { return _documento; }
            set {
                _documento = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectDocumentCommand { get; private set; }

        public override async Task LoadAsync(int? id)
        {
            var documento = (id.HasValue)
                ? await _documentoRepository.GetByIdAsync(id.Value)
                : CreateNewDocumento();
            Id = documento.Id;
            InitializaDocumento(documento);
        }

        private void InitializaDocumento(Documento documento)
        {
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
                if (e.PropertyName== nameof(Documento.Tag)) SetTitle();
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Documento.Id == 0)
            {
                Documento.Tag = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Documento.Tag}";

        }

        private Documento CreateNewDocumento()
        {
            var documento = new Documento();
            _documentoRepository.Add(documento);
            return documento;
        }

        protected override async void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog($"Do you really want to cancel the document {Documento.Tag} {Documento.FileName}",
                                                                   "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _documentoRepository.Remove(Documento.Model);
            await _documentoRepository.SaveAsync();
            RaiseDetailDeletedEvent(Documento.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return (_documento != null) && (!_documento.HasErrors) && (HasChanges);
        }

        protected override async  void OnSaveExecute()
        {
            await _documentoRepository.SaveAsync();
            Id = Documento.Id;
            HasChanges = _documentoRepository.HasChanges();
            RaiseDetailSavedEvent(Documento.Id, $"{Documento.Tag} {Documento.FileName}");
        }
    }

}
