using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface IPersonnelService
{
    Task<IEnumerable<EmployeeModel>> Select(CancellationToken cancellationToken);

    Task<ImportInfoModel> Import(IFormFile file, CancellationToken cancellationToken);
}