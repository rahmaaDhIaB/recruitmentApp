
using global::App.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class candidature
    {
        [Key]
        public int? Id { get; set; }

        // Foreign key referencing the User model
        public int? UserId { get; set; }
        public User? User { get; set; }

        // Foreign key referencing the Offre model
        public int? OffreId { get; set; }
        public Offre? Offre { get; set; }

        // Status of the candidature (en cours, refuse, accepter)
        public string?   Statut { get; set; }
    }
}
