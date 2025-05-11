using Autofac;
using Diagram.DataAccess;
using Diagram.Forms;
using Diagram.Interfaces;
using Diagram.Presenters;
using NLog;
using System.Configuration;

namespace Diagram.DependencyInjection
{
    public class ContainerConfig
    {
        public IContainer Configure()
        {
            var builder = new ContainerBuilder();

            string connectionString = Properties.Resources.connLocal;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException($"Строка подключения '{connectionString}' не найдена в конфигурации.");
            }

            //Logger
            builder.Register(c => LogManager.GetCurrentClassLogger())
                .As<ILogger>()
                .SingleInstance();

            //Repository
            builder.RegisterType<DataBaseRepository>()
                .As<IDataBaseRepository>()
                .WithParameter("connectionString", connectionString)
                .InstancePerLifetimeScope();

            // View
            builder.RegisterType<MainForm>()
                   .As<IMainForm>()
                   .PropertiesAutowired()
                   .InstancePerLifetimeScope();

            //presenter 
            builder.RegisterType<MainPresenter>()
                .AsSelf()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
