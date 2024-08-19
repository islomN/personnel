using System.Text;
using Domain.Models;
using FakeItEasy;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace UnitTests.Infrastructure.DataAccess;

[TestFixture]
public class FileDataServiceTests
{
    private FileDataService _fileDataService;

    [SetUp]
    public void Setup()
    {
        _fileDataService = new FileDataService();
    }

    [Test]
    public async Task Import_ShouldReturnListOfEmployeeModels_WhenCsvIsValid()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var csvContent = new StringBuilder();
        csvContent.AppendLine(
            "Personnel_Records.Payroll_Number," +
            "Personnel_Records.Forenames," +
            "Personnel_Records.Surname," +
            "Personnel_Records.Date_of_Birth," +
            "Personnel_Records.Telephone," +
            "Personnel_Records.Mobile," +
            "Personnel_Records.Address," +
            "Personnel_Records.Address_2," +
            "Personnel_Records.Postcode," +
            "Personnel_Records.EMail_Home," +
            "Personnel_Records.Start_Date");
        
        csvContent.AppendLine("COOP08,John ,William,26/01/1955,12345678,987654231," +
                              "12 Foreman road,London,GU12 6JW,nomadic20@hotmail.co.uk," +
                              "18/04/2013");

        var file = CreateFakeFormFile("employees.csv", csvContent.ToString(), "text/csv");

        // ACT
        var result = await _fileDataService.Import(file, cancellationToken);

        // ASSERT
        Assert.IsInstanceOf<IEnumerable<EmployeeModel>>(result);
        var resultList = new List<EmployeeModel>(result);
        Assert.That(resultList, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(resultList[0].PayrollNumber, Is.EqualTo("COOP08"));
            Assert.That(resultList[0].FirstName, Is.EqualTo("John "));
            Assert.That(resultList[0].LastName, Is.EqualTo("William"));
            Assert.That(resultList[0].BirthDay.Year, Is.EqualTo(1955));
            Assert.That(resultList[0].BirthDay.Month, Is.EqualTo(1));
            Assert.That(resultList[0].BirthDay.Day, Is.EqualTo(26));
            Assert.That(resultList[0].Telephone, Is.EqualTo("12345678"));
            Assert.That(resultList[0].Mobile, Is.EqualTo("987654231"));
            Assert.That(resultList[0].Address, Is.EqualTo("12 Foreman road"));
            Assert.That(resultList[0].AddressTwo, Is.EqualTo("London"));
            Assert.That(resultList[0].PostCode, Is.EqualTo("GU12 6JW"));
            Assert.That(resultList[0].Email, Is.EqualTo("nomadic20@hotmail.co.uk"));
            Assert.That(resultList[0].StartDate.Year, Is.EqualTo(2013));
            Assert.That(resultList[0].StartDate.Month, Is.EqualTo(4));
            Assert.That(resultList[0].StartDate.Day, Is.EqualTo(18));
        });
    }

    [Test]
    public void Import_ShouldThrowInternalServerJsonException_WhenCsvParsingFails()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var invalidCsvContent = new StringBuilder();
        invalidCsvContent.AppendLine("Id,Name,Position");
        invalidCsvContent.AppendLine("1,John Doe,Developer");
        invalidCsvContent.AppendLine("Invalid Line Without Proper CSV Formatting");

        var file = CreateFakeFormFile("employees.csv", invalidCsvContent.ToString(), "text/csv");

        // ACT
        var ex = Assert.ThrowsAsync<InternalServerJsonException>(() => _fileDataService.Import(file, cancellationToken));
        
        // ASSERT
        Assert.That(ex.Message, Is.EqualTo("Error in parsing"));
    }

    private static IFormFile CreateFakeFormFile(string fileName, string content, string contentType)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var file = A.Fake<IFormFile>();
        A.CallTo(() => file.OpenReadStream()).Returns(stream);
        A.CallTo(() => file.FileName).Returns(fileName);
        A.CallTo(() => file.ContentType).Returns(contentType);
        A.CallTo(() => file.Length).Returns(stream.Length);
        return file;
    }
}