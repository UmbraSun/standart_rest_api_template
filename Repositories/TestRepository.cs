using DAL.ApplicationDbContext;
using DAL.Models;

namespace Repositories
{
    public class TestRepository : BaseRepository<TestModel, int>
    {
        public TestRepository(AppMsSqlDbContext context) : base(context)
        { }
    }
}
