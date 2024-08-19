using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.DataAccess;

internal interface IFileDataService
{
    Task<IEnumerable<EmployeeModel>> Import(IFormFile file, CancellationToken cancellationToken);
} 