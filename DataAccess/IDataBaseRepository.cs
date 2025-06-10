using Diagram.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Diagram.DataAccess
{
    public interface IDataBaseRepository
    {
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
        Task<GraphDataPointDTO> GetLastBatchNumberInGraphAsync(int IdGraph, CancellationToken token);
        Task<List<int>> GetAllGraphIdsAsync(CancellationToken token);
        Task<List<float>> GetValuesAsync(int idGraph, CancellationToken token);
        Task<List<int>> GetTimesAsync(int idGraph, CancellationToken token);
    }
}