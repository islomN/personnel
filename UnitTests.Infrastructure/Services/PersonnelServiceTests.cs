using Domain.Models;
using FakeItEasy;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace UnitTests.Infrastructure.Services;

public class PersonnelServiceTests
{
    private IEmployeeDataService _employeeDataService;
    private IFileDataService _fileDataService;
    private PersonnelService _personnelService;

    [SetUp]
    public void Setup()
    {
        _employeeDataService = A.Fake<IEmployeeDataService>();
        _fileDataService = A.Fake<IFileDataService>();
        _personnelService = new PersonnelService(_employeeDataService, _fileDataService);
    }

    [Test]
    public async Task Select_ShouldCallEmployeeDataServiceSelect()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var expectedEmployees = A.CollectionOfFake<EmployeeModel>(3);
        A.CallTo(() => _employeeDataService.Select(cancellationToken))
            .Returns(Task.FromResult<IEnumerable<EmployeeModel>>(expectedEmployees));

        // ACT
        var result = await _personnelService.Select(cancellationToken);

        // ASSERT
        Assert.That(result, Is.EqualTo(expectedEmployees));
        A.CallTo(() => _employeeDataService.Select(cancellationToken)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Import_ShouldThrowBadRequestJsonException_WhenFileIsNull()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();

        // ACT
        var ex = Assert.ThrowsAsync<BadRequestJsonException>(() => _personnelService.Import(null, cancellationToken));
        
        // ASSERT
        Assert.That(ex.Message, Is.EqualTo("File required"));
    }

    [Test]
    public void Import_ShouldThrowBadRequestJsonException_WhenFileIsNotCsv()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var file = A.Fake<IFormFile>();
        A.CallTo(() => file.ContentType).Returns("application/json");

        // ACT
        var ex = Assert.ThrowsAsync<BadRequestJsonException>(() => _personnelService.Import(file, cancellationToken));
        
        // ASSERT
        Assert.That(ex.Message, Is.EqualTo("File invalid, only csv"));
    }

    [Test]
    public async Task Import_ShouldCallFileDataServiceImportAndEmployeeDataServiceInsertBatch_WhenFileIsValidCsv()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var file = A.Fake<IFormFile>();
        var fakeEmployees = (IEnumerable<EmployeeModel>) A.CollectionOfFake<EmployeeModel>(3);
        A.CallTo(() => file.ContentType).Returns("text/csv");
        A.CallTo(() => _fileDataService.Import(file, cancellationToken)).Returns(Task.FromResult(fakeEmployees));

        // ACT
        var result = await _personnelService.Import(file, cancellationToken);

        // ASSERT
        Assert.That(result.Count, Is.EqualTo(fakeEmployees.Count()));
        A.CallTo(() => _fileDataService.Import(file, cancellationToken)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _employeeDataService.InsertBatch(fakeEmployees, cancellationToken)).MustHaveHappenedOnceExactly();
    }
}