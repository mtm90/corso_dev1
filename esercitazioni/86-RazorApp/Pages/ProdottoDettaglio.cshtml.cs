
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ProdottoDettaglio : PageModel
{
    private readonly ILogger<ProdottoDettaglio> _logger;

    public ProdottoDettaglio(ILogger<ProdottoDettaglio> logger)
    {
        _logger = logger;
        _logger.LogInformation("Pagina prodotto dettaglio Caricata");
    }

    public Prodotto Prodotto { get; set; }

    public void OnGet(int id, string nome, decimal prezzo, string dettaglio, string immagine)
    {
        Prodotto = new Prodotto { Id = id, Nome = nome, Prezzo = prezzo, Dettaglio = dettaglio, Immagine = immagine };
    }
}
