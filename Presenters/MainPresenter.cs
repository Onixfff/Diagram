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
        public readonly IMainForm View;

        private readonly IDataBaseRepository _repository;
        private readonly ILogger _logger;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private System.Windows.Forms.Timer _autoRefreshTimer;

        private int _startIdInitializeMainPlot = 1;

        private bool _disposed = false;

        public MainPresenter(IMainForm view,IDataBaseRepository repository, ILogger logger)
        {
            this.View = view ?? throw new ArgumentNullException(nameof(view));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            //Подписка на события View
            this.View.FormLoaded += OnFormLoaded;
            this.View.PlotSelected += OnPlotSelected;
            this.View.CancelRequested += OnCancelRequested;
        }

        private async void OnFormLoaded(object sender, EventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                await LoadMiniPlotsAsync(_cts.Token, true);
                await UpdateMainPlotAsync(_startIdInitializeMainPlot, _cts.Token);

                InitializePlotsAutoRefresh(_cts.Token);
            }
            catch(OperationCanceledException)
            {
                var error = "Операция отменена";
                _logger.Warn(error);
                View.ShowErrorMessage(error);
            }
            catch (Exception ex)
            {
                var error = "Неизвестная ошибка";
                _logger.Error(error + "\n" + ex.Message);
                _cts.Cancel();
                View.ShowErrorMessage(error);
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
                View.ShowErrorMessage(error);
            }

        }

        private async Task LoadMiniPlotsAsync(CancellationToken token, bool showProgress = false)
        {
            try
            {
                View.ShowProgressIndicator(showProgress);
                IProgress<int> progress = new Progress<int>(percent =>
                {
                    View.UpdateProgress(percent);
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
                View.DisplayMiniPlots(miniPlotData);
            }
            catch (OperationCanceledException)
            {
                var error = "Операция отменена";
                _logger.Warn(error);
                View.ShowErrorMessage(error);
                throw;
            }
            catch (Exception ex)
            {
                var error = $"Ошибка загрузки данных: {ex.Message}";
                _logger.Error(error);
                View.ShowErrorMessage(error);
                throw;
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
                _autoRefreshTimer = new System.Windows.Forms.Timer();
                _autoRefreshTimer.Interval = 10000;

                _autoRefreshTimer.Tick += async (sender, arhs) =>
                {
                    await LoadMiniPlotsAsync(_cts.Token);
                    await UpdateMainPlotAsync(_startIdInitializeMainPlot, _cts.Token);
                };
                _autoRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                var error = $"Ошибка инициализации графика: {ex.Message}";
                _logger.Error(error);
                View.ShowErrorMessage(error);
            }
            finally
            {
                View.ShowProgressIndicator(false);
            }
        }

        private async Task UpdateMainPlotAsync(int plotId, CancellationToken token)
        {
            try
            {
                _startIdInitializeMainPlot = plotId;

                var xValues = await _repository.GetValuesAsync(plotId, token);
                var yTimes = await _repository.GetTimesAsync(plotId, token);
                View.UpdateMainPlot(plotId, xValues, yTimes);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage($"Ошибка обновления графика: {ex.Message}");
            }
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            _cts.Cancel(); // Отменяет текущую операцию через CancellationToken
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _cts?.Cancel();
                _cts?.Dispose();

                _autoRefreshTimer?.Stop();
                _autoRefreshTimer?.Dispose();

                View.FormLoaded -= OnFormLoaded;
                View.PlotSelected -= OnPlotSelected;
                View.CancelRequested -= OnCancelRequested;
            }

            _disposed = true;
        }
    }
}
