using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories.IRepositories;

namespace MovieETickets.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
