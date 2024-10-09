using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class Categoria
{
    public string Nome { get; set; }
}


public class ProdottiController : Controller
{
    private static List<Prodotto> prodotti;
    private static List<Categoria> categorie;
    private const int PageSize = 4; // Number of items per page

    public ProdottiController()
    {
        // Load products from JSON
        var prodottiFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json/prodotti.json");
        if (System.IO.File.Exists(prodottiFilePath))
        {
            var jsonData = System.IO.File.ReadAllText(prodottiFilePath);
            prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(jsonData) ?? new List<Prodotto>();
        }
        else
        {
            prodotti = new List<Prodotto>();
        }

        // Load categories from JSON
        var categorieFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json/categorie.json");
        if (System.IO.File.Exists(categorieFilePath))
        {
            var jsonData = System.IO.File.ReadAllText(categorieFilePath);
            categorie = JsonConvert.DeserializeObject<List<Categoria>>(jsonData) ?? new List<Categoria>();
        }
        else
        {
            categorie = new List<Categoria>();
        }
    }

    public IActionResult Home()
{
    // Randomly select 5 best-selling products (modify the logic as needed)
    var random = new Random();
    var bestSellingProducts = prodotti
        .OrderBy(p => random.Next())
        .Take(5)
        .ToList();

    return View(bestSellingProducts);
}

    public IActionResult Index(decimal? minPrezzo, decimal? maxPrezzo, int pageIndex = 1)
    {
        // Filter products based on price range
        var filteredProducts = prodotti.Where(p => 
            (!minPrezzo.HasValue || p.Prezzo >= minPrezzo.Value) &&
            (!maxPrezzo.HasValue || p.Prezzo <= maxPrezzo.Value)).ToList();

        // Calculate total pages based on filtered products count
        int totalProducts = filteredProducts.Count;
        int totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

        // Get the products for the current page
        var paginatedProducts = filteredProducts.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

        // Pass data to view
        ViewBag.MinPrezzo = minPrezzo;
        ViewBag.MaxPrezzo = maxPrezzo;
        ViewBag.CurrentPage = pageIndex;
        ViewBag.TotalPages = totalPages;
        ViewBag.Categorie = categorie;

        return View(paginatedProducts);
    }

    public IActionResult Create()
    {
        // Read the list of categories from JSON file and pass it to the view
        ViewBag.Categorie = categorie;
        return View();
    }


    [HttpPost]
public IActionResult Create(Prodotto prodotto)
{
    if (!ModelState.IsValid)
    {
        ViewBag.Categorie = categorie; // Ensure categories are available
        return View(prodotto);
    }

    prodotto.Id = prodotti.Count + 1;
    prodotti.Add(prodotto);

    // Save updated list to JSON file
    SaveToFile();

    return RedirectToAction("Index");
}


    public IActionResult Edit(int id)
    {
        var prodotto = prodotti.FirstOrDefault(p => p.Id == id);
        ViewBag.Categorie = categorie; // Pass the list of categories to the view
        return prodotto == null ? NotFound() : View(prodotto);
    }


    [HttpPost]
    public IActionResult Edit(Prodotto prodotto)
    {
        var existingProdotto = prodotti.FirstOrDefault(p => p.Id == prodotto.Id);
        if (existingProdotto != null && ModelState.IsValid)
        {
            existingProdotto.Nome = prodotto.Nome;
            existingProdotto.Prezzo = prodotto.Prezzo;
            existingProdotto.Dettaglio = prodotto.Dettaglio;
            existingProdotto.Immagine = prodotto.Immagine;
            existingProdotto.Quantita = prodotto.Quantita;
            existingProdotto.Categoria = prodotto.Categoria;

            // Save updated list to JSON file
            SaveToFile();

            return RedirectToAction("Index");
        }
        return View(prodotto);
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var prodotto = prodotti.FirstOrDefault(p => p.Id == id);
        return View(prodotto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id) // Rename this method
    {
        var prodotto = prodotti.FirstOrDefault(p => p.Id == id);

        if (prodotto != null)
        {
            prodotti.Remove(prodotto);
            SaveToFile(); // Save changes
        }
        return RedirectToAction("Index"); // Redirect after deletion
    }


    public IActionResult Details(int id)
    {
        var prodotto = prodotti.FirstOrDefault(p => p.Id == id);
        return prodotto == null ? NotFound() : View(prodotto);
    }



    // Save the updated product list to the JSON file
    private void SaveToFile()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json/prodotti.json");
        var jsonData = JsonConvert.SerializeObject(prodotti, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, jsonData);
    }
}
