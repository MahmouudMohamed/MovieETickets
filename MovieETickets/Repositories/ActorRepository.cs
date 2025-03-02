using Movie_Point.Repository.IRepository;
using MovieETickets.Data;
using MovieETickets.Models;

namespace MovieETickets.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
