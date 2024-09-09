using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        string path = @"database.db"; // la rotta del file del database
        if (!File.Exists(path)) // se il file del database non esiste
        {
            SQLiteConnection.CreateFile(path); // crea il file del database
            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;"); // crea la connessione al database la versione 3 è un indicazione della versione del database e può esser personalizzata
            connection.Open(); // apre la connessione al database
            string sql = @"
                        CREATE TABLE prodotti (id INTEGER PRIMARY KEY AUTOINCREMENT, nome TEXT UNIQUE, prezzo REAL, quantita INTEGER CHECK (quantita >= 0), id_categoria INTEGER);
                        INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('p1', 1, 10, 1);
                        INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('p2', 2, 20, 2);
                        INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('p3', 3, 30, 3);
                        ";

            SQLiteCommand command = new SQLiteCommand(sql, connection); // crea il comando sql da eseguire sulla connessione al database
            command.ExecuteNonQuery(); // esegue il comando sql sulla connessione al database
            connection.Close(); // chiude la connessione al database
        }
        while (true)
        {
            Console.WriteLine("1 - Inserisci prodotto");
            Console.WriteLine("2 - visualizza prodotti");
            Console.WriteLine("3 - elimina prodotto");
            Console.WriteLine("4 - modifica prezzo prodotto");
            Console.WriteLine("5 - esci");
            Console.WriteLine("scegli un'opzione");
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
                break;
            }
        }
        static void InserisciProdotto()
        {
            Console.WriteLine("inserisci il nome del prodotto");
            string nome = Console.ReadLine()!;
            Console.WriteLine("inserisci il prezzo del prodotto");
            string prezzo = Console.ReadLine()!;
            Console.WriteLine("inserisci la quantità del prodotto");
            string quantita = Console.ReadLine()!;
            Console.WriteLine("inserisci l'id della categoria del prodotto");
            string id_categoria = Console.ReadLine()!;
            SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
            connection.Open();
            string sql = $"INSERT INTO prodotti (nome, prezzo, quantita, id_categoria) VALUES ('{nome}', {prezzo}, {quantita}, {id_categoria})"; // crea il comando sql che inserisce un prodotto
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    static void VisualizzaProdotti()
    {
        Console.WriteLine("inserisci il nome del prodotto");
        string nome = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"SELECT * FROM prodotti WHERE nome = '{nome}'"; // crea il comando sql che seleziona tutti i dati dalla tabella prodotti con nome uguale a quello inserito
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo: {reader["prezzo"]}, quantita: {reader["quantita"]}, id_categoria: {reader["id_categoria"]}");
        }
        connection.Close();
    }
    static void EliminaProdotto()
    {
        Console.WriteLine("inserisci il nome del prodotto");
        string nome = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"DELETE FROM prodotti WHERE nome = '{nome}'"; // crea il comando sql che elimina il prodotto con nome uguale a quello inserito
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }
    static void ModificaPrezzo()
    {
        Console.WriteLine("inserisci il nome del prodotto");
        string nome = Console.ReadLine()!;
        Console.WriteLine("inserisci il nuovo prezzo");
        string prezzo = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source=database.db;Version=3;");
        connection.Open();
        string sql = $"UPDATE prodotti SET prezzo = {prezzo} WHERE nome = '{nome}'"; // crea il comando sql che elimina il prodotto con nome uguale a quello inserito
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }
}