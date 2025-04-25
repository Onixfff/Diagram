using Diagram.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Diagram.DataAccess
{
    public interface IDataBaseRepository
    {
        Task<GraphDataPointDTO> GetLastBatchNumberInGraphAsync(int IdGraph, CancellationToken token);
    }
}