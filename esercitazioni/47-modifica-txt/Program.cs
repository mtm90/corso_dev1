string path = @"test.txt";
        string[] lines = File.ReadAllLines(path);
        lines[lines.Length - 2] += "Carmine";
        File.WriteAllLines(path, lines);