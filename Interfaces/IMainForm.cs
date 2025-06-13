using System;
using System.Collections.Generic;
using Diagram.Models;

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

        void ShowProgressIndicator(bool show);
        void ShowErrorMessage(string message);
        void UpdateProgress(int progressPercentage);
    }
}
