using Domain.Models;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

internal class PersonnelService(
    IEmployeeDataService employeeDataService,
    IFileDataService fileDataService) : IPersonnelService
{
    public Task<IEnumerable<EmployeeModel>> Select(CancellationToken cancellationToken)
    {
        return employeeDataService.Select(cancellationToken);
    }

    public async Task<ImportInfoModel> Import(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null)
        {
            throw new BadRequestJsonException("File required");
        }
        
        if (file.ContentType is not ("text/csv" or "application/csv"))
        {
            throw new BadRequestJsonException("File invalid, only csv");
        }
        
        var employees = await fileDataService.Import(file, cancellationToken);

        await employeeDataService.InsertBatch(employees, cancellationToken);

        return new ImportInfoModel(employees.Count());
    }
}