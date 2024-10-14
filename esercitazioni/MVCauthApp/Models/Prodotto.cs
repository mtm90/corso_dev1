using System.ComponentModel.DataAnnotations;


public class Prodotto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore o uguale a zero.")]
    public decimal Prezzo { get; set; }

    [Required(ErrorMessage = "Il dettaglio è obbligatorio.")]
    public string Dettaglio { get; set; }
    [Required(ErrorMessage = "L'immagine è obbligatoria.")]
    public string Immagine { get; set; }

    [Required(ErrorMessage = "La quantità è obbligatoria.")]
    [Range(0, int.MaxValue, ErrorMessage = "La quantità deve essere maggiore o uguale a zero.")]
    public int Quantita { get; set; }

    [Required(ErrorMessage = "Seleziona una categoria.")]
    public string Categoria { get; set; }
}
