Console.WriteLine("Premi 'N' per terminare...");

        // Ciclo che continua fino a quando non viene premuto il tasto 'N' utilizzando KeyInfo
        // KeyInfo è una struttra che rappresenta le informazioni di un tasto premuto sulla tastiera.
        // viene utilizzata per leggere i tasti premuti dall'utente senza mostrare i caratteri a schermo.

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.N) // se premo N
            {
                break; // Esce dal ciclo se viene premuto 'N'
            }
        }

     // Imposta il colore dello sfondo e del testo
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;

        // Stampa un messaggio
        Console.WriteLine("Questo testo ha uno sfondo blu e un testo bianco");

        // Resetta i colori al loro default
        Console.ResetColor();
