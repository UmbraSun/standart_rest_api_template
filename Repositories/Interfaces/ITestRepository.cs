using DAL.Models;

namespace Repositories.Interfaces
{
    public interface ITestRepository
    {
        Task<TestModel> Create(TestModel test);
    }
}
