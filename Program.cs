using Autofac;
using Diagram.DependencyInjection;
using Diagram.Forms;
using Diagram.Presenters;
using System;
using System.Windows.Forms;

namespace Diagram
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var container = new ContainerConfig().Configure();

                using (var scope = container.BeginLifetimeScope())
                {
                    var presenter = scope.Resolve<MainPresenter>();
                    var view = presenter.View as MainForm;

                    Application.Run(view);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
