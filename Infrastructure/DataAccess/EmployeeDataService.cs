using Database;
using Database.Tables;
using Domain.Models;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

internal class EmployeeDataService(EntityContext context) : IEmployeeDataService
{
    public async Task<IEnumerable<EmployeeModel>> Select(CancellationToken cancellationToken)
    {
        return await context.Employees
            .OrderBy(i => i.Id)
            .AsNoTracking()
            .Select(i => ConvertToModel(i))
            .ToListAsync(cancellationToken);
    }

    public async Task InsertBatch(IEnumerable<EmployeeModel> items, CancellationToken cancellationToken)
    {
        var entities = items.Select(ConvertToEntity);
        context.Employees.AddRange(entities);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InternalServerJsonException("Error in saving, please try again");
        }
    }

    private static Employee ConvertToEntity(EmployeeModel employeeModel)
    {
        return new(
            default,
            employeeModel.PayrollNumber,
            employeeModel.FirstName,
            employeeModel.LastName,
            employeeModel.BirthDay,
            employeeModel.Telephone,
            employeeModel.Mobile,
            employeeModel.Address,
            employeeModel.AddressTwo,
            employeeModel.PostCode,
            employeeModel.Email,
            employeeModel.StartDate);
    }
    
    private static EmployeeModel ConvertToModel(Employee employee)
    {
        return new(
            employee.PayrollNumber,
            employee.FirstName,
            employee.LastName,
            employee.BirthDay,
            employee.Telephone,
            employee.Mobile,
            employee.Address,
            employee.AddressTwo,
            employee.PostCode,
            employee.Email,
            employee.StartDate);
    }
}