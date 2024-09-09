using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        string path = @"database.db"; // path of the database file
        if (!File.Exists(path)) // if the database file does not exist
        {
            SQLiteConnection.CreateFile(path); // create the database file
            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;"); // create connection to the database
            connection.Open(); // open the connection to the database
            string sql = @"
                        CREATE TABLE categorie (
                            id INTEGER PRIMARY KEY AUTOINCREMENT, 
                            nome TEXT UNIQUE
                        );
                        
                        CREATE TABLE prodotti (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            nome TEXT UNIQUE, 
                            prezzo REAL, 
                            quantita INTEGER CHECK (quantita >= 0), 
                            id_categoria INTEGER,
                            FOREIGN KEY (id_categoria) REFERENCES categorie(id)
                        );
                        
                        INSERT INTO categorie (nome) VALUES ('elettronica');
                        INSERT INTO categorie (nome) VALUES ('igiene');
                        INSERT INTO categorie (nome) VALUES ('gioielli');
                        
                        INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('tv', 1, 10, 1);
                        INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('carta', 2, 20, 2);
                        ";

            SQLiteCommand command = new SQLiteCommand(sql, connection); // create the SQL command
            command.ExecuteNonQuery(); // execute the command
            connection.Close(); // close the connection
        }

        while (true)
        {
            Console.WriteLine("1 - Inserisci prodotto");
            Console.WriteLine("2 - Visualizza prodotti");
            Console.WriteLine("3 - Elimina prodotto");
            Console.WriteLine("4 - Modifica prezzo prodotto");
            Console.WriteLine("5 - Inserisci categoria");
            Console.WriteLine("6 - Elimina categoria");
            Console.WriteLine("7 - Visualizza categorie");
            Console.WriteLine("8 - Inserisci prodotto con categoria");
            Console.WriteLine("9 - Esci");
            Console.WriteLine("Scegli un'opzione:");
            string scelta = Console.ReadLine()!;

            if (scelta == "1")
            {
                InserisciProdotto();
            }
            else if (scelta == "2")
            {
                VisualizzaProdotti();
            }
            else if (scelta == "3")
            {
                EliminaProdotto();
            }
            else if (scelta == "4")
            {
                ModificaPrezzo();
            }
            else if (scelta == "5")
            {
                InserisciCategoria();
            }
            else if (scelta == "6")
            {
                EliminaCategoria();
            }
            else if (scelta == "7")
            {
                VisualizzaCategorie();
            }
            else if (scelta == "8")
            {
                InserisciProdottoConCategoria();
            }
            else if (scelta == "9")
            {
                break;
            }
        }
    }

    static void InserisciProdotto()
    {
        Console.WriteLine("Inserisci il nome del prodotto:");
        string nome = Console.ReadLine()!;
        Console.WriteLine("Inserisci il prezzo del prodotto:");
        string prezzo = Console.ReadLine()!;
        Console.WriteLine("Inserisci la quantità del prodotto:");
        string quantita = Console.ReadLine()!;
        Console.WriteLine("Inserisci l'id della categoria del prodotto:");
        string id_categoria = Console.ReadLine()!;

        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('{nome}', {prezzo}, {quantita}, {id_categoria})";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    static void VisualizzaProdotti()
    {
        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = "SELECT prodotti.nome, prodotti.prezzo, prodotti.quantita, categorie.nome AS categoria FROM prodotti INNER JOIN categorie ON prodotti.id_categoria = categorie.id";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"Prodotto: {reader["nome"]}, Prezzo: {reader["prezzo"]}, Quantità: {reader["quantita"]}, Categoria: {reader["categoria"]}");
        }
        connection.Close();
    }

    static void EliminaProdotto()
    {
        Console.WriteLine("Inserisci il nome del prodotto da eliminare:");
        string nome = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"DELETE FROM prodotti WHERE nome = '{nome}'";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    static void ModificaPrezzo()
    {
        Console.WriteLine("Inserisci il nome del prodotto per cui modificare il prezzo:");
        string nome = Console.ReadLine()!;
        Console.WriteLine("Inserisci il nuovo prezzo:");
        string prezzo = Console.ReadLine()!;

        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"UPDATE prodotti SET prezzo = {prezzo} WHERE nome = '{nome}'";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    // Function to add a new category
    static void InserisciCategoria()
    {
        Console.WriteLine("Inserisci il nome della nuova categoria:");
        string nomeCategoria = Console.ReadLine()!;

        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"INSERT INTO categorie (nome) VALUES ('{nomeCategoria}')";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Categoria aggiunta con successo.");
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Errore: {e.Message}");
        }
        connection.Close();
    }

    // Function to delete a category
    static void EliminaCategoria()
    {
        Console.WriteLine("Inserisci il nome della categoria da eliminare:");
        string nomeCategoria = Console.ReadLine()!;

        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"DELETE FROM categorie WHERE nome = '{nomeCategoria}'";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Categoria eliminata con successo.");
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Errore: {e.Message}");
        }
        connection.Close();
    }
    static void VisualizzaCategorie()
    {
            SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
    connection.Open();
    string sql = "SELECT * FROM categorie"; // SQL query to select all categories
    SQLiteCommand command = new SQLiteCommand(sql, connection);
    SQLiteDataReader reader = command.ExecuteReader();

    Console.WriteLine("Categorie disponibili:");
    while (reader.Read())
    {
        Console.WriteLine($"ID: {reader["id"]}, Nome: {reader["nome"]}");
    }

    connection.Close();
    }
    static void InserisciProdottoConCategoria()
{
    // Step 1: Insert product details
    Console.WriteLine("Inserisci il nome del prodotto:");
    string nomeProdotto = Console.ReadLine()!;
    
    Console.WriteLine("Inserisci il prezzo del prodotto:");
    string prezzoProdotto = Console.ReadLine()!;
    
    Console.WriteLine("Inserisci la quantità del prodotto:");
    string quantitaProdotto = Console.ReadLine()!;
    
    // Step 2: Show existing categories
    SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
    connection.Open();
    string sql = "SELECT * FROM categorie";
    SQLiteCommand command = new SQLiteCommand(sql, connection);
    SQLiteDataReader reader = command.ExecuteReader();

    Console.WriteLine("Categorie disponibili:");
    while (reader.Read())
    {
        Console.WriteLine($"ID: {reader["id"]}, Nome: {reader["nome"]}");
    }
    reader.Close();

    // Step 3: Ask if the user wants to select an existing category or create a new one
    Console.WriteLine("Vuoi inserire una nuova categoria? (s/n)");
    string scelta = Console.ReadLine()!.ToLower();

    int idCategoria;

    if (scelta == "s")
    {
        // Step 4: Insert new category
        Console.WriteLine("Inserisci il nome della nuova categoria:");
        string nomeCategoria = Console.ReadLine()!;

        // Insert the new category into the database
        string insertCategoriaSql = $"INSERT INTO categorie (nome) VALUES ('{nomeCategoria}')";
        SQLiteCommand insertCategoriaCommand = new SQLiteCommand(insertCategoriaSql, connection);
        insertCategoriaCommand.ExecuteNonQuery();

        // Get the ID of the new category
        string getIdCategoriaSql = "SELECT last_insert_rowid()";
        SQLiteCommand getIdCategoriaCommand = new SQLiteCommand(getIdCategoriaSql, connection);
        idCategoria = Convert.ToInt32(getIdCategoriaCommand.ExecuteScalar());
    }
    else
    {
        // Step 5: Select existing category
        Console.WriteLine("Inserisci l'ID della categoria scelta:");
        idCategoria = Convert.ToInt32(Console.ReadLine());
    }

    // Step 6: Insert the product with the selected or newly created category
    string insertProdottoSql = $"INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('{nomeProdotto}', {prezzoProdotto}, {quantitaProdotto}, {idCategoria})";
    SQLiteCommand insertProdottoCommand = new SQLiteCommand(insertProdottoSql, connection);
    insertProdottoCommand.ExecuteNonQuery();

    connection.Close();
    Console.WriteLine("Prodotto inserito con successo.");
}

}
