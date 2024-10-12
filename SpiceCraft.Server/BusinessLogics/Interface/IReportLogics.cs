using SpiceCraft.Server.DTO.Report;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IReportLogics
    {
        Task<ResultDetail<IEnumerable<object>>> GetReportByNameAsync(string reportName);
    }
}
