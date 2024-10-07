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

    // Constructor: Load data from JSON file when controller is instantiated
    public ProdottiController()
{
    // Load products
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

    // Load categories
    var categorieFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json/categorie.json");
    if (System.IO.File.Exists(categorieFilePath))
    {
        var jsonData = System.IO.File.ReadAllText(categorieFilePath);
        categorie = JsonConvert.DeserializeObject<List<Categoria>>(jsonData) ?? new List<Categoria>();
    }
    else
    {
        categorie = new List<Categoria>(); // Initialize with an empty list if file is not found
    }
}

    public IActionResult Index()
    {
        return View(prodotti);
    }

    public IActionResult Create()
{
    // Read the list of categories from JSON file and pass it to the view
    var json = System.IO.File.ReadAllText("wwwroot/json/categorie.json");
    ViewBag.Categorie = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

    return View();
}


    [HttpPost]
    public IActionResult Create(Prodotto prodotto)
    {

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

    public IActionResult Delete(int id)
{
    var prodotto = prodotti.FirstOrDefault(p => p.Id == id);

    if (HttpContext.Request.Method == "POST") // Check if it's a POST request
    {
        if (prodotto != null)
        {
            prodotti.Remove(prodotto);
            SaveToFile(); // Save changes
        }
        return RedirectToAction("Index"); // Redirect after deletion
    }

    

    return prodotto == null ? NotFound() : View(prodotto); // Return the view if it's a GET request
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
