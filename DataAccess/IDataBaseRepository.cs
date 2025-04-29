using Diagram.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Diagram.DataAccess
{
    public interface IDataBaseRepository
    {
        Task<GraphDataPointDTO> GetLastBatchNumberInGraphAsync(int IdGraph, CancellationToken token);
        Task<List<int>> GetAllGraphIdsAsync(CancellationToken token);
        Task<List<float>> GetValuesAsync(int idGraph, CancellationToken token);
        Task<List<int>> GetTimesAsync(int idGraph, CancellationToken token);
    }
}