

int[] numeri = {5, 9, 4, 3, 2};
int max = numeri[0];
int min = numeri[0];
for (int i = 1; i < numeri.Length; i++)
{
    max = Math.Max(max, numeri[i]);
    min = Math.Min(min, numeri[i]);
}
Console.WriteLine("massimo: " + max);
Console.WriteLine("minimo: " + min);