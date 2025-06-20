﻿using Diagram.DTO;
using Diagram.ExceptionData;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZstdSharp;

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

        /// <summary>
        /// Асинхронно получает данные последней партии для указанного графика из базы данных.
        /// </summary>
        /// <param name="idGraph">Уникальный идентификатор графика, для которого запрашиваются данные.</param>
        /// <param name="token">Токен отмены, используемый для прерывания операции при необходимости.</param>
        /// <returns>Объект <see cref="GraphDataPointDTO"/>, содержащий информацию о последней партии.</returns>
        /// <exception cref="ArgumentNullException">
        /// Возникает, если не удалось получить данные по указанному графику (возвращено null).
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Возникает, если операция была прервана пользователем или системой через токен отмены.
        /// </exception>
        /// <exception cref="MySqlException">
        /// Возникает при ошибках взаимодействия с MySQL-базой данных (например, сетевые проблемы, синтаксические ошибки в запросе).
        /// </exception>
        /// <exception cref="Exception">
        /// Возникает при любых других непредвиденных ошибках во время выполнения операции.
        /// </exception>
        /// <remarks>
        /// Метод выполняет SQL-запрос к таблицам <c>diagramrooms.datapoints</c> и <c>diagramrooms.graph</c>,
        /// выбирая последнюю запись по максимальному номеру партии (<see cref="BatchNumber"/>).
        /// Если данные не найдены, выбрасывается исключение с типом <see cref="ArgumentNullException"/>.
        /// Все ошибки логируются через <see cref="_logger"/>.
        /// </remarks>
        public async Task<GraphDataPointDTO> GetLastBatchNumberInGraphAsync(int idGraph, CancellationToken token)
        {
            string sql = "@" +
                         "SELECT" +
                         "d.PointDateTime," +
                         "d.Value," +
                         "d.Time," +
                         "d.BatchNumber" +
                         "FROM diagramrooms.datapoints d" +
                         "INNER JOIN diagramrooms.graph g ON d.IdGraph = g.IdGraph" +
                         "WHERE g.IdGraph = @IdGraph" +
                         "ORDER BY d.BatchNumber DESC" +
                         "LIMIT 1";

            try
            {
                using(var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        GraphDataPointDTO graphDataPointDTO = null;

                        command.Parameters.AddWithValue("@IdGraph", idGraph);
                        
                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
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
                                throw new ExceptionRepository(new ArgumentNullException(),nameof(graphDataPointDTO));
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
                string error = $"Ошибка MySQL для IdGraph={idGraph}. Код ошибки: {ex.Number}.\nМесто ошибки - {ex.InnerException}";
                _logger.Error(ex, error);
                throw new ExceptionRepository(ex, error);
            }
            catch (Exception ex)
            {
                string error = $"Непредвиденная ошибка IdGraph={idGraph}.\nМесто ошибки - {ex.InnerException}";
                _logger.Error(ex, error);
                throw;
            }
        }

        /// <summary>
        /// Асинхронно получает список всех идентификаторов графиков (IdGraph) из таблицы Graph.
        /// </summary>
        /// <param name="token">Токен отмены, используемый для прерывания операции при необходимости.</param>
        /// <returns>Список целочисленных идентификаторов графиков.</returns>
        /// <exception cref="OperationCanceledException">
        /// Возникает, если операция была прервана через токен отмены.
        /// </exception>
        /// <exception cref="MySqlException">
        /// Возникает при ошибках взаимодействия с MySQL-базой данных.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Возникает, если значение IdGraph в базе данных равно NULL.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Возникает, если значение IdGraph невозможно преобразовать к типу int.
        /// </exception>
        /// <exception cref="Exception">
        /// Возникает при любых других непредвиденных ошибках.
        /// </exception>
        /// <remarks>
        /// Метод выполняет SQL-запрос: <c>SELECT IdGraph FROM Graph ORDER BY IdGraph;</c>
        /// Все ошибки логируются через <see cref="_logger"/>.
        /// </remarks>
        public async Task<List<int>> GetAllGraphIdsAsync(CancellationToken token)
        {
            string sql = "SELECT IdGraph FROM Graph ORDER BY IdGraph;";

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
                                var result = reader["IdGraph"];

                                if (result == DBNull.Value || result == null)
                                {
                                    _logger.Warn($"Поле {nameof(result)} == null");
                                    throw new ExceptionRepository(new InvalidOperationException(),$"Поле {nameof(result)} == null");
                                }

                                if (!(result is int idGraph))
                                {
                                    _logger.Error($"Неверное преобразование {nameof(idGraph)} из БД. Значение: {result}");
                                    throw new ExceptionRepository(new InvalidCastException(), $"Неверный тип данных для {nameof(idGraph)}");
                                }

                                graphIds.Add(idGraph);
                            }

                            if(graphIds == null)
                            {
                                _logger.Error($"Список {nameof(graphIds)} полученный из бд == null");
                                throw new ExceptionRepository(new NullReferenceException(), $"Список {nameof(graphIds)} полученный из бд == null");
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
                string error = "Ошибка с базы данных";
                _logger.Error(ex, error);
                throw new ExceptionRepository(ex, error);
            }
            catch (Exception ex)
            {
                string error = "Непредвиденная ошибка";
                _logger.Error(ex, error);
                throw;
            }
        }

        /// <summary>
        /// Асинхронно получает список значений (Value) для указанного графика с последней партией (BatchNumber) из базы данных.
        /// </summary>
        /// <param name="idGraph">Уникальный идентификатор графика.</param>
        /// <param name="token">Токен отмены, используемый для прерывания операции при необходимости.</param>
        /// <returns>Список числовых значений типа float.</returns>
        /// <exception cref="InvalidOperationException">
        /// Возникает, если значение поля Value равно NULL в базе данных.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Возникает, если значение поля Value невозможно преобразовать к типу float.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Возникает, если операция была прервана через токен отмены.
        /// </exception>
        /// <exception cref="MySqlException">
        /// Возникает при ошибках взаимодействия с MySQL-базой данных.
        /// </exception>
        /// <exception cref="Exception">
        /// Возникает при любых других непредвиденных ошибках.
        /// </exception>
        /// <remarks>
        /// Метод выполняет SQL-запрос, выбирающий все значения поля Value для последней партии указанного графика:
        /// <code>
        /// SELECT Value FROM datapoints 
        /// WHERE idGraph = @IdGraph 
        /// AND BatchNumber = (SELECT MAX(BatchNumber) FROM datapoints WHERE idGraph = @IdGraph)
        /// </code>
        /// Все ошибки логируются через <see cref="_logger"/>.
        /// </remarks>
        public async Task<List<float>> GetValuesAsync(int idGraph, CancellationToken token)
        {
            string sql = "SELECT Value FROM datapoints WHERE idGraph = @IdGraph AND BatchNumber = (SELECT MAX(BatchNumber) FROM datapoints WHERE idGraph = @IdGraph)";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdGraph", idGraph);

                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            token.ThrowIfCancellationRequested();

                            List<float> valueDataPoints = new List<float>();

                            while (await reader.ReadAsync(token))
                            {
                                var result = reader["Value"];

                                if (result == DBNull.Value || result == null)
                                {
                                    _logger.Warn($"Поле {nameof(result)} == null");
                                    throw new ExceptionRepository(new InvalidOperationException(),
                                        $"Поле {nameof(result)} == null");
                                }

                                if (!(result is float value))
                                {
                                    string error = $"Неверный тип данных для {nameof(value)}";
                                    _logger.Error($"Неверное преобразование {nameof(value)} из БД. Значение: {result}");
                                    throw new ExceptionRepository(new InvalidCastException(), error);
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
                string error = "Ошибка со стороны базы данных";
                _logger.Error(ex, error);
                throw new ExceptionRepository(ex,error);
            }
            catch (Exception ex)
            {
                string error = "Непредвиденная ошибка";
                _logger.Error(ex, error);
                throw;
            }
        }

        /// <summary>
        /// Асинхронно получает список временных меток (Time) для указанного графика с последней партией (BatchNumber) из базы данных.
        /// </summary>
        /// <param name="idGraph">Уникальный идентификатор графика.</param>
        /// <param name="token">Токен отмены, используемый для прерывания операции при необходимости.</param>
        /// <returns>Список целочисленных временных значений.</returns>
        /// <exception cref="InvalidOperationException">
        /// Возникает, если значение поля Time равно NULL в базе данных.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Возникает, если значение поля Time невозможно преобразовать к типу int.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Возникает, если операция была прервана через токен отмены.
        /// </exception>
        /// <exception cref="MySqlException">
        /// Возникает при ошибках взаимодействия с MySQL-базой данных.
        /// </exception>
        /// <exception cref="Exception">
        /// Возникает при любых других непредвиденных ошибках.
        /// </exception>
        /// <remarks>
        /// Метод выполняет SQL-запрос, выбирающий все значения поля Time для последней партии указанного графика:
        /// <code>
        /// SELECT Time FROM datapoints 
        /// WHERE idGraph = @IdGraph 
        /// AND BatchNumber = (SELECT MAX(BatchNumber) FROM datapoints WHERE idGraph = @IdGraph)
        /// </code>
        /// Все ошибки логируются через <see cref="_logger"/>.
        /// </remarks>
        public async Task<List<int>> GetTimesAsync(int idGraph, CancellationToken token)
        {
            string sql = "SELECT Time FROM datapoints WHERE idGraph = @IdGraph AND BatchNumber = (SELECT MAX(BatchNumber) FROM datapoints WHERE idGraph = @IdGraph);";

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdGraph", idGraph);

                        using (var reader = await command.ExecuteReaderAsync(token))
                        {
                            token.ThrowIfCancellationRequested();

                            List<int> TimeDataPoints = new List<int>();

                            while (await reader.ReadAsync(token))
                            {
                                var result = reader["Time"];

                                if (result == DBNull.Value || result == null)
                                {
                                    string error = $"Поле {nameof(result)} == null";
                                    _logger.Warn(error);
                                    throw new ExceptionRepository(new InvalidOperationException(), error);
                                }

                                if (!(result is int time))
                                {
                                    _logger.Error($"Неверное преобразование {nameof(time)} из БД. Значение: {result}");
                                    throw new ExceptionRepository(new InvalidCastException(), $"Неверный тип данных для {nameof(time)}");
                                }

                                TimeDataPoints.Add(time);
                            }

                            if (TimeDataPoints == null)
                            {
                                string errro = $"Список {nameof(TimeDataPoints)} полученный из бд == null";
                                _logger.Error(errro);
                                throw new ExceptionRepository(new NullReferenceException(), errro);
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
                string errro = "Ошибка со стороны базы данных";
                _logger.Error(ex, errro);
                throw new ExceptionRepository(ex, errro);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Непредвиденная ошибка");
                throw;
            }
        }
    }
}
