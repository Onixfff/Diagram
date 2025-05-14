using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WinForms;
using System;

namespace Diagram.Views.Utilities
{
    public class PlotStyler
    {
        private Crosshair _crosshair;

        public event EventHandler<Coordinates> MouseMove;

        /// <summary>
        /// Применяет общий стиль к графику (цвета, фрифты и тд...)
        /// </summary>
        /// <param name="plot"></param>
        public void ApplyBaseStyle(FormsPlot plot)
        {
            var plt = plot.Plot;

            plt.Add.Palette = new ScottPlot.Palettes.Aurora();

            plt.FigureBackground.Color = Color.FromHex("#181818");
            plt.DataBackground.Color = Color.FromHex("#1f1f1f");

            plt.Axes.Color(Color.FromHex("#d7d7d7"));
            plt.Grid.MajorLineColor = Color.FromHex("#404040");

            plt.Title("График");
            plt.YLabel("Value");
            plt.XLabel("Time");
        }
    }
}
