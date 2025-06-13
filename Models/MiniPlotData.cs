using System;
using System.Collections.Generic;

namespace Diagram.Models
{
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