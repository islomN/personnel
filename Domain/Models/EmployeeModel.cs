namespace Domain.Models;

public record EmployeeModel(
    string PayrollNumber,
    string FirstName,
    string LastName,
    DateOnly BirthDay,
    string Telephone,
    string Mobile,
    string Address,
    string AddressTwo,
    string PostCode,
    string Email,
    DateOnly StartDate)
{
    public EmployeeModel()
        : this(
            default!,
            default!,
            default!,
            default,
            default!,
            default!,
            default!,
            default!,
            default!,
            default!,
            default) { }
};