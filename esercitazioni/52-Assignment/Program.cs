﻿// Programma che legge un file di testo txt contenente dei nomi ed utilizza il metodo random per sorteggiare un nome da stampare e
// SE NON ESISTE crea un file txt con il nome sorteggiato
// SE ESISTE aggiunge il nome sorteggiato al file
// SE IL NOME è GIA PRESENTE NEL FILE NON LO AGGIUNGE stampando un messaggio

string path = @"test.txt"; // in questo caso il file è nella stessa cartella del programma
        string[] lines = File.ReadAllLines(path); // legge tutte le righe del file
        string[] nomi = new string[lines.Length]; // crea un array di stringhe con la lunghezza del numero di righe del file
        for (int i = 0; i < lines.Length; i++)
        {
            nomi[i] = lines[i]; // assegna ad ogni elemento dell'array di stringhe il valore della riga corrispondente
        }
        Random random = new Random(); // crea un oggetto random
        int index = random.Next(nomi.Length); // genera un numero casuale tra 0 e la lunghezza dell'array di stringhe
        Console.WriteLine(nomi[index]); // stampa il nome corrispondente all'indice generato casualmente
        string path2 = @"test2.txt"; // in questo caso il file è nella stessa cartella del programma
        if (!File.Exists(path2)) // controlla se il file esiste
        {
            File.Create(path2).Close(); // crea il file
        }
        if (!File.ReadAllLines(path2).Contains(nomi[index]))
        {
            File.AppendAllText(path2, nomi[index] + "\n"); 
        }
        else 
        {
            Console.WriteLine($"The name {nomi[index]} already exists in the file");
        }
        
