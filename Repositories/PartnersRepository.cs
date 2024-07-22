using DAL.ApplicationDbContext;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class PartnersRepository : BaseRepository<Partners, int>
    {
        public PartnersRepository(AppMsSqlDbContext context) : base(context)
        {
        }

        public async Task<List<Partners>> GetByNameWith(string name)
        {
            return _dbSet.Include(partner => partner.Tournaments).Where(partner => partner.Name == name).ToList();
        }

        public async Task<IEnumerable<Partners>> GetPartners()
        {
            var partners = await GetFilteredListAsync(_dbSet
                .Where(parnter => parnter.Tournaments.Count > 10)
                .OrderByDescending(parnter => parnter.Tournaments.Count));

            return partners;
        }

        public async Task<IEnumerable<Partners>> GetAllDeletedPartners()
        {
            var partners = await GetUnFilteredListAsync(_dbSet
                .Where(partner => partner.DeleteDate != null));

            return partners;
        }
    }
}
