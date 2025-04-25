using Diagram.DTO;
using MySql.Data.MySqlClient;
using NLog;
using System;
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
            //TODO Добавить sql код
            string sql = "@" +
                "";
            GraphDataPointDTO graphDataPointDTO = null;

            try
            {
                using(var connection = new MySqlConnection(_connectionString))
                {
                    token.ThrowIfCancellationRequested();

                    await connection.OpenAsync(token);

                    using (var command = new MySqlCommand(sql, connection))
                    {
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

        //TODO Создать метод доставки всех данных из всех idGraph и их DataPoints
    }
}
