using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebAppProdotti.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Property to hold the featured products for display
        public IEnumerable<Prodotto> FeaturedProdotti { get; set; } = new List<Prodotto>();

        // OnGet method to load and filter featured products
        public void OnGet()
        {
            try
            {
                // Load products from the JSON file
                string jsonFilePath = "wwwroot/json/prodotti.json";
                var json = System.IO.File.ReadAllText(jsonFilePath);
                var tuttiProdotti = JsonConvert.DeserializeObject<List<Prodotto>>(json);

                if (tuttiProdotti == null || !tuttiProdotti.Any())
                {
                    _logger.LogWarning("No products found in the JSON file.");
                    return;
                }

                // Randomly select 3 products to feature on the homepage
                var random = new Random();
                FeaturedProdotti = tuttiProdotti.OrderBy(x => random.Next()).Take(4);
            }
            catch (FileNotFoundException)
            {
                _logger.LogError("File JSON not found.");
                FeaturedProdotti = new List<Prodotto>(); // Empty list in case of error
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading products: {ex.Message}");
                FeaturedProdotti = new List<Prodotto>(); // Empty list in case of error
            }
        }
    }
}
