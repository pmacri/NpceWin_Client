using Autofac;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Data.Lookups;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<NpceDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MessageDialogService>().As< IMessageDialogService>();
            builder.RegisterType<FileService>().As<IFileService>();

            builder.RegisterType<MainViewModel>().AsSelf();


            // LookupDataService sarà usato per ogni Interfaccia che esso implementa
            // Sarà usata per il lookup di altri tipi con altre interfacce
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            builder.RegisterType<AnagraficaRepository>().As<IAnagraficaRepository>();
            builder.RegisterType<DocumentoRepository>().As<IDocumentoRepository>();
            builder.RegisterType<AmbienteRepository>().As<IAmbienteRepository>();


            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<AnagraficaDetailViewModel>().As<IAnagraficaDetailViewModel>();
            builder.RegisterType<DocumentoDetailViewModel>().As<IDocumentoDetailViewModel>();
            builder.RegisterType<AmbienteDetailViewModel>().As<IAmbienteDetailViewModel>();

            return builder.Build();
        }
    }
}

