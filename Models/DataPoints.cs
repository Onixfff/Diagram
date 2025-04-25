using System;

namespace Diagram.Models.Data
{
    internal class DataPoints
    {
        public readonly int BatchNumber;
        public readonly DateTime NowTime;
        public readonly float Value;
        public readonly int Time;

        private DataPoints(int batchNumber, DateTime nowTime, float value, int time)
        {
            BatchNumber = batchNumber;
            NowTime = nowTime;
            Value = value;
            Time = time;
        }

        public static DataPoints Create(int batchNumber, DateTime nowTime, float value, int time)
        {
            if (batchNumber < 0)
            {
                throw new ArgumentOutOfRangeException("Номер партии вышел за допустимый диапозон");
            }

            return new DataPoints(batchNumber, nowTime, value, time);
        }
    }
}
