using System.ComponentModel.DataAnnotations;

namespace MovieETickets.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string Description { get; set; }
        [Required]
        public string CinemaLogo { get; set; }
        public string Address { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
