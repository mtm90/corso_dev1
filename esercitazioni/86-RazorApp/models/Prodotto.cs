using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class Prodotto
    {
        public int Id { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Il nome del prodotto è obbligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Il nome deve essere compreso fra i 3 e i 50 caratteri.")]
        public string Nome { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Il prezzo del prodotto è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere un valore positivo.")]
        public decimal Prezzo { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Il dettaglio del prodotto è obbligatorio.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Il dettaglio deve essre compreso fra i 10 e i 500 caratteri.")]
        public string Dettaglio { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "L'immagine del prodotto è obbligatoria.")]

        public string Immagine{ get; set; }
        [BindProperty]
        [Required(ErrorMessage = "La quantità del prodotto è obbligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantita deve essere almeno 1.")]
        public int Quantita {get; set;}
        [BindProperty]
        [Required(ErrorMessage = "La categoria del prodotto è obbligatoria.")]
        public string Categoria {get;set;}

    }

