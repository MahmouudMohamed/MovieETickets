using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieETickets.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieETickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        [Range(0, 100)]
        public double Price { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [ValidateNever]

        public MovieStatus MovieStatus { get; set; }
        [ValidateNever]

        public int CinemaId { get; set; }
        [ValidateNever]

        public int CategoryId { get; set; }
        [ValidateNever]

        public Cinema Cinema { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]

        public List<Actor> Actors { get; set; }
        [ValidateNever]

        public List<ActorMovie> actorsMovies { get; set; }
    }
}
