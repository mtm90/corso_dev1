using System.ComponentModel.DataAnnotations;

public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        [StringLength(50, MinimumLength =3, ErrorMessage ="azz fai")]
        public string Dettaglio { get; set; }
        public string Immagine{ get; set; }
        public int Quantita {get; set;}
        public string Categoria {get;set;}
        public List<string> Categorie {get;set;}
    }

