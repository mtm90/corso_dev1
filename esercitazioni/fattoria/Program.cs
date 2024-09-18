public class Animale
{
    public string Nome {get; set;}
    public int Eta {get; set;}
    public string Verso {get; set;}

    public virtual void FaiVerso()
    {
        Console.WriteLine($"{Nome} fa: {Verso}");
    }

    public virtual void AzioneSpecifica()
    {

    }
}
public class Mucca : Animale
{
    public double QuantitaLatte {get; set;}

    public override void AzioneSpecifica()
    {
        Mungi();
    }

    public void Mungi()
    {
        Console.WriteLine($"{Nome} è stata munta e ha prodotto {QuantitaLatte} litri di latte.");
    }
}

public class Maiale : Animale
{
    public double Peso { get; set;}

    public void Pesa()
    {
        Console.WriteLine($"{Nome} sta mangiando e pesa ora {Peso} Kg.");
    }

        public override void AzioneSpecifica()
    {
        Pesa();
    }
}
public class Gallina : Animale
{
    public int Uova {get; set;}

    public void Depone()
    {
        Console.WriteLine($"{Nome} depone {Uova} uovo");
    }

        public override void AzioneSpecifica()
    {
        Depone();
    }

}

public class Pecora : Animale
{
    public double Lana {get; set;}

    public void Tosatura()
    {
        Console.WriteLine($"{Nome} viene tosata e produce {Lana} Kg di lana");

    }

    public override void AzioneSpecifica()
    {
        Tosatura();
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Animale> animali = new List<Animale>();

        Mucca mucca = new Mucca
        {
            Nome = "Paola",
            Eta = 5,
            Verso = "Muuu",
            QuantitaLatte = 10
        };
        animali.Add(mucca);

        Maiale maiale = new Maiale
        {
            Nome = "Giorgio",
            Eta = 3,
            Verso = "oink",
            Peso = 100

        };
        animali.Add(maiale);


        Gallina gallina = new Gallina
        {
            Nome = "Andreina",
            Eta = 1,
            Verso = "Coccodè",
            Uova = 1
        };

        animali.Add(gallina);

        Pecora pecora = new Pecora
        {
            Nome = "Graziella",
            Eta = 2,
            Verso = "Beee",
            Lana = 4.3
        };

        animali.Add(pecora);



            foreach (var animale in animali )
        {
            animale.FaiVerso();
            animale.AzioneSpecifica();
        }

    }
}