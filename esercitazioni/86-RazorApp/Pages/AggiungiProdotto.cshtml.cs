using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


public class AggiungiProdottoModel : PageModel
{
    private readonly ILogger<AggiungiProdottoModel> _logger;

    public AggiungiProdottoModel(ILogger<AggiungiProdottoModel> logger)
    {
        _logger = logger;
    }


    public Prodotto Prodotto { get; set; }
    public List<string> Categorie { get; set; }

    // metodo che riceve i dati dal server
    public void OnGet()
    {
        var json = System.IO.File.ReadAllText("wwwroot/json/categorie.json");
        Categorie = JsonConvert.DeserializeObject<List<string>>(json);
    }



    //invia dati al server web
    // i parametri vengono passati attraverso il form nella pagina web
    public IActionResult OnPost(string nome, decimal prezzo, string dettaglio, string immagine, int quantita, string categoria)
    {

        var json = System.IO.File.ReadAllText("wwwroot/json/prodotti.json");
        var tuttiProdotti = JsonConvert.DeserializeObject<List<Prodotto>>(json);
        int id = 1;
        if (tuttiProdotti.Count > 0)
        {
            id = tuttiProdotti[tuttiProdotti.Count - 1].Id + 1;
        }
        tuttiProdotti.Add(new Prodotto
        {
            Id = id,
            Nome = nome,
            Prezzo = prezzo,
            Dettaglio = dettaglio,
            Immagine = immagine,
            Quantita = quantita,
            Categoria = categoria
        });
        System.IO.File.WriteAllText("wwwroot/json/prodotti.json", JsonConvert.SerializeObject(tuttiProdotti, Formatting.Indented));
        return RedirectToPage("Prodotti");


    }
}
