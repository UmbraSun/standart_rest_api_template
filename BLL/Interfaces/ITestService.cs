using DTOs;

namespace BLL.Interfaces
{
    public interface ITestService
    {
        Task<TestDto> CreateTestModel(TestDto dto);
    }
}
