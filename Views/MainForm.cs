using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Diagram.Interfaces;
using NLog;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WinForms;

namespace Diagram.Views
{
    public partial class MainForm : Form, IDisposable, IMainForm
    {
        public event EventHandler FormLoaded;
        public event EventHandler<int> PlotSelected;
        public event EventHandler CancelRequested;

        private readonly ILogger _logger;

        private Crosshair _cH;

        //Размер мини диаграм
        private readonly Size _sizeFormPlot = new Size(355, 247);

        public MainForm(ILogger logger)
        {
            _logger = logger;

            InitializeComponent();

            formsPlotMain.MouseMove += Plot_MouseMove;
            formsPlotMain.MouseDown += Plot_MouseDown;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FormLoaded?.Invoke(this, EventArgs.Empty);
            
            InitializeMouseTracker(formsPlotMain);
            ChangeViewBackground(formsPlotMain);

            formsPlotMain.MouseMove += Plot_MouseMove;
            formsPlotMain.MouseDown += Plot_MouseDown;
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
            formsPlotMain.Plot.YLabel("Value");
            formsPlotMain.Plot.XLabel("Time");
            formsPlotMain.Plot.Add.Scatter(yTimes.ToArray(), xValues.ToArray());

            InitializeMouseTracker(formsPlotMain);

            // Включаем автошкалирование
            formsPlotMain.Plot.Axes.AutoScale();
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
            formsPlotMain.Plot.Axes.AutoScale();
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
            
            ChangeViewBackground(formsPlot);

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

            var scatter = plot.Plot.Add.Scatter(yValues, xValues);

            scatter.MarkerColor = new ScottPlot.Color(color: System.Drawing.Color.Red);
            scatter.MarkerSize = 11.5f;
            scatter.LineWidth = 3;
            scatter.LinePattern = LinePattern.DenselyDashed;
            scatter.MarkerShape = MarkerShape.FilledDiamond;

            // Включаем автошкалирование
            plot.Plot.Axes.AutoScale();
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

        private void InitializeMouseTracker(FormsPlot plot)
        {
            _cH = plot.Plot.Add.Crosshair(0, 0);
            _cH.TextColor = Colors.White;
            _cH.TextBackgroundColor = _cH.HorizontalLine.Color;

            plot.Refresh();
        }

        private void Plot_MouseDown(object sender, MouseEventArgs e)
        {
            FormsPlot plot = sender as FormsPlot;

            Pixel mousePixel = new Pixel(e.X, e.Y);
            Coordinates mouseCoordinates = plot.Plot.GetCoordinates(mousePixel);
            Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3} (mouse down)";
        }

        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            FormsPlot plot = sender as FormsPlot;

            Pixel mousePixel = new Pixel(e.X, e.Y);
            Coordinates mouseCoordinates = plot.Plot.GetCoordinates(mousePixel);
            this.Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
            _cH.Position = mouseCoordinates;
            _cH.VerticalLine.Text = $"{mouseCoordinates.X:N3}";
            _cH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
            plot.Refresh();
        }

        private void ChangeViewBackground(FormsPlot plot)
        {
            plot.Plot.Add.Palette = new ScottPlot.Palettes.Penumbra();

            // change figure colors
            plot.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#181818");
            plot.Plot.DataBackground.Color = ScottPlot.Color.FromHex("#1f1f1f");

            // change axis and grid colors
            plot.Plot.Axes.Color(ScottPlot.Color.FromHex("#d7d7d7"));
            plot.Plot.Grid.MajorLineColor = ScottPlot.Color.FromHex("#404040");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            //Отписка от событий
            formsPlotMain.MouseMove -= Plot_MouseMove;
            formsPlotMain.MouseDown -= Plot_MouseDown;

            //Очиска графиков и освобождение ресурсов
            formsPlotMain.Plot.Clear();
            formsPlotMain.Dispose();

            foreach (FormsPlot item in flowLayoutPanel1.Controls)
            {
                item.Dispose();
            }

            base.OnFormClosed(e);
        }
    }
}
