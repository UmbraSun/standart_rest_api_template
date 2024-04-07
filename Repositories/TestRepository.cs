using DAL.ApplicationDbContext;
using DAL.Models;
using Repositories.Interfaces;

namespace Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppMsSqlDbContext _context;

        public TestRepository(AppMsSqlDbContext context) 
        { 
            _context = context;
        }

        public async Task<TestModel> Create(TestModel test)
        {
            var result = (await _context.TestModels.AddAsync(test)).Entity;

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
