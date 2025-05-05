using Diagram.DataAccess;
using Diagram.Interfaces;
using Diagram.Presenters;
using NLog;
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
        private readonly MainPresenter _presenter;

        //Размер диаграмы
        private readonly Size _sizeFormPlot = new Size(355, 247);

        //Данные для меток в FormPlot
        int markerSize = 10;
        System.Drawing.Color markerColor = System.Drawing.Color.Blue;
        MarkerShape markerShape = MarkerShape.HashTag;

        public MainForm(IDataBaseRepository dataBaseRepository, MainPresenter mainPresenter, ILogger logger)
        {
            _dataBaseRepository = dataBaseRepository;
            _logger = logger;
            _presenter = mainPresenter;

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FormLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void DisplayMiniPlots(IEnumerable<MiniPlotData> plots)
        {
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
            formsPlotMain.Refresh();
        }

        public void ShowProgressIndicator()
        {
            // Например, отобразить ProgressBar
            progressBar1.Visible = true;
        }

        public void HideProgressIndicator()
        {
            progressBar1.Visible = false;
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

            for (int i = 0; i < xValues.Count; i++)
            {
                plot.Plot.Add.Marker(xValues[i], yValues[i]);
            }
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

    }
}
