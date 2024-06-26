/*
Console.Clear();

Console.WriteLine("Enter a number");
int number = Convert.ToInt32(Console.ReadLine());

if (number % 3 == 0 && number % 5 == 0)
{
    Console.WriteLine("FizzBuzz");
}
else if (number % 5 == 0)
{
    Console.WriteLine("Buzz");
}
else if (number % 3 == 0)
{
    Console.WriteLine("Fizz");
}
else 
{
    Console.WriteLine(number);
}
*/

for (int i = 1; i < 101; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine($"numero: {i} FizzBuzz");
    }
    else if (i % 3 == 0)
    {
        Console.WriteLine($"numero: {i} Fizz");
    }
    else if (i % 5 == 0)
    {
        Console.WriteLine($"numero: {i} Buzz");
    }
    else
    {
        Console.WriteLine($"numero: {i}");
    }
}