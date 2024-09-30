using Microsoft.AspNetCore.Mvc.RazorPages;


public class ProdottiModel : PageModel
{
    private readonly ILogger<ProdottiModel> _logger;

    public ProdottiModel(ILogger<ProdottiModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("Prodotti caricati");
    }

    public IEnumerable<Prodotto> Prodotti { get; set; }

    public void OnGet(decimal? minPrezzo, decimal? maxPrezzo) // aggiunta di argomenti per filtrare i prodotti per prezzo
    {
        Prodotti = new List<Prodotto>
        {
            new Prodotto { Id = 1, Nome = "github", Prezzo = 100, Dettaglio = "Robe", Immagine = "/img/github.png" },
            new Prodotto { Id = 2, Nome = "instagram", Prezzo = 200, Dettaglio = "Cose", Immagine = "/img/instagram.png" },
            new Prodotto { Id = 3, Nome = "linkedin", Prezzo = 300, Dettaglio = "Giri", Immagine = "/img/linkedin.png" }
        };

            /*

            List<Prodotto> prodottiFiltrati = new List<Prodotto>();

            foreach (var prodotto in Prodotti)
            {
                bool includeProduct = true;

                if (minPrezzo.HasValue && prodotto.Prezzo < minPrezzo.Value)
                {
                    includeProduct = false;
                }

                if (maxPrezzo.HasValue && prodotto.Prezzo > maxPrezzo.Value)
                {
                    includeProduct = false;
                }

                if (includeProduct)
                {
                    prodottiFiltrati.Add(prodotto);
                }
            Prodotti = prodottiFiltrati;
        }


        */

        // Metodo Alternativo
        
        if (minPrezzo.HasValue || maxPrezzo.HasValue)
        {
            Prodotti = Prodotti
                .Where(p => (minPrezzo == null || p.Prezzo >= minPrezzo) &&
                            (maxPrezzo == null || p.Prezzo <= maxPrezzo))
                .ToList();
        }
        

    }
}