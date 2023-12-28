using App.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Offre
    {


        public int Id { get; set; }
        //public string?  Titre { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        public int? Salaire { get; set; }
        public string? Competance { get; set; }
        public string? Responsabilite { get; set; }
        //public string? location { get; set; }

        public string? Remuneration { get; set; }

        // Foreign key referencing the User model
        public int? UserId { get; set; }
        public User? User { get; set; }


        public List<candidature>? Candidatures { get; set; }




    }
}
