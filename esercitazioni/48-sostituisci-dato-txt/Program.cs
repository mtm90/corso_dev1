string path = @"test.txt";
        string[] lines = File.ReadAllLines(path);
        lines[lines.Length - 2] = "Carmine"; // invece lines[lines.Length - 2] += "Carmine"; aggiunge a quella riga quella stringa

        File.WriteAllLines(path, lines);

        // lines[^ 2] = "Mario"; utilizzo dell'accento circonflesso