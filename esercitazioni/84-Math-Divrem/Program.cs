int dividendo  = 10;
int divisore = 3;
int quoziente = Math.DivRem(dividendo, divisore, out int resto);
Console.WriteLine("Quoziente: " + quoziente);
Console.WriteLine("Resto: " + resto);