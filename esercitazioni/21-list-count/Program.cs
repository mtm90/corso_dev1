List<string> nomi = new List<string>();
    nomi.Add("Mario");
    nomi.Add("Luigi");
    nomi.Add("Giovanni");

    Console.WriteLine($"Ciao {nomi[0]}, {nomi[1]} e {nomi[2]}");

    Console.WriteLine($"Il numero di elementi è {nomi.Count}");


    
/* Si possono inserire piu nomi in contemporanea invece di uno alla volta così
List<string> nomi = nomi List<string> {"Mario", "Luigi", "Giovanni"};
*/