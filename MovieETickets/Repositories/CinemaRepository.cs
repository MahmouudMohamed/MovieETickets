using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories.IRepositories;

namespace MovieETickets.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
