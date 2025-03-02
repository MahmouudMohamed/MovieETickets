using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories.IRepositories;

namespace MovieETickets.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
