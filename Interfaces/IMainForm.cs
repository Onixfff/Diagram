using System;
using System.Collections.Generic;

namespace Diagram.Interfaces
{
    public interface IMainForm : IDisposable
    {
        // Событие при загрузке формы
        event EventHandler FormLoaded;
        // Событие при выборе графика (например, двойной клик)
        event EventHandler<int> PlotSelected;
        //Событие отмены операции
        event EventHandler CancelRequested;

        // Методы для обновления UI
        void DisplayMiniPlots(IEnumerable<MiniPlotData> plots);
        void UpdateMainPlot(int plotId, List<float> xValues, List<int> yTimes);

        void ShowProgressIndicator();
        void HideProgressIndicator();
        void ShowErrorMessage(string message);
        void UpdateProgress(int progressPercentage);
    }

    public class MiniPlotData
    {
        public int Id;
        public List<float> XValue { get; }
        public List<int> YTime { get; }

        public MiniPlotData(int id, List<float> xValue, List<int> yTime)
        {
            Id = id;
            XValue = xValue ?? throw new ArgumentNullException(nameof(xValue));
            YTime = yTime ?? throw new ArgumentNullException(nameof(yTime));
        }
    }
}
