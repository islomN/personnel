using CsvHelper.Configuration;
using Domain.Models;
using Infrastructure.Converters;

namespace Infrastructure.Configurations;

internal sealed class EmployeeFileMap : ClassMap<EmployeeModel>
{
    public EmployeeFileMap()
    {
        Map(m => m.PayrollNumber)
            .Name("Personnel_Records.Payroll_Number");
        Map(m => m.FirstName)
            .Name("Personnel_Records.Forenames");
        Map(m => m.LastName)
            .Name("Personnel_Records.Surname");
        Map(m => m.BirthDay)
            .Name("Personnel_Records.Date_of_Birth")
            .TypeConverter<DateConverter>();
        Map(m => m.Telephone)
            .Name("Personnel_Records.Telephone");
        Map(m => m.Mobile)
            .Name("Personnel_Records.Mobile");
        Map(m => m.Address)
            .Name("Personnel_Records.Address");
        Map(m => m.AddressTwo)
            .Name("Personnel_Records.Address_2");
        Map(m => m.PostCode)
            .Name("Personnel_Records.Postcode");
        Map(m => m.Email)
            .Name("Personnel_Records.EMail_Home");
        Map(m => m.StartDate)
            .Name("Personnel_Records.Start_Date")
            .TypeConverter<DateConverter>();
    }
}