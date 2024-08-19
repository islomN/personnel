using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

[Table("employees")]
public record Employee(
    int Id,
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
    DateOnly StartDate);