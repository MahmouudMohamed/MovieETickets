using Movie_Point.Repository.IRepository;
using MovieETickets.Data;
using MovieETickets.Models;
using MovieETickets.Repositories;

namespace Movie_Point.Repository
{
    public class ActorMovieRepository : Repository<ActorMovie>, IActorMovieRepository

    {
        public ActorMovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
