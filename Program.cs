using Autofac;
using Diagram.DependencyInjection;
using Diagram.Forms;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаём контейнер
            var container = new ContainerConfig().Configure();

            using(var score = container.BeginLifetimeScope())
            {
                var mainForm = score.Resolve<MainForm>();
                Application.Run(mainForm);
            }
        }
    }
}
