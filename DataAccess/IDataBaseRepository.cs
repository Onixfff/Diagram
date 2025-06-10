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
        Task<List<int>> GetAllGraphIdsAsync(CancellationToken token);

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
        Task<List<float>> GetValuesAsync(int idGraph, CancellationToken token);

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
        Task<List<int>> GetTimesAsync(int idGraph, CancellationToken token);
    }
}