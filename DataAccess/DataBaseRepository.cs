using Diagram.DTO;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Diagram.DataAccess
{
    public class DataBaseRepository : IDataBaseRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DataBaseRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<GraphDataPointDTO> GetLastBatchNumberInGraphAsync(int idGraph, CancellationToken token)
        {
            //TODO Добавить sql на GetLastBatchNumberInGraphAsync
            string sql = "@" +
                "";

            try
            {
                using(var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        GraphDataPointDTO graphDataPointDTO = null;

                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            command.Parameters.AddWithValue("@IdGraph", idGraph);

                            token.ThrowIfCancellationRequested();

                            while (await reader.ReadAsync(token))
                            {
                                var pointDateTime = reader.GetDateTime(reader.GetOrdinal("PointDateTime"));
                                var value = reader.GetFloat(reader.GetOrdinal("value"));
                                var time = reader.GetInt32(reader.GetOrdinal("time"));

                                graphDataPointDTO = GraphDataPointDTO.Create
                                    (
                                        idGraph,
                                        (DateTime)pointDateTime,
                                        (float)value,
                                        (int)time
                                    );

                                _logger.Info($"Запрос на получение данных с IdGraph={idGraph} прошёл успешно ");
                            }

                            if (graphDataPointDTO == null)
                            {
                                _logger.Error($"{nameof(graphDataPointDTO)} имеет на выходе null значение");
                                throw new ArgumentNullException(nameof(graphDataPointDTO));
                            }
                            else
                            {
                                return graphDataPointDTO;
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warn($"Операция была отменена IdGraph={idGraph}.\nМесто отмены - {ex.InnerException}");
                throw;
            }
            catch (MySqlException ex)
            {
                _logger.Error(ex, $"Ошибка MySQL для IdGraph={idGraph}. Код ошибки: {ex.Number}.\nМесто ошибки - {ex.InnerException}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Непредвиденная ошибка IdGraph={idGraph}.\nМесто ошибки - {ex.InnerException}");
                throw;
            }
        }

        public async Task<List<int>> GetAllGraphIdsAsync(CancellationToken token)
        {
            //TODO Добавить sql на GetAllGraphIdsAsync
            string sql = "@";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        token.ThrowIfCancellationRequested();

                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            List<int> graphIds = new List<int>();

                            token.ThrowIfCancellationRequested();

                            while (await reader.ReadAsync(token))
                            {
                                var result = reader["id"];

                                if (result == DBNull.Value || result == null)
                                {
                                    _logger.Warn($"Поле {nameof(result)} == null");
                                    throw new InvalidOperationException($"Поле {nameof(result)} == null");
                                }

                                if (!(result is int idGraph))
                                {
                                    _logger.Error($"Неверное преобразование {nameof(idGraph)} из БД. Значение: {result}");
                                    throw new InvalidCastException($"Неверный тип данных для {nameof(idGraph)}");
                                }

                                graphIds.Add(idGraph);
                            }

                            if(graphIds == null)
                            {
                                _logger.Error($"Список {nameof(graphIds)} полученный из бд == null");
                                throw new NullReferenceException($"Список {nameof(graphIds)} полученный из бд == null");
                            }

                            _logger.Info("Операция GetAllGraphIdsAsync выполнена");
                            return graphIds;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Warn("Операция была отменена");
                throw;
            }
            catch (MySqlException ex)
            {
                _logger.Error(ex, "Ошибка со стороны базы данных");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Непредвиденная ошибка");
                throw;
            }
        }

        public async Task<List<float>> GetValuesAsync(int idGraph, CancellationToken token)
        {
            //TODO Добавить sql на GetValuesAsunc
            string sql = "@";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            command.Parameters.AddWithValue("@IdGraph", idGraph);

                            token.ThrowIfCancellationRequested();

                            List<float> valueDataPoints = new List<float>();

                            while (await reader.ReadAsync(token))
                            {
                                var result = reader["Value"];

                                if (result == DBNull.Value || result == null)
                                {
                                    _logger.Warn($"Поле {nameof(result)} == null");
                                    throw new InvalidOperationException($"Поле {nameof(result)} == null");
                                }

                                if (!(result is float value))
                                {
                                    _logger.Error($"Неверное преобразование {nameof(value)} из БД. Значение: {result}");
                                    throw new InvalidCastException($"Неверный тип данных для {nameof(value)}");
                                }

                                valueDataPoints.Add(value);
                            }

                            if (valueDataPoints == null)
                            {
                                _logger.Error($"Список {nameof(valueDataPoints)} полученный из бд == null");
                                throw new NullReferenceException($"Список {nameof(valueDataPoints)} полученный из бд == null");
                            }

                            _logger.Info("Операция GetValuesAsunc выполнена");
                            return valueDataPoints;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Warn("Операция была отменена");
                throw;
            }
            catch (MySqlException ex)
            {
                _logger.Error(ex, "Ошибка со стороны базы данных");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Непредвиденная ошибка");
                throw;
            }

        }

        public async Task<List<int>> GetTimesAsync(int idGraph, CancellationToken token)
        {
            //TODO Добавить sql на GetTimesAsunc
            string sql = "@";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            command.Parameters.AddWithValue("@IdGraph", idGraph);

                            token.ThrowIfCancellationRequested();

                            List<int> TimeDataPoints = new List<int>();

                            while (await reader.ReadAsync(token))
                            {
                                var result = reader["Time"];

                                if (result == DBNull.Value || result == null)
                                {
                                    _logger.Warn($"Поле {nameof(result)} == null");
                                    throw new InvalidOperationException($"Поле {nameof(result)} == null");
                                }

                                if (!(result is int time))
                                {
                                    _logger.Error($"Неверное преобразование {nameof(time)} из БД. Значение: {result}");
                                    throw new InvalidCastException($"Неверный тип данных для {nameof(time)}");
                                }

                                TimeDataPoints.Add(time);
                            }

                            if (TimeDataPoints == null)
                            {
                                _logger.Error($"Список {nameof(TimeDataPoints)} полученный из бд == null");
                                throw new NullReferenceException($"Список {nameof(TimeDataPoints)} полученный из бд == null");
                            }

                            _logger.Info("Операция GetTimesAsunc выполнена");
                            return TimeDataPoints;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Warn("Операция была отменена");
                throw;
            }
            catch (MySqlException ex)
            {
                _logger.Error(ex, "Ошибка со стороны базы данных");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Непредвиденная ошибка");
                throw;
            }
        }

        //TODO Создать метод доставки всех данных из всех idGraph и их DataPoints
    }
}
