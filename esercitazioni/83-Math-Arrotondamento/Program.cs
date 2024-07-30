/*
double[] numeri = {3.1441, 2.74243, 1.61231};
for (int i = 0; i < numeri.Length; i++)
{
    numeri[i] = Math.Round(numeri[i], 2);
    Console.WriteLine($"Numero arrotondato: {numeri[i]}");
}

*/
/*
double[] numeri = {3.1441, 2.74243, 1.61231};
for (int i = 0; i < numeri.Length; i++)
{
    double arrPerEccesso = Math.Ceiling(numeri[i]);
    double arrPerDifetto = Math.Floor(numeri[i]);

    Console.WriteLine("Numero arrotondato per eccesso: " + arrPerEccesso);
    Console.WriteLine("Numero arrotondato per difetto: " + arrPerDifetto);
}
*/

double[] numeri = {3.5, 2.5, 1.5};
for (int i = 0; i < numeri.Length; i++)
{
    double arrPerEccesso = Math.Round(numeri[i], MidpointRounding.AwayFromZero);
    double arrPerDifetto = Math.Round(numeri[i], MidpointRounding.ToEven);

    Console.WriteLine("Numero arrotondato per eccesso: " + arrPerEccesso);
    Console.WriteLine("Numero arrotondato per difetto: " + arrPerDifetto);
}

