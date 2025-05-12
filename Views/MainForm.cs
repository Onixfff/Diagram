using Diagram.DataAccess;
using Diagram.Interfaces;
using NLog;
using ScottPlot.Plottables;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

//TODO Решить проблему с Хранением данных в Presenter (там не должны храниться данные) так-как они могут не совпадать с ui или бд. Лучше хранить данные уж в ui
//TODO Переписать работу с левой частью по созданию мини графиков в универсальный UserControl
//TODO Организовать для левой части ленивую загрузку ну или посмотреть нужно ли это
//TODO Добавить progressbar и background worker ну или посмотреть нужно ли это

namespace Diagram.Forms
{
    public partial class MainForm : Form, IDisposable, IMainForm
    {
        public event EventHandler FormLoaded;
        public event EventHandler<int> PlotSelected;
        public event EventHandler CancelRequested;

        private readonly ILogger _logger;
        private readonly IDataBaseRepository _dataBaseRepository;

        //Размер мини диаграм
        private readonly Size _sizeFormPlot = new Size(355, 247);

        public MainForm(IDataBaseRepository dataBaseRepository, ILogger logger)
        {
            _dataBaseRepository = dataBaseRepository;
            _logger = logger;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FormLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void DisplayMiniPlots(IEnumerable<MiniPlotData> plots)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => DisplayMiniPlots(plots)));
                return;
            }

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var plotData in plots)
            {
                CreatesMiniPlot(plotData.Id, plotData.XValue, plotData.YTime);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        public void UpdateMainPlot(int plotId, List<float> xValues, List<int> yTimes)
        {
            formsPlotMain.Plot.Clear();
            formsPlotMain.Plot.Title(plotId.ToString());
            formsPlotMain.Plot.Add.Scatter(xValues.ToArray(), yTimes.ToArray());
            
            MouseTracker(formsPlotMain);
            
            formsPlotMain.Refresh();
        }

        public void ShowProgressIndicator(bool show)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => ShowProgressIndicator(show)));
                return;
            }
            // Например, отобразить ProgressBar
            progressBar1.Visible = show;    
        }

        private void LeftFormsPlot_DoubleClick(object sender, EventArgs e)
        {
            var formPlot = (FormsPlot)sender;
            int idFormPlot;

            bool isCompliteParse = int.TryParse(formPlot.Name, out idFormPlot);

            if (isCompliteParse == false)
            {
                string error = $"Неудалось преобразовать {nameof(formPlot.Name)} - {formPlot.Name} в int";
                _logger.Error(error);
                throw new FormatException(error);
            }

            PlotSelected?.Invoke(this, idFormPlot);
        }

        private void CreatesMiniPlot(int idFormPort, List<float> xValue, List<int> yTime)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() => CreatesMiniPlot(idFormPort, xValue, yTime)));
                return;
            }

            FormsPlot formsPlot = new FormsPlot()
            {
                Size = _sizeFormPlot,
                AutoScroll = true,
            };

            formsPlot.Name = idFormPort.ToString();
            formsPlot.Plot.YLabel("Value");
            formsPlot.Plot.XLabel("Time");
            formsPlot.Plot.Title(idFormPort.ToString());
            formsPlot.DoubleClick += LeftFormsPlot_DoubleClick;

            flowLayoutPanel1.Controls.Add(formsPlot);
            AddMarkers(formsPlot, xValue, yTime);
        }

        private void AddMarkers(FormsPlot plot, List<float> xValues, List<int> yValues)
        {
            if (xValues == null || yValues == null || xValues.Count != yValues.Count)
            {
                var error = "Некорректные данные для добавления маркеров";
                _logger.Error(error);
                throw new ArgumentException(error);
            }

            var scatter = plot.Plot.Add.Scatter(xValues, yValues);

            scatter.MarkerColor = new ScottPlot.Color(color: System.Drawing.Color.Red);
            scatter.MarkerSize = 11.5f;
            scatter.LineWidth = 3;
            scatter.LinePattern = LinePattern.DenselyDashed;
            scatter.MarkerShape = MarkerShape.FilledDiamond;
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<int>(UpdateProgress), progressPercentage);
                return;
            }

            progressBar1.Value = Math.Max(0, Math.Min(100, progressPercentage));
        }

        public void ShowErrorMessage(string message)
        {
            _logger.Warn($"вызвана ошибка {message}" );
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MouseTracker(FormsPlot plot)
        {
            Crosshair CH;

            CH = plot.Plot.Add.Crosshair(0, 0);
            CH.TextColor = Colors.White;
            CH.TextBackgroundColor = CH.HorizontalLine.Color;

            plot.Refresh();

            plot.MouseMove += (s, e) =>
            {
                Pixel mousePixel = new Pixel(e.X, e.Y);
                Coordinates mouseCoordinates = plot.Plot.GetCoordinates(mousePixel);
                this.Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
                CH.Position = mouseCoordinates;
                CH.VerticalLine.Text = $"{mouseCoordinates.X:N3}";
                CH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
                plot.Refresh();
            };

            plot.MouseDown += (s, e) =>
            {
                Pixel mousePixel = new Pixel(e.X, e.Y);
                Coordinates mouseCoordinates = plot.Plot.GetCoordinates(mousePixel);
                Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} (mouse down)";
            };
        }
    }
}
