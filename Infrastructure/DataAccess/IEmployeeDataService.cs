using Domain.Models;

namespace Infrastructure.DataAccess;

internal interface IEmployeeDataService
{
    Task<IEnumerable<EmployeeModel>> Select(CancellationToken cancellationToken);

    Task InsertBatch(IEnumerable<EmployeeModel> items, CancellationToken cancellationToken);
}