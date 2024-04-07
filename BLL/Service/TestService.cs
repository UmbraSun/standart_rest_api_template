using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using DTOs;
using Repositories.Interfaces;

namespace BLL.Service
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repository;
        private readonly IMapper _mapper;

        public TestService(ITestRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TestDto> CreateTestModel(TestDto dto)
        {
            return _mapper.Map<TestDto>(await _repository.Create(_mapper.Map<TestModel>(dto)));
        }
    }
}
