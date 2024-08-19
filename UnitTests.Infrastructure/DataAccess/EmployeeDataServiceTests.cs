using Database;
using Database.Tables;
using Domain.Models;
using FakeItEasy;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Infrastructure.DataAccess;

[TestFixture]
public class EmployeeDataServiceTests
{
    private EntityContext _fakeContext;
    private DbSet<Employee> _fakeDbSet;
    private EmployeeDataService _employeeDataService;

    [SetUp]
    public void Setup()
    {
        _fakeContext = A.Fake<EntityContext>();
        _fakeDbSet = A.Fake<DbSet<Employee>>(options => options.Implements<IQueryable<Employee>>().Implements<IEnumerable<Employee>>());

        A.CallTo(() => _fakeContext.Employees).Returns(_fakeDbSet);

        _employeeDataService = new EmployeeDataService(_fakeContext);
    }

    [Test]
    public async Task InsertBatch_ShouldAddEmployeesAndSaveChanges()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var employeeModels = new List<EmployeeModel>
        {
            new EmployeeModel(
                "123",
                "John",
                "Doe",
                DateOnly.FromDateTime(DateTime.Parse("1985-01-01")),
                "555-1234",
                "555-5678",
                "123 Main St",
                "Apt 4",
                "12345",
                "john.doe@example.com",
                DateOnly.FromDateTime(DateTime.Now))
        };

        // ACT
        await _employeeDataService.InsertBatch(employeeModels, cancellationToken);

        // ASSERT
        A.CallTo(() => _fakeDbSet.AddRange(A<IEnumerable<Employee>>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeContext.SaveChangesAsync(cancellationToken)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InsertBatch_ShouldThrowInternalServerJsonException_WhenSaveFails()
    {
        // ARRANGE
        var cancellationToken = new CancellationToken();
        var employeeModels = new List<EmployeeModel>
        {
            new EmployeeModel(
                "123",
                "John",
                "Doe",
                DateOnly.FromDateTime(DateTime.Parse("1985-01-01")),
                "555-1234",
                "555-5678",
                "123 Main St",
                "Apt 4",
                "12345",
                "john.doe@example.com",
                DateOnly.FromDateTime(DateTime.Now))
        };

        A.CallTo(() => _fakeContext.SaveChangesAsync(cancellationToken))
            .ThrowsAsync(new Exception("Database error"));

        // ACT
        var ex = Assert.ThrowsAsync<InternalServerJsonException>(() => _employeeDataService.InsertBatch(employeeModels, cancellationToken));
        
        // ASSERT
        Assert.That(ex.Message, Is.EqualTo("Error in saving, please try again"));
    }
    
    [TearDown]
    public void Dispose()
    {
        _fakeContext.Dispose();
    }
}