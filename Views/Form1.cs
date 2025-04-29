using Diagram.DataAccess;
using NLog;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO Добавить паттерн MVP для этой формы и посмотреть как он тут будет выглядеть
//TODO Переписать работу с левой частью по созданию мини графиков в универсальный UserControl
//TODO Организовать для левой части ленивую загрузку ну или посмотреть нужно ли это
//TODO Добавить progressbar и background worker ну или посмотреть нужно ли это
//TODO Сделать ответ на ошибки по работе с бд

namespace Diagram.Forms
{
    public partial class Form1 : Form
    {
        private readonly IDataBaseRepository _dataBaseRepository;
        private readonly ILogger _logger;
        CancellationTokenSource _cts = new CancellationTokenSource();

        //Список idGraph и formPlot
        private List<FormPlotTitle> _titles = new List<FormPlotTitle>();

        //Размер диаграмы
        private readonly Size _sizeFormPlot = new Size(355, 247);

        //Данные для меток в FormPlot
        int markerSize = 10;
        System.Drawing.Color markerColor = System.Drawing.Color.Blue;
        MarkerShape markerShape = MarkerShape.HashTag;
        
        public Form1(IDataBaseRepository dataBaseRepository, ILogger logger)
        {
            _dataBaseRepository = dataBaseRepository;
            _logger = logger;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeLeftFormPlot();
        }

        private async Task InitializeLeftFormPlot()
        {
            List<int> FormPortIds = await _dataBaseRepository.GetAllGraphIdsAsync(_cts.Token);

            for (int i = 0; i < FormPortIds.Count; i++)
            {
                CreatesLeftFormPlot(FormPortIds[i]);
            }

            SortPlotsById();
        }

        private async void CreatesLeftFormPlot(int idFormPort)
        {
            FormsPlot formsPlot = new FormsPlot()
            {
                Size = _sizeFormPlot,
                AutoScroll = true,
            };

            formsPlot.Plot.YLabel("Value");
            formsPlot.Plot.XLabel("Time");
            formsPlot.Plot.Title(idFormPort.ToString());

            await PopulatePlotDataAsync(idFormPort, formsPlot);

            _titles.Add(new FormPlotTitle(idFormPort, formsPlot));

            flowLayoutPanel1.Controls.Add(formsPlot);

        }

        private async Task PopulatePlotDataAsync(int idGraph, FormsPlot formsPlot)
        {
            //X координат
            List<float> xValues = await _dataBaseRepository.GetValuesAsync(idGraph, _cts.Token);
            //Y координат
            List<int> yTime = await _dataBaseRepository.GetTimesAsync(idGraph, _cts.Token);

            if (xValues.Count != yTime.Count)
            {
                string error = $"{nameof(xValues)} и {nameof(yTime)} не совпадают по кол-ву данных " +
                    $"{nameof(xValues)} - {xValues.Count} and {nameof(yTime)} - {yTime.Count}";
                _logger.Error(error);
                throw new ArgumentException(error);
            }

            var scatter = formsPlot.Plot.Add.Scatter(xValues, yTime);

            scatter.Color = new ScottPlot.Color(markerColor);
            scatter.MarkerSize = markerSize;
            scatter.MarkerShape = markerShape;

            AddMarkers(formsPlot, xValues, yTime);
        }

        private void AddMarkers(FormsPlot plot, List<float> xValues, List<int> yValues)
        {
            for (int i = 0; i < xValues.Count; i++)
            {
                plot.Plot.Add.Marker(xValues[i], yValues[i]);
            }
        }

        private void SortPlotsById()
        {
            var sortedScroll = _titles.OrderBy(x => x.Id).ToList();

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var control in sortedScroll)
            {
                flowLayoutPanel1.Controls.Add(control.FormsPlot);
                control.FormsPlot.Refresh();
            }

            flowLayoutPanel1.ResumeLayout();
        }
    }

    public class FormPlotTitle
    {
        public readonly int Id;
        public readonly FormsPlot FormsPlot;

        public FormPlotTitle(int id, FormsPlot formsPlot)
        {
            Id = id;
            FormsPlot = formsPlot;
        }
    }
}
