Console.Clear();

Console.WriteLine("Please insert a number from 1 to 7, it will return the corresponding day of the week");

int giorno = Convert.ToInt32(Console.ReadLine());

switch (giorno)
{
    case 1:
        Console.WriteLine("Lunedì");
        break;

    case 2:
        Console.WriteLine("Martedì");
        break;

    case 3:
        Console.WriteLine("Mercoledì");
        break;

    case 4:
        Console.WriteLine("Giovedì");
        break;

    case 5:
        Console.WriteLine("Venerdì");
        break;

    case 6:
        Console.WriteLine("Sabato");
        break;

    case 7:
        Console.WriteLine("Domenica");
        break;

    default:
        Console.WriteLine("Please enter a valid number");
        break;
}



