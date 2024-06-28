string[] nomi = new string[8];
nomi[0] = "Mattia";
nomi[1] = "Allison";
nomi[2] = "Ginevra";
nomi[3] = "Daniele";
nomi[4] = "Sergey";
nomi[5] = "Silvano";
nomi[6] = "Matteo";
nomi[7] = "Sharon";

Console.WriteLine($"Il numero di elementi è {nomi.Length}");

/* Si possono inserire piu nomi in contemporanea invece di uno alla volta così
nomi.AddRange(new string[] { "Mario", "Luigi", "Giovanni"});
