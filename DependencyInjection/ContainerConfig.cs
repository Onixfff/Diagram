using Autofac;
using Diagram.DataAccess;
using NLog;
using System.Configuration;

namespace Diagram.DependencyInjection
{
    public class ContainerConfig
    {
        public IContainer Configure()
        {
            var builder = new ContainerBuilder();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnLocal"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException($"Строка подключения '{connectionString}' не найдена в конфигурации.");
            }

            //Регистрация логера
            builder.Register(c => LogManager.GetCurrentClassLogger())
                .As<ILogger>();

            //Регистрация репозитория базы данных
            builder.RegisterType<DataBaseRepository>()
                .As<IDataBaseRepository>()
                .WithParameter("connectionString", connectionString);

            return builder.Build();
        }
    }
}
