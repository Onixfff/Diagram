using Diagram.DataAccess;
using Diagram.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Diagram.Presenters
{
    public class MainPresenter : IDisposable
    {
        private readonly IMainForm _view;
        private readonly IDataBaseRepository _repository;
        private readonly ILogger _logger;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private System.Timers.Timer _autoRefreshTimer;

        private int _startIdInitializeMainPlot = 0;
        
        public MainPresenter(IMainForm view, IDataBaseRepository repository, ILogger logger)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            //Подписка на события View
            _view.FormLoaded += OnFormLoaded;
            _view.PlotSelected += OnPlotSelected;
            _view.CancelRequested += OnCancelRequested;
        }

        private void OnFormLoaded(object sender, EventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                InitializePlotsAutoRefresh(_cts.Token);
            }
            catch(OperationCanceledException)
            {
                var error = "Операция отменена";
                _logger.Warn(error);
                _view.ShowErrorMessage(error);
            }
        }

        private async void OnPlotSelected(object sender, int plotId)
        {
            try
            {
                _cts = new CancellationTokenSource();
                await UpdateMainPlotAsync(plotId, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                var error = "Операция отменена";
                _logger.Warn(error);
                _view.ShowErrorMessage(error);
            }

        }

        private async Task LoadMiniPlotsAsync(CancellationToken token)
        {
            try
            {
                _view.ShowProgressIndicator();
                IProgress<int> progress = new Progress<int>(percent =>
                {
                    _view.UpdateProgress(percent);
                });

                var graphIds = await _repository.GetAllGraphIdsAsync(token).ConfigureAwait(false);
                
                var miniPlotData = new List<MiniPlotData>();
                int total = graphIds.Count + 1;
                int current = 0;

                foreach (var id in graphIds)
                {
                    var xValues = await _repository.GetValuesAsync(id, token);
                    var yTimes = await _repository.GetTimesAsync(id, token);
                    miniPlotData.Add(new MiniPlotData(id, xValues, yTimes));

                    //Обновление progressBar
                    current++;
                    int percentComplete = (int)((current / (double)total) * 100);
                    progress.Report(percentComplete);
                }

                miniPlotData = SortMiniPlots(miniPlotData);
                _view.DisplayMiniPlots(miniPlotData);
            }
            catch (OperationCanceledException)
            {
                var error = "Операция отменена";
                _logger.Warn(error);
                _view.ShowErrorMessage(error);
            }
            catch (Exception ex)
            {
                var error = $"Ошибка загрузки данных: {ex.Message}";
                _logger.Error(error);
                _view.ShowErrorMessage(error);
            }
        }

        private List<MiniPlotData> SortMiniPlots(List<MiniPlotData> miniPlotDatas)
        {
            return miniPlotDatas.OrderBy(x => x.Id).ToList();
        }

        private void InitializePlotsAutoRefresh(CancellationToken token)
        {
            try
            {
                _autoRefreshTimer = new System.Timers.Timer(5000);

                _autoRefreshTimer.Elapsed += async (sender, arhs) =>
                {
                    await LoadMiniPlotsAsync(_cts.Token);
                    await UpdateMainPlotAsync(_startIdInitializeMainPlot, _cts.Token);
                };

                _autoRefreshTimer.AutoReset = true;
                _autoRefreshTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                var error = $"Ошибка инициализации графика: {ex.Message}";
                _logger.Error(error);
                _view.ShowErrorMessage(error);
            }
            finally
            {
                _view.HideProgressIndicator();
            }
        }

        private async Task UpdateMainPlotAsync(int plotId, CancellationToken token)
        {
            try
            {
                _startIdInitializeMainPlot = plotId;

                var xValues = await _repository.GetValuesAsync(plotId, token);
                var yTimes = await _repository.GetTimesAsync(plotId, token);
                _view.UpdateMainPlot(plotId, xValues, yTimes);
            }
            catch (Exception ex)
            {
                _view.ShowErrorMessage($"Ошибка обновления графика: {ex.Message}");
            }
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            _cts.Cancel(); // Отменяет текущую операцию через CancellationToken
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _autoRefreshTimer?.Stop();
            _autoRefreshTimer?.Dispose();

            _view.FormLoaded -= OnFormLoaded;
            _view.PlotSelected -= OnPlotSelected;
            _view.CancelRequested -= OnCancelRequested;
        }
    }
}
