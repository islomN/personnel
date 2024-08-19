using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;
using Infrastructure.Configurations;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.DataAccess;

internal class FileDataService : IFileDataService
{
    public async Task<IEnumerable<EmployeeModel>> Import(IFormFile file, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        csv.Context.RegisterClassMap<EmployeeFileMap>();
        var items = new List<EmployeeModel>();
        
        try
        {
            await foreach (var record in csv.GetRecordsAsync<EmployeeModel>(cancellationToken))
            {
                items.Add(record);
            }
            
            return items;
        }
        catch (Exception ex)
        {
            throw new InternalServerJsonException("Error in parsing");
        }
    }
} 