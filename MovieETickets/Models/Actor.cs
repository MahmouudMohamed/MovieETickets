using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MovieETickets.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MinLength(8)]
        public string Bio { get; set; }
        [ValidateNever]
        public string ProfilePicture { get; set; }
        [Required]
        [MinLength(8)]
        public string News { get; set; }
        public List<Movie> movies { get; set; }
        public List<ActorMovie> actorsMovies { get; set; }
    }
}
