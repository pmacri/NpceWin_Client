using Autofac;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.ViewModel;
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
            builder.RegisterType<NpceDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();


            // LookupDataService sarà usato per ogni Interfaccia che esso implementa
            // Sarà usata per il lookup di altri tipi con altre interfacce
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<NpceDataService>().As<INpceDataService>();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();

            return builder.Build();
        }
    }
}

