using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lufthansa.Data;
using Lufthansa.Logic;
using Lufthansa.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace Lufthansa.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDbContext<Brand>, AirplaneDbContext>();
            services.AddSingleton<IDbContext<Airplane>, AirplaneDbContext>();

            services.AddSingleton<IRepository<Airplane>, Repository<Airplane>>();
            services.AddSingleton<IRepository<Brand>, Repository<Brand>>();

            services.AddSingleton<IAirplaneLogic, AirplaneLogic>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());

        }
    }
}
