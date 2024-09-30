using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _86_RazorApp.Pages;

public class ProdottiModel : PageModel
{
    private readonly ILogger<ProdottiModel> _logger;

    public ProdottiModel(ILogger<ProdottiModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("Prodotti caricati");
    }

    public IEnumerable<Prodotto> Prodotti { get; set; } 

    public void OnGet()
    {
        Prodotti = new List<Prodotto>
        {
            new Prodotto { Id = 1 , Nome = "github", Prezzo = 100, Dettaglio = "Robe", Immagine = "/img/github.png"},
            new Prodotto { Id = 2, Nome = "instagram", Prezzo = 200, Dettaglio = "Cose", Immagine = "/img/instagram.png"},
            new Prodotto { Id = 3, Nome = "linkedin", Prezzo = 300, Dettaglio = "Giri",Immagine = "/img/linkedin.png"},
        };
    }
}