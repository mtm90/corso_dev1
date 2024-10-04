using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


    public class AggiungiProdottoModel : PageModel
    {
        private readonly ILogger<AggiungiProdottoModel> _logger;

        public AggiungiProdottoModel(ILogger<AggiungiProdottoModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public Prodotto Prodotto { get; set; }
        
        public List<string> Categorie { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Il codice è obbligatorio.")]
        public string Codice { get; set; }

        public void OnGet()
        {
            // Read the list of categories from JSON file and set it to the Categorie property
            var json = System.IO.File.ReadAllText("wwwroot/json/categorie.json");
            Categorie = JsonConvert.DeserializeObject<List<string>>(json);
        }

        public IActionResult OnPost()
        {
            // Check for a valid code before proceeding
            if (Codice != "1234")
            {
                return RedirectToPage("Error", new { message = "Codice non valido" });
            }

            else
            {

            // Deserialize the existing products from JSON file
            var json = System.IO.File.ReadAllText("wwwroot/json/prodotti.json");
            var tuttiProdotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();

            // Generate a new product ID
            int id = 1;
            if (tuttiProdotti.Count > 0)
            {
                id = tuttiProdotti[tuttiProdotti.Count - 1].Id + 1;
            }
            Prodotto.Id = id;

            // Set a default image if not provided
            if (Prodotto.Immagine == null)
            {
                Prodotto.Immagine = "img/default.jpg";
            }

            // Add the new product to the list and save it back to the JSON file
            tuttiProdotti.Add(Prodotto);
            System.IO.File.WriteAllText("wwwroot/json/prodotti.json", JsonConvert.SerializeObject(tuttiProdotti, Formatting.Indented));

            return RedirectToPage("Prodotti");
            }
            
        }
    }
