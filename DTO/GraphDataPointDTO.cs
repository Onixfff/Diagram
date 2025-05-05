using System;

namespace Diagram.DTO
{
    public class GraphDataPointDTO
    {
        public readonly int IdGraph;
        public readonly DateTime PointDateTime;
        public readonly float Value;
        public readonly int Time;

        private GraphDataPointDTO(int idGraph, DateTime pointDateTime, float value, int time)
        {
            IdGraph = idGraph;
            PointDateTime = pointDateTime;
            Value = value;
            Time = time;
        }

        public static GraphDataPointDTO Create(int idGraph, DateTime pointDateTime, float value, int time)
        {
            //TODO Проверка

            return new GraphDataPointDTO(idGraph, pointDateTime, value, time);
        }
    }
}
