using System.ComponentModel.DataAnnotations;

namespace App.Models.Auth
{
    public class User
    {

        public int id {  get; set; }

    
            [Required(ErrorMessage ="le nom est obligatoire")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "taille entre 2 et 50")]
                        
        public string? name { get; set; }




            public string? email { get; set; }

                public string? password { get; set; }

                
        public int? number { get; set; }

        public string? address { get; set;}

        public int?  age { get; set; }

        public string? description { get; set; }    


        public string? role { get; set; }


        public List<candidature>? Candidatures { get; set; }


        public List<Offre>? Offres { get; set; }


    }
}
